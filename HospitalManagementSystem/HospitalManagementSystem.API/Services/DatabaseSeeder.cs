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
            await _context.SaveChangesAsync();
            await SeedDepartments();
            await _context.SaveChangesAsync();
            await SeedDoctors();
            await _context.SaveChangesAsync();
            await SeedPatients();
            await _context.SaveChangesAsync();
            await SeedRooms();
            await _context.SaveChangesAsync();
            await SeedInventory();
            await _context.SaveChangesAsync();
            await SeedAppointments();
            await _context.SaveChangesAsync();
            await SeedBillings();
            await _context.SaveChangesAsync();
        }

        private async Task SeedUsers()
        {
            if (await _context.Users.AnyAsync()) return;

            var passwordHash = HashPassword("Admin@123");

            var users = new List<User>
            {
                new User { Username = "admin", PasswordHash = passwordHash, Email = "admin@rutabo.rw", FullName = "System Administrator", UserType = "Admin", Status = "Active", CreatedAt = DateTime.UtcNow, IsEmailVerified = true },
                new User { Username = "dr.smith", PasswordHash = passwordHash, Email = "dr.smith@rutabo.rw", FullName = "Dr. John Smith", UserType = "Doctor", Specialization = "Cardiology", Department = "Cardiology", Status = "Active", CreatedAt = DateTime.UtcNow, IsEmailVerified = true, Gender = "Male", DateOfBirth = new DateTime(1975, 3, 15) },
                new User { Username = "dr.johnson", PasswordHash = passwordHash, Email = "dr.johnson@rutabo.rw", FullName = "Dr. Emily Johnson", UserType = "Doctor", Specialization = "Neurology", Department = "Neurology", Status = "Active", CreatedAt = DateTime.UtcNow, IsEmailVerified = true, Gender = "Female", DateOfBirth = new DateTime(1980, 7, 22) },
                new User { Username = "dr.brown", PasswordHash = passwordHash, Email = "dr.brown@rutabo.rw", FullName = "Dr. Michael Brown", UserType = "Doctor", Specialization = "Emergency Medicine", Department = "Emergency", Status = "Active", CreatedAt = DateTime.UtcNow, IsEmailVerified = true, Gender = "Male", DateOfBirth = new DateTime(1978, 11, 5) },
                new User { Username = "dr.davis", PasswordHash = passwordHash, Email = "dr.davis@rutabo.rw", FullName = "Dr. Sarah Davis", UserType = "Doctor", Specialization = "Pediatrics", Department = "Pediatrics", Status = "Active", CreatedAt = DateTime.UtcNow, IsEmailVerified = true, Gender = "Female", DateOfBirth = new DateTime(1982, 4, 18) },
                new User { Username = "nurse.alice", PasswordHash = passwordHash, Email = "alice.nurse@rutabo.rw", FullName = "Alice Uwimana", UserType = "Nurse", Department = "ICU", Status = "Active", CreatedAt = DateTime.UtcNow, IsEmailVerified = true, Gender = "Female" },
                new User { Username = "nurse.bob", PasswordHash = passwordHash, Email = "bob.nurse@rutabo.rw", FullName = "Bob Nkurunziza", UserType = "Nurse", Department = "Emergency", Status = "Active", CreatedAt = DateTime.UtcNow, IsEmailVerified = true, Gender = "Male" },
                new User { Username = "nurse.carol", PasswordHash = passwordHash, Email = "carol.nurse@rutabo.rw", FullName = "Carol Mukamana", UserType = "Nurse", Department = "Cardiology", Status = "Active", CreatedAt = DateTime.UtcNow, IsEmailVerified = true, Gender = "Female" },
                new User { Username = "reception", PasswordHash = passwordHash, Email = "reception@rutabo.rw", FullName = "Grace Ingabire", UserType = "Receptionist", Status = "Active", CreatedAt = DateTime.UtcNow, IsEmailVerified = true, Gender = "Female" },
                new User { Username = "accountant", PasswordHash = passwordHash, Email = "accountant@rutabo.rw", FullName = "Patrick Habimana", UserType = "Accountant", Status = "Active", CreatedAt = DateTime.UtcNow, IsEmailVerified = true, Gender = "Male" },
                // Patient users
                new User { Username = "patient.john", PasswordHash = passwordHash, Email = "john.doe@email.com", FullName = "John Doe", UserType = "Patient", Status = "Active", CreatedAt = DateTime.UtcNow, Gender = "Male", DateOfBirth = new DateTime(1988, 5, 12), PhoneNumber = "+250788001001" },
                new User { Username = "patient.jane", PasswordHash = passwordHash, Email = "jane.smith@email.com", FullName = "Jane Smith", UserType = "Patient", Status = "Active", CreatedAt = DateTime.UtcNow, Gender = "Female", DateOfBirth = new DateTime(1995, 8, 25), PhoneNumber = "+250788001002" },
                new User { Username = "patient.bob", PasswordHash = passwordHash, Email = "bob.wilson@email.com", FullName = "Bob Wilson", UserType = "Patient", Status = "Active", CreatedAt = DateTime.UtcNow, Gender = "Male", DateOfBirth = new DateTime(1972, 2, 14), PhoneNumber = "+250788001003" },
                new User { Username = "patient.alice", PasswordHash = passwordHash, Email = "alice.brown@email.com", FullName = "Alice Brown", UserType = "Patient", Status = "Active", CreatedAt = DateTime.UtcNow, Gender = "Female", DateOfBirth = new DateTime(1990, 11, 30), PhoneNumber = "+250788001004" },
                new User { Username = "patient.charlie", PasswordHash = passwordHash, Email = "charlie.davis@email.com", FullName = "Charlie Davis", UserType = "Patient", Status = "Active", CreatedAt = DateTime.UtcNow, Gender = "Male", DateOfBirth = new DateTime(1965, 7, 8), PhoneNumber = "+250788001005" },
                new User { Username = "patient.mary", PasswordHash = passwordHash, Email = "mary.jones@email.com", FullName = "Mary Jones", UserType = "Patient", Status = "Active", CreatedAt = DateTime.UtcNow, Gender = "Female", DateOfBirth = new DateTime(1983, 3, 19), PhoneNumber = "+250788001006" },
            };

            await _context.Users.AddRangeAsync(users);
        }

        private async Task SeedPatients()
        {
            if (await _context.Patients.AnyAsync()) return;

            var patientUsers = await _context.Users.Where(u => u.UserType == "Patient").ToListAsync();
            if (!patientUsers.Any()) return;

            var patients = new List<Patient>
            {
                new Patient { UserId = patientUsers[0].UserId, MedicalRecordNumber = "MRN-2024-001", BloodType = "O+", Height = 175, Weight = 72, EmergencyContactName = "Jane Doe", EmergencyContactPhone = "+250788002001", EmergencyContactRelation = "Spouse", InsuranceProvider = "RSSB", InsurancePolicyNumber = "RSSB-001-2024", Allergies = "Penicillin", ChronicConditions = "Hypertension", CurrentMedications = "Lisinopril 10mg", IsActive = true, AdmissionDate = DateTime.UtcNow.AddDays(-60) },
                new Patient { UserId = patientUsers[1].UserId, MedicalRecordNumber = "MRN-2024-002", BloodType = "A+", Height = 162, Weight = 58, EmergencyContactName = "Tom Smith", EmergencyContactPhone = "+250788002002", EmergencyContactRelation = "Father", InsuranceProvider = "MMI", InsurancePolicyNumber = "MMI-002-2024", Allergies = "None", ChronicConditions = "None", IsActive = true, AdmissionDate = DateTime.UtcNow.AddDays(-30) },
                new Patient { UserId = patientUsers[2].UserId, MedicalRecordNumber = "MRN-2024-003", BloodType = "B-", Height = 180, Weight = 85, EmergencyContactName = "Mary Wilson", EmergencyContactPhone = "+250788002003", EmergencyContactRelation = "Wife", InsuranceProvider = "RSSB", InsurancePolicyNumber = "RSSB-003-2024", Allergies = "Aspirin, Sulfa drugs", ChronicConditions = "Diabetes Type 2, Hypertension", CurrentMedications = "Metformin 500mg, Lisinopril 10mg", IsActive = true, AdmissionDate = DateTime.UtcNow.AddDays(-90) },
                new Patient { UserId = patientUsers[3].UserId, MedicalRecordNumber = "MRN-2024-004", BloodType = "AB+", Height = 165, Weight = 62, EmergencyContactName = "David Brown", EmergencyContactPhone = "+250788002004", EmergencyContactRelation = "Husband", InsuranceProvider = "Radiant", InsurancePolicyNumber = "RAD-004-2024", Allergies = "Latex", ChronicConditions = "Asthma", IsActive = true, AdmissionDate = DateTime.UtcNow.AddDays(-15) },
                new Patient { UserId = patientUsers[4].UserId, MedicalRecordNumber = "MRN-2024-005", BloodType = "O-", Height = 172, Weight = 78, EmergencyContactName = "Lisa Davis", EmergencyContactPhone = "+250788002005", EmergencyContactRelation = "Daughter", InsuranceProvider = "RSSB", InsurancePolicyNumber = "RSSB-005-2024", Allergies = "Codeine", ChronicConditions = "Arthritis, High Cholesterol", IsActive = true, AdmissionDate = DateTime.UtcNow.AddDays(-120) },
                new Patient { UserId = patientUsers[5].UserId, MedicalRecordNumber = "MRN-2024-006", BloodType = "A-", Height = 158, Weight = 55, EmergencyContactName = "Peter Jones", EmergencyContactPhone = "+250788002006", EmergencyContactRelation = "Brother", InsuranceProvider = "MMI", InsurancePolicyNumber = "MMI-006-2024", Allergies = "None", ChronicConditions = "Migraine", IsActive = true, AdmissionDate = DateTime.UtcNow.AddDays(-7) },
            };

            await _context.Patients.AddRangeAsync(patients);
        }

        private async Task SeedDoctors()
        {
            if (await _context.Doctors.AnyAsync()) return;

            var doctorUsers = await _context.Users.Where(u => u.UserType == "Doctor").ToListAsync();
            if (!doctorUsers.Any()) return;

            var doctors = new List<Doctor>
            {
                new Doctor { UserId = doctorUsers[0].UserId, LicenseNumber = "MD-RW-001", Qualifications = "MBBS, MD (Cardiology)", YearsOfExperience = 15, ConsultationFee = 15000, AvailableDays = "Mon,Tue,Wed,Thu,Fri", WorkingHours = "8:00-16:00", MaxPatientsPerDay = 20, IsAvailable = true, Rating = 4.8m, TotalRatings = 125 },
                new Doctor { UserId = doctorUsers[1].UserId, LicenseNumber = "MD-RW-002", Qualifications = "MBBS, MD (Neurology)", YearsOfExperience = 12, ConsultationFee = 15000, AvailableDays = "Mon,Wed,Fri", WorkingHours = "9:00-17:00", MaxPatientsPerDay = 15, IsAvailable = true, Rating = 4.7m, TotalRatings = 98 },
                new Doctor { UserId = doctorUsers[2].UserId, LicenseNumber = "MD-RW-003", Qualifications = "MBBS, Emergency Medicine Specialist", YearsOfExperience = 10, ConsultationFee = 12000, AvailableDays = "Mon,Tue,Wed,Thu,Fri,Sat,Sun", WorkingHours = "0:00-24:00", MaxPatientsPerDay = 30, IsAvailable = true, Rating = 4.9m, TotalRatings = 210 },
                new Doctor { UserId = doctorUsers[3].UserId, LicenseNumber = "MD-RW-004", Qualifications = "MBBS, MD (Pediatrics)", YearsOfExperience = 8, ConsultationFee = 12000, AvailableDays = "Mon,Tue,Thu,Fri", WorkingHours = "8:00-16:00", MaxPatientsPerDay = 25, IsAvailable = true, Rating = 4.6m, TotalRatings = 87 },
            };

            await _context.Doctors.AddRangeAsync(doctors);
        }

        private async Task SeedDepartments()
        {
            if (await _context.Departments.AnyAsync()) return;

            var departments = new List<Department>
            {
                new Department { DepartmentName = "Cardiology", DepartmentCode = "CARD", Description = "Heart and cardiovascular care", PhoneExtension = "1001", Email = "cardiology@rutabo.rw", Location = "Floor 3, East Wing", TotalBeds = 50, AvailableBeds = 35, Status = "Active", CreatedAt = DateTime.UtcNow },
                new Department { DepartmentName = "Neurology", DepartmentCode = "NEUR", Description = "Brain and nervous system disorders", PhoneExtension = "1002", Email = "neurology@rutabo.rw", Location = "Floor 4, West Wing", TotalBeds = 40, AvailableBeds = 25, Status = "Active", CreatedAt = DateTime.UtcNow },
                new Department { DepartmentName = "Emergency", DepartmentCode = "EMER", Description = "24/7 emergency care", PhoneExtension = "1003", Email = "emergency@rutabo.rw", Location = "Floor 1, Main Building", TotalBeds = 30, AvailableBeds = 10, Status = "Active", CreatedAt = DateTime.UtcNow },
                new Department { DepartmentName = "Pediatrics", DepartmentCode = "PEDI", Description = "Children's healthcare", PhoneExtension = "1004", Email = "pediatrics@rutabo.rw", Location = "Floor 2, North Wing", TotalBeds = 35, AvailableBeds = 20, Status = "Active", CreatedAt = DateTime.UtcNow },
                new Department { DepartmentName = "General Medicine", DepartmentCode = "GENM", Description = "General outpatient and inpatient care", PhoneExtension = "1005", Email = "general@rutabo.rw", Location = "Floor 2, South Wing", TotalBeds = 60, AvailableBeds = 40, Status = "Active", CreatedAt = DateTime.UtcNow },
                new Department { DepartmentName = "Surgery", DepartmentCode = "SURG", Description = "Surgical procedures and post-op care", PhoneExtension = "1006", Email = "surgery@rutabo.rw", Location = "Floor 5, West Wing", TotalBeds = 25, AvailableBeds = 15, Status = "Active", CreatedAt = DateTime.UtcNow },
            };

            await _context.Departments.AddRangeAsync(departments);
        }

        private async Task SeedRooms()
        {
            if (await _context.Rooms.AnyAsync()) return;

            var cardiology = await _context.Departments.FirstOrDefaultAsync(d => d.DepartmentCode == "CARD");
            var emergency = await _context.Departments.FirstOrDefaultAsync(d => d.DepartmentCode == "EMER");
            var pediatrics = await _context.Departments.FirstOrDefaultAsync(d => d.DepartmentCode == "PEDI");
            var general = await _context.Departments.FirstOrDefaultAsync(d => d.DepartmentCode == "GENM");

            var rooms = new List<Room>
            {
                new Room { RoomNumber = "101", RoomTypeId = null, DepartmentId = cardiology?.DepartmentId, FloorNumber = 3, BedCount = 2, AvailableBeds = 1, RoomStatus = "Available", Equipment = "Bed, Monitor, Oxygen", DailyRate = 15000, Features = "Private Bathroom, TV, WiFi" },
                new Room { RoomNumber = "102", RoomTypeId = null, DepartmentId = cardiology?.DepartmentId, FloorNumber = 3, BedCount = 2, AvailableBeds = 2, RoomStatus = "Available", Equipment = "Bed, Monitor", DailyRate = 12000, Features = "Shared Bathroom, TV" },
                new Room { RoomNumber = "ICU-01", RoomTypeId = null, DepartmentId = emergency?.DepartmentId, FloorNumber = 1, BedCount = 1, AvailableBeds = 0, RoomStatus = "Occupied", Equipment = "Ventilator, Monitor, IV Pump, Defibrillator", DailyRate = 50000, Features = "Critical Care, 24/7 Monitoring" },
                new Room { RoomNumber = "ICU-02", RoomTypeId = null, DepartmentId = emergency?.DepartmentId, FloorNumber = 1, BedCount = 1, AvailableBeds = 1, RoomStatus = "Available", Equipment = "Ventilator, Monitor, IV Pump", DailyRate = 50000, Features = "Critical Care, 24/7 Monitoring" },
                new Room { RoomNumber = "PEDI-01", RoomTypeId = null, DepartmentId = pediatrics?.DepartmentId, FloorNumber = 2, BedCount = 4, AvailableBeds = 2, RoomStatus = "Available", Equipment = "Pediatric Beds, Monitor", DailyRate = 10000, Features = "Child-friendly, Play Area" },
                new Room { RoomNumber = "GEN-201", RoomTypeId = null, DepartmentId = general?.DepartmentId, FloorNumber = 2, BedCount = 6, AvailableBeds = 3, RoomStatus = "Available", Equipment = "Beds, Basic Monitor", DailyRate = 8000, Features = "Shared Ward" },
                new Room { RoomNumber = "GEN-202", RoomTypeId = null, DepartmentId = general?.DepartmentId, FloorNumber = 2, BedCount = 6, AvailableBeds = 6, RoomStatus = "Available", Equipment = "Beds, Basic Monitor", DailyRate = 8000, Features = "Shared Ward" },
            };

            await _context.Rooms.AddRangeAsync(rooms);
        }

        private async Task SeedInventory()
        {
            if (await _context.Inventory.AnyAsync()) return;

            var items = new List<Inventory>
            {
                new Inventory { ItemCode = "MED001", ItemName = "Amoxicillin 500mg", Category = "Medication", Description = "Antibiotic capsules", Unit = "Tablets", CurrentStock = 500, MinimumStock = 100, MaximumStock = 1000, UnitPrice = 150, Supplier = "PharmaCorp RW", SupplierContact = "pharma@rutabo.rw", LastRestocked = DateTime.UtcNow.AddDays(-10), ExpiryDate = DateTime.UtcNow.AddYears(1), StorageLocation = "Pharmacy Shelf A1", Status = "Active" },
                new Inventory { ItemCode = "MED002", ItemName = "Ibuprofen 400mg", Category = "Medication", Description = "Pain relief tablets", Unit = "Tablets", CurrentStock = 800, MinimumStock = 200, MaximumStock = 1500, UnitPrice = 80, Supplier = "MediSupply RW", SupplierContact = "medi@rutabo.rw", LastRestocked = DateTime.UtcNow.AddDays(-5), ExpiryDate = DateTime.UtcNow.AddYears(2), StorageLocation = "Pharmacy Shelf A2", Status = "Active" },
                new Inventory { ItemCode = "MED003", ItemName = "Metformin 500mg", Category = "Medication", Description = "Diabetes medication", Unit = "Tablets", CurrentStock = 300, MinimumStock = 100, MaximumStock = 600, UnitPrice = 200, Supplier = "PharmaCorp RW", SupplierContact = "pharma@rutabo.rw", LastRestocked = DateTime.UtcNow.AddDays(-20), ExpiryDate = DateTime.UtcNow.AddYears(1), StorageLocation = "Pharmacy Shelf B1", Status = "Active" },
                new Inventory { ItemCode = "MED004", ItemName = "Lisinopril 10mg", Category = "Medication", Description = "Blood pressure medication", Unit = "Tablets", CurrentStock = 50, MinimumStock = 100, MaximumStock = 500, UnitPrice = 300, Supplier = "MediSupply RW", SupplierContact = "medi@rutabo.rw", LastRestocked = DateTime.UtcNow.AddDays(-45), ExpiryDate = DateTime.UtcNow.AddMonths(8), StorageLocation = "Pharmacy Shelf B2", Status = "Active" },
                new Inventory { ItemCode = "EQP001", ItemName = "Stethoscope", Category = "Equipment", Description = "Dual-head stethoscope", Unit = "Piece", CurrentStock = 50, MinimumStock = 20, MaximumStock = 100, UnitPrice = 45000, Supplier = "MedEquip Inc.", SupplierContact = "equip@rutabo.rw", LastRestocked = DateTime.UtcNow.AddDays(-30), StorageLocation = "Equipment Room B1", Status = "Active" },
                new Inventory { ItemCode = "EQP002", ItemName = "Blood Pressure Monitor", Category = "Equipment", Description = "Digital BP monitor", Unit = "Piece", CurrentStock = 25, MinimumStock = 10, MaximumStock = 50, UnitPrice = 35000, Supplier = "MedEquip Inc.", SupplierContact = "equip@rutabo.rw", LastRestocked = DateTime.UtcNow.AddDays(-60), StorageLocation = "Equipment Room B2", Status = "Active" },
                new Inventory { ItemCode = "SUP001", ItemName = "Surgical Gloves (M)", Category = "Supplies", Description = "Latex-free surgical gloves", Unit = "Box (100)", CurrentStock = 200, MinimumStock = 50, MaximumStock = 400, UnitPrice = 5000, Supplier = "MedSupplies RW", SupplierContact = "supplies@rutabo.rw", LastRestocked = DateTime.UtcNow.AddDays(-7), ExpiryDate = DateTime.UtcNow.AddYears(3), StorageLocation = "Supply Room C1", Status = "Active" },
                new Inventory { ItemCode = "SUP002", ItemName = "Syringes 5ml", Category = "Supplies", Description = "Disposable syringes", Unit = "Box (100)", CurrentStock = 15, MinimumStock = 50, MaximumStock = 300, UnitPrice = 3000, Supplier = "MedSupplies RW", SupplierContact = "supplies@rutabo.rw", LastRestocked = DateTime.UtcNow.AddDays(-14), ExpiryDate = DateTime.UtcNow.AddYears(2), StorageLocation = "Supply Room C2", Status = "Active" },
            };

            await _context.Inventory.AddRangeAsync(items);
        }

        private async Task SeedAppointments()
        {
            if (await _context.Appointments.AnyAsync()) return;

            var patients = await _context.Patients.ToListAsync();
            var doctors = await _context.Doctors.ToListAsync();
            if (!patients.Any() || !doctors.Any()) return;

            var appointments = new List<Appointment>
            {
                new Appointment { AppointmentNumber = "APT-2024-001", PatientId = patients[0].PatientId, DoctorId = doctors[0].DoctorId, AppointmentDate = DateTime.Today, AppointmentTime = new TimeSpan(9, 0, 0), AppointmentType = "Consultation", Status = "Scheduled", Reason = "Chest pain follow-up", DurationMinutes = 30, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
                new Appointment { AppointmentNumber = "APT-2024-002", PatientId = patients[1].PatientId, DoctorId = doctors[1].DoctorId, AppointmentDate = DateTime.Today, AppointmentTime = new TimeSpan(10, 30, 0), AppointmentType = "Follow-up", Status = "Confirmed", Reason = "Headache and dizziness", DurationMinutes = 30, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
                new Appointment { AppointmentNumber = "APT-2024-003", PatientId = patients[2].PatientId, DoctorId = doctors[0].DoctorId, AppointmentDate = DateTime.Today, AppointmentTime = new TimeSpan(11, 0, 0), AppointmentType = "Routine Checkup", Status = "Completed", Reason = "Diabetes management", Diagnosis = "Blood sugar controlled", DurationMinutes = 45, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
                new Appointment { AppointmentNumber = "APT-2024-004", PatientId = patients[3].PatientId, DoctorId = doctors[3 % doctors.Count].DoctorId, AppointmentDate = DateTime.Today.AddDays(1), AppointmentTime = new TimeSpan(9, 0, 0), AppointmentType = "Consultation", Status = "Scheduled", Reason = "Asthma review", DurationMinutes = 30, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
                new Appointment { AppointmentNumber = "APT-2024-005", PatientId = patients[4].PatientId, DoctorId = doctors[2 % doctors.Count].DoctorId, AppointmentDate = DateTime.Today.AddDays(-1), AppointmentTime = new TimeSpan(14, 0, 0), AppointmentType = "Emergency", Status = "Completed", Reason = "Severe joint pain", Diagnosis = "Arthritis flare-up", DurationMinutes = 60, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
                new Appointment { AppointmentNumber = "APT-2024-006", PatientId = patients[5].PatientId, DoctorId = doctors[1].DoctorId, AppointmentDate = DateTime.Today.AddDays(2), AppointmentTime = new TimeSpan(15, 0, 0), AppointmentType = "Follow-up", Status = "Scheduled", Reason = "Migraine treatment review", DurationMinutes = 30, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
            };

            await _context.Appointments.AddRangeAsync(appointments);
        }

        private async Task SeedBillings()
        {
            if (await _context.Billings.AnyAsync()) return;

            var patients = await _context.Patients.ToListAsync();
            if (!patients.Any()) return;

            var billings = new List<Billing>
            {
                new Billing { BillNumber = "BILL-2024-001", PatientId = patients[0].PatientId, BillDate = DateTime.UtcNow.AddDays(-10), DueDate = DateTime.UtcNow.AddDays(20), TotalAmount = 45000, TaxAmount = 0, DiscountAmount = 0, PaidAmount = 45000, BalanceDue = 0, PaymentStatus = "Paid", InsuranceProvider = "RSSB", InsuranceCoverage = 30000, ClaimStatus = "Approved", CreatedAt = DateTime.UtcNow },
                new Billing { BillNumber = "BILL-2024-002", PatientId = patients[1].PatientId, BillDate = DateTime.UtcNow.AddDays(-5), DueDate = DateTime.UtcNow.AddDays(25), TotalAmount = 28000, TaxAmount = 0, DiscountAmount = 2000, PaidAmount = 0, BalanceDue = 26000, PaymentStatus = "Pending", InsuranceProvider = "MMI", InsuranceCoverage = 15000, ClaimStatus = "Submitted", CreatedAt = DateTime.UtcNow },
                new Billing { BillNumber = "BILL-2024-003", PatientId = patients[2].PatientId, BillDate = DateTime.UtcNow.AddDays(-35), DueDate = DateTime.UtcNow.AddDays(-5), TotalAmount = 120000, TaxAmount = 0, DiscountAmount = 0, PaidAmount = 60000, BalanceDue = 60000, PaymentStatus = "Partial", InsuranceProvider = "RSSB", InsuranceCoverage = 80000, ClaimStatus = "Approved", CreatedAt = DateTime.UtcNow },
                new Billing { BillNumber = "BILL-2024-004", PatientId = patients[3].PatientId, BillDate = DateTime.UtcNow.AddDays(-2), DueDate = DateTime.UtcNow.AddDays(28), TotalAmount = 35000, TaxAmount = 0, DiscountAmount = 0, PaidAmount = 35000, BalanceDue = 0, PaymentStatus = "Paid", InsuranceProvider = "Radiant", InsuranceCoverage = 20000, ClaimStatus = "Approved", CreatedAt = DateTime.UtcNow },
                new Billing { BillNumber = "BILL-2024-005", PatientId = patients[4].PatientId, BillDate = DateTime.UtcNow.AddDays(-60), DueDate = DateTime.UtcNow.AddDays(-30), TotalAmount = 85000, TaxAmount = 0, DiscountAmount = 5000, PaidAmount = 0, BalanceDue = 80000, PaymentStatus = "Pending", InsuranceProvider = "RSSB", InsuranceCoverage = 60000, ClaimStatus = "Not Submitted", CreatedAt = DateTime.UtcNow },
            };

            await _context.Billings.AddRangeAsync(billings);
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