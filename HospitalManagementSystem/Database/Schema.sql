-- Create Database
CREATE DATABASE HospitalDB;
GO

USE HospitalDB;
GO

-- 1. Users Table (All system users)
CREATE TABLE Users (
    UserId INT PRIMARY KEY IDENTITY(1,1),
    Username NVARCHAR(50) UNIQUE NOT NULL,
    PasswordHash NVARCHAR(255) NOT NULL,
    Email NVARCHAR(100) UNIQUE NOT NULL,
    FullName NVARCHAR(100) NOT NULL,
    UserType NVARCHAR(20) NOT NULL CHECK (UserType IN ('Admin', 'Doctor', 'Nurse', 'Patient', 'Receptionist', 'Accountant')),
    Specialization NVARCHAR(100),
    Department NVARCHAR(100),
    PhoneNumber NVARCHAR(15),
    Address NVARCHAR(255),
    DateOfBirth DATE,
    Gender NVARCHAR(10) CHECK (Gender IN ('Male', 'Female', 'Other')),
    Status NVARCHAR(20) DEFAULT 'Active' CHECK (Status IN ('Active', 'Inactive', 'Pending', 'Suspended')),
    CreatedAt DATETIME DEFAULT GETDATE(),
    LastLogin DATETIME,
    ProfileImage NVARCHAR(500),
    IsEmailVerified BIT DEFAULT 0,
    TwoFactorEnabled BIT DEFAULT 0
);

-- 2. Patients Table
CREATE TABLE Patients (
    PatientId INT PRIMARY KEY IDENTITY(1,1),
    UserId INT UNIQUE FOREIGN KEY REFERENCES Users(UserId),
    MedicalRecordNumber NVARCHAR(20) UNIQUE NOT NULL,
    BloodType NVARCHAR(5),
    Height DECIMAL(5,2), -- in cm
    Weight DECIMAL(5,2), -- in kg
    EmergencyContactName NVARCHAR(100),
    EmergencyContactPhone NVARCHAR(15),
    EmergencyContactRelation NVARCHAR(50),
    PrimaryCarePhysician NVARCHAR(100),
    InsuranceProvider NVARCHAR(100),
    InsurancePolicyNumber NVARCHAR(50),
    Allergies NVARCHAR(500),
    ChronicConditions NVARCHAR(500),
    CurrentMedications NVARCHAR(500),
    AdmissionDate DATETIME DEFAULT GETDATE(),
    DischargeDate DATETIME,
    IsActive BIT DEFAULT 1,
    Notes NVARCHAR(1000)
);

-- 3. Doctors Table
CREATE TABLE Doctors (
    DoctorId INT PRIMARY KEY IDENTITY(1,1),
    UserId INT UNIQUE FOREIGN KEY REFERENCES Users(UserId),
    LicenseNumber NVARCHAR(50) UNIQUE NOT NULL,
    Qualifications NVARCHAR(500),
    YearsOfExperience INT DEFAULT 0,
    ConsultationFee DECIMAL(10,2) DEFAULT 0,
    AvailableDays NVARCHAR(100), -- e.g., "Mon,Wed,Fri"
    WorkingHours NVARCHAR(50), -- e.g., "9:00-17:00"
    MaxPatientsPerDay INT DEFAULT 20,
    IsAvailable BIT DEFAULT 1,
    Rating DECIMAL(3,2) DEFAULT 0,
    TotalRatings INT DEFAULT 0,
    Biography NVARCHAR(MAX)
);

-- 4. Departments Table
CREATE TABLE Departments (
    DepartmentId INT PRIMARY KEY IDENTITY(1,1),
    DepartmentName NVARCHAR(100) NOT NULL,
    DepartmentCode NVARCHAR(20) UNIQUE NOT NULL,
    Description NVARCHAR(500),
    HeadDoctorId INT FOREIGN KEY REFERENCES Doctors(DoctorId),
    PhoneExtension NVARCHAR(10),
    Email NVARCHAR(100),
    Location NVARCHAR(100),
    TotalBeds INT DEFAULT 0,
    AvailableBeds INT DEFAULT 0,
    Status NVARCHAR(20) DEFAULT 'Active' CHECK (Status IN ('Active', 'Under Maintenance', 'Closed')),
    CreatedAt DATETIME DEFAULT GETDATE()
);

