using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using HospitalManagementSystem.ConsoleApp.Models;

namespace HospitalManagementSystem.ConsoleApp.Services
{
    public interface IDataService
    {
        Task<List<Patient>> GetPatientsAsync();
        Task<List<Doctor>> GetDoctorsAsync();
        Task<List<Appointment>> GetAppointmentsAsync();
        Task<List<Room>> GetRoomsAsync();
        Task<List<Medication>> GetMedicationsAsync();
        Task<bool> AddPatientAsync(Patient patient);
        Task<bool> UpdatePatientAsync(Patient patient);
        Task<bool> ScheduleAppointmentAsync(Appointment appointment);
        Task<bool> UpdateAppointmentAsync(Appointment appointment);
        Task<DashboardStats> GetDashboardStatsAsync(string userType);
        Task<IReadOnlyList<Bill>> GetBillsForPatientAsync(int patientId);
        Task<IReadOnlyList<Payment>> GetPaymentsForPatientAsync(int patientId);
        Task<bool> PayBillAsync(int billId, int patientId, decimal amount, string method, string reference);
        Task<bool> HasPaidBillAsync(int patientId);
        Task<MedicalRecordRequest> CreateMedicalRecordRequestAsync(int patientId, string patientName);
        Task<IReadOnlyList<MedicalRecordRequest>> GetPendingRecordRequestsAsync();
        Task<bool> ApproveRecordRequestAsync(int requestId, int doctorId, string doctorName);
        Task<bool> IsRecordRequestApprovedAsync(int patientId);
        
        // User Activities & Audit
        Task<List<UserActivity>> GetUserActivitiesAsync(int? userId = null);
        Task<bool> LogUserActivityAsync(int userId, string username, string action, string details);
        Task<List<AuditLog>> GetAuditLogsAsync(DateTime? fromDate = null, DateTime? toDate = null, string? username = null);
        Task<bool> LogAuditAsync(int userId, string username, string action, string entityType, int? entityId, string details);
        
        // Departments
        Task<List<Department>> GetDepartmentsAsync();
        Task<bool> AddDepartmentAsync(Department department);
        Task<bool> UpdateDepartmentAsync(Department department);
        
        // Transactions & Finance
        Task<List<Transaction>> GetTransactionsAsync(string? status = null);
        Task<bool> ApproveTransactionAsync(int transactionId, int approvedBy);
        Task<List<Bill>> GetAllBillsAsync();
        Task<List<Payment>> GetAllPaymentsAsync();
        
        // Doctor Management
        Task<bool> AddDoctorAsync(Doctor doctor);
        Task<bool> UpdateDoctorAsync(Doctor doctor);
        Task<Dictionary<string, object>> GetDoctorPerformanceAsync(int doctorId);
        
        // Patient Management
        Task<Patient?> GetPatientByIdAsync(int patientId);
        Task<bool> DischargePatientAsync(int patientId);
        
        // Room Management
        Task<bool> AddRoomAsync(Room room);
        Task<bool> UpdateRoomAsync(Room room);
        Task<Dictionary<string, object>> GetRoomUtilizationAsync();
    }
    
    public class DataService : IDataService
    {
        // Mock in-memory data. In a real app, this would use a database or API.
        private readonly List<Bill> _bills = new();
        private readonly List<Payment> _payments = new();
        private readonly List<MedicalRecordRequest> _recordRequests = new();
        private int _nextBillId = 1001;
        private int _nextPaymentId = 5001;
        private int _nextRecordRequestId = 1;

