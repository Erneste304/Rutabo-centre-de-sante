using HospitalManagementSystem.Blazor;
using HospitalManagementSystem.Blazor.Components;
using HospitalManagementSystem.Blazor.Services;
using Microsoft.AspNetCore.Components.Authorization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

var apiBaseAddress = builder.Configuration["ApiBaseAddress"] ?? "http://localhost:5051/";
builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(apiBaseAddress) });

builder.Services.AddAuthorizationCore();
builder.Services.AddCascadingAuthenticationState();
builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthenticationStateProvider>();
builder.Services.AddScoped<ApiService>();
builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<DashboardService>();
builder.Services.AddScoped<NurseService>();
builder.Services.AddScoped<ClinicalService>();
builder.Services.AddScoped<AmbulanceService>();
builder.Services.AddScoped<BloodBankService>();
builder.Services.AddScoped<PatientFlowService>();
builder.Services.AddScoped<AppointmentService>();
builder.Services.AddScoped<BillingService>();
builder.Services.AddScoped<PatientService>();
builder.Services.AddScoped<DoctorService>();
builder.Services.AddScoped<InventoryService>();
builder.Services.AddScoped<PharmacyService>();
builder.Services.AddScoped<RealTimeService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
app.UseStatusCodePagesWithReExecute("/not-found");
app.UseHttpsRedirection();

app.UseAntiforgery();

app.UseStaticFiles();

app.MapRazorComponents<HospitalManagementSystem.Main>()
    .AddInteractiveServerRenderMode()
    .AddAdditionalAssemblies(typeof(HospitalManagementSystem.Blazor._Imports).Assembly);

app.Run();
