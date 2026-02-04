using System;
using System.Threading.Tasks;

namespace HospitalManagementSystem.ConsoleApp.Services
{
    public interface IMenuService
    {
        Task ShowMainMenuAsync();
        Task ShowUserMenuAsync();
        Task ShowSettingsMenuAsync();
        Task ShowHelpMenuAsync();
    }

    public class MenuService : IMenuService
    {
        public async Task ShowMainMenuAsync()
        {
            Console.Clear();
            Console.WriteLine("╔══════════════════════════════════════╗");
            Console.WriteLine("║     HOSPITAL MANAGEMENT SYSTEM       ║");
            Console.WriteLine("╠══════════════════════════════════════╣");
            Console.WriteLine("║   1. 📝 Register                       ║");
            Console.WriteLine("║   2. Login                           ║");
            Console.WriteLine("║   3. Emergency Information           ║");
            Console.WriteLine("║   4. Hospital Directory              ║");
            Console.WriteLine("║   5. Visitor Information             ║");
            Console.WriteLine("║   6. About                           ║");
            Console.WriteLine("║   7. Exit                            ║");
            Console.WriteLine("╚══════════════════════════════════════╝");
            Console.Write("\nSelect option: ");
        }

        public async Task ShowUserMenuAsync()
        {
            Console.WriteLine("\n╔══════════════════════════════════════╗");
            Console.WriteLine("║           USER MENU                  ║");
            Console.WriteLine("╠══════════════════════════════════════╣");
            Console.WriteLine("║   1. My Profile                      ║");
            Console.WriteLine("║   2. Change Password                 ║");
            Console.WriteLine("║   3. Notifications                   ║");
            Console.WriteLine("║   4. Help                            ║");
            Console.WriteLine("║   5. Logout                          ║");
            Console.WriteLine("╚══════════════════════════════════════╝");
            Console.Write("\nSelect option: ");
        }

        public async Task ShowSettingsMenuAsync()
        {
            Console.Clear();
            Console.WriteLine("╔══════════════════════════════════════╗");
            Console.WriteLine("║           SETTINGS                   ║");
            Console.WriteLine("╠══════════════════════════════════════╣");
            Console.WriteLine("║   1. Display Settings                ║");
            Console.WriteLine("║   2. Notification Settings           ║");
            Console.WriteLine("║   3. Privacy Settings                ║");
            Console.WriteLine("║   4. Language                        ║");
            Console.WriteLine("║   5. Back                            ║");
            Console.WriteLine("╚══════════════════════════════════════╝");
            Console.Write("\nSelect option: ");
        }

        public async Task ShowHelpMenuAsync()
        {
            Console.Clear();
            Console.WriteLine("╔══════════════════════════════════════╗");
            Console.WriteLine("║              HELP                    ║");
            Console.WriteLine("╠══════════════════════════════════════╣");
            Console.WriteLine("║   For Assistance:                    ║");
            Console.WriteLine("║   • IT Support: ext. 5555            ║");
            Console.WriteLine("║   • System Admin: ext. 5556          ║");
            Console.WriteLine("║   • Emergency: ext. 0                ║");
            Console.WriteLine("║                                      ║");
            Console.WriteLine("║   1. User Guide                      ║");
            Console.WriteLine("║   2. FAQs                            ║");
            Console.WriteLine("║   3. Contact Support                 ║");
            Console.WriteLine("║   4. Report Issue                    ║");
            Console.WriteLine("║   5. Back                            ║");
            Console.WriteLine("╚══════════════════════════════════════╝");
            Console.Write("\nSelect option: ");
        }

        public void ShowEmergencyInfo()
        {
            Console.Clear();
            Console.WriteLine("╔══════════════════════════════════════╗");
            Console.WriteLine("║        EMERGENCY INFORMATION         ║");
            Console.WriteLine("╠══════════════════════════════════════╣");
            Console.WriteLine("║   In Case of Emergency:              ║");
            Console.WriteLine("║   • Call 911                         ║");
            Console.WriteLine("║   • Hospital Emergency: 555-0100     ║");
            Console.WriteLine("║   • Poison Control: 1-800-222-1222   ║");
            Console.WriteLine("║   • Fire Department: 555-0200        ║");
            Console.WriteLine("║                                      ║");
            Console.WriteLine("║   Emergency Departments:             ║");
            Console.WriteLine("║   • Main ER: Floor 1, West Wing      ║");
            Console.WriteLine("║   • Pediatric ER: Floor 1, East Wing ║");
            Console.WriteLine("║   • Trauma Center: Floor 2           ║");
            Console.WriteLine("╚══════════════════════════════════════╝");
            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey();
        }

        public void ShowHospitalDirectory()
        {
            Console.Clear();
            Console.WriteLine("╔══════════════════════════════════════╗");
            Console.WriteLine("║        HOSPITAL DIRECTORY            ║");
            Console.WriteLine("╠══════════════════════════════════════╣");
            Console.WriteLine("║   Departments:                       ║");
            Console.WriteLine("║   • Cardiology: Floor 3, Room 301-320║");
            Console.WriteLine("║   • Neurology: Floor 4, Room 401-420 ║");
            Console.WriteLine("║   • Orthopedics: Floor 5, Room 501-520║");
            Console.WriteLine("║   • Pediatrics: Floor 2, Room 201-220║");
            Console.WriteLine("║   • ICU: Floor 6                     ║");
            Console.WriteLine("║   • Pharmacy: Floor 1, Main Lobby    ║");
            Console.WriteLine("║   • Lab: Floor 1, East Wing          ║");
            Console.WriteLine("║   • Radiology: Floor 1, West Wing    ║");
            Console.WriteLine("╚══════════════════════════════════════╝");
            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey();
        }
    }
}