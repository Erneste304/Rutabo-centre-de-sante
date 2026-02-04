using HospitalManagementSystem.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HospitalManagementSystem.Core.Repositories
{
    public interface IPatientRepository
    {
        Task<IEnumerable<Patient>> GetAllAsync();
        Task<Patient?> GetByIdAsync(int id);
        Task AddAsync(Patient entity);
        Task UpdateAsync(Patient entity);
        Task DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);
        Task<Patient?> GetByUserIdAsync(int userId);
        Task<IEnumerable<Appointment>> GetPatientAppointmentsAsync(int patientId);
    }
}
