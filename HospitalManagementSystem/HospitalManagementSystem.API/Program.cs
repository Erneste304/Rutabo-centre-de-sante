using Microsoft.EntityFrameworkCore;
using HospitalManagementSystem.Data;
using HospitalManagementSystem.Core.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using System.Text.Json.Serialization;
using System.Text;
using HospitalManagementSystem.Data.Repositories;
using HospitalManagementSystem.Core.Repositories;
using HospitalManagementSystem.Data.Services;
using HospitalManagementSystem.API.Hubs;
using HospitalManagementSystem.API.Services;


var builder = WebApplication.CreateBuilder(args);

var jwtKey = builder.Configuration["Jwt:Key"];
var jwtIssuer = builder.Configuration["Jwt:Issuer"];
var jwtAudience = builder.Configuration["Jwt:Audience"];
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
if (string.IsNullOrWhiteSpace(jwtKey) || Encoding.UTF8.GetByteCount(jwtKey) < 32 ||
    string.IsNullOrWhiteSpace(jwtIssuer) || string.IsNullOrWhiteSpace(jwtAudience))
{
    throw new InvalidOperationException(
        "JWT configuration is incomplete. Configure Jwt:Key (at least 32 UTF-8 bytes), Jwt:Issuer, and Jwt:Audience.");
}

if (string.IsNullOrWhiteSpace(connectionString))
{
    throw new InvalidOperationException(
        "Database connection is not configured. Set ConnectionStrings:DefaultConnection.");
}

// Add services to the container.
builder.Services.AddControllers().AddJsonOptions(options =>
    options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull);
builder.Services.AddEndpointsApiExplorer();

// Add SignalR
builder.Services.AddSignalR();

var allowedOrigins = builder.Configuration.GetSection("Cors:AllowedOrigins").Get<string[]>() ?? Array.Empty<string>();
if (allowedOrigins.Length == 0)
{
    throw new InvalidOperationException("Configure at least one trusted origin in Cors:AllowedOrigins.");
}

builder.Services.AddCors(options => options.AddPolicy("TrustedOrigins", policy =>
    policy.WithOrigins(allowedOrigins).AllowAnyMethod().AllowAnyHeader()));
builder.Services.AddAuthorization(options =>
{
    var hospitalStaff = new[] { "Admin", "Doctor", "Nurse", "Receptionist", "Accountant" };
    options.AddPolicy("HospitalStaff", policy => policy.RequireRole(hospitalStaff));
    options.FallbackPolicy = new AuthorizationPolicyBuilder()
        .RequireRole(hospitalStaff)
        .Build();
});

// Add DbContext
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(connectionString));

// Add Authentication
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtIssuer,
            ValidAudience = jwtAudience,
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(jwtKey))
        };
    });

// Add Repositories
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IPatientRepository, PatientRepository>();

// Add Services
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IPatientService, PatientService>();
builder.Services.AddScoped<INurseService, NurseService>();
builder.Services.AddScoped<IBillingService, BillingService>();
builder.Services.AddScoped<IAppointmentService, AppointmentService>();
builder.Services.AddScoped<IInventoryService, InventoryService>();
builder.Services.AddScoped<IPrescriptionService, PrescriptionService>();
builder.Services.AddScoped<IMedicalRecordService, MedicalRecordService>();
builder.Services.AddScoped<ILabTestService, LabTestService>();
builder.Services.AddScoped<IAmbulanceService, AmbulanceService>();
builder.Services.AddScoped<IBloodBankService, BloodBankService>();
builder.Services.AddScoped<IPatientFlowService, PatientFlowService>();
builder.Services.AddScoped<IDatabaseSeeder, DatabaseSeeder>();

// Add Notification Service
builder.Services.AddScoped<INotificationService, NotificationService>();

var app = builder.Build();

// Seed the database before starting the app
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<ApplicationDbContext>();
    await context.Database.EnsureCreatedAsync();
    if (app.Environment.IsDevelopment())
    {
        var seeder = services.GetRequiredService<IDatabaseSeeder>();
        await seeder.SeedAsync();
    }
}

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseCors("TrustedOrigins");
app.UseDefaultFiles();
app.UseStaticFiles();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapHub<HospitalHub>("/hubs/hospital").RequireAuthorization();

app.MapFallback(async context =>
{
    var indexPath = Path.Combine(app.Environment.WebRootPath, "index.html");
    if (File.Exists(indexPath))
    {
        context.Response.ContentType = "text/html";
        await context.Response.SendFileAsync(indexPath);
    }
    else
    {
        context.Response.StatusCode = 404;
    }
}).AllowAnonymous();

app.Run();
