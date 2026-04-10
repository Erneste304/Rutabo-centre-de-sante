using HospitalManagementSystem.Data.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HospitalManagementSystem.Data.Services
{
    public interface IAppointmentService
    {
        Task<IEnumerable<Appointment>> GetAllAppointmentsAsync();
        Task<IEnumerable<Appointment>> GetDoctorAppointmentsAsync(int doctorId, DateTime? date = null);
        Task<Appointment?> GetAppointmentByIdAsync(int id);
        Task<Appointment> BookAppointmentAsync(Appointment appointment);
        Task<bool> UpdateAppointmentStatusAsync(int id, string status);
        Task<IEnumerable<Schedule>> GetDoctorSchedulesAsync(int doctorId);
    }
}
