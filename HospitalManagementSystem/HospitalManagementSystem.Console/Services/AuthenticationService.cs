using System;
using System.Collections.Generic;
using System.Linq;
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
        Task<IReadOnlyCollection<UserSession>> GetPendingUsersAsync();
        Task<bool> ApproveUserAsync(string username);
    }

    public class AuthenticationService : IAuthenticationService
    {
        // In a real application, this would connect to a database.
        // For now, we'll use mock data and simple in-memory tracking.
        private readonly Dictionary<string, (string password, string email, UserSession session)> _mockUsers = new();

        private readonly HashSet<string> _emails = new(StringComparer.OrdinalIgnoreCase);

        public AuthenticationService()
        {
            // Seed initial users
            AddSeedUser("admin", "admin123", "admin@hospital.local",
                new UserSession(1, "admin", "System Administrator", "Admin", isApproved: true));

            AddSeedUser("dr.smith", "doctor123", "dr.smith@hospital.local",
                new UserSession(101, "dr.smith", "Dr. John Smith", "Doctor", isApproved: true)
                {
                    Specialization = "Cardiology",
                    Department = "Cardiology"
                });

            AddSeedUser("dr.jones", "doctor123", "dr.jones@hospital.local",
                new UserSession(102, "dr.jones", "Dr. Sarah Jones", "Doctor", isApproved: true)
                {
                    Specialization = "Neurology",
                    Department = "Neurology"
                });

            AddSeedUser("nurse.jane", "nurse123", "nurse.jane@hospital.local",
                new UserSession(201, "nurse.jane", "Jane Williams", "Nurse", isApproved: true)
                {
                    Department = "Emergency"
                });

            AddSeedUser("nurse.mike", "nurse123", "nurse.mike@hospital.local",
                new UserSession(202, "nurse.mike", "Mike Johnson", "Nurse", isApproved: true)
                {
                    Department = "ICU"
                });

            AddSeedUser("reception.sarah", "reception123", "reception.sarah@hospital.local",
                new UserSession(301, "reception.sarah", "Sarah Brown", "Receptionist", isApproved: true));

            AddSeedUser("patient.john", "patient123", "john.doe@patient.local",
                new UserSession(1001, "patient.john", "John Doe", "Patient", isApproved: true));

            AddSeedUser("patient.emily", "patient123", "emily.davis@patient.local",
                new UserSession(1002, "patient.emily", "Emily Davis", "Patient", isApproved: true));
        }

        private void AddSeedUser(string username, string password, string email, UserSession session)
        {
            _mockUsers[username] = (password, email, session);
            if (!string.IsNullOrWhiteSpace(email))
            {
                _emails.Add(email);
            }
        }

        public async Task<UserSession?> AuthenticateAsync(string username, string password)
        {
            // Simulate async operation
            await Task.Delay(100);
            if (_mockUsers.TryGetValue(username, out var user) && user.password == password)
            {
                // Only allow login if user is approved or is an admin
                if (!user.session.IsApproved && !string.Equals(user.session.UserType, "Admin", StringComparison.OrdinalIgnoreCase))
                {
                    Console.WriteLine("\nYour account is pending admin approval. Please wait for an administrator to approve your registration.");
                    return null;
                }

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

            // Patients are auto-approved, staff require admin approval
            var isApproved = string.Equals(userType, "Patient", StringComparison.OrdinalIgnoreCase);

            var session = new UserSession(nextId, username, fullName, userType, isApproved);
            _mockUsers[username] = (password, email, session);
            _emails.Add(email);

            if (!isApproved)
            {
                Console.WriteLine("\nRegistration submitted. Your account will be activated after admin approval.");
            }

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
            // Find user by userId and validate old password
            foreach (var kvp in _mockUsers.ToList())
            {
                var record = kvp.Value;
                if (record.session.UserId == userId)
                {
                    if (!string.Equals(record.password, oldPassword))
                    {
                        return false;
                    }

                    _mockUsers[kvp.Key] = (newPassword, record.email, record.session);
                    Console.WriteLine($"Password changed for user {userId}");
                    return true;
                }
            }

            return false;
        }

        public Task<IReadOnlyCollection<UserSession>> GetPendingUsersAsync()
        {
            var pending = _mockUsers.Values
                .Where(v => !v.session.IsApproved)
                .Select(v => v.session)
                .ToList()
                .AsReadOnly();

            return Task.FromResult((IReadOnlyCollection<UserSession>)pending);
        }

        public Task<bool> ApproveUserAsync(string username)
        {
            if (_mockUsers.TryGetValue(username, out var user))
            {
                if (!user.session.IsApproved)
                {
                    user.session.IsApproved = true;
                    _mockUsers[username] = (user.password, user.email, user.session);
                    Console.WriteLine($"\nUser '{username}' has been approved by admin.");
                }
                return Task.FromResult(true);
            }

            return Task.FromResult(false);
        }
    }
}

