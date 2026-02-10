using System;
using System.Collections.Generic;

namespace HospitalManagementSystem.ConsoleApp.Models
{
    public class ReceptionistAnalyticsModel
    {
        public int NewRegistrationsToday { get; set; }
        public int AppointmentsToday { get; set; }
        public int WalkInsToday { get; set; }
        public int EmergencyCasesToday { get; set; }
        public decimal RevenueToday { get; set; }
        public double PatientSatisfaction { get; set; } = 95.0; // Mocked for now
        public Dictionary<string, int> TopServices { get; set; } = new();
    }
}