-- 5. Rooms Table
CREATE TABLE Rooms (
    RoomId INT PRIMARY KEY IDENTITY(1,1),
    RoomNumber NVARCHAR(20) UNIQUE NOT NULL,
    RoomType NVARCHAR(50) CHECK (RoomType IN ('General', 'ICU', 'Emergency', 'Operation', 'Private', 'Semi-Private', 'Pediatric', 'Maternity')),
    DepartmentId INT FOREIGN KEY REFERENCES Departments(DepartmentId),
    FloorNumber INT,
    BedCount INT DEFAULT 1,
    AvailableBeds INT DEFAULT 1,
    RoomStatus NVARCHAR(20) DEFAULT 'Available' CHECK (RoomStatus IN ('Available', 'Occupied', 'Cleaning', 'Maintenance', 'Reserved')),
    Equipment NVARCHAR(500),
    DailyRate DECIMAL(10,2) DEFAULT 0,
    Features NVARCHAR(500),
    Notes NVARCHAR(500)
);

-- 6. Appointments Table
CREATE TABLE Appointments (
    AppointmentId INT PRIMARY KEY IDENTITY(1,1),
    AppointmentNumber NVARCHAR(20) UNIQUE NOT NULL,
    PatientId INT FOREIGN KEY REFERENCES Patients(PatientId),
    DoctorId INT FOREIGN KEY REFERENCES Doctors(DoctorId),
    DepartmentId INT FOREIGN KEY REFERENCES Departments(DepartmentId),
    AppointmentDate DATETIME NOT NULL,
    AppointmentTime TIME NOT NULL,
    AppointmentType NVARCHAR(50) CHECK (AppointmentType IN ('Consultation', 'Follow-up', 'Emergency', 'Check-up', 'Surgery')),
    Status NVARCHAR(20) DEFAULT 'Scheduled' CHECK (Status IN ('Scheduled', 'Confirmed', 'Completed', 'Cancelled', 'No-Show', 'Rescheduled')),
    Reason NVARCHAR(500),
    Symptoms NVARCHAR(500),
    Diagnosis NVARCHAR(500),
    Prescription NVARCHAR(MAX),
    Notes NVARCHAR(1000),
    DurationMinutes INT DEFAULT 30,
    CreatedAt DATETIME DEFAULT GETDATE(),
    CreatedBy INT FOREIGN KEY REFERENCES Users(UserId),
    UpdatedAt DATETIME DEFAULT GETDATE()
);

-- 7. Medical Records Table
CREATE TABLE MedicalRecords (
    RecordId INT PRIMARY KEY IDENTITY(1,1),
    PatientId INT FOREIGN KEY REFERENCES Patients(PatientId),
    DoctorId INT FOREIGN KEY REFERENCES Doctors(DoctorId),
    VisitDate DATETIME DEFAULT GETDATE(),
    RecordType NVARCHAR(50) CHECK (RecordType IN ('Consultation', 'Lab Test', 'X-Ray', 'Surgery', 'Emergency', 'Follow-up')),
    Symptoms NVARCHAR(1000),
    Diagnosis NVARCHAR(1000),
    Treatment NVARCHAR(1000),
    Medications NVARCHAR(1000),
    Dosage NVARCHAR(500),
    Duration NVARCHAR(100),
    LabResults NVARCHAR(MAX),
    VitalSigns NVARCHAR(500),
    Notes NVARCHAR(MAX),
    FollowUpDate DATETIME,
    IsCritical BIT DEFAULT 0,
    CreatedAt DATETIME DEFAULT GETDATE(),
    CreatedBy INT FOREIGN KEY REFERENCES Users(UserId)
);

