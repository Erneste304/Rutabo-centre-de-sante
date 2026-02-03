using HospitalManagementSystem.Core.Models;
using HospitalManagementSystem.Core.Services;
using Microsoft.Extensions.DependencyInjection;

class Program
{
    static async Task Main(string[] args)
    {
        var services = new ServiceCollection();
        ConfigureServices(services);
        
        var serviceProvider = services.BuildServiceProvider();
        var authService = serviceProvider.GetService<IAuthService>();
        
        await ShowMainMenu(authService);
    }

    static void ConfigureServices(ServiceCollection services)
    {
        // Register services here
        // services.AddScoped<IUserService, UserService>();
        // services.AddScoped<IPatientService, PatientService>();
    }

    static async Task ShowMainMenu(IAuthService? authService)
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine("=== Hospital Management System ===");
            Console.WriteLine("1. Login");
            Console.WriteLine("2. Register");
            Console.WriteLine("3. Exit");
            Console.Write("Select option: ");
            
            var choice = Console.ReadLine();
            
            switch (choice)
            {
                case "1":
                    await Login(authService);
                    break;
                case "2":
                    await Register();
                    break;
                case "3":
                    return;
                default:
                    Console.WriteLine("Invalid option. Press any key to continue...");
                    Console.ReadKey();
                    break;
            }
        }
    }

    static async Task Login(IAuthService? authService)
    {
        Console.Clear();
        Console.WriteLine("=== Login ===");
        Console.Write("Username: ");
        var username = Console.ReadLine();
        Console.Write("Password: ");
        var password = Console.ReadLine();
        
        // Implement login logic
        Console.WriteLine("Login functionality to be implemented...");
        Console.ReadKey();
    }

    static async Task Register()
    {
        Console.Clear();
        Console.WriteLine("=== Registration ===");
        
        // Implement registration logic
        
        Console.WriteLine("Registration functionality to be implemented...");
        Console.ReadKey();
    }
}