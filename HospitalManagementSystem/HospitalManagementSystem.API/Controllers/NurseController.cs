using HospitalManagementSystem.Data.Entities;
using HospitalManagementSystem.Data.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HospitalManagementSystem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NurseController : ControllerBase
    {
        private readonly INurseService _nurseService;

        public NurseController(INurseService nurseService)
        {
            _nurseService = nurseService;
        }

        // SHIFT REPORTS
        [HttpGet("shift-reports/{nurseId}")]
        public async Task<ActionResult<IEnumerable<ShiftReport>>> GetShiftReports(int nurseId, 
            [FromQuery] DateTime? startDate, [FromQuery] DateTime? endDate)
        {
            try
            {
                var reports = await _nurseService.GetNurseShiftReportsAsync(nurseId, startDate, endDate);
                return Ok(reports);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("shift-reports/report/{id}")]
        public async Task<ActionResult<ShiftReport>> GetShiftReport(int id)
        {
            try
            {
                var report = await _nurseService.GetShiftReportByIdAsync(id);
                if (report == null)
                    return NotFound();
                
                return Ok(report);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost("shift-reports")]
        public async Task<ActionResult<ShiftReport>> CreateShiftReport([FromBody] ShiftReportRequest request)
        {
            try
            {
                var report = new ShiftReport
                {
                    NurseId = request.NurseId,
                    ShiftDate = request.ShiftDate,
                    ShiftType = request.ShiftType,
                    StartTime = TimeSpan.Parse(request.StartTime),
                    EndTime = TimeSpan.Parse(request.EndTime),
                    PatientsHandled = request.PatientsHandled,
                    TasksCompleted = int.TryParse(request.TasksCompleted, out var tasks) ? tasks : 0,
                    MedicationsAdministered = int.TryParse(request.MedicationsAdministered, out var meds) ? meds : 0,
                    CriticalIncidents = request.CriticalIncidents,
                    IssuesConcerns = request.IssuesConcerns,
                    HandoverNotes = request.HandoverNotes,
                    NextShiftNotes = request.NextShiftNotes,
                    IsCompleted = request.IsCompleted,
                    SubmittedAt = request.IsCompleted ? DateTime.UtcNow : null
                };
                
                var createdReport = await _nurseService.CreateShiftReportAsync(request.NurseId, report);
                return CreatedAtAction(nameof(GetShiftReport), new { id = createdReport.ReportId }, createdReport);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPut("shift-reports/{id}")]
        public async Task<ActionResult<ShiftReport>> UpdateShiftReport(int id, [FromBody] ShiftReportRequest request)
        {
            try
            {
                var report = new ShiftReport
                {
                    PatientsHandled = request.PatientsHandled,
                    TasksCompleted = int.TryParse(request.TasksCompleted, out var tasks) ? tasks : 0,
                    MedicationsAdministered = int.TryParse(request.MedicationsAdministered, out var meds) ? meds : 0,
                    CriticalIncidents = request.CriticalIncidents,
                    IssuesConcerns = request.IssuesConcerns,
                    HandoverNotes = request.HandoverNotes,
                    NextShiftNotes = request.NextShiftNotes,
                    IsCompleted = request.IsCompleted
                };
                
                var updatedReport = await _nurseService.UpdateShiftReportAsync(id, report);
                return Ok(updatedReport);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpDelete("shift-reports/{id}")]
        public async Task<IActionResult> DeleteShiftReport(int id)
        {
            try
            {
                var success = await _nurseService.DeleteShiftReportAsync(id);
                if (!success)
                    return NotFound();
                
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // ROOM MANAGEMENT
        [HttpGet("rooms")]
        public async Task<ActionResult<IEnumerable<Room>>> GetRooms()
        {
            try
            {
                var rooms = await _nurseService.GetAvailableRoomsAsync();
                return Ok(rooms);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("rooms/{roomNumber}")]
        public async Task<ActionResult<Room>> GetRoom(string roomNumber)
        {
            try
            {
                var room = await _nurseService.GetRoomByNumberAsync(roomNumber);
                if (room == null)
                    return NotFound();
                
                return Ok(room);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPut("update-room-status")]
        public async Task<ActionResult<Room>> UpdateRoomStatus([FromBody] UpdateRoomStatusRequest request)
        {
            try
            {
                var room = await _nurseService.UpdateRoomStatusAsync(
                    request.RoomNumber, 
                    request.NewStatus, 
                    request.PatientName);
                
                return Ok(room);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost("assign-patient")]
        public async Task<ActionResult> AssignPatientToRoom([FromBody] AssignPatientRequest request)
        {
            try
            {
                var success = await _nurseService.AssignPatientToRoomAsync(
                    request.PatientId, 
                    request.RoomNumber);
                
                if (!success)
                    return BadRequest(new { message = "Failed to assign patient to room" });
                
                return Ok(new { message = "Patient assigned successfully" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("room-history/{roomNumber}")]
        public async Task<ActionResult<IEnumerable<RoomHistory>>> GetRoomHistory(string roomNumber, 
            [FromQuery] int days = 30)
        {
            try
            {
                var history = await _nurseService.GetRoomHistoryAsync(roomNumber, days);
                return Ok(history);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // INVENTORY
        [HttpGet("inventory/low-stock")]
        public async Task<ActionResult<IEnumerable<Inventory>>> GetLowStockItems()
        {
            try
            {
                var items = await _nurseService.GetLowStockItemsAsync();
                return Ok(items);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPut("inventory/update-stock")]
        public async Task<ActionResult<Inventory>> UpdateInventoryStock([FromBody] UpdateStockRequest request)
        {
            try
            {
                var item = await _nurseService.UpdateInventoryStockAsync(
                    request.ItemCode, 
                    request.QuantityChange, 
                    request.Reason);
                
                return Ok(item);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }

    // Request Models
    public class ShiftReportRequest
    {
        public int NurseId { get; set; }
        public DateTime ShiftDate { get; set; }
        public string ShiftType { get; set; } = string.Empty;
        public string StartTime { get; set; } = string.Empty;
        public string EndTime { get; set; } = string.Empty;
        public string? PatientsHandled { get; set; }
        public string? TasksCompleted { get; set; }
        public string? MedicationsAdministered { get; set; }
        public string? CriticalIncidents { get; set; }
        public string? IssuesConcerns { get; set; }
        public string? HandoverNotes { get; set; }
        public string? NextShiftNotes { get; set; }
        public bool IsCompleted { get; set; }
    }

    public class UpdateRoomStatusRequest
    {
        public string RoomNumber { get; set; } = string.Empty;
        public string NewStatus { get; set; } = string.Empty;
        public string? PatientName { get; set; }
        public string? Reason { get; set; }
    }

    public class AssignPatientRequest
    {
        public int PatientId { get; set; }
        public string RoomNumber { get; set; } = string.Empty;
        public string? Notes { get; set; }
    }

    public class UpdateStockRequest
    {
        public string ItemCode { get; set; } = string.Empty;
        public int QuantityChange { get; set; }
        public string Reason { get; set; } = string.Empty;
    }
}