-- 8. Billing Table
CREATE TABLE Billing (
    BillId INT PRIMARY KEY IDENTITY(1,1),
    BillNumber NVARCHAR(20) UNIQUE NOT NULL,
    PatientId INT FOREIGN KEY REFERENCES Patients(PatientId),
    AppointmentId INT FOREIGN KEY REFERENCES Appointments(AppointmentId),
    BillDate DATETIME DEFAULT GETDATE(),
    DueDate DATETIME,
    TotalAmount DECIMAL(12,2) NOT NULL,
    TaxAmount DECIMAL(10,2) DEFAULT 0,
    DiscountAmount DECIMAL(10,2) DEFAULT 0,
    PaidAmount DECIMAL(12,2) DEFAULT 0,
    BalanceDue DECIMAL(12,2) DEFAULT 0,
    PaymentStatus NVARCHAR(20) DEFAULT 'Pending' CHECK (PaymentStatus IN ('Pending', 'Partial', 'Paid', 'Overdue', 'Cancelled')),
    InsuranceProvider NVARCHAR(100),
    InsuranceCoverage DECIMAL(10,2) DEFAULT 0,
    InsuranceClaimNumber NVARCHAR(50),
    ClaimStatus NVARCHAR(20) DEFAULT 'Not Submitted' CHECK (ClaimStatus IN ('Not Submitted', 'Submitted', 'Approved', 'Rejected', 'Processing')),
    CreatedAt DATETIME DEFAULT GETDATE(),
    CreatedBy INT FOREIGN KEY REFERENCES Users(UserId)
);

-- 9. BillItems Table
CREATE TABLE BillItems (
    ItemId INT PRIMARY KEY IDENTITY(1,1),
    BillId INT FOREIGN KEY REFERENCES Billing(BillId),
    ItemType NVARCHAR(50) CHECK (ItemType IN ('Consultation', 'Room Charge', 'Medication', 'Lab Test', 'Procedure', 'Surgery', 'Equipment', 'Other')),
    Description NVARCHAR(255) NOT NULL,
    Quantity INT DEFAULT 1,
    UnitPrice DECIMAL(10,2) NOT NULL,
    TotalPrice DECIMAL(10,2) NOT NULL,
    Notes NVARCHAR(500)
);

-- 10. Payments Table
CREATE TABLE Payments (
    PaymentId INT PRIMARY KEY IDENTITY(1,1),
    PaymentNumber NVARCHAR(20) UNIQUE NOT NULL,
    BillId INT FOREIGN KEY REFERENCES Billing(BillId),
    PatientId INT FOREIGN KEY REFERENCES Patients(PatientId),
    PaymentDate DATETIME DEFAULT GETDATE(),
    Amount DECIMAL(12,2) NOT NULL,
    PaymentMethod NVARCHAR(50) CHECK (PaymentMethod IN ('Cash', 'Credit Card', 'Debit Card', 'Insurance', 'Bank Transfer', 'Cheque', 'Online')),
    TransactionId NVARCHAR(100),
    CardLastFour NVARCHAR(4),
    PaymentStatus NVARCHAR(20) DEFAULT 'Completed' CHECK (PaymentStatus IN ('Completed', 'Pending', 'Failed', 'Refunded')),
    ReferenceNumber NVARCHAR(100),
    Notes NVARCHAR(500),
    ReceivedBy INT FOREIGN KEY REFERENCES Users(UserId)
);

-- 11. Inventory Table
CREATE TABLE Inventory (
    ItemId INT PRIMARY KEY IDENTITY(1,1),
    ItemCode NVARCHAR(50) UNIQUE NOT NULL,
    ItemName NVARCHAR(100) NOT NULL,
    Category NVARCHAR(50) CHECK (Category IN ('Medication', 'Equipment', 'Supplies', 'Lab', 'Surgical')),
    Description NVARCHAR(500),
    Unit NVARCHAR(20),
    CurrentStock INT DEFAULT 0,
    MinimumStock INT DEFAULT 10,
    MaximumStock INT DEFAULT 1000,
    UnitPrice DECIMAL(10,2) DEFAULT 0,
    Supplier NVARCHAR(100),
    SupplierContact NVARCHAR(100),
    LastRestocked DATETIME,
    ExpiryDate DATE,
    StorageLocation NVARCHAR(100),
    Status NVARCHAR(20) DEFAULT 'Active' CHECK (Status IN ('Active', 'Discontinued', 'Out of Stock')),
    Notes NVARCHAR(500)
);

