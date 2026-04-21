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
    public class AnalyticsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public AnalyticsController(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Get all performance metrics
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<object>>> GetAllMetrics()
        {
            try
            {
                var metrics = await _context.StaffPerformanceMetrics
                    .Include(m => m.User)
                    .OrderByDescending(m => m.DateRecorded)
                    .Select(m => new
                    {
                        m.MetricId,
                        m.UserId,
                        StaffName = m.User.FullName,
                        m.MetricType,
                        m.MetricValue,
                        m.TargetValue,
                        m.PercentageOfTarget,
                        m.Period,
                        m.Department,
                        m.Status,
                        m.DateRecorded
                    })
                    .ToListAsync();

                return Ok(metrics);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Get metrics for a specific staff member
        /// </summary>
        [HttpGet("staff/{userId}")]
        public async Task<ActionResult<object>> GetStaffMetrics(int userId)
        {
            try
            {
                var user = await _context.Users.FindAsync(userId);
                if (user == null)
                    return NotFound(new { message = "User not found" });

                var metrics = await _context.StaffPerformanceMetrics
                    .Where(m => m.UserId == userId)
                    .OrderByDescending(m => m.DateRecorded)
                    .Select(m => new
                    {
                        m.MetricId,
                        m.MetricType,
                        m.MetricValue,
                        m.TargetValue,
                        m.PercentageOfTarget,
                        m.Period,
                        m.AppointmentsCompleted,
                        m.PatientsServed,
                        m.AveragePatientSatisfactionScore,
                        m.AverageWaitTimeMinutes,
                        m.OnTimePercentage,
                        m.Status,
                        m.DateRecorded
                    })
                    .ToListAsync();

                var summary = new
                {
                    StaffName = user.FullName,
                    StaffId = userId,
                    TotalMetricsRecorded = metrics.Count,
                    Metrics = metrics
                };

                return Ok(summary);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Create performance metric
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<object>> CreateMetric([FromBody] CreatePerformanceMetricDto dto)
        {
            try
            {
                var user = await _context.Users.FindAsync(dto.UserId);
                if (user == null)
                    return NotFound(new { message = "User not found" });

                var percentageOfTarget = dto.TargetValue.HasValue && dto.TargetValue > 0
                    ? (dto.MetricValue / dto.TargetValue.Value) * 100
                    : (decimal?)null;

                var metric = new StaffPerformanceMetrics
                {
                    UserId = dto.UserId,
                    MetricType = dto.MetricType,
                    MetricValue = dto.MetricValue,
                    TargetValue = dto.TargetValue,
                    PercentageOfTarget = percentageOfTarget,
                    Period = dto.Period ?? "Monthly",
                    AppointmentsCompleted = dto.AppointmentsCompleted,
                    PatientsServed = dto.PatientsServed,
                    AveragePatientSatisfactionScore = dto.AveragePatientSatisfactionScore,
                    AverageWaitTimeMinutes = dto.AverageWaitTimeMinutes,
                    OnTimePercentage = dto.OnTimePercentage,
                    NoShowCount = dto.NoShowCount,
                    CancellationCount = dto.CancellationCount,
                    Department = dto.Department,
                    Notes = dto.Notes,
                    DateRecorded = DateTime.Now,
                    IsAboveTarget = percentageOfTarget.HasValue && percentageOfTarget >= 100,
                    Status = DetermineStatus(percentageOfTarget)
                };

                _context.StaffPerformanceMetrics.Add(metric);
                await _context.SaveChangesAsync();

                return CreatedAtAction(nameof(GetMetricById), new { id = metric.MetricId }, new
                {
                    metric.MetricId,
                    metric.UserId,
                    metric.MetricType,
                    metric.MetricValue,
                    metric.Status
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Get metric by ID
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<object>> GetMetricById(int id)
        {
            try
            {
                var metric = await _context.StaffPerformanceMetrics
                    .Include(m => m.User)
                    .Where(m => m.MetricId == id)
                    .Select(m => new
                    {
                        m.MetricId,
                        m.UserId,
                        StaffName = m.User.FullName,
                        m.MetricType,
                        m.MetricValue,
                        m.TargetValue,
                        m.PercentageOfTarget,
                        m.Period,
                        m.AppointmentsCompleted,
                        m.PatientsServed,
                        m.AveragePatientSatisfactionScore,
                        m.AverageWaitTimeMinutes,
                        m.OnTimePercentage,
                        m.NoShowCount,
                        m.CancellationCount,
                        m.Department,
                        m.Status,
                        m.IsAboveTarget,
                        m.DateRecorded,
                        m.Notes
                    })
                    .FirstOrDefaultAsync();

                if (metric == null)
                    return NotFound(new { message = "Metric not found" });

                return Ok(metric);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Get department analytics
        /// </summary>
        [HttpGet("department/{department}")]
        public async Task<ActionResult<object>> GetDepartmentAnalytics(string department)
        {
            try
            {
                var staffMetrics = await _context.StaffPerformanceMetrics
                    .Where(m => m.Department == department)
                    .Include(m => m.User)
                    .GroupBy(m => m.UserId)
                    .Select(g => new
                    {
                        StaffName = g.First().User.FullName,
                        AveragePerformance = g.Average(m => m.PercentageOfTarget),
                        TotalAppointments = g.Sum(m => m.AppointmentsCompleted),
                        AverageSatisfaction = g.Average(m => m.AveragePatientSatisfactionScore),
                        AverageWaitTime = g.Average(m => m.AverageWaitTimeMinutes),
                        Status = g.First().Status
                    })
                    .ToListAsync();

                var departmentStats = new
                {
                    Department = department,
                    TotalStaff = staffMetrics.Count,
                    AverageDepartmentPerformance = staffMetrics.Average(s => s.AveragePerformance),
                    StaffMetrics = staffMetrics
                };

                return Ok(departmentStats);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Get top performers
        /// </summary>
        [HttpGet("top-performers/list")]
        public async Task<ActionResult<IEnumerable<object>>> GetTopPerformers(int limit = 10)
        {
            try
            {
                var topPerformers = await _context.StaffPerformanceMetrics
                    .Where(m => m.Status == "Excellent" || m.Status == "Good")
                    .Include(m => m.User)
                    .OrderByDescending(m => m.PercentageOfTarget)
                    .Take(limit)
                    .Select(m => new
                    {
                        m.MetricId,
                        StaffName = m.User.FullName,
                        m.Department,
                        m.MetricType,
                        m.PercentageOfTarget,
                        m.Status,
                        m.AveragePatientSatisfactionScore,
                        m.DateRecorded
                    })
                    .ToListAsync();

                return Ok(topPerformers);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Get analytics dashboard summary
        /// </summary>
        [HttpGet("dashboard/summary")]
        public async Task<ActionResult<object>> GetDashboardSummary()
        {
            try
            {
                var totalStaff = await _context.Users
                    .Where(u => u.UserType != "Admin" && u.UserType != "Patient")
                    .CountAsync();

                var excellentPerformers = await _context.StaffPerformanceMetrics
                    .Where(m => m.Status == "Excellent")
                    .Select(m => m.UserId)
                    .Distinct()
                    .CountAsync();

                var needsImprovement = await _context.StaffPerformanceMetrics
                    .Where(m => m.Status == "NeedImprovement")
                    .Select(m => m.UserId)
                    .Distinct()
                    .CountAsync();

                var averagePatientSatisfaction = await _context.StaffPerformanceMetrics
                    .Where(m => m.AveragePatientSatisfactionScore.HasValue)
                    .AverageAsync(m => m.AveragePatientSatisfactionScore);

                var departmentPerformance = await _context.StaffPerformanceMetrics
                    .GroupBy(m => m.Department)
                    .Select(g => new
                    {
                        Department = g.Key,
                        AveragePerformance = g.Average(m => m.PercentageOfTarget),
                        StaffCount = g.Select(m => m.UserId).Distinct().Count()
                    })
                    .ToListAsync();

                return Ok(new
                {
                    TotalStaff = totalStaff,
                    ExcellentPerformers = excellentPerformers,
                    NeedsImprovement = needsImprovement,
                    AveragePatientSatisfaction = Math.Round(averagePatientSatisfaction ?? 0, 2),
                    DepartmentPerformance = departmentPerformance
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        private string DetermineStatus(decimal? percentageOfTarget)
        {
            if (!percentageOfTarget.HasValue)
                return "Average";

            return percentageOfTarget switch
            {
                >= 120 => "Excellent",
                >= 100 => "Good",
                >= 80 => "Average",
                _ => "NeedImprovement"
            };
        }
    }

    // DTOs
    public class CreatePerformanceMetricDto
    {
        public int UserId { get; set; }
        public string MetricType { get; set; }
        public decimal MetricValue { get; set; }
        public decimal? TargetValue { get; set; }
        public string Period { get; set; }
        public int? AppointmentsCompleted { get; set; }
        public int? PatientsServed { get; set; }
        public decimal? AveragePatientSatisfactionScore { get; set; }
        public decimal? AverageWaitTimeMinutes { get; set; }
        public decimal? OnTimePercentage { get; set; }
        public int? NoShowCount { get; set; }
        public int? CancellationCount { get; set; }
        public string Department { get; set; }
        public string Notes { get; set; }
    }
}
