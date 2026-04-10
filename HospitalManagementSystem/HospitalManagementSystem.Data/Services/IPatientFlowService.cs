using HospitalManagementSystem.Data.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HospitalManagementSystem.Data.Services
{
    public interface IPatientFlowService
    {
        // Visit Management
        Task<PatientVisit> StartVisitAsync(int patientId, string visitType, string initialComplaint);
        Task<IEnumerable<PatientVisit>> GetActiveVisitsAsync();
        Task<PatientVisit?> GetVisitByIdAsync(int visitId);
        
        // Triage
        Task<TriageRecord> RecordTriageAsync(TriageRecord record);
        
        // Insurance
        Task<IEnumerable<InsuranceType>> GetInsuranceTypesAsync();
        
        // Flow Control
        Task<bool> UpdateVisitStatusAsync(int visitId, string status, string? priority = null);
    }
}
