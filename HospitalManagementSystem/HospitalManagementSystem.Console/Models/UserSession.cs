using System;

namespace HospitalManagementSystem.Console.Models
{
    public class UserSession
    {
        public int UserId { get; set; }
        public string Username { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public string UserType { get; set; } = string.Empty;
        public string? Department { get; set; }
        public string? Specialization { get; set; }
        public DateTime LoginTime { get; set; }
        public bool IsActive { get; set; } = true;
        
        public UserSession(int userId, string username, string fullName, string userType)
        {
            UserId = userId;
            Username = username;
            FullName = fullName;
            UserType = userType;
            LoginTime = DateTime.Now;
        }
        
        public string GetWelcomeMessage()
        {
            return $"{FullName} ({UserType})";
        }
    }
}