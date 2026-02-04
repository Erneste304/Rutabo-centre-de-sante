using System;
using System.Threading.Tasks;
using HospitalManagementSystem.ConsoleApp.Models;

namespace HospitalManagementSystem.ConsoleApp.Services
{
    public interface IAuthenticationService
    {
        Task<UserSession?> AuthenticateAsync(string username, string password);
        Task<bool> LogoutAsync(int userId);
        Task<bool> ChangePasswordAsync(int userId, string oldPassword, string newPassword);
        Task<bool> RegisterAsync(string username, string email, string password, string fullName, string userType);
    }

    public class AuthenticationService : IAuthenticationService
    {
        // In a real application, this would connect to a database
        // For now, we'll use mock data and simple in-memory email tracking
        private readonly Dictionary<string, (string password, string email, UserSession session)> _mockUsers = new()
        {
            {"admin", ("admin123", "admin@hospital.local", new UserSession(1, "admin", "System Administrator", "Admin"))},
            {"dr.smith", ("doctor123", "dr.smith@hospital.local", new UserSession(101, "dr.smith", "Dr. John Smith", "Doctor") 
                { Specialization = "Cardiology", Department = "Cardiology" })},
            {"dr.jones", ("doctor123", "dr.jones@hospital.local", new UserSession(102, "dr.jones", "Dr. Sarah Jones", "Doctor") 
                { Specialization = "Neurology", Department = "Neurology" })},
            {"nurse.jane", ("nurse123", "nurse.jane@hospital.local", new UserSession(201, "nurse.jane", "Jane Williams", "Nurse") 
                { Department = "Emergency" })},
            {"nurse.mike", ("nurse123", "nurse.mike@hospital.local", new UserSession(202, "nurse.mike", "Mike Johnson", "Nurse") 
                { Department = "ICU" })},
            {"reception.sarah", ("reception123", "reception.sarah@hospital.local", new UserSession(301, "reception.sarah", "Sarah Brown", "Receptionist"))},
            {"patient.john", ("patient123", "john.doe@patient.local", new UserSession(1001, "patient.john", "John Doe", "Patient"))},
            {"patient.emily", ("patient123", "emily.davis@patient.local", new UserSession(1002, "patient.emily", "Emily Davis", "Patient"))}
        };

        private readonly HashSet<string> _emails = new(StringComparer.OrdinalIgnoreCase);

        public AuthenticationService()
        {
            foreach (var v in _mockUsers.Values)
            {
                if (!string.IsNullOrWhiteSpace(v.email)) _emails.Add(v.email);
            }
        }

        public async Task<UserSession?> AuthenticateAsync(string username, string password)
        {
            // Simulate async operation
            await Task.Delay(100);
            if (_mockUsers.TryGetValue(username, out var user) && user.password == password)
            {
                return user.session;
            }
            
            return null;
        }

        public async Task<bool> RegisterAsync(string username, string email, string password, string fullName, string userType)
        {
            await Task.Delay(100);

            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
                return false;

            if (_mockUsers.ContainsKey(username) || _emails.Contains(email))
                return false; // username or email already in use

            var nextId = 2000 + _mockUsers.Count + 1;
            var session = new UserSession(nextId, username, fullName, userType);
            _mockUsers[username] = (password, email, session);
            _emails.Add(email);
            return true;
        }

        public async Task<bool> LogoutAsync(int userId)
        {
            await Task.Delay(100);
            Console.WriteLine($"User {userId} logged out successfully.");
            return true;
        }

        public async Task<bool> ChangePasswordAsync(int userId, string oldPassword, string newPassword)
        {
            await Task.Delay(100);
            Console.WriteLine($"Password changed for user {userId}");
            return true;
        }
    }
}