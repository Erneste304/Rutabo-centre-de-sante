using System;

namespace HospitalManagementSystem.ConsoleApp.Models
{
    public class AuditLog
    {
        public int LogId { get; set; }
        public int UserId { get; set; }
        public string Username { get; set; } = string.Empty;
        public string Action { get; set; } = string.Empty;
        public string EntityType { get; set; } = string.Empty; // User, Patient, Doctor, etc.
        public int? EntityId { get; set; }
        public string Details { get; set; } = string.Empty;
        public DateTime Timestamp { get; set; }
        public string IpAddress { get; set; } = "N/A";
    }
}