        private readonly List<UserActivity> _userActivities = new();
        private readonly List<AuditLog> _auditLogs = new();
        private readonly List<Department> _departments = new();
        private readonly List<Transaction> _transactions = new();
        private int _nextActivityId = 1;
        private int _nextLogId = 1;
        private int _nextDeptId = 1;
        public DataService()
        {
            // Seed some example billing data
            _bills.Add(new Bill
            {
                BillId = _nextBillId++,
                PatientId = 1001,
                Date = DateTime.Today.AddDays(-5),
                Amount = 150m,
                Status = "Paid",
                Description = "Consultation"
            });

            _bills.Add(new Bill
            {
                BillId = _nextBillId++,
                PatientId = 1001,
                Date = DateTime.Today.AddDays(-1),
                Amount = 200m,
                Status = "Pending",
                Description = "Lab Tests"
            });

            _bills.Add(new Bill
            {
                BillId = _nextBillId++,
                PatientId = 1002,
                Date = DateTime.Today.AddDays(-2),
                Amount = 75m,
                Status = "Pending",
                Description = "X-Ray"
            });

            _payments.Add(new Payment
            {
                PaymentId = _nextPaymentId++,
                BillId = 1001,
                PatientId = 1001,
                Date = DateTime.Today.AddDays(-4),
                Amount = 150m,
                Method = "Cash",
                Reference = "CASH-1001"
            });
        }

        // Mock data - In real app, connect to API or database
        public async Task<List<Patient>> GetPatientsAsync()
        {
            await Task.Delay(50);
            return new List<Patient>
            {
                new Patient { PatientId = 1001, Name = "John Doe", Age = 45, Gender = "Male", BloodType = "O+", LastVisit = DateTime.Now.AddDays(-10) },
                new Patient { PatientId = 1002, Name = "Emily Davis", Age = 32, Gender = "Female", BloodType = "A-", LastVisit = DateTime.Now.AddDays(-5) },
                new Patient { PatientId = 1003, Name = "Robert Johnson", Age = 58, Gender = "Male", BloodType = "B+", LastVisit = DateTime.Now.AddDays(-2) },
                new Patient { PatientId = 1004, Name = "Sarah Wilson", Age = 28, Gender = "Female", BloodType = "AB+", LastVisit = DateTime.Now.AddDays(-15) }
            };
        }

        public async Task<List<Doctor>> GetDoctorsAsync()
        {
            await Task.Delay(50);
            return new List<Doctor>
            {
                new Doctor { DoctorId = 101, Name = "Dr. John Smith", Specialization = "Cardiology", Department = "Cardiology", IsAvailable = true },
                new Doctor { DoctorId = 102, Name = "Dr. Sarah Jones", Specialization = "Neurology", Department = "Neurology", IsAvailable = true },
                new Doctor { DoctorId = 103, Name = "Dr. Michael Brown", Specialization = "Orthopedics", Department = "Orthopedics", IsAvailable = false },
                new Doctor { DoctorId = 104, Name = "Dr. Lisa Wang", Specialization = "Pediatrics", Department = "Pediatrics", IsAvailable = true }
            };
        }

        public async Task<List<Appointment>> GetAppointmentsAsync()
        {
            await Task.Delay(50);
            return new List<Appointment>
            {
                new Appointment { AppointmentId = 1, PatientName = "John Doe", DoctorName = "Dr. John Smith", Date = DateTime.Now.AddHours(2), Status = "Scheduled" },
                new Appointment { AppointmentId = 2, PatientName = "Emily Davis", DoctorName = "Dr. Sarah Jones", Date = DateTime.Now.AddDays(1), Status = "Confirmed" },
                new Appointment { AppointmentId = 3, PatientName = "Robert Johnson", DoctorName = "Dr. Michael Brown", Date = DateTime.Now.AddDays(-1), Status = "Completed" }
            };
        }

        public async Task<List<Room>> GetRoomsAsync()
        {
            await Task.Delay(50);
            return new List<Room>
            {
                new Room { RoomNumber = "101", Type = "General", Status = "Occupied", PatientName = "John Doe" },
                new Room { RoomNumber = "102", Type = "General", Status = "Available", PatientName = "" },
                new Room { RoomNumber = "ICU-01", Type = "ICU", Status = "Occupied", PatientName = "Emily Davis" },
                new Room { RoomNumber = "OP-01", Type = "Operation", Status = "Available", PatientName = "" }
            };
        }

