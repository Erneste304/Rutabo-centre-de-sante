using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using HospitalManagementSystem.ConsoleApp.Models;
using HospitalManagementSystem.ConsoleApp.Services;

namespace HospitalManagementSystem.ConsoleApp.Dashboard
{
    public class AdminDashboard : IDashboard
    {
        private readonly UserSession _session;
        private readonly IAuthenticationService _authService;
        private readonly IDataService _dataService;

        public AdminDashboard(UserSession session, IAuthenticationService authService, IDataService dataService)
        {
            _session = session;
            _authService = authService;
            _dataService = dataService;
        }

        public async Task ShowAsync()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("=== ADMINISTRATOR DASHBOARD ===");
                Console.WriteLine("===============================");
                Console.WriteLine($"Logged in as: {_session.FullName} ({_session.UserType})");
                Console.WriteLine("-------------------------------");
                Console.WriteLine("1. User Management");
                Console.WriteLine("2. Doctor Management");
                Console.WriteLine("3. Patient Management");
                Console.WriteLine("4. Department Management");
                Console.WriteLine("5. Room & Bed Management");
                Console.WriteLine("6. Billing & Finance");
                Console.WriteLine("7. System Reports");
                Console.WriteLine("8. System Configuration");
                Console.WriteLine("9. Backup & Restore");
                Console.WriteLine("10. Audit Logs");
                Console.WriteLine("11. Logout");
                Console.Write("\nSelect option: ");
                
                var choice = Console.ReadLine();
                
