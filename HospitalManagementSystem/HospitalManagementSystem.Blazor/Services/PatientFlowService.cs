using HospitalManagementSystem.Blazor.Models.DTOs;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace HospitalManagementSystem.Blazor.Services
{
    public class PatientFlowService
    {
        private readonly ApiService _api;

        public PatientFlowService(ApiService api)
        {
            _api = api;
        }

        public async Task<List<PatientVisitModel>> GetActiveVisitsAsync()
        {
            return await _api.RequestAsync<List<PatientVisitModel>>("api/patientflow/visits") ?? new List<PatientVisitModel>();
        }

        public async Task<PatientVisitModel?> StartVisitAsync(int patientId, string visitType, string complaint)
        {
            var request = new { PatientId = patientId, VisitType = visitType, Complaint = complaint };
            return await _api.RequestAsync<PatientVisitModel>("api/patientflow/start", HttpMethod.Post, request);
        }

        public async Task<bool> RecordTriageAsync(TriageRecordModel record)
        {
            var response = await _api.RequestAsync<TriageRecordModel>("api/patientflow/triage", HttpMethod.Post, record);
            return response != null;
        }

        public async Task<List<InsuranceTypeModel>> GetInsuranceTypesAsync()
        {
            return await _api.RequestAsync<List<InsuranceTypeModel>>("api/patientflow/insurance-types") ?? new List<InsuranceTypeModel>();
        }

        public async Task<bool> UpdateVisitStatusAsync(int visitId, string status, string? priority = null)
        {
            var request = new { Status = status, Priority = priority };
            var response = await _api.RequestAsync<object>($"api/patientflow/status/{visitId}", HttpMethod.Put, request);
            return response != null;
        }
    }
}
