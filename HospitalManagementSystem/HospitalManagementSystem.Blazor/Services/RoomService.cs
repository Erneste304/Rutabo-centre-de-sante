using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HospitalManagementSystem.Blazor.Services
{
    public interface IRoomService
    {
        Task<List<object>> GetAllRoomsAsync();
        Task<object> GetRoomByIdAsync(int roomId);
        Task<object> CreateRoomAsync(CreateRoomDto dto);
        Task<bool> UpdateRoomAsync(int roomId, UpdateRoomDto dto);
        Task<bool> DeleteRoomAsync(int roomId);
        Task<List<object>> GetAvailableRoomsAsync();
        Task<bool> AssignPatientToRoomAsync(int roomId, int patientId);
        Task<bool> DischargePatientFromRoomAsync(int roomId, int assignmentId);
        Task<object> GetOccupancyReportAsync();
    }

    public class RoomService : IRoomService
    {
        private readonly ApiService _apiService;

        public RoomService(ApiService apiService)
        {
            _apiService = apiService;
        }

        public async Task<List<object>> GetAllRoomsAsync()
        {
            try
            {
                var response = await _apiService.GetAsync("api/rooms");
                return response;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching rooms: {ex.Message}");
                return new List<object>();
            }
        }

        public async Task<object> GetRoomByIdAsync(int roomId)
        {
            try
            {
                var response = await _apiService.GetAsync($"api/rooms/{roomId}");
                return response;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching room {roomId}: {ex.Message}");
                return null;
            }
        }

        public async Task<object> CreateRoomAsync(CreateRoomDto dto)
        {
            try
            {
                var response = await _apiService.PostAsync("api/rooms", dto);
                return response;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error creating room: {ex.Message}");
                return null;
            }
        }

        public async Task<bool> UpdateRoomAsync(int roomId, UpdateRoomDto dto)
        {
            try
            {
                await _apiService.PutAsync($"api/rooms/{roomId}", dto);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating room {roomId}: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> DeleteRoomAsync(int roomId)
        {
            try
            {
                await _apiService.DeleteAsync($"api/rooms/{roomId}");
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting room {roomId}: {ex.Message}");
                return false;
            }
        }

        public async Task<List<object>> GetAvailableRoomsAsync()
        {
            try
            {
                var response = await _apiService.GetAsync("api/rooms/available/list");
                return response;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching available rooms: {ex.Message}");
                return new List<object>();
            }
        }

        public async Task<bool> AssignPatientToRoomAsync(int roomId, int patientId)
        {
            try
            {
                var dto = new { patientId };
                await _apiService.PostAsync($"api/rooms/{roomId}/assign", dto);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error assigning patient to room: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> DischargePatientFromRoomAsync(int roomId, int assignmentId)
        {
            try
            {
                await _apiService.PostAsync($"api/rooms/{roomId}/discharge/{assignmentId}", null);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error discharging patient from room: {ex.Message}");
                return false;
            }
        }

        public async Task<object> GetOccupancyReportAsync()
        {
            try
            {
                var response = await _apiService.GetAsync("api/rooms/occupancy/report");
                return response;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching occupancy report: {ex.Message}");
                return null;
            }
        }
    }

    // DTOs
    public class CreateRoomDto
    {
        public string RoomNumber { get; set; }
        public int RoomTypeId { get; set; }
        public int Capacity { get; set; }
    }

    public class UpdateRoomDto
    {
        public string RoomNumber { get; set; }
        public int? RoomTypeId { get; set; }
        public int? Capacity { get; set; }
    }
}
