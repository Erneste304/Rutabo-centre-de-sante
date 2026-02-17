using HospitalManagementSystem.Blazor;
using HospitalManagementSystem.Blazor.Components;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

// Register HttpClient for the API
builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("http://localhost:5051/") });

// Register Blazor Services
builder.Services.AddScoped<HospitalManagementSystem.Blazor.Services.ApiService>();
builder.Services.AddScoped<HospitalManagementSystem.Blazor.Services.AuthService>();
builder.Services.AddScoped<HospitalManagementSystem.Blazor.Services.DashboardService>();
builder.Services.AddScoped<HospitalManagementSystem.Blazor.Services.NurseService>();
builder.Services.AddScoped<HospitalManagementSystem.Blazor.Services.BillingService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
app.UseStatusCodePagesWithReExecute("/not-found", createScopeForStatusCodePages: true);
app.UseHttpsRedirection();

app.UseAntiforgery();

app.UseStaticFiles();
app.MapStaticAssets();

app.MapRazorComponents<HospitalManagementSystem.Main>()
    .AddInteractiveServerRenderMode()
    .AddAdditionalAssemblies(typeof(HospitalManagementSystem.Blazor._Imports).Assembly);

app.Run();
