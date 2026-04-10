using Microsoft.AspNetCore.Mvc;
using HospitalManagementSystem.Data;
using Microsoft.EntityFrameworkCore;
using HospitalManagementSystem.Core.Models;
using System.Threading.Tasks;
using System.Linq;

namespace HospitalManagementSystem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DashboardController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public DashboardController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet("stats")]
        public async Task<IActionResult> GetDashboardStats([FromQuery] string type)
        {
            var totalPatients = await _context.Patients.CountAsync();
            var totalUsers = await _context.Users.CountAsync();
            var totalDoctors = await _context.Users.CountAsync(u => u.UserType == "Doctor");
            var totalNurses = await _context.Users.CountAsync(u => u.UserType == "Nurse");

            return Ok(new
            {
                TotalPatients = totalPatients,
                TotalStaff = totalDoctors + totalNurses,
                ClinicRevenue = 42800, // Placeholder
                AvgWaitTime = 18,      // Placeholder
                RoleStats = type switch
                {
                    "Doctor" => (object)new { Appointments = 12, Surgeries = 4, InPatients = 8, LabReports = 14 },
                    "Admin" => (object)new { RevenueGrowth = 8, BedOccupancy = 75 },
                    _ => (object)new { Info = "No specific stats for this role" }
                }
            });
        }
    }
}
