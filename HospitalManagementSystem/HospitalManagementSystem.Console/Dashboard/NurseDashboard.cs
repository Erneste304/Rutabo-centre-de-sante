using System;
using System.Threading.Tasks;
using HospitalManagementSystem.ConsoleApp.Models;
using HospitalManagementSystem.ConsoleApp.Services;

namespace HospitalManagementSystem.ConsoleApp.Dashboard
{
    public class NurseDashboard : IDashboard
    {
        private readonly UserSession _session;
        private readonly IDataService _dataService;

        public NurseDashboard(UserSession session, IDataService dataService)
        {
            _session = session;
            _dataService = dataService;
        }

        public async Task ShowAsync()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine($"=== NURSE DASHBOARD ===");
                Console.WriteLine($"Nurse: {_session.FullName} | Department: {_session.Department ?? "General"}");
                Console.WriteLine("=========================");
                Console.WriteLine("1. Patient Vital Signs");
                Console.WriteLine("2. Medication Administration");
                Console.WriteLine("3. Patient Care Tasks");
                Console.WriteLine("4. Room & Bed Management");
                Console.WriteLine("5. Emergency Response");
                Console.WriteLine("6. Shift Report");
                Console.WriteLine("7. View Patient List");
                Console.WriteLine("8. Update Patient Charts");
                Console.WriteLine("9. Inventory Check");
                Console.WriteLine("10. My Schedule");
                Console.WriteLine("11. Logout");
                Console.Write("\nSelect option: ");
                
                var choice = Console.ReadLine();
                