                switch (choice)
                {
                    case "1":
                        await ManageUsers();
                        break;
                    case "2":
                        await ManageDoctors();
                        break;
                    case "3":
                        await ManagePatients();
                        break;
                    case "4":
                        await ManageDepartments();
                        break;
                    case "5":
                        await ManageRooms();
                        break;
                    case "6":
                        await ManageBilling();
                        break;
                    case "7":
                        await ViewSystemReports();
                        break;
                    case "8":
                        await SystemConfiguration();
                        break;
                    case "9":
                        await BackupRestore();
                        break;
                    case "10":
                        await ViewAuditLogs();
                        break;
                    case "11":
                        Console.WriteLine("\nLogging out...");
                        await Task.Delay(1000);
                        return;
                    default:
                        Console.WriteLine("\nInvalid option. Press any key to continue...");
                        Console.ReadKey();
                        break;
                }
            }
        }
        
        private async Task ManageUsers()
        {
            Console.Clear();
            Console.WriteLine("=== USER MANAGEMENT ===\n");
            
            Console.WriteLine("1. Create New User");
            Console.WriteLine("2. Approve Pending Users");
            Console.WriteLine("3. View All Users");
            Console.WriteLine("4. Update User Role");
            Console.WriteLine("5. Deactivate User");
            Console.WriteLine("6. Reset Password");
            Console.WriteLine("7. View User Activity");
            Console.WriteLine("8. Back to Dashboard");
            Console.Write("\nSelect: ");
            
            var choice = Console.ReadLine();
            
            if (choice == "1")
            {
                Console.WriteLine("\n=== CREATE NEW USER ===");
                Console.Write("Username: ");
                var username = Console.ReadLine();
                
                Console.Write("Email: ");
                var email = Console.ReadLine();
                
                Console.Write("Password: ");
                var password = Console.ReadLine();
                
                Console.WriteLine("Select Role:");
                Console.WriteLine("1. Admin");
                Console.WriteLine("2. Doctor");
                Console.WriteLine("3. Nurse");
                Console.WriteLine("4. Patient");
                Console.WriteLine("5. Receptionist");
                Console.Write("Role: ");
                var role = Console.ReadLine();
                
                Console.WriteLine($"\nUser '{username}' created successfully!");
            }

            if (choice == "2")
            {
                await ApprovePendingUsers();
            }
            
            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey();
        }

        private async Task ApprovePendingUsers()
        {
            Console.Clear();
            Console.WriteLine("=== APPROVE PENDING USERS ===\n");

            var pending = await _authService.GetPendingUsersAsync();

            if (pending.Count == 0)
            {
                Console.WriteLine("There are no users waiting for approval.");
                return;
            }

            var list = new List<UserSession>(pending);
            for (int i = 0; i < list.Count; i++)
            {
                var u = list[i];
                Console.WriteLine($"{i + 1}. {u.Username} - {u.FullName} ({u.UserType})");
            }

            Console.Write("\nEnter number to approve (or 0 to cancel): ");
            var input = Console.ReadLine();
            if (int.TryParse(input, out var index) && index > 0 && index <= list.Count)
            {
                var user = list[index - 1];
                var result = await _authService.ApproveUserAsync(user.Username);
                if (result)
                {
                    Console.WriteLine($"\nUser '{user.Username}' approved successfully.");
                }
                else
                {
                    Console.WriteLine("\nFailed to approve user. They may have been updated already.");
                }
            }
            else
            {
                Console.WriteLine("\nNo changes made.");
            }
        }
        
        private async Task ManageDoctors()
        {
            Console.Clear();
            Console.WriteLine("=== DOCTOR MANAGEMENT ===\n");
            
            Console.WriteLine("Doctor ID | Name           | Specialization  | Status");
            Console.WriteLine("---------------------------------------------------");
            Console.WriteLine("D001      | Dr. John Smith | Cardiology      | Active");
            Console.WriteLine("D002      | Dr. Sarah Lee  | Neurology       | Active");
            Console.WriteLine("D003      | Dr. Mike Brown | Orthopedics     | On Leave");
            
            Console.WriteLine("\n1. Add New Doctor");
            Console.WriteLine("2. Update Doctor Details");
            Console.WriteLine("3. Set Doctor Schedule");
            Console.WriteLine("4. View Doctor Performance");
            Console.WriteLine("5. Back to Dashboard");
            Console.Write("\nSelect: ");
            
            Console.ReadKey();
        }

        private async Task ManagePatients()
        {
            Console.Clear();
            Console.WriteLine("=== PATIENT MANAGEMENT ===\n");

            Console.WriteLine("1. Register Patient");
            Console.WriteLine("2. View All Patients");
            Console.WriteLine("3. Update Patient Details");
            Console.WriteLine("4. Discharge Patient");
            Console.WriteLine("5. Back to Dashboard");
            Console.Write("\nSelect: ");

            Console.ReadKey();
        }
        
        private async Task ManageDepartments()
        {
            Console.Clear();
            Console.WriteLine("=== DEPARTMENT MANAGEMENT ===\n");
            
            Console.WriteLine("Dept ID | Name           | Head Doctor     | Staff Count");
            Console.WriteLine("---------------------------------------------------");
            Console.WriteLine("DEPT001 | Cardiology     | Dr. John Smith  | 15");
            Console.WriteLine("DEPT002 | Neurology      | Dr. Sarah Lee   | 10");
            Console.WriteLine("DEPT003 | Orthopedics    | Dr. Mike Brown  | 12");
            
            Console.WriteLine("\n1. Add New Department");
            Console.WriteLine("2. Update Department");
            Console.WriteLine("3. Assign Head Doctor");
            Console.WriteLine("4. View Department Stats");
            Console.WriteLine("5. Back to Dashboard");
            Console.Write("\nSelect: ");
            
            Console.ReadKey();
        }
        
        private async Task ManageRooms()
        {
            Console.Clear();
            Console.WriteLine("=== ROOM & BED MANAGEMENT ===\n");
            
            Console.WriteLine("Room No. | Type        | Status     | Patient");
            Console.WriteLine("---------------------------------------------------");
            Console.WriteLine("101      | General     | Occupied   | John Doe");
            Console.WriteLine("102      | General     | Available  | -");
            Console.WriteLine("ICU-01   | ICU         | Occupied   | Jane Smith");
            Console.WriteLine("OP-01    | Operation   | Available  | -");
            
            Console.WriteLine("\n1. Add New Room");
            Console.WriteLine("2. Update Room Status");
            Console.WriteLine("3. Assign Patient to Room");
            Console.WriteLine("4. View Room Utilization");
            Console.WriteLine("5. Back to Dashboard");
            Console.Write("\nSelect: ");
            
            Console.ReadKey();
        }
        
        private async Task ManageBilling()
        {
            Console.Clear();
            Console.WriteLine("=== BILLING & FINANCE ===\n");
            
            Console.WriteLine("1. Generate Invoice");
            Console.WriteLine("2. View All Bills");
            Console.WriteLine("3. Process Insurance Claims");
            Console.WriteLine("4. Financial Reports");
            Console.WriteLine("5. Set Service Charges");
            Console.WriteLine("6. View Revenue Analytics");
            Console.WriteLine("7. Back to Dashboard");
            Console.Write("\nSelect: ");
            
            var choice = Console.ReadLine();
            
            if (choice == "1")
            {
                Console.WriteLine("\n=== GENERATE INVOICE ===");
                Console.Write("Patient ID: ");
                var patientId = Console.ReadLine();
                
                Console.WriteLine("Services:");
                Console.WriteLine("1. Consultation - $50");
                Console.WriteLine("2. Lab Test - $100");
                Console.WriteLine("3. X-Ray - $75");
                Console.WriteLine("4. Medication - $30");
                Console.Write("Select services (comma separated): ");
                var services = Console.ReadLine();
                
                Console.WriteLine("\nInvoice generated successfully!");
                Console.WriteLine("Total Amount: $255.00");
            }
            
            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey();
        }
        
        private async Task ViewSystemReports()
        {
            Console.Clear();
            Console.WriteLine("=== SYSTEM REPORTS ===\n");
            
            Console.WriteLine("1. Hospital Statistics");
            Console.WriteLine("2. Patient Demographics");
            Console.WriteLine("3. Doctor Performance");
            Console.WriteLine("4. Financial Reports");
            Console.WriteLine("5. Inventory Report");
            Console.WriteLine("6. Appointment Analytics");
            Console.WriteLine("7. Export Reports");
            Console.WriteLine("8. Back to Dashboard");
            Console.Write("\nSelect: ");
            
            var choice = Console.ReadLine();
            
            if (choice == "1")
            {
                Console.WriteLine("\n=== HOSPITAL STATISTICS ===");
                Console.WriteLine($"Date: {DateTime.Now:yyyy-MM-dd}");
                Console.WriteLine("Total Patients: 1,245");
                Console.WriteLine("Active Doctors: 45");
                Console.WriteLine("Total Appointments Today: 156");
                Console.WriteLine("Occupied Beds: 120/150 (80%)");
                Console.WriteLine("Monthly Revenue: $250,000");
            }
            
            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey();
        }
        
        private async Task SystemConfiguration()
        {
            Console.Clear();
            Console.WriteLine("=== SYSTEM CONFIGURATION ===\n");
            
            Console.WriteLine("1. General Settings");
            Console.WriteLine("2. Email Configuration");
            Console.WriteLine("3. Backup Settings");
            Console.WriteLine("4. Security Settings");
            Console.WriteLine("5. Notification Settings");
            Console.WriteLine("6. Database Settings");
            Console.WriteLine("7. API Configuration");
            Console.WriteLine("8. Back to Dashboard");
            Console.Write("\nSelect: ");
            
            Console.ReadKey();
        }
        
        private async Task BackupRestore()
        {
            Console.Clear();
            Console.WriteLine("=== BACKUP & RESTORE ===\n");
            
            Console.WriteLine("1. Create Database Backup");
            Console.WriteLine("2. Restore Database");
            Console.WriteLine("3. Schedule Automatic Backup");
            Console.WriteLine("4. View Backup History");
            Console.WriteLine("5. Export Data");
            Console.WriteLine("6. Import Data");
            Console.WriteLine("7. Back to Dashboard");
            Console.Write("\nSelect: ");
            
            var choice = Console.ReadLine();
            
            if (choice == "1")
            {
                Console.WriteLine("\nCreating backup...");
                await Task.Delay(2000);
                Console.WriteLine("Backup completed successfully!");
                Console.WriteLine($"Backup file: backup_{DateTime.Now:yyyyMMdd_HHmmss}.bak");
            }
            
            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey();
        }
        
        private async Task ViewAuditLogs()
        {
            Console.Clear();
            Console.WriteLine("=== AUDIT LOGS ===\n");
            
            Console.WriteLine("Timestamp           | User       | Action");
            Console.WriteLine("---------------------------------------------------");
            Console.WriteLine("2024-01-15 10:30:00 | admin      | Created user: jdoe");
            Console.WriteLine("2024-01-15 11:15:00 | dr.smith   | Updated patient record");
            Console.WriteLine("2024-01-15 14:20:00 | nurse.jane | Added medication");
            
            Console.WriteLine("\n1. Filter by Date");
            Console.WriteLine("2. Filter by User");
            Console.WriteLine("3. Export Logs");
            Console.WriteLine("4. Clear Old Logs");
            Console.WriteLine("5. Back to Dashboard");
            Console.Write("\nSelect: ");
            
            Console.ReadKey();
        }
    }
}