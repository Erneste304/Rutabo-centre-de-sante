namespace HospitalManagementSystem.Blazor.Models.DTOs
{
    public class UserModel
    {
        public int Id { get; set; }
        public string Username { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string UserType { get; set; } = string.Empty;
        public int? DoctorId { get; set; }
        public int? PatientId { get; set; }
        public string? Specialization { get; set; }
        public string? Department { get; set; }
        public string Status { get; set; } = "Active";
        public DateTime CreatedAt { get; set; }
        public DateTime? LastLogin { get; set; }
        public string? Token { get; set; }
    }

    // DTOs for patient CRUD
    public class PatientApiModel
    {
        public int PatientId { get; set; }
        public int? UserId { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? PhoneNumber { get; set; }
        public string MedicalRecordNumber { get; set; } = string.Empty;
        public string? BloodType { get; set; }
        public string? Gender { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string? Allergies { get; set; }
        public string? ChronicConditions { get; set; }
        public string? InsuranceProvider { get; set; }
        public string? InsurancePolicyNumber { get; set; }
        public string? EmergencyContactName { get; set; }
        public string? EmergencyContactPhone { get; set; }
        public bool IsActive { get; set; } = true;
        public DateTime AdmissionDate { get; set; }
        public string Status => IsActive ? "Active" : "Inactive";
        public int Age => DateOfBirth.HasValue
            ? (int)((DateTime.Today - DateOfBirth.Value).TotalDays / 365.25)
            : 0;
    }

    public class CreatePatientRequest
    {
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? PhoneNumber { get; set; }
        public string? BloodType { get; set; }
        public string? Gender { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string? Allergies { get; set; }
        public string? ChronicConditions { get; set; }
        public string? InsuranceProvider { get; set; }
        public string? InsurancePolicyNumber { get; set; }
        public string? EmergencyContactName { get; set; }
        public string? EmergencyContactPhone { get; set; }
        public string? EmergencyContactRelation { get; set; }
        public string? Address { get; set; }
    }

    public class DoctorApiModel
    {
        public int DoctorId { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? PhoneNumber { get; set; }
        public string? Qualifications { get; set; }
        public int YearsOfExperience { get; set; }
        public bool IsAvailable { get; set; }
        public decimal Rating { get; set; }
        public List<string> Departments { get; set; } = new();
        public string DepartmentDisplay => Departments.Any() ? string.Join(", ", Departments) : "General";
    }
}