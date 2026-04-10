using HospitalManagementSystem.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;

namespace HospitalManagementSystem.Data.Services
{
    public interface IDatabaseSeeder
    {
        Task SeedAsync();
    }

    public class DatabaseSeeder : IDatabaseSeeder
    {
        private readonly ApplicationDbContext _context;

        public DatabaseSeeder(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task SeedAsync()
        {
            await SeedUsers();
            await SeedPatients();
            await SeedDoctors();
            await SeedDepartments();
            await SeedRooms();
            await SeedInventory();
            
            await _context.SaveChangesAsync();
        }

        private async Task SeedUsers()
        {
            if (!await _context.Users.AnyAsync())
            {
                // Password hash for "Admin@123"
                var passwordHash = HashPassword("Admin@123");
                
                var users = new List<User>
                {
                    new User
                    {
                        Username = "admin",
                        PasswordHash = passwordHash,
                        Email = "admin@hospital.com",
                        FullName = "System Administrator",
                        UserType = "Admin",
                        Status = "Active",
                        CreatedAt = DateTime.UtcNow,
                        IsEmailVerified = true
                    },
                    new User
                    {
                        Username = "doctor",
                        PasswordHash = passwordHash,
                        Email = "doctor@hospital.com",
                        FullName = "Dr. John Smith",
                        UserType = "Doctor",
                        Status = "Active",
                        CreatedAt = DateTime.UtcNow,
                        IsEmailVerified = true
                    },
                    new User
                    {
                        Username = "patient",
                        PasswordHash = passwordHash,
                        Email = "patient@hospital.com",
                        FullName = "John Doe",
                        UserType = "Patient",
                        Status = "Active",
                        CreatedAt = DateTime.UtcNow,
                        IsEmailVerified = true
                    },
                    new User
                    {
                        Username = "reception",
                        PasswordHash = passwordHash,
                        Email = "reception@hospital.com",
                        FullName = "Sarah Johnson",
                        UserType = "Receptionist",
                        Status = "Active",
                        CreatedAt = DateTime.UtcNow,
                        IsEmailVerified = true
                    },
                    new User
                    {
                        Username = "accountant",
                        PasswordHash = passwordHash,
                        Email = "accountant@hospital.com",
                        FullName = "Mike Wilson",
                        UserType = "Accountant",
                        Status = "Active",
                        CreatedAt = DateTime.UtcNow,
                        IsEmailVerified = true
                    }
                };

                await _context.Users.AddRangeAsync(users);
            }
        }

        private async Task SeedPatients()
        {
            if (!await _context.Patients.AnyAsync())
            {
                var adminUser = await _context.Users.FirstOrDefaultAsync(u => u.Username == "admin");
                var patientUser = await _context.Users.FirstOrDefaultAsync(u => u.Username == "patient");

                if (patientUser != null)
                {
                    var patient = new Patient
                    {
                        UserId = patientUser.UserId,
                        MedicalRecordNumber = "MRN2024001",
                        BloodType = "O+",
                        Height = 175.5m,
                        Weight = 70.2m,
                        EmergencyContactName = "Jane Doe",
                        EmergencyContactPhone = "555-1234",
                        EmergencyContactRelation = "Spouse",
                        PrimaryCarePhysician = "Dr. John Smith",
                        InsuranceProvider = "Blue Cross",
                        InsurancePolicyNumber = "BC123456789",
                        Allergies = "Penicillin, Peanuts",
                        ChronicConditions = "Hypertension",
                        CurrentMedications = "Lisinopril 10mg daily",
                        AdmissionDate = DateTime.UtcNow.AddDays(-30),
                        IsActive = true,
                        Notes = "Regular follow-up required"
                    };

                    await _context.Patients.AddAsync(patient);
                }
            }
        }

        private async Task SeedDoctors()
        {
            if (!await _context.Doctors.AnyAsync())
            {
                var doctorUser = await _context.Users.FirstOrDefaultAsync(u => u.Username == "doctor");

                if (doctorUser != null)
                {
                    var doctor = new Doctor
                    {
                        UserId = doctorUser.UserId,
                        LicenseNumber = "MD123456",
                        Qualifications = "MBBS, MD (Cardiology)",
                        YearsOfExperience = 15,
                        ConsultationFee = 150.00m,
                        AvailableDays = "Mon,Wed,Fri",
                        WorkingHours = "9:00-17:00",
                        MaxPatientsPerDay = 20,
                        IsAvailable = true,
                        Rating = 4.8m,
                        TotalRatings = 125,
                        Biography = "Senior Cardiologist with 15 years of experience..."
                    };

                    await _context.Doctors.AddAsync(doctor);
                }
            }
        }

        private async Task SeedDepartments()
        {
            if (!await _context.Departments.AnyAsync())
            {
                var departments = new List<Department>
                {
                    new Department
                    {
                        DepartmentName = "Cardiology",
                        DepartmentCode = "CARD",
                        Description = "Heart and cardiovascular care department",
                        PhoneExtension = "1001",
                        Email = "cardiology@hospital.com",
                        Location = "Floor 3, East Wing",
                        TotalBeds = 50,
                        AvailableBeds = 35,
                        Status = "Active",
                        CreatedAt = DateTime.UtcNow
                    },
                    new Department
                    {
                        DepartmentName = "Neurology",
                        DepartmentCode = "NEUR",
                        Description = "Brain and nervous system disorders",
                        PhoneExtension = "1002",
                        Email = "neurology@hospital.com",
                        Location = "Floor 4, West Wing",
                        TotalBeds = 40,
                        AvailableBeds = 25,
                        Status = "Active",
                        CreatedAt = DateTime.UtcNow
                    },
                    new Department
                    {
                        DepartmentName = "Emergency",
                        DepartmentCode = "EMER",
                        Description = "24/7 emergency care",
                        PhoneExtension = "1003",
                        Email = "emergency@hospital.com",
                        Location = "Floor 1, Main Building",
                        TotalBeds = 30,
                        AvailableBeds = 10,
                        Status = "Active",
                        CreatedAt = DateTime.UtcNow
                    }
                };

                await _context.Departments.AddRangeAsync(departments);
            }
        }

        private async Task SeedRooms()
        {
            if (!await _context.Rooms.AnyAsync())
            {
                var cardiologyDept = await _context.Departments.FirstOrDefaultAsync(d => d.DepartmentCode == "CARD");
                var emergencyDept = await _context.Departments.FirstOrDefaultAsync(d => d.DepartmentCode == "EMER");

                var rooms = new List<Room>
                {
                    new Room
                    {
                        RoomNumber = "101",
                        RoomTypeId = null,
                        DepartmentId = cardiologyDept?.DepartmentId,
                        FloorNumber = 3,
                        BedCount = 2,
                        AvailableBeds = 1,
                        RoomStatus = "Available",
                        Equipment = "Bed, Monitor, Oxygen",
                        DailyRate = 150.00m,
                        Features = "Private Bathroom, TV, WiFi"
                    },
                    new Room
                    {
                        RoomNumber = "102",
                        RoomTypeId = null,
                        DepartmentId = cardiologyDept?.DepartmentId,
                        FloorNumber = 3,
                        BedCount = 2,
                        AvailableBeds = 2,
                        RoomStatus = "Available",
                        Equipment = "Bed, Monitor",
                        DailyRate = 120.00m,
                        Features = "Shared Bathroom, TV"
                    },
                    new Room
                    {
                        RoomNumber = "ICU-01",
                        RoomTypeId = null,
                        DepartmentId = emergencyDept?.DepartmentId,
                        FloorNumber = 1,
                        BedCount = 1,
                        AvailableBeds = 0,
                        RoomStatus = "Occupied",
                        Equipment = "Ventilator, Monitor, IV Pump, Defibrillator",
                        DailyRate = 500.00m,
                        Features = "Critical Care, 24/7 Monitoring"
                    }
                };

                await _context.Rooms.AddRangeAsync(rooms);
            }
        }

        private async Task SeedInventory()
        {
            if (!await _context.Inventory.AnyAsync())
            {
                var inventoryItems = new List<Inventory>
                {
                    new Inventory
                    {
                        ItemCode = "MED001",
                        ItemName = "Amoxicillin 500mg",
                        Category = "Medication",
                        Description = "Antibiotic capsules",
                        Unit = "Tablets",
                        CurrentStock = 500,
                        MinimumStock = 100,
                        MaximumStock = 1000,
                        UnitPrice = 0.50m,
                        Supplier = "PharmaCorp",
                        SupplierContact = "supplier@pharmacorp.com",
                        LastRestocked = DateTime.UtcNow.AddDays(-10),
                        ExpiryDate = DateTime.UtcNow.AddYears(1),
                        StorageLocation = "Pharmacy Shelf A1",
                        Status = "Active"
                    },
                    new Inventory
                    {
                        ItemCode = "MED002",
                        ItemName = "Ibuprofen 400mg",
                        Category = "Medication",
                        Description = "Pain relief tablets",
                        Unit = "Tablets",
                        CurrentStock = 800,
                        MinimumStock = 200,
                        MaximumStock = 1500,
                        UnitPrice = 0.25m,
                        Supplier = "MediSupply",
                        SupplierContact = "contact@medisupply.com",
                        LastRestocked = DateTime.UtcNow.AddDays(-5),
                        ExpiryDate = DateTime.UtcNow.AddYears(2),
                        StorageLocation = "Pharmacy Shelf A2",
                        Status = "Active"
                    },
                    new Inventory
                    {
                        ItemCode = "EQP001",
                        ItemName = "Stethoscope",
                        Category = "Equipment",
                        Description = "Dual-head stethoscope",
                        Unit = "Piece",
                        CurrentStock = 50,
                        MinimumStock = 20,
                        MaximumStock = 100,
                        UnitPrice = 45.00m,
                        Supplier = "MedEquip Inc.",
                        SupplierContact = "sales@medequip.com",
                        LastRestocked = DateTime.UtcNow.AddDays(-30),
                        StorageLocation = "Equipment Room B1",
                        Status = "Active"
                    }
                };

                await _context.Inventory.AddRangeAsync(inventoryItems);
            }
        }

        private string HashPassword(string password)
        {
            using var sha256 = SHA256.Create();
            var bytes = Encoding.UTF8.GetBytes(password);
            var hash = sha256.ComputeHash(bytes);
            return Convert.ToBase64String(hash);
        }
    }
}