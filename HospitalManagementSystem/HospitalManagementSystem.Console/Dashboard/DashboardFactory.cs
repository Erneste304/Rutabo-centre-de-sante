using System;

namespace HospitalManagementSystem.ConsoleApp.Dashboard
{
    public static class DashboardFactory
    {
        public static IDashboard CreateDashboard(string userType, int userId, string userName, string? additionalInfo = null)
        {
            return userType.ToLower() switch
            {
                "admin" => new AdminDashboard(),
                "doctor" => new DoctorDashboard(userId, userName, additionalInfo ?? "General"),
                "patient" => new PatientDashboard(userId, userName),
                "nurse" => new NurseDashboard(userId, userName, additionalInfo),
                "receptionist" => new ReceptionistDashboard(userId, userName),
                _ => throw new ArgumentException($"Unknown user type: {userType}")
            };
        }
    }
}