using System.Collections.Generic;
using System.Threading.Tasks;
using HospitalManagementSystem.Core.Interfaces;
using HospitalManagementSystem.Core.Models;

namespace HospitalManagementSystem.Core.Services
{
    public class PatientService
    {
        private readonly IPatientRepository _patientRepository;

        public PatientService(IPatientRepository patientRepository)
        {
            _patientRepository = patientRepository;
        }

        public async Task<IEnumerable<Patient>> GetAllPatientsAsync()
        {
            return await _patientRepository.GetAllPatientsAsync();
        }

        public async Task AddPatientAsync(Patient patient)
        {
            if (patient == null)
            {
                throw new ArgumentNullException(nameof(patient));
            }
            if (string.IsNullOrWhiteSpace(patient.FirstName) || string.IsNullOrWhiteSpace(patient.LastName))
            {
                throw new ArgumentException("Patient first name and last name cannot be empty.");
            }
            if (patient.DateOfBirth > DateTime.Now)
            {
                throw new ArgumentException("Date of birth cannot be in the future.");
            }

            await _patientRepository.AddPatientAsync(patient);
        }

        public async Task<Patient> GetPatientByIdAsync(int id)
        {
            return await _patientRepository.GetPatientByIdAsync(id);
        }

        public async Task UpdatePatientAsync(Patient patient)
        {
            if (patient == null)
            {
                throw new ArgumentNullException(nameof(patient));
            }
            if (string.IsNullOrWhiteSpace(patient.FirstName) || string.IsNullOrWhiteSpace(patient.LastName))
            {
                throw new ArgumentException("Patient first name and last name cannot be empty.");
            }
            if (patient.DateOfBirth > DateTime.Now)
            {
                throw new ArgumentException("Date of birth cannot be in the future.");
            }

            // Ensure the patient exists before attempting to update
            var existingPatient = await _patientRepository.GetPatientByIdAsync(patient.Id);
            if (existingPatient == null)
            {
                throw new InvalidOperationException($"Patient with ID {patient.Id} not found.");
            }

            await _patientRepository.UpdatePatientAsync(patient);
        }
        
        // Add other methods as needed
    }
}