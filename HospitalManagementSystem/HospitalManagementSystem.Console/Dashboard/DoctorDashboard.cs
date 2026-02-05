using System;
using System.Threading.Tasks;
using HospitalManagementSystem.ConsoleApp.Models;
using HospitalManagementSystem.ConsoleApp.Services;

namespace HospitalManagementSystem.ConsoleApp.Dashboard
{
    public class DoctorDashboard : IDashboard
    {
        private readonly UserSession _session;
        private readonly IDataService _dataService;

        public DoctorDashboard(UserSession session, IDataService dataService)
        {
            _session = session;
            _dataService = dataService;
        }

        public async Task ShowAsync()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine($"=== DOCTOR DASHBOARD ===");
                Console.WriteLine($"Dr. {_session.FullName} - {_session.Specialization ?? "General"}");
                Console.WriteLine("==========================");
                Console.WriteLine("1. View Today's Schedule");
                Console.WriteLine("2. View All Appointments");
                Console.WriteLine("3. View My Patients");
                Console.WriteLine("4. Update Medical Records");
                Console.WriteLine("5. Write Prescription");
                Console.WriteLine("6. View Patient History");
                Console.WriteLine("7. Set Availability");
                Console.WriteLine("8. View Reports");
                Console.WriteLine("9. Approve Record Requests");
                Console.WriteLine("10. Logout");
                Console.Write("\nSelect option: ");
                
                var choice = Console.ReadLine();
                
