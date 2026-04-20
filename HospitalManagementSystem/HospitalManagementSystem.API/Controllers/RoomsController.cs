using HospitalManagementSystem.Data;
using HospitalManagementSystem.Data.Entities;
using HospitalManagementSystem.Data.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace HospitalManagementSystem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoomsController : ControllerBase
    {
        private readonly INurseService _nurseService;
        private readonly ApplicationDbContext _context;

        public RoomsController(INurseService nurseService, ApplicationDbContext context)
        {
            _nurseService = nurseService;
            _context = context;
        }

        /// <summary>
        /// Get all rooms
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<object>>> GetRooms()
        {
            try
            {
                var rooms = await _context.Rooms
                    .Include(r => r.RoomType)
                    .Include(r => r.RoomAssignments)
                    .Select(r => new
                    {
                        r.RoomId,
                        r.RoomNumber,
                        r.RoomType.RoomTypeName,
                        r.Capacity,
                        OccupiedBeds = r.RoomAssignments.Count(ra => ra.DischargeDate == null),
                        AvailableBeds = r.Capacity - r.RoomAssignments.Count(ra => ra.DischargeDate == null),
                        Status = r.RoomAssignments.Count(ra => ra.DischargeDate == null) >= r.Capacity ? "Full" : "Available"
                    })
                    .ToListAsync();
                return Ok(rooms);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Get room by ID
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<object>> GetRoom(int id)
        {
            try
            {
                var room = await _context.Rooms
                    .Include(r => r.RoomType)
                    .Include(r => r.RoomAssignments)
                    .ThenInclude(ra => ra.Patient)
                    .ThenInclude(p => p.User)
                    .Where(r => r.RoomId == id)
                    .Select(r => new
                    {
                        r.RoomId,
                        r.RoomNumber,
                        r.RoomType.RoomTypeName,
                        r.Capacity,
                        OccupiedBeds = r.RoomAssignments.Count(ra => ra.DischargeDate == null),
                        AvailableBeds = r.Capacity - r.RoomAssignments.Count(ra => ra.DischargeDate == null),
                        Status = r.RoomAssignments.Count(ra => ra.DischargeDate == null) >= r.Capacity ? "Full" : "Available",
                        CurrentPatients = r.RoomAssignments
                            .Where(ra => ra.DischargeDate == null)
                            .Select(ra => new
                            {
                                ra.RoomAssignmentId,
                                PatientName = ra.Patient.User.FullName,
                                ra.Patient.User.Email,
                                ra.AdmissionDate
                            })
                            .ToList()
                    })
                    .FirstOrDefaultAsync();

                if (room == null)
                    return NotFound(new { message = "Room not found" });

                return Ok(room);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Create a new room
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<object>> CreateRoom([FromBody] CreateRoomDto dto)
        {
            try
            {
                if (dto.Capacity <= 0)
                    return BadRequest(new { message = "Capacity must be greater than 0" });

                var room = new Room
                {
                    RoomNumber = dto.RoomNumber,
                    RoomTypeId = dto.RoomTypeId,
                    Capacity = dto.Capacity
                };

                _context.Rooms.Add(room);
                await _context.SaveChangesAsync();

                return CreatedAtAction(nameof(GetRoom), new { id = room.RoomId }, new
                {
                    room.RoomId,
                    room.RoomNumber,
                    room.RoomTypeId,
                    room.Capacity
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Update room information
        /// </summary>
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateRoom(int id, [FromBody] UpdateRoomDto dto)
        {
            try
            {
                var room = await _context.Rooms.FindAsync(id);
                if (room == null)
                    return NotFound(new { message = "Room not found" });

                if (!string.IsNullOrWhiteSpace(dto.RoomNumber))
                    room.RoomNumber = dto.RoomNumber;
                if (dto.RoomTypeId.HasValue)
                    room.RoomTypeId = dto.RoomTypeId.Value;
                if (dto.Capacity.HasValue && dto.Capacity > 0)
                    room.Capacity = dto.Capacity.Value;

                _context.Rooms.Update(room);
                await _context.SaveChangesAsync();

                return Ok(new { message = "Room updated successfully" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Delete a room
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRoom(int id)
        {
            try
            {
                var room = await _context.Rooms.FindAsync(id);
                if (room == null)
                    return NotFound(new { message = "Room not found" });

                _context.Rooms.Remove(room);
                await _context.SaveChangesAsync();

                return Ok(new { message = "Room deleted successfully" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Get available rooms
        /// </summary>
        [HttpGet("available/list")]
        public async Task<ActionResult<IEnumerable<object>>> GetAvailableRooms()
        {
            try
            {
                var availableRooms = await _context.Rooms
                    .Include(r => r.RoomType)
                    .Include(r => r.RoomAssignments)
                    .Where(r => r.RoomAssignments.Count(ra => ra.DischargeDate == null) < r.Capacity)
                    .Select(r => new
                    {
                        r.RoomId,
                        r.RoomNumber,
                        r.RoomType.RoomTypeName,
                        r.Capacity,
                        OccupiedBeds = r.RoomAssignments.Count(ra => ra.DischargeDate == null),
                        AvailableBeds = r.Capacity - r.RoomAssignments.Count(ra => ra.DischargeDate == null)
                    })
                    .ToListAsync();

                return Ok(availableRooms);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Assign patient to room
        /// </summary>
        [HttpPost("{id}/assign")]
        public async Task<IActionResult> AssignPatientToRoom(int id, [FromBody] AssignRoomDto dto)
        {
            try
            {
                var room = await _context.Rooms
                    .Include(r => r.RoomAssignments)
                    .FirstOrDefaultAsync(r => r.RoomId == id);

                if (room == null)
                    return NotFound(new { message = "Room not found" });

                // Check if room has available beds
                var occupiedBeds = room.RoomAssignments.Count(ra => ra.DischargeDate == null);
                if (occupiedBeds >= room.Capacity)
                    return BadRequest(new { message = "Room is full" });

                var patient = await _context.Patients.FindAsync(dto.PatientId);
                if (patient == null)
                    return NotFound(new { message = "Patient not found" });

                var roomAssignment = new RoomAssignment
                {
                    RoomId = id,
                    PatientId = dto.PatientId,
                    AdmissionDate = DateTime.Now
                };

                _context.RoomAssignments.Add(roomAssignment);
                await _context.SaveChangesAsync();

                return Ok(new
                {
                    message = "Patient assigned to room successfully",
                    roomAssignment.RoomAssignmentId,
                    roomAssignment.RoomId,
                    roomAssignment.PatientId,
                    roomAssignment.AdmissionDate
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Discharge patient from room
        /// </summary>
        [HttpPost("{id}/discharge/{assignmentId}")]
        public async Task<IActionResult> DischargePatient(int id, int assignmentId)
        {
            try
            {
                var roomAssignment = await _context.RoomAssignments.FindAsync(assignmentId);
                if (roomAssignment == null)
                    return NotFound(new { message = "Room assignment not found" });

                if (roomAssignment.RoomId != id)
                    return BadRequest(new { message = "Room assignment does not match the room" });

                roomAssignment.DischargeDate = DateTime.Now;
                _context.RoomAssignments.Update(roomAssignment);
                await _context.SaveChangesAsync();

                return Ok(new
                {
                    message = "Patient discharged successfully",
                    roomAssignment.RoomAssignmentId,
                    roomAssignment.DischargeDate
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Get room occupancy report
        /// </summary>
        [HttpGet("occupancy/report")]
        public async Task<ActionResult<object>> GetOccupancyReport()
        {
            try
            {
                var totalRooms = await _context.Rooms.CountAsync();
                var totalCapacity = await _context.Rooms.SumAsync(r => r.Capacity);
                var occupiedBeds = await _context.RoomAssignments
                    .Where(ra => ra.DischargeDate == null)
                    .CountAsync();

                var occupancyRate = totalCapacity > 0 ? (occupiedBeds * 100.0 / totalCapacity) : 0;

                var roomDetails = await _context.Rooms
                    .Include(r => r.RoomType)
                    .Include(r => r.RoomAssignments)
                    .Select(r => new
                    {
                        r.RoomNumber,
                        r.RoomType.RoomTypeName,
                        r.Capacity,
                        OccupiedBeds = r.RoomAssignments.Count(ra => ra.DischargeDate == null),
                        OccupancyPercentage = r.Capacity > 0 ? (r.RoomAssignments.Count(ra => ra.DischargeDate == null) * 100.0 / r.Capacity) : 0
                    })
                    .ToListAsync();

                return Ok(new
                {
                    TotalRooms = totalRooms,
                    TotalCapacity = totalCapacity,
                    OccupiedBeds = occupiedBeds,
                    AvailableBeds = totalCapacity - occupiedBeds,
                    OccupancyRate = Math.Round(occupancyRate, 2),
                    RoomDetails = roomDetails
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
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

    public class AssignRoomDto
    {
        public int PatientId { get; set; }
    }
}
