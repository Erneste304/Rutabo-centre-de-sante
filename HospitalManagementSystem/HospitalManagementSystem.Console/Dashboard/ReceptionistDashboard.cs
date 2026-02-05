using System;
using System.Threading.Tasks;
using HospitalManagementSystem.ConsoleApp.Models;
using HospitalManagementSystem.ConsoleApp.Services;

namespace HospitalManagementSystem.ConsoleApp.Dashboard
{
    public class ReceptionistDashboard : IDashboard
    {
        private readonly UserSession _session;
        private readonly IDataService _dataService;

        public ReceptionistDashboard(UserSession session, IDataService dataService)
        {
            _session = session;
            _dataService = dataService;
        }

        public async Task ShowAsync()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine($"=== RECEPTIONIST DASHBOARD ===");
                Console.WriteLine($"Welcome, {_session.FullName}!");
                Console.WriteLine("===============================");
                Console.WriteLine("1. Patient Registration");
                Console.WriteLine("2. Appointment Scheduling");
                Console.WriteLine("3. Check-in/Check-out");
                Console.WriteLine("4. Billing & Payments");
                Console.WriteLine("5. Insurance Verification");
                Console.WriteLine("6. Phone & Reception");
                Console.WriteLine("7. Visitor Management");
                Console.WriteLine("8. Doctor Schedule");
                Console.WriteLine("9. Reports & Analytics");
                Console.WriteLine("10. Emergency Contacts");
                Console.WriteLine("11. Logout");
                Console.Write("\nSelect option: ");
                
                var choice = Console.ReadLine();
                