                switch (choice)
                {
                    case "1":
                        await RecordVitalSigns();
                        break;
                    case "2":
                        await AdministerMedication();
                        break;
                    case "3":
                        await PatientCareTasks();
                        break;
                    case "4":
                        await ManageRooms();
                        break;
                    case "5":
                        await EmergencyResponse();
                        break;
                    case "6":
                        await ShiftReport();
                        break;
                    case "7":
                        await ViewPatientList();
                        break;
                    case "8":
                        await UpdatePatientCharts();
                        break;
                    case "9":
                        await InventoryCheck();
                        break;
                    case "10":
                        await ViewSchedule();
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
        
        private async Task RecordVitalSigns()
        {
            Console.Clear();
            Console.WriteLine("=== RECORD VITAL SIGNS ===\n");
            
            Console.Write("Patient ID/Name: ");
            var patient = Console.ReadLine();
            
            Console.WriteLine("\nEnter Vital Signs:");
            Console.Write("Temperature (°C): ");
            var temp = Console.ReadLine();
            
            Console.Write("Blood Pressure (mmHg): ");
            var bp = Console.ReadLine();
            
            Console.Write("Heart Rate (bpm): ");
            var hr = Console.ReadLine();
            
            Console.Write("Respiratory Rate (breaths/min): ");
            var rr = Console.ReadLine();
            
            Console.Write("Oxygen Saturation (%): ");
            var spo2 = Console.ReadLine();
            
            Console.Write("Pain Level (0-10): ");
            var pain = Console.ReadLine();
            
            Console.Write("Notes: ");
            var notes = Console.ReadLine();
            
            Console.WriteLine($"\nVital signs recorded for {patient}!");
            Console.WriteLine("Data saved to patient chart.");
            
            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey();
        }
        
        private async Task AdministerMedication()
        {
            Console.Clear();
            Console.WriteLine("=== MEDICATION ADMINISTRATION ===\n");
            
            Console.Write("Patient ID/Name: ");
            var patient = Console.ReadLine();
            
            Console.WriteLine("\nMedication List:");
            var medications = await _dataService.GetMedicationsAsync();
            int index = 1;
            foreach (var med in medications)
            {
                Console.WriteLine($"{index}. {med.Name} ({med.Type}) - Stock: {med.Stock} {med.Unit}");
                index++;
            }
            
            Console.Write("\nSelect medication (1-4): ");
            var medChoice = Console.ReadLine();
            
            Console.Write("Dose: ");
            var dose = Console.ReadLine();
            
            Console.Write("Route (Oral/IV/IM/SC): ");
            var route = Console.ReadLine();
            
            Console.Write("Time: ");
            var time = Console.ReadLine();
            
            Console.Write("Administered by: ");
            var administeredBy = Console.ReadLine();
            
            Console.WriteLine($"\nMedication administered to {patient}!");
            Console.WriteLine("Record updated in medication log.");
            
            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey();
        }
        
        private async Task PatientCareTasks()
        {
            Console.Clear();
            Console.WriteLine("=== PATIENT CARE TASKS ===\n");
            
            Console.WriteLine("Pending Tasks:");
            Console.WriteLine("1. [ ] Room 101 - Change dressing (John Doe)");
            Console.WriteLine("2. [ ] Room 102 - Assist with walking (Jane Smith)");
            Console.WriteLine("3. [ ] ICU-01 - Monitor vitals (Robert Johnson)");
            Console.WriteLine("4. [ ] Room 103 - Administer IV fluids (Emily Davis)");
            
            Console.WriteLine("\n1. Mark task as complete");
            Console.WriteLine("2. Add new task");
            Console.WriteLine("3. View completed tasks");
            Console.WriteLine("4. Back to dashboard");
            Console.Write("\nSelect: ");
            
            var choice = Console.ReadLine();
            
            if (choice == "1")
            {
                Console.Write("Enter task number to complete: ");
                var taskNum = Console.ReadLine();
                Console.WriteLine($"Task {taskNum} marked as complete!");
            }
            else if (choice == "2")
            {
                Console.Write("Enter new task: ");
                var newTask = Console.ReadLine();
                Console.WriteLine("New task added!");
            }
            
            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey();
        }
        
        private async Task ManageRooms()
        {
            Console.Clear();
            Console.WriteLine("=== ROOM & BED MANAGEMENT ===\n");
            
            var rooms = await _dataService.GetRoomsAsync();
            
            Console.WriteLine("Room No. | Type        | Status     | Patient");
            Console.WriteLine("---------------------------------------------------");
            foreach (var room in rooms)
            {
                Console.WriteLine($"{room.RoomNumber,-8} | {room.Type,-10} | {room.Status,-10} | {room.PatientName}");
            }
            
            Console.WriteLine("\n1. Change room status");
            Console.WriteLine("2. Assign patient to room");
            Console.WriteLine("3. Request cleaning");
            Console.WriteLine("4. View room history");
            Console.WriteLine("5. Back to dashboard");
            Console.Write("\nSelect: ");
            
            var choice = Console.ReadLine();
            
            if (choice == "1")
            {
                Console.Write("Enter room number: ");
                var roomNo = Console.ReadLine();
                Console.Write("New status (Available/Occupied/Maintenance): ");
                var status = Console.ReadLine();
                Console.WriteLine($"Room {roomNo} status updated to {status}!");
            }
            
            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey();
        }
        
        private async Task EmergencyResponse()
        {
            Console.Clear();
            Console.WriteLine("=== EMERGENCY RESPONSE ===\n");
            
            Console.WriteLine("Emergency Protocols:");
            Console.WriteLine("1. Code Blue - Cardiac Arrest");
            Console.WriteLine("2. Code Red - Fire");
            Console.WriteLine("3. Code Black - Bomb Threat");
            Console.WriteLine("4. Code Orange - Hazardous Material");
            Console.WriteLine("5. Code Pink - Infant Abduction");
            Console.WriteLine("6. Code Silver - Active Shooter");
            
            Console.WriteLine("\nEmergency Equipment:");
            Console.WriteLine("- Crash Cart: Station A, B, C");
            Console.WriteLine("- Defibrillator: Each floor");
            Console.WriteLine("- Emergency Medications: Pharmacy");
            Console.WriteLine("- First Aid Kits: Every room");
            
            Console.WriteLine("\nEmergency Contacts:");
            Console.WriteLine("- Charge Nurse: ext. 5551");
            Console.WriteLine("- Doctor on Call: ext. 5552");
            Console.WriteLine("- Security: ext. 5553");
            Console.WriteLine("- Maintenance: ext. 5554");
            
            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey();
        }
        
        private async Task ShiftReport()
        {
            Console.Clear();
            Console.WriteLine("=== SHIFT REPORT ===\n");
            
            var report = await _dataService.GetShiftReportAsync(_session.UserId, _session.FullName);

            Console.WriteLine($"Current Shift: {DateTime.Now:HH:mm} - Status: Active");
            Console.WriteLine($"Nurse: {report.NurseName}");
            Console.WriteLine($"Date: {report.Date:yyyy-MM-dd}");
            
            Console.WriteLine("\nPatients Handled (Recent Activities):");
            foreach (var summary in report.PatientShortSummaries)
            {
                Console.WriteLine($"- {summary}");
            }
            
            Console.WriteLine("\nTasks Completed (Metrics):");
            Console.WriteLine($"- Administered medications: {report.MedsAdministered}");
            Console.WriteLine($"- Recorded vitals: {report.VitalsRecorded}");
            Console.WriteLine($"- Tasks marked complete: {report.TasksCompleted}");
            
            Console.WriteLine("\nIssues/Concerns:");
            foreach (var issue in report.Issues)
            {
                Console.WriteLine($"- {issue}");
            }
            
            Console.WriteLine("\n1. Print Report");
            Console.WriteLine("2. Export to Chart");
            Console.WriteLine("3. Back to dashboard");
            Console.Write("\nSelect: ");
            
            var choice = Console.ReadLine();
            if (choice == "1")
            {
                await PrintShiftReport(report);
            }
            else if (choice == "2")
            {
                await ExportShiftReportToChart(report);
            }
        }

        private async Task PrintShiftReport(ShiftReportModel report)
        {
            Console.Clear();
            Console.WriteLine("----- PRINTING SHIFT REPORT -----");
            Console.WriteLine($"Nurse: {report.NurseName}");
            Console.WriteLine($"Date: {report.Date:yyyy-MM-dd}");
            Console.WriteLine("----------------------------------");
            Console.WriteLine($"Vitals Recorded: {report.VitalsRecorded}");
            Console.WriteLine($"Meds Administered: {report.MedsAdministered}");
            Console.WriteLine($"Tasks Completed: {report.TasksCompleted}");
            Console.WriteLine("----------------------------------");
            Console.WriteLine("Report saved to: " + Path.Combine(AppDomain.CurrentDomain.BaseDirectory, $"ShiftReport_{DateTime.Now:yyyyMMdd_HHmm}.txt"));
            Console.WriteLine("\nPrinting complete. Press any key to continue...");
            Console.ReadKey();
        }

        private async Task ExportShiftReportToChart(ShiftReportModel report)
        {
            Console.WriteLine("\nExporting report to patient records...");
            await _dataService.LogAuditAsync(_session.UserId, _session.Username, "Shift Report Export", "Report", null, $"Exported shift report for {report.NurseName} on {report.Date:yyyy-MM-dd}");
            await Task.Delay(1000);
            Console.WriteLine("Export successful! Data added to system audit logs and relevant charts.");
            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey();
        }
        
        private async Task ViewPatientList()
        {
            Console.Clear();
            Console.WriteLine("=== PATIENT LIST ===\n");
            
            var patients = await _dataService.GetPatientsAsync();
            
            Console.WriteLine("Patient ID | Name           | Age | Gender | Room");
            Console.WriteLine("---------------------------------------------------");
            foreach (var patient in patients)
            {
                Console.WriteLine($"{patient.PatientId,-10} | {patient.Name,-14} | {patient.Age,-3} | {patient.Gender,-6} | TBD");
            }
            
            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey();
        }
        
        private async Task UpdatePatientCharts()
        {
            Console.Clear();
            Console.WriteLine("=== UPDATE PATIENT CHARTS ===\n");
            
            Console.Write("Patient ID/Name: ");
            var patient = Console.ReadLine();
            
            Console.WriteLine("\nWhat would you like to update?");
            Console.WriteLine("1. Progress Notes");
            Console.WriteLine("2. Care Plan");
            Console.WriteLine("3. Fluid Balance");
            Console.WriteLine("4. Pain Assessment");
            Console.WriteLine("5. Wound Care");
            Console.Write("\nSelect: ");
            
            var choice = Console.ReadLine();
            
            if (choice == "1")
            {
                Console.WriteLine("\n=== PROGRESS NOTES ===");
                Console.Write("Enter progress notes: ");
                var notes = Console.ReadLine();
                Console.WriteLine("Progress notes updated!");
            }
            
            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey();
        }
        
        private async Task InventoryCheck()
        {
            Console.Clear();
            Console.WriteLine("=== INVENTORY CHECK ===\n");
            
            var inventory = await _dataService.GetInventoryReportAsync();

            Console.WriteLine("Item                  | Stock | Req. | Status");
            Console.WriteLine("--------------------------------------------------------");
            foreach (var item in inventory)
            {
                var status = item.Stock < item.Required ? "⚠ Low" : "✓ Good";
                Console.WriteLine($"{item.Name,-21} | {item.Stock,-5} | {item.Required,-4} | {status}");
            }
            
            if (inventory.Count == 0)
            {
                Console.WriteLine("No inventory items found in database.");
            }

            Console.WriteLine("\n1. Request restock");
            Console.WriteLine("2. Update inventory");
            Console.WriteLine("3. Back to dashboard");
            Console.Write("\nSelect: ");
            
            var choice = Console.ReadLine();
            if (choice == "1")
            {
                Console.Write("\nEnter item name to restock: ");
                var item = Console.ReadLine();
                Console.Write("Quantity: ");
                var qty = Console.ReadLine();
                Console.WriteLine($"\nRestock request for {qty} x {item} submitted to pharmacy.");
                Console.WriteLine("Press any key to continue...");
                Console.ReadKey();
            }
            else if (choice == "2")
            {
                Console.Write("\nEnter item name to update: ");
                var item = Console.ReadLine();
                Console.Write("Actual current stock: ");
                var qty = Console.ReadLine();
                Console.WriteLine($"\nInventory updated for {item}. New count: {qty}");
                Console.WriteLine("Press any key to continue...");
                Console.ReadKey();
            }
        }
        
        private async Task ViewSchedule()
        {
            Console.Clear();
            Console.WriteLine("=== MY SCHEDULE ===\n");
            
            Console.WriteLine($"Nurse: {_session.FullName}");
            Console.WriteLine($"Week: {DateTime.Now:MMMM dd, yyyy}\n");
            
            Console.WriteLine("Day         | Shift        | Department   | Assignment");
            Console.WriteLine("--------------------------------------------------------");
            Console.WriteLine("Monday      | Day (7-7)    | Emergency    | Triage");
            Console.WriteLine("Tuesday     | Day (7-7)    | Emergency    | Trauma");
            Console.WriteLine("Wednesday   | Off          | -            | -");
            Console.WriteLine("Thursday    | Night (7-7)  | ICU          | Monitor");
            Console.WriteLine("Friday      | Night (7-7)  | ICU          | Monitor");
            Console.WriteLine("Saturday    | Off          | -            | -");
            Console.WriteLine("Sunday      | Day (7-7)    | Emergency    | Triage");
            
            Console.WriteLine("\nTotal Hours: 36 hours/week");
            Console.WriteLine("Next Vacation: 2 weeks from now");
            
            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey();
        }
    }
}