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

            // Seed rooms
            modelBuilder.Entity<Room>().HasData(
                new Room
                {
                    RoomId = 1,
                    RoomNumber = "101",
                    RoomType = "General",
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
                    RoomType = "General",
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
                    RoomType = "ICU",
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