namespace HospitalManagementSystem.Core.Models
{
    public class Patient
    {
        public int PatientId { get; set; }
        public int UserId { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public DateTime DateOfBirth { get; set; }
        public Gender Gender { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Address { get; set; }
        public string? BloodType { get; set; }
        public string? EmergencyContact { get; set; }
        public DateTime RegistrationDate { get; set; } = DateTime.UtcNow;
        
        // Navigation properties
        public User? User { get; set; }
        public List<Appointment>? Appointments { get; set; }
        public List<MedicalRecord>? MedicalRecords { get; set; }
    }

    public enum Gender
    {
        Male,
        Female,
        Other
    }
}