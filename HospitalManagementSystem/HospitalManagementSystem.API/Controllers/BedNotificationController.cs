using HospitalManagementSystem.Data;
using HospitalManagementSystem.Data.Entities;
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
    public class BedNotificationController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public BedNotificationController(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Get all bed availability notifications
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<object>>> GetAllNotifications()
        {
            try
            {
                var notifications = await _context.BedAvailabilityNotifications
                    .Include(n => n.Room)
                    .Include(n => n.AcknowledgedByUser)
                    .OrderByDescending(n => n.CreatedAt)
                    .Select(n => new
                    {
                        n.NotificationId,
                        n.RoomId,
                        n.Room.RoomNumber,
                        n.EventType,
                        n.Status,
                        n.AvailableBeds,
                        n.OccupiedBeds,
                        n.TotalCapacity,
                        n.OccupancyPercentage,
                        n.Message,
                        n.CreatedAt,
                        n.SentAt,
                        n.IsUrgent,
                        n.Department
                    })
                    .ToListAsync();

                return Ok(notifications);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Get active bed availability notifications
        /// </summary>
        [HttpGet("active/list")]
        public async Task<ActionResult<IEnumerable<object>>> GetActiveNotifications()
        {
            try
            {
                var notifications = await _context.BedAvailabilityNotifications
                    .Where(n => n.Status == "Pending" || n.Status == "Sent")
                    .Include(n => n.Room)
                    .OrderByDescending(n => n.IsUrgent)
                    .ThenByDescending(n => n.CreatedAt)
                    .Select(n => new
                    {
                        n.NotificationId,
                        n.RoomId,
                        n.Room.RoomNumber,
                        n.EventType,
                        n.AvailableBeds,
                        n.OccupiedBeds,
                        n.OccupancyPercentage,
                        n.Message,
                        n.CreatedAt,
                        n.IsUrgent,
                        n.Department
                    })
                    .ToListAsync();

                return Ok(notifications);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Create bed availability notification
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<object>> CreateNotification([FromBody] CreateBedNotificationDto dto)
        {
            try
            {
                var room = await _context.Rooms
                    .Include(r => r.RoomAssignments)
                    .FirstOrDefaultAsync(r => r.RoomId == dto.RoomId);

                if (room == null)
                    return NotFound(new { message = "Room not found" });

                var occupiedBeds = room.RoomAssignments.Count(ra => ra.DischargeDate == null);
                var availableBeds = room.Capacity - occupiedBeds;
                var occupancyPercentage = (occupiedBeds * 100) / room.Capacity;

                var notification = new BedAvailabilityNotification
                {
                    RoomId = dto.RoomId,
                    EventType = dto.EventType,
                    Status = "Pending",
                    AvailableBeds = availableBeds,
                    OccupiedBeds = occupiedBeds,
                    TotalCapacity = room.Capacity,
                    OccupancyPercentage = occupancyPercentage,
                    Message = GenerateMessage(dto.EventType, room.RoomNumber, availableBeds, occupancyPercentage),
                    CreatedAt = DateTime.Now,
                    IsUrgent = occupancyPercentage >= 90,
                    Department = dto.Department
                };

                _context.BedAvailabilityNotifications.Add(notification);
                await _context.SaveChangesAsync();

                return CreatedAtAction(nameof(GetNotificationById), new { id = notification.NotificationId }, new
                {
                    notification.NotificationId,
                    notification.RoomId,
                    notification.EventType,
                    notification.Status,
                    notification.AvailableBeds,
                    notification.OccupancyPercentage
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Get notification by ID
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<object>> GetNotificationById(int id)
        {
            try
            {
                var notification = await _context.BedAvailabilityNotifications
                    .Include(n => n.Room)
                    .Include(n => n.AcknowledgedByUser)
                    .Where(n => n.NotificationId == id)
                    .Select(n => new
                    {
                        n.NotificationId,
                        n.RoomId,
                        n.Room.RoomNumber,
                        n.EventType,
                        n.Status,
                        n.AvailableBeds,
                        n.OccupiedBeds,
                        n.TotalCapacity,
                        n.OccupancyPercentage,
                        n.Message,
                        n.CreatedAt,
                        n.SentAt,
                        n.AcknowledgedAt,
                        AcknowledgedBy = n.AcknowledgedByUser != null ? n.AcknowledgedByUser.FullName : null,
                        n.IsUrgent
                    })
                    .FirstOrDefaultAsync();

                if (notification == null)
                    return NotFound(new { message = "Notification not found" });

                return Ok(notification);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Mark notification as sent
        /// </summary>
        [HttpPost("{id}/mark-sent")]
        public async Task<IActionResult> MarkAsSent(int id)
        {
            try
            {
                var notification = await _context.BedAvailabilityNotifications.FindAsync(id);
                if (notification == null)
                    return NotFound(new { message = "Notification not found" });

                notification.Status = "Sent";
                notification.SentAt = DateTime.Now;

                _context.BedAvailabilityNotifications.Update(notification);
                await _context.SaveChangesAsync();

                return Ok(new { message = "Notification marked as sent" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Acknowledge notification
        /// </summary>
        [HttpPost("{id}/acknowledge")]
        public async Task<IActionResult> AcknowledgeNotification(int id, [FromBody] AcknowledgeNotificationDto dto)
        {
            try
            {
                var notification = await _context.BedAvailabilityNotifications.FindAsync(id);
                if (notification == null)
                    return NotFound(new { message = "Notification not found" });

                notification.Status = "Acknowledged";
                notification.AcknowledgedAt = DateTime.Now;
                notification.AcknowledgedByUserId = dto.AcknowledgedByUserId;

                _context.BedAvailabilityNotifications.Update(notification);
                await _context.SaveChangesAsync();

                return Ok(new { message = "Notification acknowledged" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Get notifications by room
        /// </summary>
        [HttpGet("room/{roomId}")]
        public async Task<ActionResult<IEnumerable<object>>> GetNotificationsByRoom(int roomId)
        {
            try
            {
                var notifications = await _context.BedAvailabilityNotifications
                    .Where(n => n.RoomId == roomId)
                    .OrderByDescending(n => n.CreatedAt)
                    .Select(n => new
                    {
                        n.NotificationId,
                        n.EventType,
                        n.Status,
                        n.AvailableBeds,
                        n.OccupancyPercentage,
                        n.Message,
                        n.CreatedAt,
                        n.IsUrgent
                    })
                    .ToListAsync();

                return Ok(notifications);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Get bed availability dashboard
        /// </summary>
        [HttpGet("dashboard/status")]
        public async Task<ActionResult<object>> GetBedAvailabilityDashboard()
        {
            try
            {
                var totalRooms = await _context.Rooms.CountAsync();
                var totalCapacity = await _context.Rooms.SumAsync(r => r.Capacity);
                var occupiedBeds = await _context.RoomAssignments
                    .Where(ra => ra.DischargeDate == null)
                    .CountAsync();

                var availableBeds = totalCapacity - occupiedBeds;
                var occupancyRate = totalCapacity > 0 ? (occupiedBeds * 100.0 / totalCapacity) : 0;

                var criticalRooms = await _context.Rooms
                    .Include(r => r.RoomAssignments)
                    .Where(r => r.RoomAssignments.Count(ra => ra.DischargeDate == null) >= r.Capacity)
                    .CountAsync();

                var urgentNotifications = await _context.BedAvailabilityNotifications
                    .Where(n => n.IsUrgent && (n.Status == "Pending" || n.Status == "Sent"))
                    .CountAsync();

                return Ok(new
                {
                    TotalRooms = totalRooms,
                    TotalCapacity = totalCapacity,
                    OccupiedBeds = occupiedBeds,
                    AvailableBeds = availableBeds,
                    OccupancyRate = Math.Round(occupancyRate, 2),
                    CriticalRooms = criticalRooms,
                    UrgentNotifications = urgentNotifications
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        private string GenerateMessage(string eventType, string roomNumber, int availableBeds, int occupancyPercentage)
        {
            return eventType switch
            {
                "BedAvailable" => $"Bed available in Room {roomNumber}. {availableBeds} beds now available.",
                "BedOccupied" => $"Bed occupied in Room {roomNumber}. Occupancy: {occupancyPercentage}%",
                "RoomFull" => $"Room {roomNumber} is now FULL. No beds available.",
                "RoomEmpty" => $"Room {roomNumber} is now EMPTY. All beds available.",
                _ => $"Room {roomNumber} status updated. Available beds: {availableBeds}"
            };
        }
    }

    // DTOs
    public class CreateBedNotificationDto
    {
        public int RoomId { get; set; }
        public string EventType { get; set; } // BedAvailable, BedOccupied, RoomFull, RoomEmpty
        public string Department { get; set; }
    }

    public class AcknowledgeNotificationDto
    {
        public int AcknowledgedByUserId { get; set; }
    }
}
