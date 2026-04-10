using HospitalManagementSystem.Blazor.Models.DTOs;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace HospitalManagementSystem.Blazor.Services
{
    public class AppointmentService
    {
        private readonly ApiService _api;

        public AppointmentService(ApiService api)
        {
            _api = api;
        }

        public async Task<List<AppointmentModel>> GetAppointmentsAsync()
        {
            return await _api.RequestAsync<List<AppointmentModel>>("api/appointments") ?? new List<AppointmentModel>();
        }

        public async Task<List<AppointmentModel>> GetDoctorAppointmentsAsync(int doctorId, DateTime? date = null)
        {
            var url = $"api/appointments/doctor/{doctorId}";
            if (date.HasValue) url += $"?date={date.Value:yyyy-MM-dd}";
            
            return await _api.RequestAsync<List<AppointmentModel>>(url) ?? new List<AppointmentModel>();
        }

        public async Task<bool> BookAppointmentAsync(AppointmentModel appointment)
        {
            var result = await _api.RequestAsync<AppointmentModel>("api/appointments", HttpMethod.Post, appointment);
            return result != null;
        }

        public async Task<bool> UpdateStatusAsync(int id, string status)
        {
            var result = await _api.RequestAsync<object>($"api/appointments/{id}/status", new HttpMethod("PATCH"), status);
            return true; // Simple error handling for now
        }

        public async Task<List<ScheduleModel>> GetDoctorSchedulesAsync(int doctorId)
        {
            return await _api.RequestAsync<List<ScheduleModel>>($"api/appointments/doctor/{doctorId}/schedules") ?? new List<ScheduleModel>();
        }
    }
}
