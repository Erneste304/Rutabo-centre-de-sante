using HospitalManagementSystem.Blazor.Models.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HospitalManagementSystem.Blazor.Services
{
    public class AmbulanceService
    {
        private readonly ApiService _api;

        public AmbulanceService(ApiService api)
        {
            _api = api;
        }

        public async Task<List<AmbulanceModel>> GetFleetAsync()
        {
            return await _api.RequestAsync<List<AmbulanceModel>>("api/ambulance") ?? new List<AmbulanceModel>();
        }

        public async Task<List<AmbulanceLogModel>> GetActiveLogsAsync()
        {
            return await _api.RequestAsync<List<AmbulanceLogModel>>("api/ambulance/logs/active") ?? new List<AmbulanceLogModel>();
        }

        public async Task<AmbulanceLogModel?> DispatchAmbulanceAsync(AmbulanceLogModel log)
        {
            return await _api.RequestAsync<AmbulanceLogModel>("api/ambulance/dispatch", HttpMethod.Post, log);
        }

        public async Task<bool> UpdateTripStatusAsync(int logId, string status)
        {
            var request = new { Status = status };
            var response = await _api.RequestAsync<object>($"api/ambulance/logs/{logId}/status", HttpMethod.Post, request);
            return response != null;
        }
    }
}
