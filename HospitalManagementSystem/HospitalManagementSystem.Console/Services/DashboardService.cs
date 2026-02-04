using System;
using System.Threading.Tasks;
using HospitalManagementSystem.ConsoleApp.Dashboard;
using HospitalManagementSystem.ConsoleApp.Models;

namespace HospitalManagementSystem.ConsoleApp.Services
{
    public interface IDashboardService
    {
        Task ShowDashboardAsync(UserSession session);
        Task ShowDashboardStatsAsync(UserSession session);
        Task ShowNotificationsAsync(UserSession session);
    }

    public class DashboardService : IDashboardService
    {
        private readonly IDataService _dataService;

        public DashboardService(IDataService dataService)
        {
            _dataService = dataService;
        }

        public async Task ShowDashboardAsync(UserSession session)
        {
            var dashboard = DashboardFactory.CreateDashboard(
                session.UserType, 
                session.UserId, 
                session.FullName, 
                session.Specialization
            );
            
            await dashboard.ShowAsync();
        }

        public async Task ShowDashboardStatsAsync(UserSession session)
        {
            var stats = await _dataService.GetDashboardStatsAsync(session.UserType);
            
            Console.Clear();
            Console.WriteLine($"╔══════════════════════════════════════╗");
            Console.WriteLine($"║       DASHBOARD STATISTICS           ║");
            Console.WriteLine($"╠══════════════════════════════════════╣");
            Console.WriteLine($"║   User: {session.GetWelcomeMessage(),-25} ║");
            Console.WriteLine($"║   Date: {DateTime.Now:yyyy-MM-dd HH:mm}             ║");
            Console.WriteLine($"╠══════════════════════════════════════╣");
            Console.WriteLine($"║   Total Patients: {stats.TotalPatients,-18} ║");
            Console.WriteLine($"║   Today's Appointments: {stats.TodayAppointments,-11} ║");
            Console.WriteLine($"║   Pending Tasks: {stats.PendingTasks,-17} ║");
            Console.WriteLine($"║   {stats.Stat1Label}: {stats.Stat1Value,-22} ║");
            Console.WriteLine($"║   {stats.Stat2Label}: {stats.Stat2Value,-22} ║");
            Console.WriteLine($"╚══════════════════════════════════════╝");
            
            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey();
        }

        public async Task ShowNotificationsAsync(UserSession session)
        {
            Console.Clear();
            Console.WriteLine($"╔══════════════════════════════════════╗");
            Console.WriteLine($"║          NOTIFICATIONS               ║");
            Console.WriteLine($"╠══════════════════════════════════════╣");
            
            var notifications = GetNotificationsForUser(session);
            foreach (var notification in notifications)
            {
                Console.WriteLine($"║ • {notification,-34} ║");
            }
            
            if (notifications.Count == 0)
            {
                Console.WriteLine($"║   No new notifications              ║");
            }
            
            Console.WriteLine($"╚══════════════════════════════════════╝");
            Console.WriteLine("\n1. Mark all as read");
            Console.WriteLine("2. Clear notifications");
            Console.WriteLine("3. Back to dashboard");
            Console.Write("\nSelect: ");
            
            var choice = Console.ReadLine();
            if (choice == "1")
            {
                Console.WriteLine("\nAll notifications marked as read.");
            }
            
            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey();
        }

        private List<string> GetNotificationsForUser(UserSession session)
        {
            var notifications = new List<string>();
            
            switch (session.UserType.ToLower())
            {
                case "doctor":
                    notifications.Add("New lab results available");
                    notifications.Add("Patient John Doe awaiting review");
                    notifications.Add("Staff meeting at 3:00 PM");
                    notifications.Add("2 pending prescriptions");
                    break;
                    
                case "nurse":
                    notifications.Add("Medication due for Room 101");
                    notifications.Add("New patient admission");
                    notifications.Add("Vital signs check required");
                    notifications.Add("Shift handover in 30 minutes");
                    break;
                    
                case "receptionist":
                    notifications.Add("5 new appointment requests");
                    notifications.Add("Insurance verification needed");
                    notifications.Add("Patient check-in required");
                    notifications.Add("Phone system update scheduled");
                    break;
                    
                case "patient":
                    notifications.Add("Appointment reminder: Tomorrow");
                    notifications.Add("Test results are ready");
                    notifications.Add("Prescription refill available");
                    break;
                    
                default:
                    notifications.Add("System maintenance scheduled");
                    notifications.Add("3 new user registrations");
                    notifications.Add("Backup completed successfully");
                    break;
            }
            
            return notifications;
        }
    }
}