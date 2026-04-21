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
    public class EmergencyAlertController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public EmergencyAlertController(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Get all emergency alerts
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<object>>> GetAllAlerts()
        {
            try
            {
                var alerts = await _context.EmergencyAlerts
                    .Include(a => a.Patient)
                    .ThenInclude(p => p.User)
                    .Include(a => a.AcknowledgedByUser)
                    .OrderByDescending(a => a.Priority)
                    .ThenByDescending(a => a.CreatedAt)
                    .Select(a => new
                    {
                        a.AlertId,
                        a.PatientId,
                        PatientName = a.Patient.User.FullName,
                        a.AlertType,
                        a.Description,
                        a.Status,
                        a.Priority,
                        a.Department,
                        a.CreatedAt,
                        a.AcknowledgedAt,
                        AcknowledgedBy = a.AcknowledgedByUser != null ? a.AcknowledgedByUser.FullName : null,
                        a.ResolutionNotes,
                        a.ResolvedAt,
                        a.NotificationSent
                    })
                    .ToListAsync();

                return Ok(alerts);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Get active alerts only
        /// </summary>
        [HttpGet("active/list")]
        public async Task<ActionResult<IEnumerable<object>>> GetActiveAlerts()
        {
            try
            {
                var alerts = await _context.EmergencyAlerts
                    .Where(a => a.Status == "Active")
                    .Include(a => a.Patient)
                    .ThenInclude(p => p.User)
                    .OrderByDescending(a => a.Priority)
                    .ThenByDescending(a => a.CreatedAt)
                    .Select(a => new
                    {
                        a.AlertId,
                        a.PatientId,
                        PatientName = a.Patient.User.FullName,
                        a.AlertType,
                        a.Description,
                        a.Status,
                        a.Priority,
                        a.Department,
                        a.CreatedAt,
                        TimeElapsed = DateTime.Now - a.CreatedAt,
                        a.NotificationSent
                    })
                    .ToListAsync();

                return Ok(alerts);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Get alert by ID
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<object>> GetAlertById(int id)
        {
            try
            {
                var alert = await _context.EmergencyAlerts
                    .Include(a => a.Patient)
                    .ThenInclude(p => p.User)
                    .Include(a => a.AcknowledgedByUser)
                    .Where(a => a.AlertId == id)
                    .Select(a => new
                    {
                        a.AlertId,
                        a.PatientId,
                        PatientName = a.Patient.User.FullName,
                        a.Patient.User.Email,
                        a.Patient.User.PhoneNumber,
                        a.AlertType,
                        a.Description,
                        a.Status,
                        a.Priority,
                        a.Department,
                        a.CreatedAt,
                        a.AcknowledgedAt,
                        AcknowledgedBy = a.AcknowledgedByUser != null ? a.AcknowledgedByUser.FullName : null,
                        a.ResolutionNotes,
                        a.ResolvedAt
                    })
                    .FirstOrDefaultAsync();

                if (alert == null)
                    return NotFound(new { message = "Alert not found" });

                return Ok(alert);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Create a new emergency alert
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<object>> CreateAlert([FromBody] CreateEmergencyAlertDto dto)
        {
            try
            {
                if (dto.Priority < 1 || dto.Priority > 5)
                    return BadRequest(new { message = "Priority must be between 1 and 5" });

                var alert = new EmergencyAlert
                {
                    PatientId = dto.PatientId,
                    AlertType = dto.AlertType,
                    Description = dto.Description,
                    Status = "Active",
                    Priority = dto.Priority,
                    Department = dto.Department,
                    CreatedAt = DateTime.Now,
                    NotificationSent = false
                };

                _context.EmergencyAlerts.Add(alert);
                await _context.SaveChangesAsync();

                return CreatedAtAction(nameof(GetAlertById), new { id = alert.AlertId }, new
                {
                    alert.AlertId,
                    alert.PatientId,
                    alert.AlertType,
                    alert.Status,
                    alert.Priority,
                    alert.CreatedAt
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Acknowledge an alert
        /// </summary>
        [HttpPost("{id}/acknowledge")]
        public async Task<IActionResult> AcknowledgeAlert(int id, [FromBody] AcknowledgeAlertDto dto)
        {
            try
            {
                var alert = await _context.EmergencyAlerts.FindAsync(id);
                if (alert == null)
                    return NotFound(new { message = "Alert not found" });

                alert.Status = "Acknowledged";
                alert.AcknowledgedAt = DateTime.Now;
                alert.AcknowledgedByUserId = dto.AcknowledgedByUserId;

                _context.EmergencyAlerts.Update(alert);
                await _context.SaveChangesAsync();

                return Ok(new { message = "Alert acknowledged successfully" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Resolve an alert
        /// </summary>
        [HttpPost("{id}/resolve")]
        public async Task<IActionResult> ResolveAlert(int id, [FromBody] ResolveAlertDto dto)
        {
            try
            {
                var alert = await _context.EmergencyAlerts.FindAsync(id);
                if (alert == null)
                    return NotFound(new { message = "Alert not found" });

                alert.Status = "Resolved";
                alert.ResolvedAt = DateTime.Now;
                alert.ResolutionNotes = dto.ResolutionNotes;

                _context.EmergencyAlerts.Update(alert);
                await _context.SaveChangesAsync();

                return Ok(new { message = "Alert resolved successfully" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Get alerts by priority
        /// </summary>
        [HttpGet("priority/{priority}")]
        public async Task<ActionResult<IEnumerable<object>>> GetAlertsByPriority(int priority)
        {
            try
            {
                if (priority < 1 || priority > 5)
                    return BadRequest(new { message = "Priority must be between 1 and 5" });

                var alerts = await _context.EmergencyAlerts
                    .Where(a => a.Priority == priority && a.Status == "Active")
                    .Include(a => a.Patient)
                    .ThenInclude(p => p.User)
                    .OrderByDescending(a => a.CreatedAt)
                    .Select(a => new
                    {
                        a.AlertId,
                        a.PatientId,
                        PatientName = a.Patient.User.FullName,
                        a.AlertType,
                        a.Description,
                        a.Priority,
                        a.Department,
                        a.CreatedAt
                    })
                    .ToListAsync();

                return Ok(alerts);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Get alerts by department
        /// </summary>
        [HttpGet("department/{department}")]
        public async Task<ActionResult<IEnumerable<object>>> GetAlertsByDepartment(string department)
        {
            try
            {
                var alerts = await _context.EmergencyAlerts
                    .Where(a => a.Department == department && a.Status == "Active")
                    .Include(a => a.Patient)
                    .ThenInclude(p => p.User)
                    .OrderByDescending(a => a.Priority)
                    .ThenByDescending(a => a.CreatedAt)
                    .Select(a => new
                    {
                        a.AlertId,
                        a.PatientId,
                        PatientName = a.Patient.User.FullName,
                        a.AlertType,
                        a.Description,
                        a.Priority,
                        a.CreatedAt
                    })
                    .ToListAsync();

                return Ok(alerts);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Get alert statistics
        /// </summary>
        [HttpGet("statistics/dashboard")]
        public async Task<ActionResult<object>> GetAlertStatistics()
        {
            try
            {
                var totalAlerts = await _context.EmergencyAlerts.CountAsync();
                var activeAlerts = await _context.EmergencyAlerts.CountAsync(a => a.Status == "Active");
                var acknowledgedAlerts = await _context.EmergencyAlerts.CountAsync(a => a.Status == "Acknowledged");
                var resolvedAlerts = await _context.EmergencyAlerts.CountAsync(a => a.Status == "Resolved");

                var criticalAlerts = await _context.EmergencyAlerts
                    .Where(a => a.AlertType == "Critical" && a.Status == "Active")
                    .CountAsync();

                var averageResolutionTime = await _context.EmergencyAlerts
                    .Where(a => a.ResolvedAt.HasValue)
                    .Select(a => (a.ResolvedAt.Value - a.CreatedAt).TotalMinutes)
                    .ToListAsync();

                var avgTime = averageResolutionTime.Any() ? averageResolutionTime.Average() : 0;

                return Ok(new
                {
                    TotalAlerts = totalAlerts,
                    ActiveAlerts = activeAlerts,
                    AcknowledgedAlerts = acknowledgedAlerts,
                    ResolvedAlerts = resolvedAlerts,
                    CriticalAlerts = criticalAlerts,
                    AverageResolutionTimeMinutes = Math.Round(avgTime, 2),
                    AlertsByType = await GetAlertsByType(),
                    AlertsByDepartment = await GetAlertsByDepartmentStats()
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        private async Task<object> GetAlertsByType()
        {
            return await _context.EmergencyAlerts
                .GroupBy(a => a.AlertType)
                .Select(g => new { Type = g.Key, Count = g.Count() })
                .ToListAsync();
        }

        private async Task<object> GetAlertsByDepartmentStats()
        {
            return await _context.EmergencyAlerts
                .Where(a => a.Status == "Active")
                .GroupBy(a => a.Department)
                .Select(g => new { Department = g.Key, Count = g.Count() })
                .ToListAsync();
        }
    }

    // DTOs
    public class CreateEmergencyAlertDto
    {
        public int PatientId { get; set; }
        public string AlertType { get; set; } // Critical, Severe, Moderate, Mild
        public string Description { get; set; }
        public int Priority { get; set; } // 1-5
        public string Department { get; set; }
    }

    public class AcknowledgeAlertDto
    {
        public int AcknowledgedByUserId { get; set; }
    }

    public class ResolveAlertDto
    {
        public string ResolutionNotes { get; set; }
    }
}
