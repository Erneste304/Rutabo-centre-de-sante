using System.Collections.Generic;
using System.Threading.Tasks;
using HospitalManagementSystem.Core.Models;

namespace HospitalManagementSystem.Core.Interfaces
{
    public interface IPatientRepository
    {
        Task<IEnumerable<Patient>> GetAllPatientsAsync();
        Task<Patient> GetPatientByIdAsync(int id);
        Task AddPatientAsync(Patient patient);
        Task UpdatePatientAsync(Patient patient);
        Task DeletePatientAsync(int id);
    }
}