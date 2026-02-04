using HospitalManagementSystem.Core.Models;
using HospitalManagementSystem.Core.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HospitalManagementSystem.Core.Services
{
    public class PatientService : IPatientService
    {
        private readonly IPatientRepository _patientRepository;

        public PatientService(IPatientRepository patientRepository)
        {
            _patientRepository = patientRepository;
        }

        public async Task<Patient> CreatePatientAsync(Patient patient)
        {
            await _patientRepository.AddAsync(patient);
            return patient;
        }

        public async Task DeletePatientAsync(int id)
        {
            await _patientRepository.DeleteAsync(id);
        }

        public async Task<IEnumerable<Patient>> GetAllPatientsAsync()
        {
            return await _patientRepository.GetAllAsync();
        }

        public async Task<IEnumerable<Appointment>> GetPatientAppointmentsAsync(int id)
        {
            return await _patientRepository.GetPatientAppointmentsAsync(id);
        }

        public async Task<Patient?> GetPatientByIdAsync(int id)
        {
            return await _patientRepository.GetByIdAsync(id);
        }

        public async Task UpdatePatientAsync(Patient patient)
        {
            await _patientRepository.UpdateAsync(patient);
        }
    }
}
