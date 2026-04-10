using HospitalManagementSystem.Data.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HospitalManagementSystem.Data.Services
{
    public interface IMedicalRecordService
    {
        Task<IEnumerable<MedicalRecord>> GetPatientHistoryAsync(int patientId);
        Task<MedicalRecord?> GetRecordByIdAsync(int id);
        Task<MedicalRecord> AddRecordAsync(MedicalRecord record);
        Task<IEnumerable<MedicalRecord>> GetCriticalRecordsAsync();
    }

    public interface ILabTestService
    {
        Task<IEnumerable<LabTest>> GetPendingTestsAsync();
        Task<LabTest?> GetTestByIdAsync(int id);
        Task<LabTest> OrderTestAsync(LabTest test);
        Task<bool> SubmitResultAsync(int testId, string result, string performedBy);
        Task<IEnumerable<LabTest>> GetPatientTestsAsync(int patientId);
    }
}
