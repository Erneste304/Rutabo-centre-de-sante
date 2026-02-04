using Microsoft.EntityFrameworkCore;
using HospitalManagementSystem.Core.Models;
using HospitalManagementSystem.Core.Repositories;

namespace HospitalManagementSystem.Data.Repositories
{
    public class PatientRepository : IPatientRepository
    {
        private readonly HospitalContext _context;

        public PatientRepository(HospitalContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Patient>> GetAllAsync()
        {
            return await _context.Patients
                .Include(p => p.User)
                .ToListAsync();
        }

        public async Task<Patient?> GetByIdAsync(int id)
        {
            return await _context.Patients
                .Include(p => p.User)
                .Include(p => p.Appointments)
                .FirstOrDefaultAsync(p => p.PatientId == id);
        }

        public async Task<Patient?> GetByUserIdAsync(int userId)
        {
            return await _context.Patients
                .Include(p => p.User)
                .FirstOrDefaultAsync(p => p.UserId == userId);
        }

        public async Task AddAsync(Patient patient)
        {
            await _context.Patients.AddAsync(patient);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Patient patient)
        {
            _context.Patients.Update(patient);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var patient = await GetByIdAsync(id);
            if (patient != null)
            {
                _context.Patients.Remove(patient);
                await _context.SaveChangesAsync();
            }
        }

                public async Task<bool> ExistsAsync(int id)
        {
            return await _context.Patients.AnyAsync(p => p.PatientId == id);
        }

        public async Task<IEnumerable<Appointment>> GetPatientAppointmentsAsync(int patientId)
        {
            return await _context.Appointments
                .Include(a => a.Doctor)
                .Where(a => a.PatientId == patientId)
                .OrderByDescending(a => a.AppointmentDate)
                .ToListAsync();
        }
    }
}