using System;
using System.Threading.Tasks;

namespace HospitalManagementSystem.Console.Dashboard
{
    public class PatientDashboard : IDashboard
    {
        private readonly int _patientId;
        private readonly string _patientName;

        public PatientDashboard(int patientId, string patientName)
        {
            _patientId = patientId;
            _patientName = patientName;
        }

        public async Task ShowAsync()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine($"=== PATIENT DASHBOARD ===");
                Console.WriteLine($"Welcome, {_patientName}!");
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
            
            Console.WriteLine("\n1. Download Records");
            Console.WriteLine("2. Back to Dashboard");
            Console.Write("\nSelect: ");
            
            Console.ReadKey();
        }
        
        private async Task ViewBills()
        {
            Console.Clear();
            Console.WriteLine("=== BILLS & PAYMENTS ===\n");
            
            Console.WriteLine("Bill No. | Date       | Amount | Status");
            Console.WriteLine("-----------------------------------------");
            Console.WriteLine("B1001    | 2024-01-15 | $150.00 | Paid");
            Console.WriteLine("B1002    | 2024-01-20 | $200.00 | Pending");
            Console.WriteLine("B1003    | 2024-01-25 | $75.00  | Insurance");
            
            Console.WriteLine("\n1. Pay Online");
            Console.WriteLine("2. View Payment History");
            Console.WriteLine("3. Back to Dashboard");
            Console.Write("\nSelect: ");
            
            Console.ReadKey();
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
            
            if (choice == "1")
            {
                Console.Write("Enter new phone number: ");
                var phone = Console.ReadLine();
                Console.WriteLine("Phone number updated successfully!");
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
    }
}