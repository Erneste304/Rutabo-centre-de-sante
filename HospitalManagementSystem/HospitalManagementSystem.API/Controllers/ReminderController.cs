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
    public class ReminderController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ReminderController(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Get all reminders
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<object>>> GetAllReminders()
        {
            try
            {
                var reminders = await _context.AppointmentReminders
                    .Include(r => r.Appointment)
                    .Include(r => r.Patient)
                    .ThenInclude(p => p.User)
                    .OrderByDescending(r => r.ScheduledTime)
                    .Select(r => new
                    {
                        r.ReminderId,
                        r.AppointmentId,
                        r.PatientId,
                        PatientName = r.Patient.User.FullName,
                        r.ReminderType,
                        r.Status,
                        r.ScheduledTime,
                        r.SentTime,
                        r.IsAcknowledged,
                        r.Message
                    })
                    .ToListAsync();

                return Ok(reminders);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Get pending reminders to send
        /// </summary>
        [HttpGet("pending/list")]
        public async Task<ActionResult<IEnumerable<object>>> GetPendingReminders()
        {
            try
            {
                var now = DateTime.Now;
                var reminders = await _context.AppointmentReminders
                    .Where(r => r.Status == "Pending" && r.ScheduledTime <= now)
                    .Include(r => r.Appointment)
                    .Include(r => r.Patient)
                    .ThenInclude(p => p.User)
                    .OrderBy(r => r.ScheduledTime)
                    .Select(r => new
                    {
                        r.ReminderId,
                        r.AppointmentId,
                        r.PatientId,
                        PatientName = r.Patient.User.FullName,
                        r.Patient.User.Email,
                        r.Patient.User.PhoneNumber,
                        r.ReminderType,
                        r.ScheduledTime,
                        r.Message,
                        r.HoursBeforeAppointment
                    })
                    .ToListAsync();

                return Ok(reminders);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Create appointment reminder
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<object>> CreateReminder([FromBody] CreateReminderDto dto)
        {
            try
            {
                var appointment = await _context.Appointments.FindAsync(dto.AppointmentId);
                if (appointment == null)
                    return NotFound(new { message = "Appointment not found" });

                // Calculate scheduled time
                var scheduledTime = appointment.AppointmentDate.AddHours(-(dto.HoursBeforeAppointment ?? 24));

                var reminder = new AppointmentReminder
                {
                    AppointmentId = dto.AppointmentId,
                    PatientId = appointment.PatientId,
                    ReminderType = dto.ReminderType,
                    Status = "Pending",
                    ScheduledTime = scheduledTime,
                    HoursBeforeAppointment = dto.HoursBeforeAppointment ?? 24,
                    Message = $"Reminder: You have an appointment on {appointment.AppointmentDate:MMMM dd, yyyy} at {appointment.AppointmentTime}",
                    CreatedAt = DateTime.Now
                };

                _context.AppointmentReminders.Add(reminder);
                await _context.SaveChangesAsync();

                return CreatedAtAction(nameof(GetReminderById), new { id = reminder.ReminderId }, new
                {
                    reminder.ReminderId,
                    reminder.AppointmentId,
                    reminder.Status,
                    reminder.ScheduledTime
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Get reminder by ID
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<object>> GetReminderById(int id)
        {
            try
            {
                var reminder = await _context.AppointmentReminders
                    .Include(r => r.Appointment)
                    .Include(r => r.Patient)
                    .ThenInclude(p => p.User)
                    .Where(r => r.ReminderId == id)
                    .Select(r => new
                    {
                        r.ReminderId,
                        r.AppointmentId,
                        r.PatientId,
                        PatientName = r.Patient.User.FullName,
                        r.Patient.User.Email,
                        r.ReminderType,
                        r.Status,
                        r.ScheduledTime,
                        r.SentTime,
                        r.Message,
                        r.IsAcknowledged,
                        r.HoursBeforeAppointment
                    })
                    .FirstOrDefaultAsync();

                if (reminder == null)
                    return NotFound(new { message = "Reminder not found" });

                return Ok(reminder);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Mark reminder as sent
        /// </summary>
        [HttpPost("{id}/mark-sent")]
        public async Task<IActionResult> MarkAsSent(int id)
        {
            try
            {
                var reminder = await _context.AppointmentReminders.FindAsync(id);
                if (reminder == null)
                    return NotFound(new { message = "Reminder not found" });

                reminder.Status = "Sent";
                reminder.SentTime = DateTime.Now;
                reminder.UpdatedAt = DateTime.Now;

                _context.AppointmentReminders.Update(reminder);
                await _context.SaveChangesAsync();

                return Ok(new { message = "Reminder marked as sent" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Acknowledge reminder
        /// </summary>
        [HttpPost("{id}/acknowledge")]
        public async Task<IActionResult> AcknowledgeReminder(int id)
        {
            try
            {
                var reminder = await _context.AppointmentReminders.FindAsync(id);
                if (reminder == null)
                    return NotFound(new { message = "Reminder not found" });

                reminder.IsAcknowledged = true;
                reminder.AcknowledgedTime = DateTime.Now;
                reminder.UpdatedAt = DateTime.Now;

                _context.AppointmentReminders.Update(reminder);
                await _context.SaveChangesAsync();

                return Ok(new { message = "Reminder acknowledged" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Get reminders by appointment
        /// </summary>
        [HttpGet("appointment/{appointmentId}")]
        public async Task<ActionResult<IEnumerable<object>>> GetRemindersByAppointment(int appointmentId)
        {
            try
            {
                var reminders = await _context.AppointmentReminders
                    .Where(r => r.AppointmentId == appointmentId)
                    .Include(r => r.Patient)
                    .ThenInclude(p => p.User)
                    .Select(r => new
                    {
                        r.ReminderId,
                        r.ReminderType,
                        r.Status,
                        r.ScheduledTime,
                        r.SentTime,
                        r.IsAcknowledged,
                        r.Message
                    })
                    .ToListAsync();

                return Ok(reminders);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Get reminder statistics
        /// </summary>
        [HttpGet("statistics/dashboard")]
        public async Task<ActionResult<object>> GetReminderStatistics()
        {
            try
            {
                var totalReminders = await _context.AppointmentReminders.CountAsync();
                var sentReminders = await _context.AppointmentReminders.CountAsync(r => r.Status == "Sent");
                var pendingReminders = await _context.AppointmentReminders.CountAsync(r => r.Status == "Pending");
                var failedReminders = await _context.AppointmentReminders.CountAsync(r => r.Status == "Failed");
                var acknowledgedReminders = await _context.AppointmentReminders.CountAsync(r => r.IsAcknowledged);

                var remindersByType = await _context.AppointmentReminders
                    .GroupBy(r => r.ReminderType)
                    .Select(g => new { Type = g.Key, Count = g.Count() })
                    .ToListAsync();

                var acknowledgmentRate = totalReminders > 0 ? (acknowledgedReminders * 100.0 / sentReminders) : 0;

                return Ok(new
                {
                    TotalReminders = totalReminders,
                    SentReminders = sentReminders,
                    PendingReminders = pendingReminders,
                    FailedReminders = failedReminders,
                    AcknowledgedReminders = acknowledgedReminders,
                    AcknowledgmentRate = Math.Round(acknowledgmentRate, 2),
                    RemindersByType = remindersByType
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }

    // DTOs
    public class CreateReminderDto
    {
        public int AppointmentId { get; set; }
        public string ReminderType { get; set; } // Email, SMS, InApp
        public int? HoursBeforeAppointment { get; set; } // 24, 12, 2, etc.
    }
}
