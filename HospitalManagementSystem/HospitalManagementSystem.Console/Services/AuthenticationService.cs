using System;
using System.Threading.Tasks;
using HospitalManagementSystem.Console.Models;

namespace HospitalManagementSystem.Console.Services
{
    public interface IAuthenticationService
    {
        Task<UserSession?> AuthenticateAsync(string username, string password);
        Task<bool> LogoutAsync(int userId);
        Task<bool> ChangePasswordAsync(int userId, string oldPassword, string newPassword);
    }

    public class AuthenticationService : IAuthenticationService
    {
        // In a real application, this would connect to a database
        // For now, we'll use mock data
        private readonly Dictionary<string, (string password, UserSession session)> _mockUsers = new()
        {
            {"admin", ("admin123", new UserSession(1, "admin", "System Administrator", "Admin"))},
            {"dr.smith", ("doctor123", new UserSession(101, "dr.smith", "Dr. John Smith", "Doctor") 
                { Specialization = "Cardiology", Department = "Cardiology" })},
            {"dr.jones", ("doctor123", new UserSession(102, "dr.jones", "Dr. Sarah Jones", "Doctor") 
                { Specialization = "Neurology", Department = "Neurology" })},
            {"nurse.jane", ("nurse123", new UserSession(201, "nurse.jane", "Jane Williams", "Nurse") 
                { Department = "Emergency" })},
            {"nurse.mike", ("nurse123", new UserSession(202, "nurse.mike", "Mike Johnson", "Nurse") 
                { Department = "ICU" })},
            {"reception.sarah", ("reception123", new UserSession(301, "reception.sarah", "Sarah Brown", "Receptionist"))},
            {"patient.john", ("patient123", new UserSession(1001, "patient.john", "John Doe", "Patient"))},
            {"patient.emily", ("patient123", new UserSession(1002, "patient.emily", "Emily Davis", "Patient"))}
        };

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