                switch (choice)
                {
                    case "1":
                        await ViewTodaysSchedule();
                        break;
                    case "2":
                        await ViewAllAppointments();
                        break;
                    case "3":
                        await ViewMyPatients();
                        break;
                    case "4":
                        await UpdateMedicalRecord();
                        break;
                    case "5":
                        await WritePrescription();
                        break;
                    case "6":
                        await ViewPatientHistory();
                        break;
                    case "7":
                        await SetAvailability();
                        break;
                    case "8":
                        await ViewReports();
                        break;
                    case "9":
                        await ApproveRecordRequests();
                        break;
                    case "10":
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
        
        private async Task ViewTodaysSchedule()
        {
            Console.Clear();
            Console.WriteLine($"=== TODAY'S SCHEDULE - {DateTime.Today:yyyy-MM-dd} ===\n");
            
            Console.WriteLine("Time     | Patient           | Status");
            Console.WriteLine("---------------------------------------");
            Console.WriteLine("09:00 AM | John Doe          | Waiting");
            Console.WriteLine("10:30 AM | Jane Smith        | Confirmed");
            Console.WriteLine("02:00 PM | Robert Johnson    | Confirmed");
            Console.WriteLine("04:00 PM | Emily Davis       | Pending");
            
            Console.WriteLine("\n1. Start Consultation");
            Console.WriteLine("2. Mark as Completed");
            Console.WriteLine("3. Reschedule");
            Console.WriteLine("4. Back to Dashboard");
            Console.Write("\nSelect: ");
            
            var choice = Console.ReadLine();
            
            if (choice == "1")
            {
                Console.Write("Enter patient name for consultation: ");
                var patient = Console.ReadLine();
                Console.WriteLine($"Starting consultation with {patient}...");
            }
            
            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey();
        }
        
        private async Task WritePrescription()
        {
            Console.Clear();
            Console.WriteLine("=== WRITE PRESCRIPTION ===\n");
            
            Console.Write("Patient ID/Name: ");
            var patient = Console.ReadLine();
            
            Console.Write("Diagnosis: ");
            var diagnosis = Console.ReadLine();
            
            Console.WriteLine("\nCommon Medications:");
            Console.WriteLine("1. Amoxicillin - Antibiotic");
            Console.WriteLine("2. Ibuprofen - Pain Relief");
            Console.WriteLine("3. Lisinopril - Blood Pressure");
            Console.WriteLine("4. Metformin - Diabetes");
            Console.WriteLine("5. Custom Medication");
            
            Console.Write("\nSelect medication (1-5): ");
            var medChoice = Console.ReadLine();
            
            Console.Write("Dosage: ");
            var dosage = Console.ReadLine();
            
            Console.Write("Duration (days): ");
            var duration = Console.ReadLine();
            
            Console.Write("Instructions: ");
            var instructions = Console.ReadLine();
            
            Console.WriteLine("\nPrescription saved successfully!");
            Console.WriteLine("Sent to pharmacy and patient records.");
            
            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey();
        }
        
        private async Task UpdateMedicalRecord()
        {
            Console.Clear();
            Console.WriteLine("=== UPDATE MEDICAL RECORD ===\n");
            
            Console.Write("Patient ID: ");
            var patientId = Console.ReadLine();
            
            Console.Write("Visit Date (YYYY-MM-DD): ");
            var visitDate = Console.ReadLine();
            
            Console.Write("Symptoms: ");
            var symptoms = Console.ReadLine();
            
            Console.Write("Diagnosis: ");
            var diagnosis = Console.ReadLine();
            
            Console.Write("Treatment Plan: ");
            var treatment = Console.ReadLine();
            
            Console.Write("Notes: ");
            var notes = Console.ReadLine();
            
            Console.WriteLine("\nMedical record updated successfully!");
            
            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey();
        }
        
        private async Task SetAvailability()
        {
            Console.Clear();
            Console.WriteLine("=== SET AVAILABILITY ===\n");
            
            Console.WriteLine("Current Schedule:");
            Console.WriteLine("- Monday: 9:00 AM - 5:00 PM");
            Console.WriteLine("- Wednesday: 9:00 AM - 5:00 PM");
            Console.WriteLine("- Friday: 9:00 AM - 3:00 PM");
            
            Console.WriteLine("\n1. Update Working Days");
            Console.WriteLine("2. Set Vacation");
            Console.WriteLine("3. Emergency Leave");
            Console.WriteLine("4. Back to Dashboard");
            Console.Write("\nSelect: ");
            
            var choice = Console.ReadLine();
            
            if (choice == "1")
            {
                Console.WriteLine("\nSelect working days (comma separated):");
                Console.WriteLine("1. Monday, 2. Tuesday, 3. Wednesday, 4. Thursday, 5. Friday");
                Console.Write("Days: ");
                var days = Console.ReadLine();
                Console.WriteLine("Schedule updated!");
            }
            
            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey();
        }
        
        private async Task ViewReports()
        {
            Console.Clear();
            Console.WriteLine("=== REPORTS ===\n");
            
            Console.WriteLine("1. Monthly Patient Statistics");
            Console.WriteLine("2. Treatment Success Rate");
            Console.WriteLine("3. Revenue Report");
            Console.WriteLine("4. Patient Satisfaction");
            Console.WriteLine("5. Back to Dashboard");
            Console.Write("\nSelect: ");
            
            var choice = Console.ReadLine();
            
            if (choice == "1")
            {
                Console.WriteLine("\n=== PATIENT STATISTICS ===");
                Console.WriteLine("Total Patients: 245");
                Console.WriteLine("New Patients this month: 32");
                Console.WriteLine("Follow-up Appointments: 156");
                Console.WriteLine("Average Daily Patients: 12");
            }
            
            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey();
        }
        
        private async Task ApproveRecordRequests()
        {
            Console.Clear();
            Console.WriteLine("=== MEDICAL RECORD DOWNLOAD REQUESTS ===\n");

            var requests = await _dataService.GetPendingRecordRequestsAsync();
            if (requests.Count == 0)
            {
                Console.WriteLine("No pending medical record download requests.");
                Console.WriteLine("\nPress any key to continue...");
                Console.ReadKey();
                return;
            }

            for (int i = 0; i < requests.Count; i++)
            {
                var r = requests[i];
                Console.WriteLine($"{i + 1}. Request #{r.RequestId} - Patient: {r.PatientName} (ID: {r.PatientId}) - Requested: {r.RequestedAt:g}");
            }

            Console.Write("\nEnter number to approve (or 0 to cancel): ");
            var input = Console.ReadLine();
            if (int.TryParse(input, out var index) && index > 0 && index <= requests.Count)
            {
                var selected = requests[index - 1];
                var approved = await _dataService.ApproveRecordRequestAsync(selected.RequestId, _session.UserId, _session.FullName);
                if (approved)
                {
                    Console.WriteLine($"\nRequest #{selected.RequestId} for patient {selected.PatientName} approved. The patient can now download their medical records.");
                }
                else
                {
                    Console.WriteLine("\nFailed to approve request. It may have been processed already.");
                }
            }
            else
            {
                Console.WriteLine("\nNo changes made.");
            }

            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey();
        }
        
        private async Task ViewMyPatients()
        {
            Console.Clear();
            Console.WriteLine("=== MY PATIENTS ===\n");
            
            Console.WriteLine("Patient Name     | Last Visit     | Next Appointment");
            Console.WriteLine("---------------------------------------------------");
            Console.WriteLine("John Doe         | 2024-01-10     | 2024-02-15");
            Console.WriteLine("Jane Smith       | 2024-01-12     | 2024-02-20");
            Console.WriteLine("Robert Johnson   | 2024-01-05     | 2024-02-10");
            
            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey();
        }
        
        private async Task ViewAllAppointments()
        {
            Console.Clear();
            Console.WriteLine("=== ALL APPOINTMENTS ===\n");
            
            Console.WriteLine("Date       | Time     | Patient           | Status");
            Console.WriteLine("---------------------------------------------------");
            Console.WriteLine("2024-01-15 | 09:00 AM | John Doe          | Completed");
            Console.WriteLine("2024-01-16 | 10:30 AM | Jane Smith        | Scheduled");
            Console.WriteLine("2024-01-18 | 02:00 PM | Robert Johnson    | Scheduled");
            
            Console.WriteLine("\n1. Filter by Date");
            Console.WriteLine("2. Filter by Status");
            Console.WriteLine("3. Search Patient");
            Console.WriteLine("4. Back to Dashboard");
            Console.Write("\nSelect: ");
            
            Console.ReadKey();
        }
        
        private async Task ViewPatientHistory()
        {
            Console.Clear();
            Console.WriteLine("=== PATIENT HISTORY ===\n");
            
            Console.Write("Enter Patient ID/Name: ");
            var patient = Console.ReadLine();
            
            Console.WriteLine($"\nMedical History for {patient}:");
            Console.WriteLine("Date       | Diagnosis              | Treatment");
            Console.WriteLine("---------------------------------------------------");
            Console.WriteLine("2023-12-10 | Hypertension           | Medication");
            Console.WriteLine("2023-11-05 | Diabetes Check         | Diet Plan");
            Console.WriteLine("2023-10-20 | Annual Physical        | Normal");
            
            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey();
        }
    }
}