-- 12. AuditLogs Table
CREATE TABLE AuditLogs (
    LogId INT PRIMARY KEY IDENTITY(1,1),
    Timestamp DATETIME DEFAULT GETDATE(),
    UserId INT FOREIGN KEY REFERENCES Users(UserId),
    UserRole NVARCHAR(50),
    Action NVARCHAR(100) NOT NULL,
    EntityType NVARCHAR(50),
    EntityId INT,
    Details NVARCHAR(MAX),
    IPAddress NVARCHAR(45),
    UserAgent NVARCHAR(500),
    Changes NVARCHAR(MAX)
);

-- 13. Notifications Table
CREATE TABLE Notifications (
    NotificationId INT PRIMARY KEY IDENTITY(1,1),
    UserId INT FOREIGN KEY REFERENCES Users(UserId),
    Title NVARCHAR(200) NOT NULL,
    Message NVARCHAR(MAX) NOT NULL,
    Type NVARCHAR(50) CHECK (Type IN ('Appointment', 'Payment', 'System', 'Alert', 'Reminder')),
    Priority NVARCHAR(20) DEFAULT 'Normal' CHECK (Priority IN ('Low', 'Normal', 'High', 'Critical')),
    IsRead BIT DEFAULT 0,
    CreatedAt DATETIME DEFAULT GETDATE(),
    ExpiresAt DATETIME,
    ActionUrl NVARCHAR(500)
);

-- 14. Prescriptions Table
CREATE TABLE Prescriptions (
    PrescriptionId INT PRIMARY KEY IDENTITY(1,1),
    PatientId INT FOREIGN KEY REFERENCES Patients(PatientId),
    DoctorId INT FOREIGN KEY REFERENCES Doctors(DoctorId),
    AppointmentId INT FOREIGN KEY REFERENCES Appointments(AppointmentId),
    PrescriptionDate DATETIME DEFAULT GETDATE(),
    Diagnosis NVARCHAR(500),
    Instructions NVARCHAR(MAX),
    Status NVARCHAR(20) DEFAULT 'Active' CHECK (Status IN ('Active', 'Completed', 'Cancelled')),
    ValidUntil DATETIME,
    CreatedAt DATETIME DEFAULT GETDATE()
);

-- 15. PrescriptionItems Table
CREATE TABLE PrescriptionItems (
    PrescriptionItemId INT PRIMARY KEY IDENTITY(1,1),
    PrescriptionId INT FOREIGN KEY REFERENCES Prescriptions(PrescriptionId),
    MedicationName NVARCHAR(100) NOT NULL,
    Dosage NVARCHAR(50),
    Frequency NVARCHAR(50),
    Duration NVARCHAR(50),
    Quantity INT,
    Instructions NVARCHAR(500),
    Status NVARCHAR(20) DEFAULT 'Prescribed' CHECK (Status IN ('Prescribed', 'Dispensed', 'Cancelled'))
);

-- 16. LabTests Table
CREATE TABLE LabTests (
    LabTestId INT PRIMARY KEY IDENTITY(1,1),
    TestCode NVARCHAR(50) UNIQUE NOT NULL,
    TestName NVARCHAR(100) NOT NULL,
    PatientId INT FOREIGN KEY REFERENCES Patients(PatientId),
    DoctorId INT FOREIGN KEY REFERENCES Doctors(DoctorId),
    TestType NVARCHAR(50),
    TestDate DATETIME DEFAULT GETDATE(),
    SampleType NVARCHAR(50),
    Status NVARCHAR(20) DEFAULT 'Pending' CHECK (Status IN ('Pending', 'In Progress', 'Completed', 'Cancelled')),
    Result NVARCHAR(MAX),
    NormalRange NVARCHAR(200),
    Units NVARCHAR(50),
    PerformedBy NVARCHAR(100),
    VerifiedBy NVARCHAR(100),
    Notes NVARCHAR(500),
    ReportFile NVARCHAR(500)
);

