using HospitalManagementSystem.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HospitalManagementSystem.Data.Services
{
    public interface INurseService
    {
        // Shift Reports
        Task<ShiftReport> CreateShiftReportAsync(int nurseId, ShiftReport report);
        Task<ShiftReport?> GetShiftReportByIdAsync(int reportId);
        Task<IEnumerable<ShiftReport>> GetNurseShiftReportsAsync(int nurseId, DateTime? startDate = null, DateTime? endDate = null);
        Task<ShiftReport> UpdateShiftReportAsync(int reportId, ShiftReport report);
        Task<bool> DeleteShiftReportAsync(int reportId);
        
        // Room Management
        Task<Room?> GetRoomByNumberAsync(string roomNumber);
        Task<IEnumerable<Room>> GetAvailableRoomsAsync();
        Task<Room> UpdateRoomStatusAsync(string roomNumber, string newStatus, string? patientName = null);
        Task<IEnumerable<Room>> GetRoomsByStatusAsync(string status);
        Task<IEnumerable<RoomHistory>> GetRoomHistoryAsync(string roomNumber, int days = 30);
        
        // Patient Assignment
        Task<bool> AssignPatientToRoomAsync(int patientId, string roomNumber);
        Task<bool> RemovePatientFromRoomAsync(string roomNumber);
        
        // Inventory
        Task<IEnumerable<Inventory>> GetLowStockItemsAsync();
        Task<Inventory?> GetInventoryItemAsync(string itemCode);
        Task<Inventory> UpdateInventoryStockAsync(string itemCode, int quantityChange, string reason);
        
        // Schedule
        Task<IEnumerable<EmployeeShift>> GetNurseScheduleAsync(int nurseId, DateTime startDate, DateTime endDate);
    }

    public class NurseService : INurseService
    {
        private readonly ApplicationDbContext _context;

        public NurseService(ApplicationDbContext context)
        {
            _context = context;
        }

        // SHIFT REPORTS
        public async Task<ShiftReport> CreateShiftReportAsync(int nurseId, ShiftReport report)
        {
            report.NurseId = nurseId;
            report.CreatedAt = DateTime.UtcNow;
            
            _context.ShiftReports.Add(report);
            await _context.SaveChangesAsync();
            
            return report;
        }

        public async Task<ShiftReport?> GetShiftReportByIdAsync(int reportId)
        {
            return await _context.ShiftReports
                .Include(r => r.Nurse)
                .FirstOrDefaultAsync(r => r.ReportId == reportId);
        }

        public async Task<IEnumerable<ShiftReport>> GetNurseShiftReportsAsync(int nurseId, DateTime? startDate = null, DateTime? endDate = null)
        {
            var query = _context.ShiftReports
                .Where(r => r.NurseId == nurseId);
            
            if (startDate.HasValue)
                query = query.Where(r => r.ShiftDate >= startDate.Value);
            
            if (endDate.HasValue)
                query = query.Where(r => r.ShiftDate <= endDate.Value);
            
            return await query.OrderByDescending(r => r.ShiftDate).ToListAsync();
        }

        public async Task<ShiftReport> UpdateShiftReportAsync(int reportId, ShiftReport report)
        {
            var existingReport = await GetShiftReportByIdAsync(reportId);
            if (existingReport == null)
                throw new Exception("Shift report not found");
            
            // Update fields
            existingReport.PatientsHandled = report.PatientsHandled;
            existingReport.TasksCompleted = report.TasksCompleted;
            existingReport.MedicationsAdministered = report.MedicationsAdministered;
            existingReport.CriticalIncidents = report.CriticalIncidents;
            existingReport.IssuesConcerns = report.IssuesConcerns;
            existingReport.HandoverNotes = report.HandoverNotes;
            existingReport.NextShiftNotes = report.NextShiftNotes;
            existingReport.IsCompleted = report.IsCompleted;
            
            if (report.IsCompleted && !existingReport.SubmittedAt.HasValue)
                existingReport.SubmittedAt = DateTime.UtcNow;
            
            await _context.SaveChangesAsync();
            return existingReport;
        }

        public async Task<bool> DeleteShiftReportAsync(int reportId)
        {
            var report = await GetShiftReportByIdAsync(reportId);
            if (report == null)
                return false;
            
            _context.ShiftReports.Remove(report);
            await _context.SaveChangesAsync();
            return true;
        }

        // ROOM MANAGEMENT
        public async Task<Room?> GetRoomByNumberAsync(string roomNumber)
        {
            return await _context.Rooms
                .Include(r => r.Department)
                .FirstOrDefaultAsync(r => r.RoomNumber == roomNumber);
        }

        public async Task<IEnumerable<Room>> GetAvailableRoomsAsync()
        {
            return await _context.Rooms
                .Where(r => r.RoomStatus == "Available" && r.AvailableBeds > 0)
                .Include(r => r.Department)
                .OrderBy(r => r.RoomNumber)
                .ToListAsync();
        }

        public async Task<Room> UpdateRoomStatusAsync(string roomNumber, string newStatus, string? patientName = null)
        {
            var room = await GetRoomByNumberAsync(roomNumber);
            if (room == null)
                throw new Exception("Room not found");
            
            room.RoomStatus = newStatus;
            
            if (!string.IsNullOrEmpty(patientName))
                room.PatientName = patientName;
            
            // Update bed counts
            if (newStatus == "Occupied")
                room.AvailableBeds = Math.Max(0, room.AvailableBeds - 1);
            else if (newStatus == "Available")
                room.AvailableBeds = room.BedCount;
            
            await _context.SaveChangesAsync();
            return room;
        }

        public async Task<IEnumerable<Room>> GetRoomsByStatusAsync(string status)
        {
            return await _context.Rooms
                .Where(r => r.RoomStatus == status)
                .Include(r => r.Department)
                .OrderBy(r => r.RoomNumber)
                .ToListAsync();
        }

        public async Task<IEnumerable<RoomHistory>> GetRoomHistoryAsync(string roomNumber, int days = 30)
        {
            
            
            return new List<RoomHistory>
            {
                new RoomHistory
                {
                    Date = DateTime.Now.AddDays(-1),
                    Status = "Occupied",
                    PatientName = "John Doe",
                    Notes = "Patient discharged"
                },
                new RoomHistory
                {
                    Date = DateTime.Now.AddDays(-3),
                    Status = "Cleaning",
                    Notes = "Regular cleaning"
                }
            };
        }

        public async Task<bool> AssignPatientToRoomAsync(int patientId, string roomNumber)
        {
            var room = await GetRoomByNumberAsync(roomNumber);
            if (room == null || room.AvailableBeds <= 0)
                return false;
            
            var patient = await _context.Patients.FindAsync(patientId);
            if (patient == null)
                return false;
            
            // Update room
            room.RoomStatus = "Occupied";
            room.AvailableBeds--;
            room.PatientName = patient.User?.FullName ?? "Unknown";
            room.OccupiedSince = DateTime.UtcNow;
            
            // Update patient
            patient.RoomNumber = roomNumber;
            
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> RemovePatientFromRoomAsync(string roomNumber)
        {
            var room = await GetRoomByNumberAsync(roomNumber);
            if (room == null)
                return false;
            
            room.RoomStatus = "Available";
            room.AvailableBeds = room.BedCount;
            room.PatientName = null;
            room.OccupiedSince = null;
            
            await _context.SaveChangesAsync();
            return true;
        }

        // INVENTORY
        public async Task<IEnumerable<Inventory>> GetLowStockItemsAsync()
        {
            return await _context.Inventory
                .Where(i => i.CurrentStock <= i.MinimumStock && i.Status == "Active")
                .OrderBy(i => i.CurrentStock)
                .ToListAsync();
        }

        public async Task<Inventory?> GetInventoryItemAsync(string itemCode)
        {
            return await _context.Inventory
                .FirstOrDefaultAsync(i => i.ItemCode == itemCode);
        }

        public async Task<Inventory> UpdateInventoryStockAsync(string itemCode, int quantityChange, string reason)
        {
            var item = await GetInventoryItemAsync(itemCode);
            if (item == null)
                throw new Exception("Inventory item not found");
            
            item.CurrentStock += quantityChange;
            
            if (item.CurrentStock <= 0)
            {
                item.CurrentStock = 0;
                item.Status = "Out of Stock";
            }
            else if (item.CurrentStock <= item.MinimumStock)
            {
                item.Status = "Low Stock";
            }
            
            await _context.SaveChangesAsync();
            return item;
        }

        // SCHEDULE
        public async Task<IEnumerable<EmployeeShift>> GetNurseScheduleAsync(int nurseId, DateTime startDate, DateTime endDate)
        {
            return await _context.EmployeeShifts
                .Where(s => s.UserId == nurseId && 
                           s.ShiftDate >= startDate && 
                           s.ShiftDate <= endDate)
                .Include(s => s.Department)
                .OrderBy(s => s.ShiftDate)
                .ThenBy(s => s.StartTime)
                .ToListAsync();
        }
    }

    public class RoomHistory
    {
        public DateTime Date { get; set; }
        public string Status { get; set; } = string.Empty;
        public string? PatientName { get; set; }
        public string? Notes { get; set; }
    }
}