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

        /// <summary>
        /// Get all doctors with their details
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<object>>> GetDoctors()
        {
            try
            {
                var doctors = await _context.Doctors
                    .Include(d => d.User)
                    .Include(d => d.DoctorDepartments)
                    .ThenInclude(dd => dd.Department)
                    .Select(d => new
                    {
                        d.DoctorId,
                        FullName = d.User != null ? d.User.FullName : "Unknown",
                        d.User.Email,
                        d.User.PhoneNumber,
                        d.Qualifications,
                        d.YearsOfExperience,
                        d.IsAvailable,
                        d.Rating,
                        Departments = d.DoctorDepartments.Select(dd => dd.Department.DepartmentName).ToList()
                    })
                    .ToListAsync();
                return Ok(doctors);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Get doctor by ID
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<object>> GetDoctor(int id)
        {
            try
            {
                var doctor = await _context.Doctors
                    .Include(d => d.User)
                    .Include(d => d.DoctorDepartments)
                    .ThenInclude(dd => dd.Department)
                    .Where(d => d.DoctorId == id)
                    .Select(d => new
                    {
                        d.DoctorId,
                        FullName = d.User != null ? d.User.FullName : "Unknown",
                        d.User.Email,
                        d.User.PhoneNumber,
                        d.Qualifications,
                        d.YearsOfExperience,
                        d.IsAvailable,
                        d.Rating,
                        Departments = d.DoctorDepartments.Select(dd => new { dd.Department.DepartmentId, dd.Department.DepartmentName }).ToList()
                    })
                    .FirstOrDefaultAsync();

                if (doctor == null)
                    return NotFound(new { message = "Doctor not found" });

                return Ok(doctor);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Create a new doctor
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<object>> CreateDoctor([FromBody] CreateDoctorDto dto)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(dto.FullName) || string.IsNullOrWhiteSpace(dto.Email))
                    return BadRequest(new { message = "FullName and Email are required" });

                // Create user first
                var user = new User
                {
                    FullName = dto.FullName,
                    Email = dto.Email,
                    PhoneNumber = dto.PhoneNumber,
                    UserType = "Doctor",
                    DateOfBirth = dto.DateOfBirth,
                    Gender = dto.Gender,
                    Address = dto.Address
                };

                _context.Users.Add(user);
                await _context.SaveChangesAsync();

                // Create doctor record
                var doctor = new Doctor
                {
                    UserId = user.UserId,
                    Qualifications = dto.Qualifications,
                    YearsOfExperience = dto.YearsOfExperience,
                    IsAvailable = true,
                    Rating = 0
                };

                _context.Doctors.Add(doctor);
                await _context.SaveChangesAsync();

                return CreatedAtAction(nameof(GetDoctor), new { id = doctor.DoctorId }, new
                {
                    doctor.DoctorId,
                    FullName = user.FullName,
                    user.Email,
                    user.PhoneNumber,
                    doctor.Qualifications,
                    doctor.YearsOfExperience,
                    doctor.IsAvailable,
                    doctor.Rating
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Update doctor information
        /// </summary>
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateDoctor(int id, [FromBody] UpdateDoctorDto dto)
        {
            try
            {
                var doctor = await _context.Doctors
                    .Include(d => d.User)
                    .FirstOrDefaultAsync(d => d.DoctorId == id);

                if (doctor == null)
                    return NotFound(new { message = "Doctor not found" });

                // Update user info
                if (!string.IsNullOrWhiteSpace(dto.FullName))
                    doctor.User.FullName = dto.FullName;
                if (!string.IsNullOrWhiteSpace(dto.Email))
                    doctor.User.Email = dto.Email;
                if (!string.IsNullOrWhiteSpace(dto.PhoneNumber))
                    doctor.User.PhoneNumber = dto.PhoneNumber;

                // Update doctor info
                if (!string.IsNullOrWhiteSpace(dto.Qualifications))
                    doctor.Qualifications = dto.Qualifications;
                if (dto.YearsOfExperience.HasValue)
                    doctor.YearsOfExperience = dto.YearsOfExperience.Value;
                if (dto.IsAvailable.HasValue)
                    doctor.IsAvailable = dto.IsAvailable.Value;

                _context.Doctors.Update(doctor);
                await _context.SaveChangesAsync();

                return Ok(new { message = "Doctor updated successfully" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Delete a doctor
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDoctor(int id)
        {
            try
            {
                var doctor = await _context.Doctors.FindAsync(id);
                if (doctor == null)
                    return NotFound(new { message = "Doctor not found" });

                _context.Doctors.Remove(doctor);
                await _context.SaveChangesAsync();

                return Ok(new { message = "Doctor deleted successfully" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Get doctor's schedule
        /// </summary>
        [HttpGet("{id}/schedule")]
        public async Task<ActionResult<IEnumerable<object>>> GetDoctorSchedule(int id)
        {
            try
            {
                var schedule = await _context.Schedules
                    .Where(s => s.DoctorId == id)
                    .Select(s => new
                    {
                        s.ScheduleId,
                        s.DayOfWeek,
                        s.StartTime,
                        s.EndTime,
                        s.IsActive
                    })
                    .ToListAsync();

                if (!schedule.Any())
                    return NotFound(new { message = "No schedule found for this doctor" });

                return Ok(schedule);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Get doctor's appointments
        /// </summary>
        [HttpGet("{id}/appointments")]
        public async Task<ActionResult<IEnumerable<object>>> GetDoctorAppointments(int id)
        {
            try
            {
                var appointments = await _context.Appointments
                    .Where(a => a.DoctorId == id)
                    .Include(a => a.Patient)
                    .ThenInclude(p => p.User)
                    .Select(a => new
                    {
                        a.AppointmentId,
                        a.AppointmentDate,
                        a.AppointmentTime,
                        a.Status,
                        PatientName = a.Patient.User.FullName,
                        a.Patient.User.Email,
                        a.Reason
                    })
                    .OrderByDescending(a => a.AppointmentDate)
                    .ToListAsync();

                return Ok(appointments);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Assign doctor to department
        /// </summary>
        [HttpPost("{id}/departments")]
        public async Task<IActionResult> AssignDepartment(int id, [FromBody] AssignDepartmentDto dto)
        {
            try
            {
                var doctor = await _context.Doctors.FindAsync(id);
                if (doctor == null)
                    return NotFound(new { message = "Doctor not found" });

                var department = await _context.Departments.FindAsync(dto.DepartmentId);
                if (department == null)
                    return NotFound(new { message = "Department not found" });

                var existing = await _context.DoctorDepartments
                    .FirstOrDefaultAsync(dd => dd.DoctorId == id && dd.DepartmentId == dto.DepartmentId);

                if (existing != null)
                    return BadRequest(new { message = "Doctor is already assigned to this department" });

                var doctorDepartment = new DoctorDepartment
                {
                    DoctorId = id,
                    DepartmentId = dto.DepartmentId
                };

                _context.DoctorDepartments.Add(doctorDepartment);
                await _context.SaveChangesAsync();

                return Ok(new { message = "Doctor assigned to department successfully" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Get available doctors
        /// </summary>
        [HttpGet("available/list")]
        public async Task<ActionResult<IEnumerable<object>>> GetAvailableDoctors()
        {
            try
            {
                var doctors = await _context.Doctors
                    .Where(d => d.IsAvailable)
                    .Include(d => d.User)
                    .Include(d => d.DoctorDepartments)
                    .ThenInclude(dd => dd.Department)
                    .Select(d => new
                    {
                        d.DoctorId,
                        FullName = d.User.FullName,
                        d.User.Email,
                        d.Qualifications,
                        d.Rating,
                        Departments = d.DoctorDepartments.Select(dd => dd.Department.DepartmentName).ToList()
                    })
                    .ToListAsync();

                return Ok(doctors);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }

    // DTOs
    public class CreateDoctorDto
    {
        public string FullName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Gender { get; set; }
        public string Address { get; set; }
        public string Qualifications { get; set; }
        public int YearsOfExperience { get; set; }
    }

    public class UpdateDoctorDto
    {
        public string FullName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Qualifications { get; set; }
        public int? YearsOfExperience { get; set; }
        public bool? IsAvailable { get; set; }
    }

    public class AssignDepartmentDto
    {
        public int DepartmentId { get; set; }
    }
}
