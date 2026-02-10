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
            Console.WriteLine("5. Change User Status (Active/Stopped)");
            Console.WriteLine("6. Reset User Password");
            Console.WriteLine("7. Deactivate User");
            Console.WriteLine("8. View User Activity");
            Console.WriteLine("9. Back to Dashboard");
            Console.Write("\nSelect: ");
            
            var choice = Console.ReadLine();
            
            if (choice == "1")
            {
                Console.WriteLine("\n=== CREATE NEW USER ===");
                Console.Write("Username: ");
                var username = Console.ReadLine()?.Trim();
                
                Console.Write("Email: ");
                var email = Console.ReadLine()?.Trim();
                
                Console.Write("Full name: ");
                var fullName = Console.ReadLine()?.Trim();
                
                Console.Write("Password: ");
                var password = Console.ReadLine()?.Trim();
                
                Console.WriteLine("\nSelect Role:");
                Console.WriteLine("1. Admin");
                Console.WriteLine("2. Doctor");
                Console.WriteLine("3. Nurse");
                Console.WriteLine("4. Receptionist");
                Console.WriteLine("5. Accountant");
                Console.WriteLine("6. Patient");
                Console.Write("Role: ");
                var roleChoice = Console.ReadLine()?.Trim();
                
                var role = roleChoice switch
                {
                    "1" => "Admin",
                    "2" => "Doctor",
                    "3" => "Nurse",
                    "4" => "Receptionist",
                    "5" => "Accountant",
                    _ => "Patient"
                };
                
                var success = await _authService.RegisterAsync(username ?? "", email ?? "", password ?? "", fullName ?? "", role);
                
                if (success)
                {
                    // Admin-created users are auto-approved
                    await _authService.ApproveUserAsync(username ?? "");
                    Console.WriteLine($"\nUser '{username}' created and approved successfully!");
                }
                else
                {
                    Console.WriteLine("\nFailed to create user. Username or email may already be in use.");
                }
            }

            if (choice == "2")
            {
                await ApprovePendingUsers();
            }

            if (choice == "5")
            {
                await ChangeUserStatus();
            }

            if (choice == "6")
            {
                await ResetUserPassword();
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

        private async Task ChangeUserStatus()
        {
            Console.Clear();
            Console.WriteLine("=== CHANGE USER STATUS ===\n");
            var users = await _authService.GetAllUsersAsync();
            for (int i = 0; i < users.Count; i++)
            {
                var u = users[i];
                var status = u.IsActive ? "ACTIVE" : "STOPPED";
                Console.WriteLine($"{i + 1}. {u.Username} - {u.FullName} [{status}]");
            }

            Console.Write("\nSelect user number to toggle status (or 0 to cancel): ");
            if (int.TryParse(Console.ReadLine(), out var index) && index > 0 && index <= users.Count)
            {
                var user = users[index - 1];
                var newStatus = !user.IsActive;
                await _authService.UpdateUserStatusAsync(user.Username, newStatus);
                Console.WriteLine($"\nUser '{user.Username}' status changed to {(newStatus ? "ACTIVE" : "STOPPED")}.");
            }
        }

        private async Task ResetUserPassword()
        {
            Console.Clear();
            Console.WriteLine("=== RESET USER PASSWORD ===\n");
            var users = await _authService.GetAllUsersAsync();
            for (int i = 0; i < users.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {users[i].Username} - {users[i].FullName}");
            }

            Console.Write("\nSelect user number (or 0 to cancel): ");
            if (int.TryParse(Console.ReadLine(), out var index) && index > 0 && index <= users.Count)
            {
                var user = users[index - 1];
                Console.Write($"Enter new password for {user.Username}: ");
                var newPassword = Console.ReadLine()?.Trim();
                if (!string.IsNullOrEmpty(newPassword))
                {
                    await _authService.ResetPasswordAsync(user.Username, newPassword);
                    Console.WriteLine("\nPassword reset successfully!");
                }
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
            
            Console.WriteLine("1. Process Pending Transactions");
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
                await ProcessPendingTransactions();
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

            switch (choice)
            {
                case "1":
                    Console.WriteLine("\n=== HOSPITAL STATISTICS ===");
                    var stats = await _dataService.GetDashboardStatsAsync("Admin");
                    Console.WriteLine($"Date: {DateTime.Now:yyyy-MM-dd}");
                    Console.WriteLine($"Total Patients: {stats.TotalPatients:N0}");
                    Console.WriteLine($"Total Appointments Today: {stats.TodayAppointments}");
                    Console.WriteLine($"Occupied Beds: {stats.Stat2Value}");
                    Console.WriteLine($"Staff Count: {stats.Stat1Value}");
                    break;
                case "2":
                    await ShowPatientDemographics();
                    break;
                case "3":
                    await ShowDoctorPerformanceReport();
                    break;
                case "4":
                    await ShowFinancialSummary();
                    break;
                case "5":
                    await ShowInventoryReport();
                    break;
            }
            
            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey();
        }

        private async Task ProcessPendingTransactions()
        {
            Console.Clear();
            Console.WriteLine("=== PROCESS PENDING TRANSACTIONS ===\n");
            var pending = await _dataService.GetTransactionsAsync("Pending");
            if (pending.Count == 0)
            {
                Console.WriteLine("No pending transactions.");
                return;
            }

            for (int i = 0; i < pending.Count; i++)
            {
                var t = pending[i];
                Console.WriteLine($"{i + 1}. ID:{t.TransactionId} | Type:{t.Type} | Amt:${t.Amount:N2} | Date:{t.Date:MM-dd HH:mm}");
                Console.WriteLine($"   Notes: {t.Notes}");
                Console.WriteLine("----------------------------------------------------------");
            }

            Console.Write("\nEnter number to handle (or 0 to cancel): ");
            if (int.TryParse(Console.ReadLine(), out var index) && index > 0 && index <= pending.Count)
            {
                var transaction = pending[index - 1];
                Console.Write("Approve or Reject? (a/r): ");
                var action = Console.ReadLine()?.ToLower();
                if (action == "a")
                {
                    await _dataService.ApproveTransactionAsync(transaction.TransactionId, _session.UserId);
                    Console.WriteLine("\nTransaction APPROVED.");
                }
                else if (action == "r")
                {
                    await _dataService.RejectTransactionAsync(transaction.TransactionId, _session.UserId);
                    Console.WriteLine("\nTransaction REJECTED.");
                }
            }
        }

        private async Task ShowPatientDemographics()
        {
            Console.Clear();
            Console.WriteLine("=== PATIENT DEMOGRAPHICS ===\n");
            var patients = await _dataService.GetPatientsAsync();
            var byGender = patients.GroupBy(p => p.Gender).ToDictionary(g => g.Key, g => g.Count());
            
            Console.WriteLine("By Gender:");
            foreach (var kvp in byGender) Console.WriteLine($"  {kvp.Key}: {kvp.Value}");
            
            Console.WriteLine("\nBy Age Group:");
            Console.WriteLine($"  0-18:  {patients.Count(p => p.Age <= 18)}");
            Console.WriteLine($"  19-60: {patients.Count(p => p.Age > 18 && p.Age <= 60)}");
            Console.WriteLine($"  60+:   {patients.Count(p => p.Age > 60)}");
        }

        private async Task ShowDoctorPerformanceReport()
        {
            Console.Clear();
            Console.WriteLine("=== DOCTOR PERFORMANCE ===\n");
            var doctors = await _dataService.GetDoctorsAsync();
            foreach (var d in doctors)
            {
                var perf = await _dataService.GetDoctorPerformanceAsync(d.DoctorId);
                Console.WriteLine($"{d.Name,-20} | Success: {perf["SuccessRate"]} | Consultations: {perf["MonthlyConsultations"]}");
            }
        }

        private async Task ShowFinancialSummary()
        {
            Console.Clear();
            Console.WriteLine("=== FINANCIAL SUMMARY ===\n");
            var bills = await _dataService.GetAllBillsAsync();
            var payments = await _dataService.GetAllPaymentsAsync();
            Console.WriteLine($"Total Billed:  ${bills.Sum(b => b.Amount):N2}");
            Console.WriteLine($"Total Paid:    ${payments.Sum(p => p.Amount):N2}");
            Console.WriteLine($"Outstanding:   ${bills.Where(b => b.Status != "Paid").Sum(b => b.Amount):N2}");
        }

        private async Task ShowInventoryReport()
        {
            Console.Clear();
            Console.WriteLine("=== INVENTORY REPORT ===\n");
            var items = await _dataService.GetMedicationsAsync();
            Console.WriteLine($"{"Item Name",-20} | {"Stock",-10} | {"Unit"}");
            Console.WriteLine(new string('-', 40));
            foreach (var item in items)
            {
                Console.WriteLine($"{item.Name,-20} | {item.Stock,-10} | {item.Unit}");
            }
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