using System;

namespace HospitalManagementSystem.ConsoleApp.Models
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
        public bool IsApproved { get; set; } = true;

        // Optional profile details for patient/user profile screens
        public string? PhoneNumber { get; set; }
        public string? Address { get; set; }
        public string? EmergencyContactName { get; set; }
        public string? EmergencyContactPhone { get; set; }
        
        public UserSession(int userId, string username, string fullName, string userType, bool isApproved = true)
        {
            UserId = userId;
            Username = username;
            FullName = fullName;
            UserType = userType;
            LoginTime = DateTime.Now;
            IsApproved = isApproved;
        }
        
        public string GetWelcomeMessage()
        {
            return $"{FullName} ({UserType})";
        }
    }
}