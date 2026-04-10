using System;

namespace HospitalManagementSystem.Blazor.Models.DTOs
{
    public class AppointmentModel
    {
        public int AppointmentId { get; set; }
        public string AppointmentNumber { get; set; } = string.Empty;
        public int PatientId { get; set; }
        public string PatientName { get; set; } = string.Empty;
        public int DoctorId { get; set; }
        public string DoctorName { get; set; } = string.Empty;
        public int? DepartmentId { get; set; }
        public string? DepartmentName { get; set; }
        public DateTime AppointmentDate { get; set; }
        public TimeSpan AppointmentTime { get; set; }
        public string AppointmentType { get; set; } = "Consultation";
        public string Status { get; set; } = "Scheduled"; // Scheduled, Confirmed, Completed, Cancelled, Waiting, Ready
        public string? Reason { get; set; }
        public string? Symptoms { get; set; }
        public int DurationMinutes { get; set; } = 30;
    }

    public class ScheduleModel
    {
        public int ScheduleId { get; set; }
        public int DoctorId { get; set; }
        public int DayOfWeek { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public int SlotDuration { get; set; }
        public bool IsActive { get; set; }
    }
}
