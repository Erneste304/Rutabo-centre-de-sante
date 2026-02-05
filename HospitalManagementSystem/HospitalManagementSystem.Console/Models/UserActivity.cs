using System;

namespace HospitalManagementSystem.ConsoleApp.Models
{
    public class UserActivity
    {
        public int ActivityId { get; set; }
        public int UserId { get; set; }
        public string Username { get; set; } = string.Empty;
        public string Action { get; set; } = string.Empty; // e.g., "Login", "View Patient", "Update Record"
        public string Details { get; set; } = string.Empty;
        public DateTime Timestamp { get; set; }
        public string IpAddress { get; set; } = "N/A";
    }
}