        public async Task<List<Medication>> GetMedicationsAsync()
        {
            await Task.Delay(50);
            return new List<Medication>
            {
                new Medication { Id = 1, Name = "Amoxicillin", Type = "Antibiotic", Stock = 150, Unit = "mg" },
                new Medication { Id = 2, Name = "Ibuprofen", Type = "Pain Reliever", Stock = 200, Unit = "mg" },
                new Medication { Id = 3, Name = "Lisinopril", Type = "Blood Pressure", Stock = 100, Unit = "mg" },
                new Medication { Id = 4, Name = "Metformin", Type = "Diabetes", Stock = 120, Unit = "mg" }
            };
        }

        public async Task<bool> AddPatientAsync(Patient patient)
        {
            await Task.Delay(100);
            Console.WriteLine($"Patient {patient.Name} added successfully!");
            return true;
        }

        public async Task<bool> UpdatePatientAsync(Patient patient)
        {
            await Task.Delay(100);
            Console.WriteLine($"Patient {patient.Name} updated successfully!");
            return true;
        }

        public async Task<bool> ScheduleAppointmentAsync(Appointment appointment)
        {
            await Task.Delay(100);
            Console.WriteLine($"Appointment scheduled for {appointment.PatientName} with {appointment.DoctorName}");
            return true;
        }

        public async Task<bool> UpdateAppointmentAsync(Appointment appointment)
        {
            await Task.Delay(100);
            Console.WriteLine($"Appointment {appointment.AppointmentId} updated successfully!");
            return true;
        }

        public async Task<DashboardStats> GetDashboardStatsAsync(string userType)
        {
            await Task.Delay(100);
            var stats = new DashboardStats
            {
                Date = DateTime.Now
            };

            switch (userType.ToLower())
            {
                case "doctor":
                    stats.TotalPatients = 45;
                    stats.TodayAppointments = 8;
                    stats.PendingTasks = 3;
                    stats.Stat1Label = "Monthly Consultations";
                    stats.Stat1Value = "156";
                    stats.Stat2Label = "Success Rate";
                    stats.Stat2Value = "94%";
                    break;
                    
                case "nurse":
                    stats.TotalPatients = 25;
                    stats.TodayAppointments = 15;
                    stats.PendingTasks = 10;
                    stats.Stat1Label = "Medications Administered";
                    stats.Stat1Value = "89";
                    stats.Stat2Label = "Patient Satisfaction";
                    stats.Stat2Value = "96%";
                    break;
                    
                case "receptionist":
                    stats.TotalPatients = 120;
                    stats.TodayAppointments = 45;
                    stats.PendingTasks = 12;
                    stats.Stat1Label = "Calls Handled";
                    stats.Stat1Value = "156";
                    stats.Stat2Label = "Appointments Booked";
                    stats.Stat2Value = "89";
                    break;
                    
                default: // Admin
                    stats.TotalPatients = 1245;
                    stats.TodayAppointments = 156;
                    stats.PendingTasks = 23;
                    stats.Stat1Label = "Total Staff";
                    stats.Stat1Value = "145";
                    stats.Stat2Label = "Bed Occupancy";
                    stats.Stat2Value = "80%";
                    break;
            }
            
            return stats;
        }

        public Task<IReadOnlyList<Bill>> GetBillsForPatientAsync(int patientId)
        {
            var result = _bills
                .Where(b => b.PatientId == patientId)
                .OrderByDescending(b => b.Date)
                .ToList()
                .AsReadOnly();

            return Task.FromResult((IReadOnlyList<Bill>)result);
        }

        public Task<IReadOnlyList<Payment>> GetPaymentsForPatientAsync(int patientId)
        {
            var result = _payments
                .Where(p => p.PatientId == patientId)
                .OrderByDescending(p => p.Date)
                .ToList()
                .AsReadOnly();

            return Task.FromResult((IReadOnlyList<Payment>)result);
        }

        public Task<bool> PayBillAsync(int billId, int patientId, decimal amount, string method, string reference)
        {
            var bill = _bills.FirstOrDefault(b => b.BillId == billId && b.PatientId == patientId);
            if (bill == null)
            {
                return Task.FromResult(false);
            }

            bill.Status = "Paid";

            var payment = new Payment
            {
                PaymentId = _nextPaymentId++,
                BillId = bill.BillId,
                PatientId = patientId,
                Date = DateTime.Now,
                Amount = amount,
                Method = method,
                Reference = reference
            };

            _payments.Add(payment);

            return Task.FromResult(true);
        }

