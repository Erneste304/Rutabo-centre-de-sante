using System;
using System.IO;
using System.Threading.Tasks;
using HospitalManagementSystem.ConsoleApp.Models;
using HospitalManagementSystem.ConsoleApp.Services;

namespace HospitalManagementSystem.ConsoleApp.Dashboard
{
    public class PatientDashboard : IDashboard
    {
        private readonly UserSession _session;
        private readonly IDataService _dataService;
        private readonly IAuthenticationService _authService;

        public PatientDashboard(UserSession session, IDataService dataService, IAuthenticationService authService)
        {
            _session = session;
            _dataService = dataService;
            _authService = authService;
        }

        public async Task ShowAsync()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine($"=== PATIENT DASHBOARD ===");
                Console.WriteLine($"Welcome, {_session.FullName}!");
                Console.WriteLine("==========================");
                Console.WriteLine("1. View My Appointments");
                Console.WriteLine("2. Book New Appointment");
                Console.WriteLine("3. View Medical Records");
                Console.WriteLine("4. View Bills & Payments");
                Console.WriteLine("5. Update Profile");
                Console.WriteLine("6. View Doctors");
                Console.WriteLine("7. Emergency Contact");
                Console.WriteLine("8. Logout");
                Console.Write("\nSelect option: ");
                
                var choice = Console.ReadLine();
                
                switch (choice)
                {
                    case "1":
                        await ViewAppointments();
                        break;
                    case "2":
                        await BookAppointment();
                        break;
                    case "3":
                        await ViewMedicalRecords();
                        break;
                    case "4":
                        await ViewBills();
                        break;
                    case "5":
                        await UpdateProfile();
                        break;
                    case "6":
                        await ViewDoctors();
                        break;
                    case "7":
                        ViewEmergencyContact();
                        break;
                    case "8":
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
        
        private async Task ViewAppointments()
        {
            Console.Clear();
            Console.WriteLine("=== MY APPOINTMENTS ===\n");
            
            // Simulated data - Replace with actual database calls
            Console.WriteLine("1. Dr. Smith - Cardiology - 2024-01-15 10:00 AM [Confirmed]");
            Console.WriteLine("2. Dr. Johnson - General Checkup - 2024-01-20 02:00 PM [Pending]");
            Console.WriteLine("3. Dr. Williams - Follow-up - 2024-01-25 11:30 AM [Completed]");
            
            Console.WriteLine("\n1. Cancel Appointment");
            Console.WriteLine("2. Reschedule Appointment");
            Console.WriteLine("3. Back to Dashboard");
            Console.Write("\nSelect: ");
            
            var choice = Console.ReadLine();
            
            if (choice == "1")
            {
                Console.Write("Enter appointment number to cancel: ");
                var apptNum = Console.ReadLine();
                Console.WriteLine($"Appointment {apptNum} cancelled successfully!");
            }
            
            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey();
        }
        
        private async Task BookAppointment()
        {
            Console.Clear();
            Console.WriteLine("=== BOOK APPOINTMENT ===\n");
            
            Console.WriteLine("Available Specializations:");
            Console.WriteLine("1. Cardiology");
            Console.WriteLine("2. Neurology");
            Console.WriteLine("3. Orthopedics");
            Console.WriteLine("4. Pediatrics");
            Console.WriteLine("5. General Medicine");
            
            Console.Write("\nSelect specialization (1-5): ");
            var specialization = Console.ReadLine();
            
            Console.Write("Enter preferred date (YYYY-MM-DD): ");
            var date = Console.ReadLine();
            
            Console.Write("Enter symptoms/reason: ");
            var reason = Console.ReadLine();
            
            Console.WriteLine("\nAppointment request submitted!");
            Console.WriteLine("You will receive confirmation via email.");
            
            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey();
        }
        
        private async Task ViewMedicalRecords()
        {
            Console.Clear();
            Console.WriteLine("=== MEDICAL RECORDS ===\n");
            
            Console.WriteLine("Date         | Doctor           | Diagnosis");
            Console.WriteLine("---------------------------------------------");
            Console.WriteLine("2024-01-10   | Dr. Smith        | Hypertension");
            Console.WriteLine("2023-12-05   | Dr. Johnson      | Annual Checkup");
            Console.WriteLine("2023-11-20   | Dr. Williams     | Flu Treatment");

            var hasPaid = await _dataService.HasPaidBillAsync(_session.UserId);
            var isApproved = await _dataService.IsRecordRequestApprovedAsync(_session.UserId);

            Console.WriteLine("\nPayment status : " + (hasPaid ? "OK (at least one bill is paid)" : "No paid bills found"));
            Console.WriteLine("Doctor approval: " + (isApproved ? "Approved" : "Not yet approved"));
            
            Console.WriteLine("\n1. Download Medical Records");
            Console.WriteLine("2. Request Doctor Approval");
            Console.WriteLine("3. Back to Dashboard");
            Console.Write("\nSelect: ");
            
            var choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    await DownloadMedicalRecords(hasPaid, isApproved);
                    break;
                case "2":
                    await RequestMedicalRecordApproval(hasPaid);
                    break;
                default:
                    break;
            }
        }
        
