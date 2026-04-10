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

        public async Task<DashboardStatsModel?> GetDashboardStatsAsync(string userType)
        {
            return await _api.RequestAsync<DashboardStatsModel>($"api/dashboard/stats?type={userType}");
        }
    }
}