                switch (choice)
                {
                    case "1":
                        await PatientRegistration();
                        break;
                    case "2":
                        await AppointmentScheduling();
                        break;
                    case "3":
                        await CheckInCheckOut();
                        break;
                    case "4":
                        await BillingAndPayments();
                        break;
                    case "5":
                        await InsuranceVerification();
                        break;
                    case "6":
                        await PhoneAndReception();
                        break;
                    case "7":
                        await VisitorManagement();
                        break;
                    case "8":
                        await ViewDoctorSchedule();
                        break;
                    case "9":
                        await ReportsAndAnalytics();
                        break;
                    case "10":
                        await EmergencyContacts();
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
        
        private async Task PatientRegistration()
        {
            Console.Clear();
            Console.WriteLine("=== PATIENT REGISTRATION ===\n");
            
            Console.Write("First Name: ");
            var firstName = Console.ReadLine();
            
            Console.Write("Last Name: ");
            var lastName = Console.ReadLine();
            
            Console.Write("Date of Birth (YYYY-MM-DD): ");
            var dob = Console.ReadLine();
            
            Console.Write("Gender (M/F/Other): ");
            var gender = Console.ReadLine();
            
            Console.Write("Phone Number: ");
            var phone = Console.ReadLine();
            
            Console.Write("Email: ");
            var email = Console.ReadLine();
            
            Console.Write("Address: ");
            var address = Console.ReadLine();
            
            Console.Write("Emergency Contact Name: ");
            var emergencyName = Console.ReadLine();
            
            Console.Write("Emergency Contact Phone: ");
            var emergencyPhone = Console.ReadLine();
            
            Console.Write("Insurance Provider: ");
            var insurance = Console.ReadLine();
            
            Console.Write("Insurance ID: ");
            var insuranceId = Console.ReadLine();
            
            Console.WriteLine($"\nPatient {firstName} {lastName} registered successfully!");
            Console.WriteLine($"Patient ID: PAT{DateTime.Now:yyMMddHHmmss}");
            Console.WriteLine($"Registration Date: {DateTime.Now:yyyy-MM-dd}");
            
            Console.WriteLine("\n1. Print Registration Form");
            Console.WriteLine("2. Email Confirmation");
            Console.WriteLine("3. Back to dashboard");
            Console.Write("\nSelect: ");
            
            Console.ReadKey();
        }
        
        private async Task AppointmentScheduling()
        {
            Console.Clear();
            Console.WriteLine("=== APPOINTMENT SCHEDULING ===\n");
            
            Console.Write("Patient ID/Name: ");
            var patient = Console.ReadLine();
            
            var doctors = await _dataService.GetDoctorsAsync();
            Console.WriteLine("\nAvailable Doctors:");
            int index = 1;
            foreach (var doctor in doctors)
            {
                var availability = doctor.IsAvailable ? "Available" : "Not Available";
                Console.WriteLine($"{index}. {doctor.Name} - {doctor.Specialization} [{availability}]");
                index++;
            }
            
            Console.Write("\nSelect doctor (1-4): ");
            var doctorChoice = Console.ReadLine();
            
            Console.Write("Preferred Date (YYYY-MM-DD): ");
            var date = Console.ReadLine();
            
            Console.WriteLine("Available Time Slots:");
            Console.WriteLine("1. 09:00 AM");
            Console.WriteLine("2. 10:30 AM");
            Console.WriteLine("3. 02:00 PM");
            Console.WriteLine("4. 04:00 PM");
            
            Console.Write("\nSelect time slot: ");
            var timeSlot = Console.ReadLine();
            
            Console.Write("Reason for visit: ");
            var reason = Console.ReadLine();
            
            Console.Write("Referral Doctor (if any): ");
            var referral = Console.ReadLine();
            
            Console.WriteLine($"\nAppointment scheduled successfully!");
            Console.WriteLine($"Appointment ID: APP{DateTime.Now:yyMMddHHmmss}");
            Console.WriteLine($"Confirmation sent to patient.");
            
            Console.WriteLine("\n1. Print Appointment Card");
            Console.WriteLine("2. Send SMS Reminder");
            Console.WriteLine("3. Back to dashboard");
            Console.Write("\nSelect: ");
            
            Console.ReadKey();
        }
        
        private async Task CheckInCheckOut()
        {
            Console.Clear();
            Console.WriteLine("=== PATIENT CHECK-IN/CHECK-OUT ===\n");
            
            Console.WriteLine("Waiting Patients:");
            Console.WriteLine("1. John Doe - Appointment: 09:00 AM - Dr. Smith");
            Console.WriteLine("2. Jane Smith - Appointment: 10:30 AM - Dr. Jones");
            Console.WriteLine("3. Robert Johnson - Walk-in - Emergency");
            
            Console.WriteLine("\n1. Check-in Patient");
            Console.WriteLine("2. Check-out Patient");
            Console.WriteLine("3. View Waiting List");
            Console.WriteLine("4. Update Patient Status");
            Console.WriteLine("5. Back to dashboard");
            Console.Write("\nSelect: ");
            
            var choice = Console.ReadLine();
            
            if (choice == "1")
            {
                Console.Write("Enter Patient ID/Name: ");
                var patient = Console.ReadLine();
                Console.WriteLine($"{patient} checked in successfully!");
                Console.WriteLine("Waiting area: Main Lobby");
            }
            else if (choice == "2")
            {
                Console.Write("Enter Patient ID/Name: ");
                var patient = Console.ReadLine();
                Console.WriteLine($"{patient} checked out successfully!");
                Console.WriteLine("Discharge instructions provided.");
            }
            
            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey();
        }
        
        private async Task BillingAndPayments()
        {
            Console.Clear();
            Console.WriteLine("=== BILLING & PAYMENTS ===\n");
            
            Console.WriteLine("Pending Bills:");
            Console.WriteLine("Bill No. | Patient         | Amount | Due Date");
            Console.WriteLine("------------------------------------------------");
            Console.WriteLine("B1001    | John Doe        | $150.00| 2024-01-31");
            Console.WriteLine("B1002    | Jane Smith      | $200.00| 2024-01-31");
            Console.WriteLine("B1003    | Robert Johnson  | $75.00 | 2024-02-05");
            
            Console.WriteLine("\n1. Generate New Bill");
            Console.WriteLine("2. Process Payment");
            Console.WriteLine("3. Print Receipt");
            Console.WriteLine("4. View Payment History");
            Console.WriteLine("5. Insurance Claims");
            Console.WriteLine("6. Back to dashboard");
            Console.Write("\nSelect: ");
            
            var choice = Console.ReadLine();
            
            if (choice == "2")
            {
                Console.Write("Enter Bill Number: ");
                var billNo = Console.ReadLine();
                
                Console.Write("Payment Amount: $");
                var amount = Console.ReadLine();
                
                Console.WriteLine("Payment Method:");
                Console.WriteLine("1. Cash");
                Console.WriteLine("2. Credit Card");
                Console.WriteLine("3. Debit Card");
                Console.WriteLine("4. Insurance");
                Console.Write("Select: ");
                var method = Console.ReadLine();
                
                Console.WriteLine($"Payment of ${amount} processed successfully!");
                Console.WriteLine("Receipt printed.");
            }
            
            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey();
        }
        
        private async Task InsuranceVerification()
        {
            Console.Clear();
            Console.WriteLine("=== INSURANCE VERIFICATION ===\n");
            
            Console.Write("Enter Insurance ID or Patient Name: ");
            var input = Console.ReadLine();
            
            Console.WriteLine("\nInsurance Details:");
            Console.WriteLine("Patient: John Doe");
            Console.WriteLine("Insurance Provider: Blue Cross");
            Console.WriteLine("Policy Number: BC123456789");
            Console.WriteLine("Coverage: 80%");
            Console.WriteLine("Valid Until: 2024-12-31");
            Console.WriteLine("Deductible Met: Yes");
            Console.WriteLine("Co-pay: $20");
            
            Console.WriteLine("\n1. Verify Eligibility");
            Console.WriteLine("2. Submit Claim");
            Console.WriteLine("3. Check Claim Status");
            Console.WriteLine("4. Update Insurance Info");
            Console.WriteLine("5. Back to dashboard");
            Console.Write("\nSelect: ");
            
            Console.ReadKey();
        }
        
        private async Task PhoneAndReception()
        {
            Console.Clear();
            Console.WriteLine("=== PHONE & RECEPTION ===\n");
            
            Console.WriteLine("Call Log:");
            Console.WriteLine("Time     | Caller           | Purpose");
            Console.WriteLine("----------------------------------------");
            Console.WriteLine("09:15 AM | Mrs. Davis       | Appointment");
            Console.WriteLine("10:30 AM | Dr. Smith Office | Referral");
            Console.WriteLine("11:45 AM | Pharmacy         | Prescription");
            Console.WriteLine("02:20 PM | Insurance Co.    | Verification");
            
            Console.WriteLine("\nQuick Dial:");
            Console.WriteLine("1. Emergency Department");
            Console.WriteLine("2. Doctor's Office");
            Console.WriteLine("3. Pharmacy");
            Console.WriteLine("4. Billing Department");
            Console.WriteLine("5. Hospital Administration");
            Console.WriteLine("6. Make External Call");
            
            Console.WriteLine("\n1. Log New Call");
            Console.WriteLine("2. Transfer Call");
            Console.WriteLine("3. Take Message");
            Console.WriteLine("4. Back to dashboard");
            Console.Write("\nSelect: ");
            
            var choice = Console.ReadLine();
            
            if (choice == "1")
            {
                Console.Write("Caller Name: ");
                var caller = Console.ReadLine();
                
                Console.Write("Phone Number: ");
                var phone = Console.ReadLine();
                
                Console.Write("Purpose: ");
                var purpose = Console.ReadLine();
                
                Console.Write("Message: ");
                var message = Console.ReadLine();
                
                Console.WriteLine("Call logged successfully!");
            }
            
            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey();
        }
        
        private async Task VisitorManagement()
        {
            Console.Clear();
            Console.WriteLine("=== VISITOR MANAGEMENT ===\n");
            
            Console.WriteLine("Current Visitors:");
            Console.WriteLine("Visitor Name    | Patient        | Room | Check-in Time");
            Console.WriteLine("--------------------------------------------------------");
            Console.WriteLine("Mary Johnson    | John Doe       | 101  | 10:30 AM");
            Console.WriteLine("Tom Davis       | Jane Smith     | 102  | 11:15 AM");
            Console.WriteLine("Sarah Wilson    | Robert Johnson | ICU-01 | 02:00 PM");
            
            Console.WriteLine("\nVisitor Rules:");
            Console.WriteLine("- Visiting Hours: 10:00 AM - 8:00 PM");
            Console.WriteLine("- Maximum 2 visitors per patient");
            Console.WriteLine("- Children under 12 must be supervised");
            Console.WriteLine("- No flowers in ICU");
            Console.WriteLine("- Masks required in certain areas");
            
            Console.WriteLine("\n1. Register New Visitor");
            Console.WriteLine("2. Check-out Visitor");
            Console.WriteLine("3. Print Visitor Pass");
            Console.WriteLine("4. Back to dashboard");
            Console.Write("\nSelect: ");
            
            var choice = Console.ReadLine();
            
            if (choice == "1")
            {
                Console.Write("Visitor Name: ");
                var visitor = Console.ReadLine();
                
                Console.Write("Patient Visiting: ");
                var patient = Console.ReadLine();
                
                Console.Write("Relationship: ");
                var relationship = Console.ReadLine();
                
                Console.Write("Phone Number: ");
                var phone = Console.ReadLine();
                
                Console.WriteLine($"\nVisitor {visitor} registered!");
                Console.WriteLine("Visitor Pass issued.");
            }
            
            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey();
        }
        
        private async Task ViewDoctorSchedule()
        {
            Console.Clear();
            Console.WriteLine("=== DOCTOR SCHEDULE ===\n");
            
            var doctors = await _dataService.GetDoctorsAsync();
            
            Console.WriteLine("Doctor           | Monday         | Tuesday        | Wednesday");
            Console.WriteLine("---------------------------------------------------------------");
            Console.WriteLine("Dr. John Smith   | 9-5 (Cardio)   | 9-5 (Cardio)   | Surgery");
            Console.WriteLine("Dr. Sarah Jones  | Research       | 10-4 (Neuro)   | 10-4 (Neuro)");
            Console.WriteLine("Dr. Mike Brown   | 8-3 (Ortho)    | Surgery        | 8-3 (Ortho)");
            Console.WriteLine("Dr. Lisa Wang    | 9-6 (Peds)     | 9-6 (Peds)     | Admin");
            
            Console.WriteLine("\nOn-Call Schedule:");
            Console.WriteLine("Today (Night): Dr. John Smith");
            Console.WriteLine("Tomorrow: Dr. Sarah Jones");
            Console.WriteLine("Weekend: Dr. Mike Brown");
            
            Console.WriteLine("\n1. View Detailed Schedule");
            Console.WriteLine("2. Print Schedule");
            Console.WriteLine("3. Back to dashboard");
            Console.Write("\nSelect: ");
            
            Console.ReadKey();
        }
        
        private async Task ReportsAndAnalytics()
        {
            Console.Clear();
            Console.WriteLine("=== REPORTS & ANALYTICS ===\n");
            
            Console.WriteLine("Daily Report - " + DateTime.Now.ToString("yyyy-MM-dd"));
            Console.WriteLine("----------------------------------------");
            Console.WriteLine("New Registrations: 12");
            Console.WriteLine("Appointments: 45");
            Console.WriteLine("Walk-ins: 8");
            Console.WriteLine("Emergency Cases: 5");
            Console.WriteLine("Revenue: $8,450");
            Console.WriteLine("Patient Satisfaction: 92%");
            
            Console.WriteLine("\nTop Services:");
            Console.WriteLine("1. General Consultation: 25");
            Console.WriteLine("2. Lab Tests: 18");
            Console.WriteLine("3. X-Ray: 12");
            Console.WriteLine("4. Pharmacy: 35");
            
            Console.WriteLine("\n1. Generate Daily Report");
            Console.WriteLine("2. Monthly Analytics");
            Console.WriteLine("3. Patient Demographics");
            Console.WriteLine("4. Export to Excel");
            Console.WriteLine("5. Back to dashboard");
            Console.Write("\nSelect: ");
            
            Console.ReadKey();
        }
        
        private async Task EmergencyContacts()
        {
            Console.Clear();
            Console.WriteLine("=== EMERGENCY CONTACTS ===\n");
            
            Console.WriteLine("Hospital Departments:");
            Console.WriteLine("Emergency Room: ext. 1000");
            Console.WriteLine("ICU: ext. 1001");
            Console.WriteLine("Surgery: ext. 1002");
            Console.WriteLine("Pharmacy: ext. 1003");
            Console.WriteLine("Lab: ext. 1004");
            Console.WriteLine("Radiology: ext. 1005");
            
            Console.WriteLine("\nEmergency Services:");
            Console.WriteLine("Ambulance: ext. 911");
            Console.WriteLine("Fire Department: ext. 912");
            Console.WriteLine("Security: ext. 913");
            Console.WriteLine("Maintenance: ext. 914");
            
            Console.WriteLine("\nKey Personnel:");
            Console.WriteLine("Hospital Director: ext. 2000");
            Console.WriteLine("Head of Medicine: ext. 2001");
            Console.WriteLine("Head Nurse: ext. 2002");
            Console.WriteLine("IT Support: ext. 2003");
            
            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey();
        }
    }
}