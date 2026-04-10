using HospitalManagementSystem.Data.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HospitalManagementSystem.Data.Services
{
    public interface IAmbulanceService
    {
        Task<IEnumerable<Ambulance>> GetFleetAsync();
        Task<Ambulance?> GetAmbulanceByIdAsync(int id);
        Task<IEnumerable<AmbulanceLog>> GetActiveLogsAsync();
        Task<AmbulanceLog> DispatchAmbulanceAsync(AmbulanceLog log);
        Task<bool> UpdateLogStatusAsync(int logId, string status, DateTime? timestamp = null);
        Task<bool> UpdateAmbulanceStatusAsync(int ambulanceId, string status);
    }
}
