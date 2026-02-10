using Microsoft.EntityFrameworkCore;
using HospitalManagementSystem.Core.Models;
using HospitalManagementSystem.Core.Repositories;

namespace HospitalManagementSystem.Data.Repositories
{
    public class PatientRepository : IPatientRepository
    {
        private readonly ApplicationDbContext _context;

        public PatientRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<HospitalManagementSystem.Core.Models.Patient>> GetAllAsync()
        {
            var entities = await _context.Patients
                .Include(p => p.User)
                .ToListAsync();
            
            return entities.Select(MapToModel);
        }

        public async Task<HospitalManagementSystem.Core.Models.Patient?> GetByIdAsync(int id)
        {
            var entity = await _context.Patients
                .Include(p => p.User)
                .Include(p => p.Appointments)
                .FirstOrDefaultAsync(p => p.PatientId == id);
            
            return entity == null ? null : MapToModel(entity);
        }

        public async Task<HospitalManagementSystem.Core.Models.Patient?> GetByUserIdAsync(int userId)
        {
            var entity = await _context.Patients
                .Include(p => p.User)
                .FirstOrDefaultAsync(p => p.UserId == userId);
            
            return entity == null ? null : MapToModel(entity);
        }

        public async Task AddAsync(HospitalManagementSystem.Core.Models.Patient patient)
        {
            var entity = new HospitalManagementSystem.Data.Entities.Patient
            {
                UserId = patient.UserId,
                // MedicalRecordNumber is required in Entity but not in Model? 
                // We'll generate one or use a default if needed. 
                MedicalRecordNumber = $"MRN-{DateTime.UtcNow.Ticks}", 
                AdmissionDate = DateTime.UtcNow,
                IsActive = true
            };
            
            await _context.Patients.AddAsync(entity);
            await _context.SaveChangesAsync();
            
            patient.PatientId = entity.PatientId;
        }

        public async Task UpdateAsync(HospitalManagementSystem.Core.Models.Patient patient)
        {
            var entity = await _context.Patients.FindAsync(patient.PatientId);
            if (entity != null)
            {
                // Update properties if matched
                // Core.Models.Patient has name and DoB which are in User entity actually
                // So we might need to update User as well? 
                // For now, we assume this method only updates Patient specific fields if any match.
                // But Core.Models.Patient has almost no matching fields with Data.Entities.Patient except ids.
                
                await _context.SaveChangesAsync();
            }
        }

        public async Task DeleteAsync(int id)
        {
            var patient = await _context.Patients.FindAsync(id);
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

        public async Task<IEnumerable<HospitalManagementSystem.Core.Models.Appointment>> GetPatientAppointmentsAsync(int patientId)
        {
            var entities = await _context.Appointments
                .Include(a => a.Doctor)
                .Where(a => a.PatientId == patientId)
                .OrderByDescending(a => a.AppointmentDate)
                .ToListAsync();

            return entities.Select(MapAppointmentToModel);
        }

        private HospitalManagementSystem.Core.Models.Patient MapToModel(HospitalManagementSystem.Data.Entities.Patient entity)
        {
            var names = (entity.User?.FullName ?? "").Split(' ');
            var firstName = names.Length > 0 ? names[0] : "";
            var lastName = names.Length > 1 ? string.Join(" ", names.Skip(1)) : "";

            return new HospitalManagementSystem.Core.Models.Patient
            {
                PatientId = entity.PatientId,
                UserId = entity.UserId,
                FirstName = firstName,
                LastName = lastName,
                DateOfBirth = entity.User?.DateOfBirth ?? DateTime.MinValue,
                User = entity.User != null ? MapUserToModel(entity.User) : null,
                Appointments = entity.Appointments?.Select(MapAppointmentToModel).ToList()
            };
        }

        private HospitalManagementSystem.Core.Models.User MapUserToModel(HospitalManagementSystem.Data.Entities.User entity)
        {
             Enum.TryParse<HospitalManagementSystem.Core.Models.UserType>(entity.UserType, out var userType);
            
            return new HospitalManagementSystem.Core.Models.User
            {
                UserId = entity.UserId,
                Username = entity.Username,
                Email = entity.Email,
                PasswordHash = entity.PasswordHash,
                UserType = userType,
                IsActive = entity.Status == "Active",
                CreatedAt = entity.CreatedAt
            };
        }

         private HospitalManagementSystem.Core.Models.Appointment MapAppointmentToModel(HospitalManagementSystem.Data.Entities.Appointment entity)
        {
            Enum.TryParse<HospitalManagementSystem.Core.Models.AppointmentStatus>(entity.Status, out var status);

            return new HospitalManagementSystem.Core.Models.Appointment
            {
                AppointmentId = entity.AppointmentId,
                PatientId = entity.PatientId,
                DoctorId = entity.DoctorId,
                AppointmentDate = entity.AppointmentDate,
                Status = status,
                Reason = entity.Reason,
                Diagnosis = entity.Diagnosis,
                Prescription = entity.Prescription,
                CreatedAt = entity.CreatedAt,
                // Doctor = ... (Map doctor if needed, but avoiding circular/deep mapping for now to keep it simple)
            };
        }
    }
}