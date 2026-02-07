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

        public DbSet<User> Users { get; set; }
        public DbSet<Patient> Patients { get; set; }
        public DbSet<Doctor> Doctors { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Room> Rooms { get; set; }
        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<MedicalRecord> MedicalRecords { get; set; }
        public DbSet<Billing> Billings { get; set; }
        public DbSet<BillItem> BillItems { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<Inventory> Inventory { get; set; }
        public DbSet<AuditLog> AuditLogs { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<Prescription> Prescriptions { get; set; }
        public DbSet<PrescriptionItem> PrescriptionItems { get; set; }
        public DbSet<LabTest> LabTests { get; set; }
        public DbSet<Schedule> Schedules { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure relationships
            modelBuilder.Entity<User>()
                .HasOne(u => u.Patient)
                .WithOne(p => p.User)
                .HasForeignKey<Patient>(p => p.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<User>()
                .HasOne(u => u.Doctor)
                .WithOne(d => d.User)
                .HasForeignKey<Doctor>(d => d.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Appointment>()
                .HasOne(a => a.Patient)
                .WithMany(p => p.Appointments)
                .HasForeignKey(a => a.PatientId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Appointment>()
                .HasOne(a => a.Doctor)
                .WithMany(d => d.Appointments)
                .HasForeignKey(a => a.DoctorId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Billing>()
                .HasMany(b => b.BillItems)
                .WithOne(i => i.Billing)
                .HasForeignKey(i => i.BillId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Prescription>()
                .HasMany(p => p.PrescriptionItems)
                .WithOne(pi => pi.Prescription)
                .HasForeignKey(pi => pi.PrescriptionId)
                .OnDelete(DeleteBehavior.Cascade);

            // Configure decimal precision for SQLite
            modelBuilder.Entity<Billing>()
                .Property(b => b.TotalAmount)
                .HasConversion<double>();

            modelBuilder.Entity<Billing>()
                .Property(b => b.PaidAmount)
                .HasConversion<double>();

            modelBuilder.Entity<Billing>()
                .Property(b => b.BalanceDue)
                .HasConversion<double>();

            modelBuilder.Entity<Payment>()
                .Property(p => p.Amount)
                .HasConversion<double>();

            modelBuilder.Entity<Doctor>()
                .Property(d => d.ConsultationFee)
                .HasConversion<double>();

            modelBuilder.Entity<Doctor>()
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