using System;
using System.Threading.Tasks;
using HospitalManagementSystem.ConsoleApp.Dashboard;
using HospitalManagementSystem.ConsoleApp.Models;
using HospitalManagementSystem.ConsoleApp.Services;
using Microsoft.Extensions.DependencyInjection;

class Program
{
    static async Task Main(string[] args)
    {
        Console.Title = "Hospital Management System";
        
        // Setup Dependency Injection
        var services = new ServiceCollection();
        ConfigureServices(services);
        var serviceProvider = services.BuildServiceProvider();
        
        // Get services
        var menuService = serviceProvider.GetRequiredService<IMenuService>();
        var authService = serviceProvider.GetRequiredService<IAuthenticationService>();
        var dashboardService = serviceProvider.GetRequiredService<IDashboardService>();
        
        await RunApplication(menuService, authService, dashboardService);
    }
    
    static void ConfigureServices(ServiceCollection services)
    {
        services.AddSingleton<IAuthenticationService, AuthenticationService>();
        services.AddSingleton<IDataService, DataService>();
        services.AddSingleton<IMenuService, MenuService>();
        services.AddSingleton<IDashboardService, DashboardService>();
    }
    
    static async Task RunApplication(IMenuService menuService, IAuthenticationService authService, IDashboardService dashboardService)
    {
        while (true)
        {
            await menuService.ShowMainMenuAsync();
            var choice = Console.ReadLine();
            
            switch (choice)
            {
                case "1":
                    await Register(authService);
                    break;
                case "2":
                    await Login(authService, dashboardService);
                    break;
                case "3":
                    ((MenuService)menuService).ShowEmergencyInfo();
                    break;
                case "4":
                    ((MenuService)menuService).ShowHospitalDirectory();
                    break;
                case "5":
                    await ShowVisitorInformation();
                    break;
                case "6":
                    await ShowAbout();
                    break;
                case "7":
                    Console.WriteLine("\nThank you for using Hospital Management System!");
                    return;
                default:
                    Console.WriteLine("\nInvalid option. Press any key to continue...");
                    Console.ReadKey();
                    break;
            }
        }
    }
    
    static async Task Login(IAuthenticationService authService, IDashboardService dashboardService)
    {
        Console.Clear();
        Console.WriteLine("=== LOGIN ===");
        
        Console.Write("Username: ");
        var username = Console.ReadLine();
        
        Console.Write("Password: ");
        var password = Console.ReadLine();
        
        Console.WriteLine("\nAuthenticating...");
        
        var session = await authService.AuthenticateAsync(username ?? string.Empty, password ?? string.Empty);
        
        if (session != null)
        {
            Console.WriteLine($"\nLogin successful! Welcome, {session.FullName}");
            Console.WriteLine($"User Type: {session.UserType}");
            await Task.Delay(1500);
            
            await dashboardService.ShowDashboardAsync(session);
        }
        else
        {
            Console.WriteLine("\nLogin failed. Please check your credentials or wait for admin approval if you registered as staff.");
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
        }
    }
    
    static async Task Register(IAuthenticationService authService)
    {
        Console.Clear();
        Console.WriteLine("=== REGISTER ===");
        
        Console.Write("Username: ");
        var username = Console.ReadLine()?.Trim();
        
        Console.Write("Email: ");
        var email = Console.ReadLine()?.Trim();
        
        Console.Write("Full name: ");
        var fullName = Console.ReadLine()?.Trim();
        
        Console.Write("Password: ");
        var password = Console.ReadLine();

        Console.WriteLine("\nSelect your role:");
        Console.WriteLine("1. Patient (no approval required)");
        Console.WriteLine("2. Doctor (admin approval required)");
        Console.WriteLine("3. Nurse (admin approval required)");
        Console.WriteLine("4. Receptionist (admin approval required)");
        Console.Write("Role: ");
        var roleChoice = Console.ReadLine();

        var userType = roleChoice switch
        {
            "2" => "Doctor",
            "3" => "Nurse",
            "4" => "Receptionist",
            _ => "Patient"
        };
        
        Console.WriteLine("\nRegistering...");
        
        var created = await authService.RegisterAsync(
            username ?? string.Empty, 
            email ?? string.Empty, 
            password ?? string.Empty, 
            fullName ?? string.Empty, 
            userType);
        
        if (created)
        {
            if (string.Equals(userType, "Patient", StringComparison.OrdinalIgnoreCase))
            {
                Console.WriteLine("\nRegistration successful! You can now login as a Patient.");
            }
            else
            {
                Console.WriteLine($"\nRegistration submitted as {userType}. An administrator must approve your account before you can login.");
            }
        }
        else
        {
            Console.WriteLine("\nRegistration failed. Username or email may already be in use, or input was invalid.");
        }
        
        Console.WriteLine("Press any key to continue...");
        Console.ReadKey();
    }
    
    static async Task ShowVisitorInformation()
    {
        Console.Clear();
        Console.WriteLine("╔══════════════════════════════════════╗");
        Console.WriteLine("║         VISITOR INFORMATION          ║");
        Console.WriteLine("╠══════════════════════════════════════╣");
        Console.WriteLine("║   Visiting Hours:                    ║");
        Console.WriteLine("║   • General: 10:00 AM - 8:00 PM      ║");
        Console.WriteLine("║   • ICU: 11:00 AM - 7:00 PM          ║");
        Console.WriteLine("║   • Pediatrics: 9:00 AM - 9:00 PM    ║");
        Console.WriteLine("║                                      ║");
        Console.WriteLine("║   Parking:                           ║");
        Console.WriteLine("║   • Visitor Parking: Lot A & B       ║");
        Console.WriteLine("║   • First 2 hours: Free              ║");
        Console.WriteLine("║   • Disabled Parking: Available      ║");
        Console.WriteLine("║                                      ║");
        Console.WriteLine("║   Amenities:                         ║");
        Console.WriteLine("║   • Cafeteria: Floor 1               ║");
        Console.WriteLine("║   • Gift Shop: Main Lobby            ║");
        Console.WriteLine("║   • Chapel: Floor 2                  ║");
        Console.WriteLine("║   • WiFi: Free for visitors          ║");
        Console.WriteLine("╚══════════════════════════════════════╝");
        Console.WriteLine("\nPress any key to continue...");
        Console.ReadKey();
    }
    
    static async Task ShowAbout()
    {
        Console.Clear();
        Console.WriteLine("╔══════════════════════════════════════╗");
        Console.WriteLine("║             ABOUT                    ║");
        Console.WriteLine("╠══════════════════════════════════════╣");
        Console.WriteLine("║   Hospital Management System v1.0    ║");
        Console.WriteLine("║                                      ║");
        Console.WriteLine("║   Developed by:                      ║");
        Console.WriteLine("║   • Your Name/Team                  ║");
        Console.WriteLine("║                                      ║");
        Console.WriteLine("║   Features:                          ║");
        Console.WriteLine("║   • Patient Management              ║");
        Console.WriteLine("║   • Appointment Scheduling          ║");
        Console.WriteLine("║   • Medical Records                 ║");
        Console.WriteLine("║   • Billing System                  ║");
        Console.WriteLine("║   • Role-based Dashboards           ║");
        Console.WriteLine("║                                      ║");
        Console.WriteLine("║   Contact:                          ║");
        Console.WriteLine("║   • support@hospital.com            ║");
        Console.WriteLine("║   • (555) 123-4567                  ║");
        Console.WriteLine("╚══════════════════════════════════════╝");
        Console.WriteLine("\nPress any key to continue...");
        Console.ReadKey();
    }
}