        private async Task ViewBills()
        {
            Console.Clear();
            Console.WriteLine("=== BILLS & PAYMENTS ===\n");

            var bills = await _dataService.GetBillsForPatientAsync(_session.UserId);

            Console.WriteLine("Bill ID | Date       | Amount   | Status");
            Console.WriteLine("-----------------------------------------");
            foreach (var bill in bills)
            {
                Console.WriteLine($"{bill.BillId,-7} | {bill.Date:yyyy-MM-dd} | {bill.Amount,7:C} | {bill.Status}");
            }
            
            Console.WriteLine("\n1. Pay Online");
            Console.WriteLine("2. View Payment History");
            Console.WriteLine("3. Back to Dashboard");
            Console.Write("\nSelect: ");
            
            var choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    await PayOnline(bills);
                    break;
                case "2":
                    await ViewPaymentHistory();
                    break;
                default:
                    break;
            }
        }
        
        private async Task UpdateProfile()
        {
            Console.Clear();
            Console.WriteLine("=== UPDATE PROFILE ===\n");
            
            Console.WriteLine("1. Change Phone Number");
            Console.WriteLine("2. Update Address");
            Console.WriteLine("3. Change Password");
            Console.WriteLine("4. Update Emergency Contact");
            Console.WriteLine("5. Back to Dashboard");
            Console.Write("\nSelect: ");
            
            var choice = Console.ReadLine();
            
            switch (choice)
            {
                case "1":
                    Console.Write("Enter new phone number: ");
                    _session.PhoneNumber = Console.ReadLine();
                    Console.WriteLine("Phone number updated successfully!");
                    break;
                case "2":
                    Console.Write("Enter new address: ");
                    var address = Console.ReadLine();
                    if (string.IsNullOrWhiteSpace(address))
                    {
                        Console.WriteLine("Address cannot be empty.");
                    }
                    else
                    {
                        _session.Address = address;
                        Console.WriteLine("Address updated successfully!");
                    }
                    break;
                case "3":
                    await ChangePassword();
                    break;
                case "4":
                    Console.Write("Emergency contact name: ");
                    _session.EmergencyContactName = Console.ReadLine();
                    Console.Write("Emergency contact phone: ");
                    _session.EmergencyContactPhone = Console.ReadLine();
                    Console.WriteLine("Emergency contact updated successfully!");
                    break;
                default:
                    break;
            }
            
            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey();
        }
        
        private async Task ViewDoctors()
        {
            Console.Clear();
            Console.WriteLine("=== AVAILABLE DOCTORS ===\n");
            
            Console.WriteLine("Doctor Name      | Specialization   | Availability");
            Console.WriteLine("---------------------------------------------------");
            Console.WriteLine("Dr. John Smith   | Cardiology       | Mon-Fri, 9AM-5PM");
            Console.WriteLine("Dr. Sarah Johnson| Neurology        | Tue-Thu, 10AM-4PM");
            Console.WriteLine("Dr. Mike Williams| Orthopedics      | Mon-Wed-Fri, 8AM-3PM");
            Console.WriteLine("Dr. Lisa Brown   | Pediatrics       | Mon-Fri, 9AM-6PM");
            
            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey();
        }
        
