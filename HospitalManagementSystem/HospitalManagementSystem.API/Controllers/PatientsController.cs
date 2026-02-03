using Microsoft.AspNetCore.Mvc;
using HospitalManagementSystem.Core.Models;
using HospitalManagementSystem.Core.Services;

namespace HospitalManagementSystem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PatientsController : ControllerBase
    {
        private readonly IPatientService _patientService;

        public PatientsController(IPatientService patientService)
        {
            _patientService = patientService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Patient>>> GetPatients()
        {
            var patients = await _patientService.GetAllPatientsAsync();
            return Ok(patients);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Patient>> GetPatient(int id)
        {
            var patient = await _patientService.GetPatientByIdAsync(id);
            
            if (patient == null)
                return NotFound();
            
            return Ok(patient);
        }

        [HttpPost]
        public async Task<ActionResult<Patient>> CreatePatient(Patient patient)
        {
            var createdPatient = await _patientService.CreatePatientAsync(patient);
            return CreatedAtAction(nameof(GetPatient), new { id = createdPatient.PatientId }, createdPatient);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePatient(int id, Patient patient)
        {
            if (id != patient.PatientId)
                return BadRequest();

            await _patientService.UpdatePatientAsync(patient);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePatient(int id)
        {
            await _patientService.DeletePatientAsync(id);
            return NoContent();
        }

        [HttpGet("{id}/appointments")]
        public async Task<ActionResult<IEnumerable<Appointment>>> GetPatientAppointments(int id)
        {
            var appointments = await _patientService.GetPatientAppointmentsAsync(id);
            return Ok(appointments);
        }
    }
}