using System;
using System.Collections.Generic;

namespace HospitalManagementSystem.Core.Models
{
    public class Patient
    {
        public int PatientId { get; set; }
        public int UserId { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public DateTime DateOfBirth { get; set; }

        public User? User { get; set; }
        public List<Appointment>? Appointments { get; set; }
    }
}