        private void ViewEmergencyContact()
        {
            Console.Clear();
            Console.WriteLine("=== EMERGENCY CONTACT ===\n");
            Console.WriteLine("Hospital Emergency: 911");
            Console.WriteLine("Ambulance Service: 123-456-7890");
            Console.WriteLine("Poison Control: 1-800-222-1222");
            Console.WriteLine("24/7 Helpline: 1-800-HOSPITAL");
            
            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey();
        }

        private async Task DownloadMedicalRecords(bool hasPaid, bool isApproved)
        {
            if (!hasPaid)
            {
                Console.WriteLine("\nYou must have at least one paid bill before downloading your medical records.");
                Console.WriteLine("Please go to 'Bills & Payments' and use 'Pay Online' first.");
            }
            else if (!isApproved)
            {
                Console.WriteLine("\nYour doctor has not yet approved your medical record download request.");
                Console.WriteLine("Please choose 'Request Doctor Approval' or wait until your doctor approves.");
            }
            else
            {
                var fileName = $"MedicalRecords_{_session.UserId}_{DateTime.Now:yyyyMMdd_HHmmss}.txt";
                var content = $"Medical Records for {_session.FullName} (ID: {_session.UserId}){Environment.NewLine}" +
                              "--------------------------------------------------------" + Environment.NewLine +
                              "2024-01-10 - Dr. Smith    - Hypertension - Medication & lifestyle changes" + Environment.NewLine +
                              "2023-12-05 - Dr. Johnson  - Annual Checkup - Normal" + Environment.NewLine +
                              "2023-11-20 - Dr. Williams - Flu Treatment - Recovered";

                File.WriteAllText(fileName, content);
                Console.WriteLine($"\nMedical records downloaded to file: {Path.GetFullPath(fileName)}");
            }

            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey();
        }

        private async Task RequestMedicalRecordApproval(bool hasPaid)
        {
            if (!hasPaid)
            {
                Console.WriteLine("\nYou must have at least one paid bill before requesting doctor approval.");
                Console.WriteLine("Please go to 'Bills & Payments' and use 'Pay Online' first.");
            }
            else
            {
                var request = await _dataService.CreateMedicalRecordRequestAsync(_session.UserId, _session.FullName);
                Console.WriteLine($"\nRequest #{request.RequestId} sent to your doctor for approval.");
                Console.WriteLine("After your doctor approves, you will be able to download your medical records.");
            }

            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey();
        }

