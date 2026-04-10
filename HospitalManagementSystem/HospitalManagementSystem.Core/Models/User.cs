namespace HospitalManagementSystem.Core.Models
{
    public class User
    {
        public int UserId { get; set; }
        public string Username { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public UserType UserType { get; set; }
        public bool IsActive { get; set; } = true;
        public int? DoctorId { get; set; }
        public int? PatientId { get; set; }
        public bool ResetRequested { get; set; } = false;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    }

    public enum UserType
    {
        Admin,
        Doctor,
        Nurse,
        Patient,
        Receptionist,
        Accountant
    }
}