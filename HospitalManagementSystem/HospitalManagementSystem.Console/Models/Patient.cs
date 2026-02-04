using System;

namespace HospitalManagementSystem.ConsoleApp.Models
{
    public class Patient
    {
        public int PatientId { get; set; }
        public string Name { get; set; } = string.Empty;
        public int Age { get; set; }
        public string Gender { get; set; } = string.Empty;
        public string? BloodType { get; set; }
        public DateTime? LastVisit { get; set; }
    }
}