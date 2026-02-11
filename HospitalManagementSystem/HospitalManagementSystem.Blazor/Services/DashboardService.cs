using HospitalManagementSystem.Blazor.Models.DTOs;

namespace HospitalManagementSystem.Blazor.Services
{
    public class DashboardService
    {
        private readonly ApiService _api;
        
        public DashboardService(ApiService api)
        {
            _api = api;
        }

        public async Task<object?> GetDashboardStatsAsync(string userType)
        {
            // Placeholder for fetching dashboard stats
            return await _api.RequestAsync<object>($"api/dashboard/stats?type={userType}");
        }
    }
}
