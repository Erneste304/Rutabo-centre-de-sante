using System;
using Microsoft.EntityFrameworkCore;
using HospitalManagementSystem.Data;
using HospitalManagementSystem.Data.Repositories;
using HospitalManagementSystem.Core.Services;
using HospitalManagementSystem.ConsoleApp.Dashboard;

namespace HospitalManagementSystem.ConsoleApp
{
    class Program
    {
        static async Task Main(string[] args)
        {
            // 1. Setup Database
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseSqlite("Data Source=hospital.db")
                .Options;

            using var context = new AppDbContext(options);
            
            // Ensure database is created
            context.Database.EnsureCreated();

            // 2. Setup Dependencies
            var repository = new PatientRepository(context);
            var service = new PatientService(repository);

            // 3. Run Dashboard
            System.Console.WriteLine("Hospital Management System Loaded.");
            System.Console.WriteLine("Database connected.");
            
            var mainMenu = new MainMenu(service);
            await mainMenu.ShowAsync();
        }
    }
}