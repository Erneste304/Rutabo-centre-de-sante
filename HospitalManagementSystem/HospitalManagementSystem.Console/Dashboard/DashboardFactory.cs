using System;
using HospitalManagementSystem.ConsoleApp.Models;
using HospitalManagementSystem.ConsoleApp.Services;

namespace HospitalManagementSystem.ConsoleApp.Dashboard
{
    public static class DashboardFactory
    {
        public static IDashboard CreateDashboard(UserSession session, IDataService dataService, IAuthenticationService authService)
        {
            var userType = session.UserType.ToLower();

            return userType switch
            {
                "admin" => new AdminDashboard(session, authService, dataService),
                "doctor" => new DoctorDashboard(session, dataService),
                "patient" => new PatientDashboard(session, dataService, authService),
                "nurse" => new NurseDashboard(session, dataService),
                "receptionist" => new ReceptionistDashboard(session, dataService),
                "accountant" => new AccountantDashboard(session, dataService),
                _ => throw new ArgumentException($"Unknown user type: {session.UserType}")
            };
        }
    }
}