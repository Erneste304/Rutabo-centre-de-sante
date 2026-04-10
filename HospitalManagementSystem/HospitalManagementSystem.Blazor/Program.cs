using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using HospitalManagementSystem.Blazor;
using HospitalManagementSystem.Blazor.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

// Register HttpClient
var apiBaseAddress = builder.Configuration["ApiBaseAddress"] ?? "http://localhost:5051/";
builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(apiBaseAddress) });

// Register Services
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

await builder.Build().RunAsync();