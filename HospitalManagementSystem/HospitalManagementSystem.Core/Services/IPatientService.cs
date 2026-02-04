using HospitalManagementSystem.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HospitalManagementSystem.Core.Services
{
    public interface IPatientService
    {
        Task<IEnumerable<Patient>> GetAllPatientsAsync();
        Task<Patient> GetPatientByIdAsync(int id);
        Task<Patient> CreatePatientAsync(Patient patient);
        Task UpdatePatientAsync(Patient patient);
        Task DeletePatientAsync(int id);
        Task<IEnumerable<Appointment>> GetPatientAppointmentsAsync(int id);
    }
}
