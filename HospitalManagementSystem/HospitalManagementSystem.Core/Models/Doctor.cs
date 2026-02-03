namespace HospitalManagementSystem.Core.Models
{
    public class Doctor
    {
        public int DoctorId { get; set; }
        public int UserId { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Specialization { get; set; } = string.Empty;
        public string LicenseNumber { get; set; } = string.Empty;
        public string? PhoneNumber { get; set; }
        public string? Email { get; set; }
        public string? Department { get; set; }
        public bool IsAvailable { get; set; } = true;
        
        public User? User { get; set; }
        public List<Appointment>? Appointments { get; set; }
    }
}