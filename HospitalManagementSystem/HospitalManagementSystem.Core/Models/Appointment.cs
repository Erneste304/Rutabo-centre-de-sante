namespace HospitalManagementSystem.Core.Models
{
    public class Appointment
    {
        public int AppointmentId { get; set; }
        public int PatientId { get; set; }
        public int DoctorId { get; set; }
        public DateTime AppointmentDate { get; set; }
        public AppointmentStatus Status { get; set; } = AppointmentStatus.Scheduled;
        public string? Reason { get; set; }
        public string? Diagnosis { get; set; }
        public string? Prescription { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        
        public Patient? Patient { get; set; }
        public Doctor? Doctor { get; set; }
    }

    public enum AppointmentStatus
    {
        Scheduled,
        Completed,
        Cancelled,
        NoShow
    }
}