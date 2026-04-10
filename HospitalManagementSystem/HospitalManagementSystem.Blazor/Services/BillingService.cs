using HospitalManagementSystem.Blazor.Models.DTOs;

namespace HospitalManagementSystem.Blazor.Services
{
    public class BillingService
    {
        private readonly ApiService _api;
        
        public BillingService(ApiService api)
        {
            _api = api;
        }

        public async Task<List<BillingModel>> GetBillingsAsync()
        {
            return await _api.RequestAsync<List<BillingModel>>("api/billings") ?? new List<BillingModel>();
        }

        public async Task<BillingModel?> GetBillByIdAsync(int id)
        {
            return await _api.RequestAsync<BillingModel>($"api/billings/{id}");
        }

        public async Task<bool> ProcessPaymentAsync(int billId, PaymentModel payment)
        {
            var result = await _api.RequestAsync<PaymentModel>($"api/billings/{billId}/payments", HttpMethod.Post, payment);
            return result != null;
        }
    }
}
