using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using HospitalManagementSystem.Blazor.Models;

namespace HospitalManagementSystem.Blazor.Services
{
    public interface IDoctorService
    {
        Task<List<object>> GetAllDoctorsAsync();
        Task<object> GetDoctorByIdAsync(int doctorId);
        Task<object> CreateDoctorAsync(CreateDoctorDto dto);
        Task<bool> UpdateDoctorAsync(int doctorId, UpdateDoctorDto dto);
        Task<bool> DeleteDoctorAsync(int doctorId);
        Task<List<object>> GetDoctorScheduleAsync(int doctorId);
        Task<List<object>> GetDoctorAppointmentsAsync(int doctorId);
        Task<bool> AssignDoctorToDepartmentAsync(int doctorId, int departmentId);
        Task<List<object>> GetAvailableDoctorsAsync();
    }

    public class DoctorService : IDoctorService
    {
        private readonly ApiService _apiService;

        public DoctorService(ApiService apiService)
        {
            _apiService = apiService;
        }

        public async Task<List<object>> GetAllDoctorsAsync()
        {
            try
            {
                var response = await _apiService.GetAsync("api/doctors");
                return response;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching doctors: {ex.Message}");
                return new List<object>();
            }
        }

        public async Task<object> GetDoctorByIdAsync(int doctorId)
        {
            try
            {
                var response = await _apiService.GetAsync($"api/doctors/{doctorId}");
                return response;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching doctor {doctorId}: {ex.Message}");
                return null;
            }
        }

        public async Task<object> CreateDoctorAsync(CreateDoctorDto dto)
        {
            try
            {
                var response = await _apiService.PostAsync("api/doctors", dto);
                return response;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error creating doctor: {ex.Message}");
                return null;
            }
        }

        public async Task<bool> UpdateDoctorAsync(int doctorId, UpdateDoctorDto dto)
        {
            try
            {
                await _apiService.PutAsync($"api/doctors/{doctorId}", dto);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating doctor {doctorId}: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> DeleteDoctorAsync(int doctorId)
        {
            try
            {
                await _apiService.DeleteAsync($"api/doctors/{doctorId}");
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting doctor {doctorId}: {ex.Message}");
                return false;
            }
        }

        public async Task<List<object>> GetDoctorScheduleAsync(int doctorId)
        {
            try
            {
                var response = await _apiService.GetAsync($"api/doctors/{doctorId}/schedule");
                return response;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching doctor schedule: {ex.Message}");
                return new List<object>();
            }
        }

        public async Task<List<object>> GetDoctorAppointmentsAsync(int doctorId)
        {
            try
            {
                var response = await _apiService.GetAsync($"api/doctors/{doctorId}/appointments");
                return response;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching doctor appointments: {ex.Message}");
                return new List<object>();
            }
        }

        public async Task<bool> AssignDoctorToDepartmentAsync(int doctorId, int departmentId)
        {
            try
            {
                var dto = new { departmentId };
                await _apiService.PostAsync($"api/doctors/{doctorId}/departments", dto);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error assigning doctor to department: {ex.Message}");
                return false;
            }
        }

        public async Task<List<object>> GetAvailableDoctorsAsync()
        {
            try
            {
                var response = await _apiService.GetAsync("api/doctors/available/list");
                return response;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching available doctors: {ex.Message}");
                return new List<object>();
            }
        }
    }

    // DTOs
    public class CreateDoctorDto
    {
        public string FullName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Gender { get; set; }
        public string Address { get; set; }
        public string Qualifications { get; set; }
        public int YearsOfExperience { get; set; }
    }

    public class UpdateDoctorDto
    {
        public string FullName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Qualifications { get; set; }
        public int? YearsOfExperience { get; set; }
        public bool? IsAvailable { get; set; }
    }
}
