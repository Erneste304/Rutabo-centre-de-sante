using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using HospitalManagementSystem.ConsoleApp.Models;
using HospitalManagementSystem.Data;
using Microsoft.EntityFrameworkCore;
using Entities = HospitalManagementSystem.Data.Entities;

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
        Task<bool> LogAuditAsync(int? userId, string username, string action, string entityType, int? entityId, string details);
        
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

        Task<bool> RejectTransactionAsync(int transactionId, int rejectedBy);
        Task<bool> AddTransactionAsync(Transaction transaction, int? userId = null, string username = "System");

        // New Dashboard Feature Methods
        Task<ShiftReportModel> GetShiftReportAsync(int userId, string fullName);
        Task<List<DoctorScheduleModel>> GetDoctorSchedulesAsync();
        Task<ReceptionistAnalyticsModel> GetReceptionistAnalyticsAsync();
        Task<List<Medication>> GetInventoryReportAsync();
    }
    
    public class DataService : IDataService
    {
        private readonly ApplicationDbContext _db;

        public DataService(ApplicationDbContext db)
        {
            _db = db;
        }

        // --- Helper Mappings ---

        private Patient MapPatient(Entities.Patient p)
        {
            var user = p.User;
            return new Patient
            {
                PatientId = p.PatientId,
                Name = user?.FullName ?? "Unknown",
                Age = user?.DateOfBirth != null ? DateTime.UtcNow.Year - user.DateOfBirth.Value.Year : 0,
                Gender = user?.Gender ?? "Unknown",
                BloodType = p.BloodType,
                LastVisit = p.AdmissionDate
            };
        }

        private Doctor MapDoctor(Entities.Doctor d)
        {
            var user = d.User;
            return new Doctor
            {
                DoctorId = d.DoctorId,
                Name = user?.FullName ?? "Unknown",
                Specialization = user?.Specialization ?? "General",
                Department = d.Department?.DepartmentName ?? "General",
                IsAvailable = d.IsAvailable
            };
        }

        private Appointment MapAppointment(Entities.Appointment a)
        {
            return new Appointment
            {
                AppointmentId = a.AppointmentId,
                PatientName = a.Patient?.User?.FullName ?? "Unknown",
                DoctorName = a.Doctor?.User?.FullName ?? "Unknown",
                Date = a.AppointmentDate,
                Status = a.Status
            };
        }

        private Medication MapMedication(Entities.Inventory i)
        {
            return new Medication
            {
                Id = i.ItemId,
                Name = i.ItemName,
                Type = i.Category,
                Stock = i.CurrentStock,
                Unit = i.Unit
            };
        }

        private Bill MapBill(Entities.Billing b)
        {
            return new Bill
            {
                BillId = b.BillId,
                PatientId = b.PatientId,
                Date = b.BillDate,
                Amount = b.TotalAmount,
                Status = b.PaymentStatus,
                Description = b.BillNumber ?? "Hospital Bill"
            };
        }

        private Payment MapPayment(Entities.Payment p)
        {
            return new Payment
            {
                PaymentId = p.PaymentId,
                BillId = p.BillId,
                PatientId = p.PatientId,
                Date = p.PaymentDate,
                Amount = p.Amount,
                Method = p.PaymentMethod,
                Reference = p.TransactionId ?? $"REF-{p.PaymentId}"
            };
        }

        // --- IDataService Implementation ---

        public async Task<List<Patient>> GetPatientsAsync()
        {
            var patients = await _db.Patients.ToListAsync();
            return patients.Select(MapPatient).ToList();
        }

        public async Task<List<Doctor>> GetDoctorsAsync()
        {
            var doctors = await _db.Doctors.Include(d => d.Department).ToListAsync();
            return doctors.Select(MapDoctor).ToList();
        }

        public async Task<List<Appointment>> GetAppointmentsAsync()
        {
            var appts = await _db.Appointments
                .Include(a => a.Patient)
                .Include(a => a.Doctor)
                .ToListAsync();
            return appts.Select(MapAppointment).ToList();
        }

        public async Task<List<Room>> GetRoomsAsync()
        {
            var rooms = await _db.Rooms.ToListAsync();
            return rooms.Select(r => new Room
            {
                RoomNumber = r.RoomNumber,
                Type = r.RoomType?.RoomTypeName ?? "Unknown",
                Status = r.RoomStatus,
                PatientName = "" // Room entity doesn't directly link to patient name in DB
            }).ToList();
        }

        public async Task<List<Medication>> GetMedicationsAsync()
        {
            var items = await _db.Inventory.ToListAsync();
            return items.Select(MapMedication).ToList();
        }

        public async Task<bool> AddPatientAsync(Patient patient)
        {
            // Note: In real scenarios, a User would be created first. 
            // Here we just add a Patient record for simplicity of the migration.
            var dbPatient = new Entities.Patient
            {
                BloodType = patient.BloodType,
                AdmissionDate = DateTime.UtcNow,
                IsActive = true
            };
            _db.Patients.Add(dbPatient);
            await _db.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdatePatientAsync(Patient patient)
        {
            var dbPatient = await _db.Patients.Include(p => p.User).FirstOrDefaultAsync(p => p.PatientId == patient.PatientId);
            if (dbPatient == null) return false;

            if (dbPatient.User != null)
            {
                dbPatient.User.FullName = patient.Name;
                dbPatient.User.Gender = patient.Gender;
            }
            dbPatient.BloodType = patient.BloodType;

            await _db.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ScheduleAppointmentAsync(Appointment appointment)
        {
            // Simplified: in a real app would need to match patient/doctor IDs
            var dbAppt = new HospitalManagementSystem.Data.Entities.Appointment
            {
                AppointmentDate = appointment.Date,
                Status = "Scheduled",
                CreatedAt = DateTime.UtcNow
            };
            _db.Appointments.Add(dbAppt);
            await _db.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdateAppointmentAsync(Appointment appointment)
        {
            var dbAppt = await _db.Appointments.FindAsync(appointment.AppointmentId);
            if (dbAppt == null) return false;
            dbAppt.Status = appointment.Status;
            await _db.SaveChangesAsync();
            return true;
        }

        public async Task<DashboardStats> GetDashboardStatsAsync(string userType)
        {
            var stats = new DashboardStats { Date = DateTime.Now };
            
            stats.TotalPatients = await _db.Patients.CountAsync();
            stats.TodayAppointments = await _db.Appointments.CountAsync(a => a.AppointmentDate.Date == DateTime.Today);
            
            switch (userType.ToLower())
            {
                case "doctor":
                    stats.Stat1Label = "Total Consultations";
                    stats.Stat1Value = (await _db.Appointments.CountAsync(a => a.Status == "Completed")).ToString();
                    break;
                case "admin":
                    stats.Stat1Label = "Total Staff";
                    stats.Stat1Value = (await _db.Users.CountAsync(u => u.UserType != "Patient")).ToString();
                    stats.Stat2Label = "Room Occupancy";
                    var totalRooms = await _db.Rooms.CountAsync();
                    var occupied = await _db.Rooms.CountAsync(r => r.RoomStatus == "Occupied");
                    stats.Stat2Value = totalRooms > 0 ? $"{(occupied * 100) / totalRooms}%" : "0%";
                    break;
            }
            return stats;
        }

        public async Task<IReadOnlyList<Bill>> GetBillsForPatientAsync(int patientId)
        {
            var bills = await _db.Billings.Where(b => b.PatientId == patientId).ToListAsync();
            return bills.Select(MapBill).ToList().AsReadOnly();
        }

        public async Task<IReadOnlyList<Payment>> GetPaymentsForPatientAsync(int patientId)
        {
            var payments = await _db.Payments.Where(p => p.PatientId == patientId).ToListAsync();
            return payments.Select(MapPayment).ToList().AsReadOnly();
        }

        public async Task<bool> PayBillAsync(int billId, int patientId, decimal amount, string method, string reference)
        {
            var bill = await _db.Billings.FindAsync(billId);
            if (bill == null) return false;

            bill.PaymentStatus = "Paid";
            bill.PaidAmount += amount;

            var payment = new Entities.Payment
            {
                BillId = billId,
                PatientId = patientId,
                Amount = amount,
                PaymentMethod = method,
                TransactionId = reference,
                PaymentNumber = "PAY-" + Guid.NewGuid().ToString().Substring(0, 8),
                PaymentDate = DateTime.Now
            };

            _db.Payments.Add(payment);
            await _db.SaveChangesAsync();
            return true;
        }

        public async Task<bool> HasPaidBillAsync(int patientId)
        {
            return await _db.Billings.AnyAsync(b => b.PatientId == patientId && b.PaymentStatus == "Paid");
        }

        public async Task<MedicalRecordRequest> CreateMedicalRecordRequestAsync(int patientId, string patientName)
        {
            // Mapping Console's "MedicalRecordRequest" to DB's simple AuditLog or custom logic
            // Since there's no RecordRequest table, we'll return a mock for now
            return new MedicalRecordRequest { RequestId = 1, PatientId = patientId, Status = "Pending" };
        }

        public async Task<IReadOnlyList<MedicalRecordRequest>> GetPendingRecordRequestsAsync()
        {
            return new List<MedicalRecordRequest>().AsReadOnly();
        }

        public async Task<bool> ApproveRecordRequestAsync(int requestId, int doctorId, string doctorName)
        {
            return true;
        }

        public async Task<bool> IsRecordRequestApprovedAsync(int patientId)
        {
            return true;
        }

        public async Task<List<UserActivity>> GetUserActivitiesAsync(int? userId = null)
        {
            var logs = await _db.AuditLogs.Include(l => l.User).ToListAsync();
            return logs.Select(l => new UserActivity
            {
                ActivityId = l.LogId,
                UserId = l.UserId ?? 0,
                Username = l.User?.Username ?? "Unknown",
                Action = l.Action,
                Timestamp = l.Timestamp
            }).ToList();
        }

        public async Task<bool> LogUserActivityAsync(int userId, string username, string action, string details)
        {
            return await LogAuditAsync(userId, username, action, "User", userId, details);
        }

        public async Task<List<AuditLog>> GetAuditLogsAsync(DateTime? fromDate = null, DateTime? toDate = null, string? username = null)
        {
            var query = _db.AuditLogs.Include(l => l.User).AsQueryable();
            if (fromDate.HasValue) query = query.Where(l => l.Timestamp >= fromDate.Value);
            if (toDate.HasValue) query = query.Where(l => l.Timestamp <= toDate.Value);
            if (!string.IsNullOrEmpty(username)) query = query.Where(l => l.User != null && l.User.Username == username);
            
            var logs = await query.ToListAsync();
            return logs.Select(l => new AuditLog
            {
                LogId = l.LogId,
                Username = l.User?.Username ?? "Unknown",
                Action = l.Action,
                Timestamp = l.Timestamp,
                Details = l.Details
            }).ToList();
        }

        public async Task<bool> LogAuditAsync(int? userId, string username, string action, string entityType, int? entityId, string details)
        {
            var log = new Entities.AuditLog
            {
                UserId = userId == 0 ? null : userId,
                Action = action,
                EntityType = entityType,
                EntityId = entityId,
                Details = details,
                Timestamp = DateTime.UtcNow
            };
            _db.AuditLogs.Add(log);
            await _db.SaveChangesAsync();
            return true;
        }

        public async Task<List<Department>> GetDepartmentsAsync()
        {
            var depts = await _db.Departments.ToListAsync();
            return depts.Select(d => new Department
            {
                DepartmentId = d.DepartmentId,
                Name = d.DepartmentName,
                Description = d.Description,
                IsActive = true
            }).ToList();
        }

        public async Task<bool> AddDepartmentAsync(Department department)
        {
            _db.Departments.Add(new HospitalManagementSystem.Data.Entities.Department
            {
                DepartmentName = department.Name,
                Description = department.Description,
                DepartmentCode = department.Name.Substring(0, Math.Min(4, department.Name.Length)).ToUpper()
            });
            await _db.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdateDepartmentAsync(Department department)
        {
            var dbDept = await _db.Departments.FindAsync(department.DepartmentId);
            if (dbDept == null) return false;
            dbDept.DepartmentName = department.Name;
            dbDept.Description = department.Description;
            await _db.SaveChangesAsync();
            return true;
        }

        public async Task<List<Transaction>> GetTransactionsAsync(string? status = null)
        {
            // We use AuditLogs with type "Transaction" to persist these
            var query = _db.AuditLogs.Where(l => l.EntityType == "Transaction");
            var logs = await query.ToListAsync();
            
            return logs.Select(l => new Transaction
            {
                TransactionId = l.LogId,
                Status = l.Details?.Contains("APPROVED") == true ? "Approved" : (l.Details?.Contains("REJECTED") == true ? "Rejected" : "Pending"),
                Date = l.Timestamp,
                Notes = l.Details
            }).Where(t => status == null || t.Status.Equals(status, StringComparison.OrdinalIgnoreCase)).ToList();
        }

        public async Task<bool> ApproveTransactionAsync(int transactionId, int approvedBy)
        {
            var log = await _db.AuditLogs.FindAsync(transactionId);
            if (log != null)
            {
                log.Details += $" | APPROVED by {approvedBy} at {DateTime.Now}";
                await _db.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<bool> RejectTransactionAsync(int transactionId, int rejectedBy)
        {
            var log = await _db.AuditLogs.FindAsync(transactionId);
            if (log != null)
            {
                log.Details += $" | REJECTED by {rejectedBy} at {DateTime.Now}";
                await _db.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<bool> AddTransactionAsync(Transaction transaction, int? userId = null, string username = "System")
        {
            return await LogAuditAsync(userId, username, "Money Transfer", "Transaction", null, transaction.Notes + $" | Amount: ${transaction.Amount}");
        }

        public async Task<List<Bill>> GetAllBillsAsync()
        {
            var bills = await _db.Billings.ToListAsync();
            return bills.Select(MapBill).ToList();
        }

        public async Task<List<Payment>> GetAllPaymentsAsync()
        {
            var payments = await _db.Payments.ToListAsync();
            return payments.Select(MapPayment).ToList();
        }

        public async Task<bool> AddDoctorAsync(Doctor doctor)
        {
            _db.Doctors.Add(new Entities.Doctor
            {
                LicenseNumber = "LIC-" + Guid.NewGuid().ToString().Substring(0, 8),
                IsAvailable = doctor.IsAvailable
            });
            await _db.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdateDoctorAsync(Doctor doctor)
        {
            var dbDoctor = await _db.Doctors.Include(d => d.User).FirstOrDefaultAsync(d => d.DoctorId == doctor.DoctorId);
            if (dbDoctor == null) return false;
            
            if (dbDoctor.User != null)
            {
                dbDoctor.User.FullName = doctor.Name;
                dbDoctor.User.Specialization = doctor.Specialization;
            }
            dbDoctor.IsAvailable = doctor.IsAvailable;
            
            await _db.SaveChangesAsync();
            return true;
        }

        public async Task<Dictionary<string, object>> GetDoctorPerformanceAsync(int doctorId)
        {
            return new Dictionary<string, object>
            {
                { "TotalPatients", 0 },
                { "MonthlyConsultations", 0 },
                { "SuccessRate", "N/A" }
            };
        }

        public async Task<Patient?> GetPatientByIdAsync(int patientId)
        {
            var p = await _db.Patients.FindAsync(patientId);
            return p != null ? MapPatient(p) : null;
        }

        public async Task<bool> DischargePatientAsync(int patientId)
        {
            return true;
        }

        public async Task<bool> AddRoomAsync(Room room)
        {
            var roomType = await _db.RoomTypes.FirstOrDefaultAsync(rt => rt.RoomTypeName == room.Type);
            _db.Rooms.Add(new HospitalManagementSystem.Data.Entities.Room
            {
                RoomNumber = room.RoomNumber,
                RoomType = roomType,
                RoomStatus = room.Status
            });
            await _db.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdateRoomAsync(Room room)
        {
            var dbRoom = await _db.Rooms.FirstOrDefaultAsync(r => r.RoomNumber == room.RoomNumber);
            if (dbRoom == null) return false;
            var roomType = await _db.RoomTypes.FirstOrDefaultAsync(rt => rt.RoomTypeName == room.Type);
            dbRoom.RoomType = roomType;
            dbRoom.RoomStatus = room.Status;
            await _db.SaveChangesAsync();
            return true;
        }

        public async Task<Dictionary<string, object>> GetRoomUtilizationAsync()
        {
            return new Dictionary<string, object>
            {
                { "TotalRooms", await _db.Rooms.CountAsync() },
                { "OccupiedRooms", await _db.Rooms.CountAsync(r => r.RoomStatus == "Occupied") }
            };
        }

        public async Task<ShiftReportModel> GetShiftReportAsync(int userId, string fullName)
        {
            var today = DateTime.UtcNow.Date;
            var logs = await _db.AuditLogs
                .Where(l => l.UserId == userId && l.Timestamp >= today)
                .ToListAsync();

            var report = new ShiftReportModel
            {
                NurseName = fullName,
                Date = DateTime.Now,
                MedsAdministered = logs.Count(l => l.Action.Contains("Medication", StringComparison.OrdinalIgnoreCase)),
                VitalsRecorded = logs.Count(l => l.Action.Contains("Vital", StringComparison.OrdinalIgnoreCase)),
                TasksCompleted = logs.Count(l => l.Action.Contains("Task", StringComparison.OrdinalIgnoreCase) && l.Action.Contains("Complete", StringComparison.OrdinalIgnoreCase)),
                Issues = new List<string> { "No critical issues reported" }
            };

            // Get some patient summaries from logs
            report.PatientShortSummaries = logs
                .Where(l => l.EntityType == "Patient" && l.EntityId.HasValue)
                .Take(5)
                .Select(l => $"{l.Action} for Patient #{l.EntityId}")
                .ToList();

            if (report.PatientShortSummaries.Count == 0)
                report.PatientShortSummaries.Add("No patients handled this shift yet.");

            return report;
        }

        public async Task<List<DoctorScheduleModel>> GetDoctorSchedulesAsync()
        {
            var doctors = await _db.Doctors
                .Include(d => d.User)
                .Include(d => d.Schedules)
                .ToListAsync();

            var result = new List<DoctorScheduleModel>();
            foreach (var d in doctors)
            {
                var model = new DoctorScheduleModel
                {
                    DoctorName = d.User?.FullName ?? "Unknown",
                    Specialization = d.User?.Specialization ?? "General"
                };

                foreach (var s in d.Schedules)
                {
                    var timeStr = $"{s.StartTime:hh\\:mm}-{s.EndTime:hh\\:mm}";
                    switch (s.DayOfWeek)
                    {
                        case 1: model.Monday = timeStr; break;
                        case 2: model.Tuesday = timeStr; break;
                        case 3: model.Wednesday = timeStr; break;
                        case 4: model.Thursday = timeStr; break;
                        case 5: model.Friday = timeStr; break;
                        case 6: model.Saturday = timeStr; break;
                        case 7: model.Sunday = timeStr; break;
                    }
                }
                result.Add(model);
            }

            if (result.Count == 0)
            {
                // Fallback dummy if no doctors in SQL yet
                result.Add(new DoctorScheduleModel { DoctorName = "Dr. John Smith", Specialization = "Cardiology", Monday = "09:00-17:00", Tuesday = "09:00-17:00" });
            }

            return result;
        }

        public async Task<ReceptionistAnalyticsModel> GetReceptionistAnalyticsAsync()
        {
            var today = DateTime.UtcNow.Date;
            var analytics = new ReceptionistAnalyticsModel
            {
                NewRegistrationsToday = await _db.Patients.CountAsync(p => p.AdmissionDate >= today),
                AppointmentsToday = await _db.Appointments.CountAsync(a => a.AppointmentDate.Date == today),
                RevenueToday = await _db.Payments.Where(p => p.PaymentDate >= today).SumAsync(p => p.Amount),
                WalkInsToday = await _db.Appointments.CountAsync(a => a.AppointmentDate.Date == today && a.Status == "Walk-in"),
                EmergencyCasesToday = await _db.Appointments.CountAsync(a => a.AppointmentDate.Date == today && a.Status == "Emergency")
            };

            var topServicesList = await _db.BillItems
                .GroupBy(bi => bi.Description)
                .Select(g => new { Name = g.Key, Count = g.Count() })
                .OrderByDescending(x => x.Count)
                .Take(4)
                .ToListAsync();
            
            analytics.TopServices = topServicesList.ToDictionary(x => x.Name, x => x.Count);

            return analytics;
        }

        public async Task<List<Medication>> GetInventoryReportAsync()
        {
            var items = await _db.Inventory.ToListAsync();
            return items.Select(i => new Medication
            {
                Id = i.ItemId,
                Name = i.ItemName,
                Type = i.Category,
                Stock = i.CurrentStock,
                Unit = i.Unit,
                Required = i.MinimumStock // Using MinimumStock as 'Required' for UI compatibility
            }).ToList();
        }
    }
}
