using HospitalManagementSystem.Blazor.Models.DTOs;

namespace HospitalManagementSystem.Blazor.Services
{
    public class PatientService
    {
        private readonly ApiService _api;

        public PatientService(ApiService api)
        {
            _api = api;
        }

        public async Task<List<PatientApiModel>> GetPatientsAsync()
        {
            return await _api.GetAsync<List<PatientApiModel>>("api/patients") ?? new List<PatientApiModel>();
        }

        public async Task<PatientApiModel?> GetPatientByIdAsync(int id)
        {
            return await _api.GetAsync<PatientApiModel>($"api/patients/{id}");
        }

        public async Task<PatientApiModel?> CreatePatientAsync(CreatePatientRequest request)
        {
            return await _api.PostAsync<PatientApiModel>("api/patients", request);
        }

        public async Task<bool> UpdatePatientAsync(int id, CreatePatientRequest request)
        {
            var result = await _api.PutAsync<object>($"api/patients/{id}", request);
            return true;
        }

        public async Task<bool> DeletePatientAsync(int id)
        {
            await _api.DeleteAsync($"api/patients/{id}");
            return true;
        }
    }
}
