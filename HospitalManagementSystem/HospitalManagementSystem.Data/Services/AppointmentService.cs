using HospitalManagementSystem.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HospitalManagementSystem.Data.Services
{
    public class AppointmentService : IAppointmentService
    {
        private readonly ApplicationDbContext _context;

        public AppointmentService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Appointment>> GetAllAppointmentsAsync()
        {
            return await _context.Appointments
                .Include(a => a.Patient)
                .Include(a => a.Doctor)
                .ThenInclude(d => d.User)
                .OrderByDescending(a => a.AppointmentDate)
                .ThenBy(a => a.AppointmentTime)
                .ToListAsync();
        }

        public async Task<IEnumerable<Appointment>> GetDoctorAppointmentsAsync(int doctorId, DateTime? date = null)
        {
            var query = _context.Appointments
                .Include(a => a.Patient)
                .Where(a => a.DoctorId == doctorId);

            if (date.HasValue)
            {
                var targetDate = date.Value.Date;
                query = query.Where(a => a.AppointmentDate.Date == targetDate);
            }

            return await query
                .OrderBy(a => a.AppointmentTime)
                .ToListAsync();
        }

        public async Task<Appointment?> GetAppointmentByIdAsync(int id)
        {
            return await _context.Appointments
                .Include(a => a.Patient)
                .Include(a => a.Doctor)
                .ThenInclude(d => d.User)
                .FirstOrDefaultAsync(a => a.AppointmentId == id);
        }

        public async Task<Appointment> BookAppointmentAsync(Appointment appointment)
        {
            // Simple double-booking check
            var conflict = await _context.Appointments
                .AnyAsync(a => a.DoctorId == appointment.DoctorId && 
                               a.AppointmentDate.Date == appointment.AppointmentDate.Date && 
                               a.AppointmentTime == appointment.AppointmentTime &&
                               a.Status != "Cancelled");

            if (conflict)
                throw new Exception("This time slot is already booked for this doctor.");

            appointment.CreatedAt = DateTime.UtcNow;
            appointment.AppointmentNumber = "APT-" + DateTime.UtcNow.Ticks.ToString().Substring(10);
            
            _context.Appointments.Add(appointment);
            await _context.SaveChangesAsync();
            return appointment;
        }

        public async Task<bool> UpdateAppointmentStatusAsync(int id, string status)
        {
            var appointment = await _context.Appointments.FindAsync(id);
            if (appointment == null) return false;

            appointment.Status = status;
            appointment.UpdatedAt = DateTime.UtcNow;
            
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<Schedule>> GetDoctorSchedulesAsync(int doctorId)
        {
            return await _context.Schedules
                .Where(s => s.DoctorId == doctorId && s.IsActive)
                .ToListAsync();
        }
    }
}
