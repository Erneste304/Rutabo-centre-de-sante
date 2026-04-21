using HospitalManagementSystem.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;

namespace HospitalManagementSystem.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Entities.User> Users { get; set; }
        public DbSet<Entities.Patient> Patients { get; set; }
        public DbSet<Entities.Doctor> Doctors { get; set; }
        public DbSet<Entities.Department> Departments { get; set; }
        public DbSet<Entities.Room> Rooms { get; set; }
        public DbSet<Entities.Appointment> Appointments { get; set; }
        public DbSet<Entities.MedicalRecord> MedicalRecords { get; set; }
        public DbSet<Entities.Billing> Billings { get; set; }
        public DbSet<Entities.BillItem> BillItems { get; set; }
        public DbSet<Entities.Payment> Payments { get; set; }
        public DbSet<Entities.Inventory> Inventory { get; set; }
        public DbSet<Entities.AuditLog> AuditLogs { get; set; }
        public DbSet<Entities.Notification> Notifications { get; set; }
        public DbSet<Entities.Prescription> Prescriptions { get; set; }
        public DbSet<Entities.PrescriptionItem> PrescriptionItems { get; set; }
        public DbSet<Entities.LabTest> LabTests { get; set; }
        public DbSet<Entities.Schedule> Schedules { get; set; }
        public DbSet<Entities.ShiftReport> ShiftReports { get; set; }
        public DbSet<Entities.EmployeeShift> EmployeeShifts { get; set; }

        // New entities matching ER diagram schema
        public DbSet<Entities.Ambulance> Ambulances { get; set; }
        public DbSet<Entities.AmbulanceLog> AmbulanceLogs { get; set; }
        public DbSet<Entities.BloodBank> BloodBanks { get; set; }
        public DbSet<Entities.InsuranceType> InsuranceTypes { get; set; }
        public DbSet<Entities.PatientVisit> PatientVisits { get; set; }
        public DbSet<Entities.TriageRecord> TriageRecords { get; set; }
        public DbSet<Entities.Medicine> Medicines { get; set; }
        public DbSet<Entities.Pharmacy> Pharmacies { get; set; }
        public DbSet<Entities.RoomType> RoomTypes { get; set; }
        public DbSet<Entities.RoomAssignment> RoomAssignments { get; set; }
        public DbSet<Entities.CleaningService> CleaningServices { get; set; }
        public DbSet<Entities.DoctorDepartment> DoctorDepartments { get; set; }
        public DbSet<Entities.MedicalRecordMedicine> MedicalRecordMedicines { get; set; }
        public DbSet<Entities.BedAvailabilityNotification> BedAvailabilityNotifications { get; set; }
        public DbSet<Entities.AppointmentReminder> AppointmentReminders { get; set; }
        public DbSet<Entities.EmergencyAlert> EmergencyAlerts { get; set; }
        public DbSet<Entities.PatientQueue> PatientQueues { get; set; }
        public DbSet<Entities.StaffPerformanceMetrics> StaffPerformanceMetrics { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure relationships
            modelBuilder.Entity<Entities.User>()
                .HasOne(u => u.Patient)
                .WithOne(p => p.User)
                .HasForeignKey<Entities.Patient>(p => p.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Entities.User>()
                .HasOne(u => u.Doctor)
                .WithOne(d => d.User)
                .HasForeignKey<Entities.Doctor>(d => d.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Entities.Appointment>()
                .HasOne(a => a.Patient)
                .WithMany(p => p.Appointments)
                .HasForeignKey(a => a.PatientId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Entities.Appointment>()
                .HasOne(a => a.Doctor)
                .WithMany(d => d.Appointments)
                .HasForeignKey(a => a.DoctorId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Entities.Billing>()
                .HasMany(b => b.BillItems)
                .WithOne(i => i.Billing)
                .HasForeignKey(i => i.BillId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Entities.Prescription>()
                .HasMany(p => p.PrescriptionItems)
                .WithOne(pi => pi.Prescription)
                .HasForeignKey(pi => pi.PrescriptionId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Entities.Department>()
                .HasMany(d => d.Doctors)
                .WithOne(dr => dr.Department)
                .HasForeignKey("DepartmentId")
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<Entities.Department>()
                .HasMany(d => d.Rooms)
                .WithOne(r => r.Department)
                .HasForeignKey(r => r.DepartmentId)
                .OnDelete(DeleteBehavior.SetNull);

            // RoomType -> Room (one-to-many)
            modelBuilder.Entity<Entities.RoomType>()
                .HasMany(rt => rt.Rooms)
                .WithOne(r => r.RoomType)
                .HasForeignKey(r => r.RoomTypeId)
                .OnDelete(DeleteBehavior.SetNull);

            // Ambulance -> AmbulanceLog
            modelBuilder.Entity<Entities.AmbulanceLog>()
                .HasOne(al => al.Ambulance)
                .WithMany(a => a.AmbulanceLogs)
                .HasForeignKey(al => al.AmbulanceId)
                .OnDelete(DeleteBehavior.Restrict);

            // Patient -> AmbulanceLog
            modelBuilder.Entity<Entities.AmbulanceLog>()
                .HasOne(al => al.Patient)
                .WithMany(p => p.AmbulanceLogs)
                .HasForeignKey(al => al.PatientId)
                .OnDelete(DeleteBehavior.SetNull);

            // Medicine -> Pharmacy
            modelBuilder.Entity<Entities.Pharmacy>()
                .HasOne(ph => ph.Medicine)
                .WithMany(m => m.PharmacyEntries)
                .HasForeignKey(ph => ph.MedicineId)
                .OnDelete(DeleteBehavior.Restrict);

            // Patient -> Pharmacy
            modelBuilder.Entity<Entities.Pharmacy>()
                .HasOne(ph => ph.Patient)
                .WithMany(p => p.PharmacyEntries)
                .HasForeignKey(ph => ph.PatientId)
                .OnDelete(DeleteBehavior.SetNull);

            // Room -> RoomAssignment
            modelBuilder.Entity<Entities.RoomAssignment>()
                .HasOne(ra => ra.Room)
                .WithMany(r => r.RoomAssignments)
                .HasForeignKey(ra => ra.RoomId)
                .OnDelete(DeleteBehavior.Restrict);

            // Room -> CleaningService
            modelBuilder.Entity<Entities.CleaningService>()
                .HasOne(cs => cs.Room)
                .WithMany(r => r.CleaningServices)
                .HasForeignKey(cs => cs.RoomId)
                .OnDelete(DeleteBehavior.Restrict);

            // DoctorDepartment join table
            modelBuilder.Entity<Entities.DoctorDepartment>()
                .HasOne(dd => dd.Doctor)
                .WithMany(d => d.DoctorDepartments)
                .HasForeignKey(dd => dd.DoctorId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Entities.DoctorDepartment>()
                .HasOne(dd => dd.Department)
                .WithMany()
                .HasForeignKey(dd => dd.DepartmentId)
                .OnDelete(DeleteBehavior.Cascade);

            // MedicalRecordMedicine join table
            modelBuilder.Entity<Entities.MedicalRecordMedicine>()
                .HasOne(mrm => mrm.MedicalRecord)
                .WithMany(mr => mr.MedicalRecordMedicines)
                .HasForeignKey(mrm => mrm.RecordId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Entities.MedicalRecordMedicine>()
                .HasOne(mrm => mrm.Medicine)
                .WithMany(m => m.MedicalRecordMedicines)
                .HasForeignKey(mrm => mrm.MedicineId)
                .OnDelete(DeleteBehavior.Restrict);

            // Configure decimal precision for SQLite
            modelBuilder.Entity<Entities.Billing>()
                .Property(b => b.TotalAmount)
                .HasConversion<double>();

            modelBuilder.Entity<Entities.Billing>()
                .Property(b => b.PaidAmount)
                .HasConversion<double>();

            modelBuilder.Entity<Entities.Billing>()
                .Property(b => b.BalanceDue)
                .HasConversion<double>();

            modelBuilder.Entity<Entities.Payment>()
                .Property(p => p.Amount)
                .HasConversion<double>();

            modelBuilder.Entity<Entities.Doctor>()
                .Property(d => d.ConsultationFee)
                .HasConversion<double>();

            modelBuilder.Entity<Entities.Doctor>()
                .Property(d => d.Rating)
                .HasConversion<double>();

            // Seed data
            SeedData(modelBuilder);
        }

        private void SeedData(ModelBuilder modelBuilder)
        {
            // Seed admin user (password: Admin@123)
            modelBuilder.Entity<User>().HasData(
                new User
                {
                    UserId = 1,
                    Username = "admin",
                    PasswordHash = "AQAAAAIAAYagAAAAECg8FLL1EfnVr9qWtsoeUFfccRYKxNK+9M0iVGTk6UqCV5HF8BRo1sKyyw7lLpRz1A==",
                    Email = "admin@hospital.com",
                    FullName = "System Administrator",
                    UserType = "Admin",
                    Status = "Active",
                    CreatedAt = DateTime.UtcNow,
                    IsEmailVerified = true
                },
                new User
                {
                    UserId = 2,
                    Username = "doctor",
                    PasswordHash = "AQAAAAIAAYagAAAAECg8FLL1EfnVr9qWtsoeUFfccRYKxNK+9M0iVGTk6UqCV5HF8BRo1sKyyw7lLpRz1A==",
                    Email = "doctor@hospital.com",
                    FullName = "Dr. John Smith",
                    UserType = "Doctor",
                    Status = "Active",
                    CreatedAt = DateTime.UtcNow,
                    IsEmailVerified = true
                },
                new User
                {
                    UserId = 3,
                    Username = "patient",
                    PasswordHash = "AQAAAAIAAYagAAAAECg8FLL1EfnVr9qWtsoeUFfccRYKxNK+9M0iVGTk6UqCV5HF8BRo1sKyyw7lLpRz1A==",
                    Email = "patient@hospital.com",
                    FullName = "John Doe",
                    UserType = "Patient",
                    Status = "Active",
                    CreatedAt = DateTime.UtcNow,
                    IsEmailVerified = true
                },
                new User
                {
                    UserId = 4,
                    Username = "reception",
                    PasswordHash = "AQAAAAIAAYagAAAAECg8FLL1EfnVr9qWtsoeUFfccRYKxNK+9M0iVGTk6UqCV5HF8BRo1sKyyw7lLpRz1A==",
                    Email = "reception@hospital.com",
                    FullName = "Sarah Johnson",
                    UserType = "Receptionist",
                    Status = "Active",
                    CreatedAt = DateTime.UtcNow,
                    IsEmailVerified = true
                },
                new User
                {
                    UserId = 5,
                    Username = "accountant",
                    PasswordHash = "AQAAAAIAAYagAAAAECg8FLL1EfnVr9qWtsoeUFfccRYKxNK+9M0iVGTk6UqCV5HF8BRo1sKyyw7lLpRz1A==",
                    Email = "accountant@hospital.com",
                    FullName = "Mike Wilson",
                    UserType = "Accountant",
                    Status = "Active",
                    CreatedAt = DateTime.UtcNow,
                    IsEmailVerified = true
                }
            );

            // Seed departments
            modelBuilder.Entity<Department>().HasData(
                new Department 
                { 
                    DepartmentId = 1, 
                    DepartmentName = "Cardiology", 
                    DepartmentCode = "CARD", 
                    Description = "Heart and cardiovascular care department", 
                    Location = "Floor 3, East Wing", 
                    TotalBeds = 50, 
                    AvailableBeds = 35 
                },
                new Department 
                { 
                    DepartmentId = 2, 
                    DepartmentName = "Neurology", 
                    DepartmentCode = "NEUR", 
                    Description = "Brain and nervous system disorders", 
                    Location = "Floor 4, West Wing", 
                    TotalBeds = 40, 
                    AvailableBeds = 25 
                },
                new Department 
                { 
                    DepartmentId = 3, 
                    DepartmentName = "Emergency", 
                    DepartmentCode = "EMER", 
                    Description = "24/7 emergency care", 
                    Location = "Floor 1, Main Building", 
                    TotalBeds = 30, 
                    AvailableBeds = 10 
                }
            );

            // Seed room types
            modelBuilder.Entity<RoomType>().HasData(
                new RoomType { RoomTypeId = 1, RoomTypeName = "General", Description = "Standard general ward" },
                new RoomType { RoomTypeId = 2, RoomTypeName = "ICU", Description = "Intensive Care Unit" },
                new RoomType { RoomTypeId = 3, RoomTypeName = "Emergency", Description = "Emergency ward" },
                new RoomType { RoomTypeId = 4, RoomTypeName = "Private", Description = "Private room" },
                new RoomType { RoomTypeId = 5, RoomTypeName = "Maternity", Description = "Maternity ward" }
            );

            // Seed blood bank
            modelBuilder.Entity<BloodBank>().HasData(
                new BloodBank { BloodId = 1, BloodType = "A+", StockQuantity = 20, LastUpdated = new DateTime(2026, 3, 1) },
                new BloodBank { BloodId = 2, BloodType = "A-", StockQuantity = 10, LastUpdated = new DateTime(2026, 3, 1) },
                new BloodBank { BloodId = 3, BloodType = "B+", StockQuantity = 15, LastUpdated = new DateTime(2026, 3, 1) },
                new BloodBank { BloodId = 4, BloodType = "B-", StockQuantity = 8,  LastUpdated = new DateTime(2026, 3, 1) },
                new BloodBank { BloodId = 5, BloodType = "O+", StockQuantity = 30, LastUpdated = new DateTime(2026, 3, 1) },
                new BloodBank { BloodId = 6, BloodType = "O-", StockQuantity = 12, LastUpdated = new DateTime(2026, 3, 1) },
                new BloodBank { BloodId = 7, BloodType = "AB+", StockQuantity = 9, LastUpdated = new DateTime(2026, 3, 1) },
                new BloodBank { BloodId = 8, BloodType = "AB-", StockQuantity = 5, LastUpdated = new DateTime(2026, 3, 1) }
            );

            // Seed insurance types
            modelBuilder.Entity<InsuranceType>().HasData(
                new InsuranceType { InsuranceId = 1, Name = "Mutuelle de Santé", Code = "MUTUELLE", CoveragePercentage = 90.00m },
                new InsuranceType { InsuranceId = 2, Name = "RAMA", Code = "RAMA", CoveragePercentage = 100.00m },
                new InsuranceType { InsuranceId = 3, Name = "MMI (Military)", Code = "MMI", CoveragePercentage = 100.00m },
                new InsuranceType { InsuranceId = 4, Name = "RSSB", Code = "RSSB", CoveragePercentage = 85.00m },
                new InsuranceType { InsuranceId = 5, Name = "Britam", Code = "BRITAM", CoveragePercentage = 80.00m },
                new InsuranceType { InsuranceId = 6, Name = "Sanlam", Code = "SANLAM", CoveragePercentage = 80.00m },
                new InsuranceType { InsuranceId = 7, Name = "UAP Insurance", Code = "UAP", CoveragePercentage = 85.00m }
            );

            // Seed rooms
            modelBuilder.Entity<Room>().HasData(
                new Room
                {
                    RoomId = 1,
                    RoomNumber = "101",
                    RoomTypeId = 1,
                    DepartmentId = 1,
                    FloorNumber = 3,
                    BedCount = 2,
                    AvailableBeds = 1,
                    RoomStatus = "Available",
                    DailyRate = 150.00m
                },
                new Room
                {
                    RoomId = 2,
                    RoomNumber = "102",
                    RoomTypeId = 1,
                    DepartmentId = 1,
                    FloorNumber = 3,
                    BedCount = 2,
                    AvailableBeds = 2,
                    RoomStatus = "Available",
                    DailyRate = 150.00m
                },
                new Room
                {
                    RoomId = 3,
                    RoomNumber = "ICU-01",
                    RoomTypeId = 2,
                    DepartmentId = 3,
                    FloorNumber = 1,
                    BedCount = 1,
                    AvailableBeds = 0,
                    RoomStatus = "Occupied",
                    DailyRate = 500.00m
                }
            );
        }
    }
}