-- 17. Schedules Table
CREATE TABLE Schedules (
    ScheduleId INT PRIMARY KEY IDENTITY(1,1),
    DoctorId INT FOREIGN KEY REFERENCES Doctors(DoctorId),
    DayOfWeek INT CHECK (DayOfWeek BETWEEN 1 AND 7),
    StartTime TIME NOT NULL,
    EndTime TIME NOT NULL,
    SlotDuration INT DEFAULT 30, -- in minutes
    MaxAppointments INT DEFAULT 10,
    IsActive BIT DEFAULT 1,
    CreatedAt DATETIME DEFAULT GETDATE()
);

-- 18. EmployeeShifts Table
CREATE TABLE EmployeeShifts (
    ShiftId INT PRIMARY KEY IDENTITY(1,1),
    UserId INT FOREIGN KEY REFERENCES Users(UserId),
    ShiftDate DATE NOT NULL,
    ShiftType NVARCHAR(50) CHECK (ShiftType IN ('Morning', 'Evening', 'Night', 'General')),
    StartTime TIME NOT NULL,
    EndTime TIME NOT NULL,
    DepartmentId INT FOREIGN KEY REFERENCES Departments(DepartmentId),
    Status NVARCHAR(20) DEFAULT 'Scheduled' CHECK (Status IN ('Scheduled', 'Working', 'Completed', 'Cancelled')),
    Notes NVARCHAR(500)
);

-- Create Indexes for Performance
CREATE INDEX IX_Users_Username ON Users(Username);
CREATE INDEX IX_Users_Email ON Users(Email);
CREATE INDEX IX_Users_UserType ON Users(UserType);
CREATE INDEX IX_Patients_MedicalRecordNumber ON Patients(MedicalRecordNumber);
CREATE INDEX IX_Appointments_Date ON Appointments(AppointmentDate);
CREATE INDEX IX_Appointments_PatientId ON Appointments(PatientId);
CREATE INDEX IX_Appointments_DoctorId ON Appointments(DoctorId);
CREATE INDEX IX_MedicalRecords_PatientId ON MedicalRecords(PatientId);
CREATE INDEX IX_Billing_PatientId ON Billing(PatientId);
CREATE INDEX IX_Billing_PaymentStatus ON Billing(PaymentStatus);
CREATE INDEX IX_AuditLogs_Timestamp ON AuditLogs(Timestamp);
CREATE INDEX IX_AuditLogs_UserId ON AuditLogs(UserId);

-- Insert Initial Data
INSERT INTO Departments (DepartmentName, DepartmentCode, Description, Location, TotalBeds, AvailableBeds) VALUES
('Cardiology', 'CARD', 'Heart and cardiovascular care department', 'Floor 3, East Wing', 50, 35),
('Neurology', 'NEUR', 'Brain and nervous system disorders', 'Floor 4, West Wing', 40, 25),
('Orthopedics', 'ORTH', 'Bone and joint care', 'Floor 5, East Wing', 60, 40),
('Pediatrics', 'PEDI', 'Child healthcare', 'Floor 2, North Wing', 45, 30),
('Emergency', 'EMER', '24/7 emergency care', 'Floor 1, Main Building', 30, 10),
('ICU', 'ICU', 'Intensive Care Unit', 'Floor 6, Special Wing', 20, 5);

INSERT INTO Users (Username, PasswordHash, Email, FullName, UserType, Status) VALUES
('admin', 'AQAAAAIAAYagAAAAECg8FLL1EfnVr9qWtsoeUFfccRYKxNK+9M0iVGTk6UqCV5HF8BRo1sKyyw7lLpRz1A==', 'admin@hospital.com', 'System Administrator', 'Admin', 'Active'),
('reception', 'AQAAAAIAAYagAAAAECg8FLL1EfnVr9qWtsoeUFfccRYKxNK+9M0iVGTk6UqCV5HF8BRo1sKyyw7lLpRz1A==', 'reception@hospital.com', 'Reception Desk', 'Receptionist', 'Active');

GO

PRINT 'Database created successfully!';