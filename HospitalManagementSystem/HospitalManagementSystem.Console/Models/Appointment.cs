using System;

namespace HospitalManagementSystem.ConsoleApp.Models
{
    public class Appointment
    {
        public int AppointmentId { get; set; }
        public string PatientName { get; set; } = string.Empty;
        public string DoctorName { get; set; } = string.Empty;
        public DateTime Date { get; set; }
        public string Status { get; set; } = string.Empty;
    }
}