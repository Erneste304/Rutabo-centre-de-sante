using HospitalManagementSystem.Blazor.Models.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HospitalManagementSystem.Blazor.Services
{
    public class ClinicalService
    {
        private readonly ApiService _api;

        public ClinicalService(ApiService api)
        {
            _api = api;
        }

        // Medical Records
        public async Task<List<MedicalRecordModel>> GetPatientHistoryAsync(int patientId)
        {
            return await _api.RequestAsync<List<MedicalRecordModel>>($"api/medicalrecords/patient/{patientId}") ?? new List<MedicalRecordModel>();
        }

        public async Task<MedicalRecordModel?> AddMedicalRecordAsync(MedicalRecordModel record)
        {
            return await _api.RequestAsync<MedicalRecordModel>("api/medicalrecords", HttpMethod.Post, record);
        }

        public async Task<List<MedicalRecordModel>> GetCriticalRecordsAsync()
        {
            return await _api.RequestAsync<List<MedicalRecordModel>>("api/medicalrecords/critical") ?? new List<MedicalRecordModel>();
        }

        // Lab Tests
        public async Task<List<LabTestModel>> GetPendingTestsAsync()
        {
            return await _api.RequestAsync<List<LabTestModel>>("api/labtests/pending") ?? new List<LabTestModel>();
        }

        public async Task<bool> SubmitLabResultAsync(int testId, string result, string performedBy)
        {
            var request = new { Result = result, PerformedBy = performedBy };
            var response = await _api.RequestAsync<object>($"api/labtests/{testId}/result", HttpMethod.Post, request);
            return response != null;
        }

        public async Task<List<LabTestModel>> GetPatientTestsAsync(int patientId)
        {
            return await _api.RequestAsync<List<LabTestModel>>($"api/labtests/patient/{patientId}") ?? new List<LabTestModel>();
        }
    }
}
