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

        // Add billing specific methods here
    }
}
