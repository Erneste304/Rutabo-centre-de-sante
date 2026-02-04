namespace HospitalManagementSystem.ConsoleApp.Models
{
    public class Doctor
    {
        public int DoctorId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Specialization { get; set; } = string.Empty;
        public string? Department { get; set; }
        public bool IsAvailable { get; set; }
    }
}