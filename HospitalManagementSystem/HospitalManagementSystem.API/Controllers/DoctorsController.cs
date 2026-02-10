using HospitalManagementSystem.Data;
using HospitalManagementSystem.Data.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HospitalManagementSystem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DoctorsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public DoctorsController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<object>>> GetDoctors()
        {
            var doctors = await _context.Doctors
                .Include(d => d.User)
                .Select(d => new {
                    d.DoctorId,
                    FullName = d.User != null ? d.User.FullName : "Unknown",
                    d.Qualifications,
                    d.YearsOfExperience,
                    d.IsAvailable,
                    d.Rating
                })
                .ToListAsync();
            return Ok(doctors);
        }
    }
}
