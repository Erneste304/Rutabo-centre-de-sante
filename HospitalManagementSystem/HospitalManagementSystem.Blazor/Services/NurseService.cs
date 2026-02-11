using HospitalManagementSystem.Blazor.Models.DTOs;

namespace HospitalManagementSystem.Blazor.Services
{
    public class NurseService
    {
        private readonly ApiService _api;
        
        public NurseService(ApiService api)
        {
            _api = api;
        }

        public async Task<bool> CreateShiftReportAsync(ShiftReportModel report)
        {
            var result = await _api.RequestAsync<ShiftReportModel>("api/nurse/shift-reports", HttpMethod.Post, report);
            return result != null;
        }
    }
}
