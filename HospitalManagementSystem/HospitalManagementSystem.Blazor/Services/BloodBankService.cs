using HospitalManagementSystem.Blazor.Models.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HospitalManagementSystem.Blazor.Services
{
    public class BloodBankService
    {
        private readonly ApiService _api;

        public BloodBankService(ApiService api)
        {
            _api = api;
        }

        public async Task<List<BloodBankModel>> GetStockAsync()
        {
            return await _api.RequestAsync<List<BloodBankModel>>("api/bloodbank") ?? new List<BloodBankModel>();
        }

        public async Task<bool> UpdateBloodStockAsync(string bloodType, int delta)
        {
            var request = new { BloodType = bloodType, Delta = delta };
            var response = await _api.RequestAsync<object>("api/bloodbank/update", HttpMethod.Post, request);
            return response != null;
        }
    }
}