        public Task<bool> HasPaidBillAsync(int patientId)
        {
            var hasPaid = _bills.Any(b => b.PatientId == patientId &&
                                          string.Equals(b.Status, "Paid", StringComparison.OrdinalIgnoreCase));
            return Task.FromResult(hasPaid);
        }

        public Task<MedicalRecordRequest> CreateMedicalRecordRequestAsync(int patientId, string patientName)
        {
            // Reuse existing approved or pending request if present
            var existing = _recordRequests
                .FirstOrDefault(r => r.PatientId == patientId && (r.Status == "Pending" || r.Status == "Approved"));
            if (existing != null)
            {
                return Task.FromResult(existing);
            }

            var request = new MedicalRecordRequest
            {
                RequestId = _nextRecordRequestId++,
                PatientId = patientId,
                PatientName = patientName,
                Status = "Pending",
                RequestedAt = DateTime.Now
            };

            _recordRequests.Add(request);
            return Task.FromResult(request);
        }

        public Task<IReadOnlyList<MedicalRecordRequest>> GetPendingRecordRequestsAsync()
        {
            var result = _recordRequests
                .Where(r => string.Equals(r.Status, "Pending", StringComparison.OrdinalIgnoreCase))
                .OrderBy(r => r.RequestedAt)
                .ToList()
                .AsReadOnly();

            return Task.FromResult((IReadOnlyList<MedicalRecordRequest>)result);
        }

        public Task<bool> ApproveRecordRequestAsync(int requestId, int doctorId, string doctorName)
        {
            var request = _recordRequests.FirstOrDefault(r => r.RequestId == requestId);
            if (request == null)
            {
                return Task.FromResult(false);
            }

            request.Status = "Approved";
            request.DoctorId = doctorId;
            request.ApprovedBy = doctorName;
            request.ApprovedAt = DateTime.Now;

            return Task.FromResult(true);
        }

        public Task<bool> IsRecordRequestApprovedAsync(int patientId)
        {
            var approved = _recordRequests.Any(r => r.PatientId == patientId &&
                                                    string.Equals(r.Status, "Approved", StringComparison.OrdinalIgnoreCase));
            return Task.FromResult(approved);
        }

        // User Activities & Audit
        public Task<List<UserActivity>> GetUserActivitiesAsync(int? userId = null)
        {
            var activities = userId.HasValue
                ? _userActivities.Where(a => a.UserId == userId.Value).ToList()
                : _userActivities.ToList();
            
            return Task.FromResult(activities.OrderByDescending(a => a.Timestamp).ToList());
        }

        public Task<bool> LogUserActivityAsync(int userId, string username, string action, string details)
        {
            _userActivities.Add(new UserActivity
            {
                ActivityId = _nextActivityId++,
                UserId = userId,
                Username = username,
                Action = action,
                Details = details,
                Timestamp = DateTime.Now
            });
            return Task.FromResult(true);
        }

        public Task<List<AuditLog>> GetAuditLogsAsync(DateTime? fromDate = null, DateTime? toDate = null, string? username = null)
        {
            var logs = _auditLogs.AsEnumerable();
            
            if (fromDate.HasValue)
                logs = logs.Where(l => l.Timestamp >= fromDate.Value);
            if (toDate.HasValue)
                logs = logs.Where(l => l.Timestamp <= toDate.Value);
            if (!string.IsNullOrWhiteSpace(username))
                logs = logs.Where(l => l.Username.Equals(username, StringComparison.OrdinalIgnoreCase));
            
            return Task.FromResult(logs.OrderByDescending(l => l.Timestamp).ToList());
        }

        public Task<bool> LogAuditAsync(int userId, string username, string action, string entityType, int? entityId, string details)
        {
            _auditLogs.Add(new AuditLog
            {
                LogId = _nextLogId++,
                UserId = userId,
                Username = username,
                Action = action,
                EntityType = entityType,
                EntityId = entityId,
                Details = details,
                Timestamp = DateTime.Now
            });
            return Task.FromResult(true);
        }

