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
    public class QueueController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public QueueController(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Get all queue entries
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<object>>> GetQueue()
        {
            try
            {
                var queue = await _context.PatientQueues
                    .Include(q => q.Patient)
                    .ThenInclude(p => p.User)
                    .Include(q => q.AssignedDoctor)
                    .ThenInclude(d => d.User)
                    .OrderBy(q => q.QueueNumber)
                    .Select(q => new
                    {
                        q.QueueId,
                        q.QueueNumber,
                        q.PatientId,
                        PatientName = q.Patient.User.FullName,
                        q.Patient.User.PhoneNumber,
                        q.Department,
                        q.Status,
                        q.Priority,
                        q.CheckInTime,
                        q.CalledTime,
                        WaitTimeMinutes = (int)(DateTime.Now - q.CheckInTime).TotalMinutes,
                        DoctorName = q.AssignedDoctor != null ? q.AssignedDoctor.User.FullName : null,
                        q.Reason
                    })
                    .ToListAsync();

                return Ok(queue);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Get current waiting queue
        /// </summary>
        [HttpGet("waiting/list")]
        public async Task<ActionResult<IEnumerable<object>>> GetWaitingQueue()
        {
            try
            {
                var queue = await _context.PatientQueues
                    .Where(q => q.Status == "Waiting")
                    .Include(q => q.Patient)
                    .ThenInclude(p => p.User)
                    .OrderBy(q => q.QueueNumber)
                    .Select(q => new
                    {
                        q.QueueId,
                        q.QueueNumber,
                        PatientName = q.Patient.User.FullName,
                        q.Department,
                        q.Priority,
                        q.CheckInTime,
                        WaitTimeMinutes = (int)(DateTime.Now - q.CheckInTime).TotalMinutes,
                        q.Reason
                    })
                    .ToListAsync();

                return Ok(queue);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Add patient to queue
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<object>> AddToQueue([FromBody] AddToQueueDto dto)
        {
            try
            {
                var patient = await _context.Patients.FindAsync(dto.PatientId);
                if (patient == null)
                    return NotFound(new { message = "Patient not found" });

                // Generate queue number
                var lastQueue = await _context.PatientQueues
                    .Where(q => q.Department == dto.Department && q.CheckInTime.Date == DateTime.Now.Date)
                    .OrderByDescending(q => q.QueueNumber)
                    .FirstOrDefaultAsync();

                int queueNumber = (lastQueue?.QueueNumber ?? 0) + 1;

                var queueEntry = new PatientQueue
                {
                    PatientId = dto.PatientId,
                    Department = dto.Department,
                    Status = "Waiting",
                    QueueNumber = queueNumber,
                    CheckInTime = DateTime.Now,
                    Reason = dto.Reason,
                    Priority = dto.Priority ?? "Normal",
                    EstimatedWaitTimeMinutes = dto.EstimatedWaitTimeMinutes ?? 15
                };

                _context.PatientQueues.Add(queueEntry);
                await _context.SaveChangesAsync();

                return CreatedAtAction(nameof(GetQueueEntry), new { id = queueEntry.QueueId }, new
                {
                    queueEntry.QueueId,
                    queueEntry.QueueNumber,
                    queueEntry.Status,
                    queueEntry.CheckInTime
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Get queue entry by ID
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<object>> GetQueueEntry(int id)
        {
            try
            {
                var entry = await _context.PatientQueues
                    .Include(q => q.Patient)
                    .ThenInclude(p => p.User)
                    .Include(q => q.AssignedDoctor)
                    .ThenInclude(d => d.User)
                    .Where(q => q.QueueId == id)
                    .Select(q => new
                    {
                        q.QueueId,
                        q.QueueNumber,
                        q.PatientId,
                        PatientName = q.Patient.User.FullName,
                        q.Department,
                        q.Status,
                        q.Priority,
                        q.CheckInTime,
                        q.CalledTime,
                        q.CompletionTime,
                        WaitTimeMinutes = (int)(DateTime.Now - q.CheckInTime).TotalMinutes,
                        DoctorName = q.AssignedDoctor != null ? q.AssignedDoctor.User.FullName : null,
                        q.Reason
                    })
                    .FirstOrDefaultAsync();

                if (entry == null)
                    return NotFound(new { message = "Queue entry not found" });

                return Ok(entry);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Call next patient in queue
        /// </summary>
        [HttpPost("{id}/call")]
        public async Task<IActionResult> CallPatient(int id)
        {
            try
            {
                var entry = await _context.PatientQueues.FindAsync(id);
                if (entry == null)
                    return NotFound(new { message = "Queue entry not found" });

                entry.Status = "Called";
                entry.CalledTime = DateTime.Now;

                _context.PatientQueues.Update(entry);
                await _context.SaveChangesAsync();

                return Ok(new { message = "Patient called successfully" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Mark patient as in-progress
        /// </summary>
        [HttpPost("{id}/in-progress")]
        public async Task<IActionResult> MarkInProgress(int id, [FromBody] MarkInProgressDto dto)
        {
            try
            {
                var entry = await _context.PatientQueues.FindAsync(id);
                if (entry == null)
                    return NotFound(new { message = "Queue entry not found" });

                entry.Status = "In-Progress";
                entry.AssignedDoctorId = dto.DoctorId;

                _context.PatientQueues.Update(entry);
                await _context.SaveChangesAsync();

                return Ok(new { message = "Patient marked as in-progress" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Complete patient queue entry
        /// </summary>
        [HttpPost("{id}/complete")]
        public async Task<IActionResult> CompleteQueue(int id)
        {
            try
            {
                var entry = await _context.PatientQueues.FindAsync(id);
                if (entry == null)
                    return NotFound(new { message = "Queue entry not found" });

                entry.Status = "Completed";
                entry.CompletionTime = DateTime.Now;
                entry.ActualWaitTimeMinutes = (int)(entry.CompletionTime.Value - entry.CheckInTime).TotalMinutes;

                _context.PatientQueues.Update(entry);
                await _context.SaveChangesAsync();

                return Ok(new { message = "Patient queue completed" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Get queue statistics
        /// </summary>
        [HttpGet("statistics/dashboard")]
        public async Task<ActionResult<object>> GetQueueStatistics()
        {
            try
            {
                var totalInQueue = await _context.PatientQueues
                    .CountAsync(q => q.Status == "Waiting" || q.Status == "Called");

                var averageWaitTime = await _context.PatientQueues
                    .Where(q => q.CompletionTime.HasValue)
                    .Select(q => (q.CompletionTime.Value - q.CheckInTime).TotalMinutes)
                    .ToListAsync();

                var avgWait = averageWaitTime.Any() ? averageWaitTime.Average() : 0;

                var queueByDepartment = await _context.PatientQueues
                    .Where(q => q.Status == "Waiting")
                    .GroupBy(q => q.Department)
                    .Select(g => new { Department = g.Key, Count = g.Count() })
                    .ToListAsync();

                var completedToday = await _context.PatientQueues
                    .Where(q => q.Status == "Completed" && q.CompletionTime.Value.Date == DateTime.Now.Date)
                    .CountAsync();

                return Ok(new
                {
                    TotalInQueue = totalInQueue,
                    AverageWaitTimeMinutes = Math.Round(avgWait, 2),
                    CompletedToday = completedToday,
                    QueueByDepartment = queueByDepartment
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }

    // DTOs
    public class AddToQueueDto
    {
        public int PatientId { get; set; }
        public string Department { get; set; }
        public string Reason { get; set; }
        public string Priority { get; set; }
        public int? EstimatedWaitTimeMinutes { get; set; }
    }

    public class MarkInProgressDto
    {
        public int DoctorId { get; set; }
    }
}
