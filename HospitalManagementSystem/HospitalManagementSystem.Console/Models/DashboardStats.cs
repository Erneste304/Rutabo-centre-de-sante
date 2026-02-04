using System;

namespace HospitalManagementSystem.ConsoleApp.Models
{
    public class DashboardStats
    {
        public DateTime Date { get; set; }
        public int TotalPatients { get; set; }
        public int TodayAppointments { get; set; }
        public int PendingTasks { get; set; }
        public string Stat1Label { get; set; } = string.Empty;
        public string Stat1Value { get; set; } = string.Empty;
        public string Stat2Label { get; set; } = string.Empty;
        public string Stat2Value { get; set; } = string.Empty;
    }
}