        // Departments
        public Task<List<Department>> GetDepartmentsAsync()
        {
            return Task.FromResult(_departments.ToList());
        }

        public Task<bool> AddDepartmentAsync(Department department)
        {
            department.DepartmentId = _nextDeptId++;
            _departments.Add(department);
            return Task.FromResult(true);
        }

        public Task<bool> UpdateDepartmentAsync(Department department)
        {
            var existing = _departments.FirstOrDefault(d => d.DepartmentId == department.DepartmentId);
            if (existing != null)
            {
                existing.Name = department.Name;
                existing.HeadDoctorName = department.HeadDoctorName;
                existing.HeadDoctorId = department.HeadDoctorId;
                existing.StaffCount = department.StaffCount;
                existing.Description = department.Description;
                existing.IsActive = department.IsActive;
                return Task.FromResult(true);
            }
            return Task.FromResult(false);
        }

        // Transactions & Finance
        public Task<List<Transaction>> GetTransactionsAsync(string? status = null)
        {
            var transactions = string.IsNullOrWhiteSpace(status)
                ? _transactions.ToList()
                : _transactions.Where(t => t.Status.Equals(status, StringComparison.OrdinalIgnoreCase)).ToList();
            
            return Task.FromResult(transactions.OrderByDescending(t => t.Date).ToList());
        }

        public Task<bool> ApproveTransactionAsync(int transactionId, int approvedBy)
        {
            var transaction = _transactions.FirstOrDefault(t => t.TransactionId == transactionId);
            if (transaction != null)
            {
                transaction.Status = "Approved";
                transaction.ApprovedBy = approvedBy;
                transaction.ApprovedAt = DateTime.Now;
                return Task.FromResult(true);
            }
            return Task.FromResult(false);
        }

        public Task<List<Bill>> GetAllBillsAsync()
        {
            return Task.FromResult(_bills.OrderByDescending(b => b.Date).ToList());
        }

        public Task<List<Payment>> GetAllPaymentsAsync()
        {
            return Task.FromResult(_payments.OrderByDescending(p => p.Date).ToList());
        }

        // Doctor Management
        public Task<bool> AddDoctorAsync(Doctor doctor)
        {
            // In real app, this would save to database
            return Task.FromResult(true);
        }

        public Task<bool> UpdateDoctorAsync(Doctor doctor)
        {
            // In real app, this would update database
            return Task.FromResult(true);
        }

        public Task<Dictionary<string, object>> GetDoctorPerformanceAsync(int doctorId)
        {
            var performance = new Dictionary<string, object>
            {
                { "TotalPatients", 45 },
                { "MonthlyConsultations", 156 },
                { "SuccessRate", "94%" },
                { "AverageRating", 4.7 },
                { "TotalAppointments", 320 },
                { "CompletedAppointments", 301 }
            };
            return Task.FromResult(performance);
        }

        // Patient Management
        public async Task<Patient?> GetPatientByIdAsync(int patientId)
        {
            var patients = await GetPatientsAsync();
            return patients.FirstOrDefault(p => p.PatientId == patientId);
        }

        public Task<bool> DischargePatientAsync(int patientId)
        {
            // In real app, this would update patient status and room
            return Task.FromResult(true);
        }

        // Room Management
        public Task<bool> AddRoomAsync(Room room)
        {
            // In real app, this would save to database
            return Task.FromResult(true);
        }

        public Task<bool> UpdateRoomAsync(Room room)
        {
            // In real app, this would update database
            return Task.FromResult(true);
        }

        public Task<Dictionary<string, object>> GetRoomUtilizationAsync()
        {
            var utilization = new Dictionary<string, object>
            {
                { "TotalRooms", 150 },
                { "OccupiedRooms", 120 },
                { "AvailableRooms", 30 },
                { "UtilizationRate", "80%" },
                { "GeneralRooms", 100 },
                { "ICURooms", 20 },
                { "OperationRooms", 30 }
            };
            return Task.FromResult(utilization);
        }
    }
}