        private async Task PayOnline(System.Collections.Generic.IReadOnlyCollection<Bill> bills)
        {
            Console.Clear();
            Console.WriteLine("=== PAY ONLINE ===\n");

            var unpaidBills = bills.Where(b => !string.Equals(b.Status, "Paid", StringComparison.OrdinalIgnoreCase)).ToList();
            if (unpaidBills.Count == 0)
            {
                Console.WriteLine("You have no unpaid bills.");
                Console.WriteLine("\nPress any key to continue...");
                Console.ReadKey();
                return;
            }

            Console.WriteLine("Unpaid Bills:");
            foreach (var bill in unpaidBills)
            {
                Console.WriteLine($"{bill.BillId}. {bill.Date:yyyy-MM-dd} - {bill.Amount:C} ({bill.Status})");
            }

            Console.Write("\nEnter Bill ID to pay: ");
            var input = Console.ReadLine();
            if (!int.TryParse(input, out var billId))
            {
                Console.WriteLine("Invalid Bill ID.");
                Console.WriteLine("\nPress any key to continue...");
                Console.ReadKey();
                return;
            }

            var selectedBill = unpaidBills.FirstOrDefault(b => b.BillId == billId);
            if (selectedBill == null)
            {
                Console.WriteLine("Bill not found.");
                Console.WriteLine("\nPress any key to continue...");
                Console.ReadKey();
                return;
            }

            Console.WriteLine("\nSelect payment method:");
            Console.WriteLine("1. MTN Mobile Money");
            Console.WriteLine("2. Tigo Pesa");
            Console.WriteLine("3. Virtual Card");
            Console.Write("Method: ");
            var methodChoice = Console.ReadLine();

            string methodName = methodChoice switch
            {
                "2" => "Tigo Pesa",
                "3" => "Virtual Card",
                _ => "MTN Mobile Money"
            };

            if (methodName == "Virtual Card")
            {
                Console.Write("Card Number: ");
                var card = Console.ReadLine();
                Console.Write("Expiry (MM/YY): ");
                var expiry = Console.ReadLine();
                Console.Write("CVV: ");
                var cvv = Console.ReadLine();
            }
            else
            {
                Console.Write("Mobile Number: ");
                var phone = Console.ReadLine();
            }

            Console.WriteLine("\nProcessing payment...");
            await Task.Delay(1500);

            var reference = $"PAY-{DateTime.Now:yyyyMMddHHmmss}";
            var success = await _dataService.PayBillAsync(selectedBill.BillId, _session.UserId, selectedBill.Amount, methodName, reference);

            if (success)
            {
                Console.WriteLine($"\nPayment successful via {methodName}.");
                Console.WriteLine($"Reference: {reference}");
            }
            else
            {
                Console.WriteLine("\nPayment failed. Please try again.");
            }

            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey();
        }

        private async Task ViewPaymentHistory()
        {
            Console.Clear();
            Console.WriteLine("=== PAYMENT HISTORY ===\n");

            var payments = await _dataService.GetPaymentsForPatientAsync(_session.UserId);

            if (payments.Count == 0)
            {
                Console.WriteLine("No payments found.");
                Console.WriteLine("\nPress any key to continue...");
                Console.ReadKey();
                return;
            }

            Console.WriteLine("Payment ID | Date       | Amount   | Method          | Reference");
            Console.WriteLine("----------------------------------------------------------------");
            foreach (var p in payments)
            {
                Console.WriteLine($"{p.PaymentId,-10} | {p.Date:yyyy-MM-dd} | {p.Amount,7:C} | {p.Method,-14} | {p.Reference}");
            }

            Console.WriteLine("\n1. Download Payment History (All)");
            Console.WriteLine("2. Back to Dashboard");
            Console.Write("\nSelect: ");
            var choice = Console.ReadLine();

            if (choice == "1")
            {
                var fileName = $"PaymentHistory_{_session.UserId}_{DateTime.Now:yyyyMMdd_HHmmss}.txt";
                using var writer = new StreamWriter(fileName);
                writer.WriteLine($"Payment History for {_session.FullName} (ID: {_session.UserId})");
                writer.WriteLine("------------------------------------------------------------");
                foreach (var p in payments)
                {
                    writer.WriteLine($"{p.Date:yyyy-MM-dd} - {p.Amount:C} via {p.Method} (Ref: {p.Reference})");
                }

                Console.WriteLine($"\nPayment history downloaded to file: {Path.GetFullPath(fileName)}");
            }

            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey();
        }

        private async Task ChangePassword()
        {
            Console.Write("Current password: ");
            var oldPwd = Console.ReadLine();
            Console.Write("New password: ");
            var newPwd = Console.ReadLine();
            Console.Write("Confirm new password: ");
            var confirmPwd = Console.ReadLine();

            if (string.IsNullOrEmpty(newPwd) || newPwd != confirmPwd)
            {
                Console.WriteLine("New passwords do not match or are empty.");
                return;
            }

            var changed = await _authService.ChangePasswordAsync(_session.UserId, oldPwd ?? string.Empty, newPwd);
            Console.WriteLine(changed 
                ? "Password changed successfully!" 
                : "Failed to change password. Please check your current password.");
        }
    }
}