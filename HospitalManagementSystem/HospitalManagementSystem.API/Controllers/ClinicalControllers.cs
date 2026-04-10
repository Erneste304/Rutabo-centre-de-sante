using HospitalManagementSystem.Data.Entities;
using HospitalManagementSystem.Data.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HospitalManagementSystem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MedicalRecordsController : ControllerBase
    {
        private readonly IMedicalRecordService _medicalRecordService;

        public MedicalRecordsController(IMedicalRecordService medicalRecordService)
        {
            _medicalRecordService = medicalRecordService;
        }

        [HttpGet("patient/{patientId}")]
        public async Task<ActionResult<IEnumerable<MedicalRecord>>> GetPatientHistory(int patientId)
        {
            try
            {
                var history = await _medicalRecordService.GetPatientHistoryAsync(patientId);
                return Ok(history);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost]
        public async Task<ActionResult<MedicalRecord>> AddRecord([FromBody] MedicalRecord record)
        {
            try
            {
                var created = await _medicalRecordService.AddRecordAsync(record);
                return CreatedAtAction(nameof(GetPatientHistory), new { patientId = created.PatientId }, created);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("critical")]
        public async Task<ActionResult<IEnumerable<MedicalRecord>>> GetCriticalRecords()
        {
            try
            {
                var critical = await _medicalRecordService.GetCriticalRecordsAsync();
                return Ok(critical);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }

    [Route("api/[controller]")]
    [ApiController]
    public class LabTestsController : ControllerBase
    {
        private readonly ILabTestService _labTestService;

        public LabTestsController(ILabTestService labTestService)
        {
            _labTestService = labTestService;
        }

        [HttpGet("pending")]
        public async Task<ActionResult<IEnumerable<LabTest>>> GetPendingTests()
        {
            try
            {
                var pending = await _labTestService.GetPendingTestsAsync();
                return Ok(pending);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost("order")]
        public async Task<ActionResult<LabTest>> OrderTest([FromBody] LabTest test)
        {
            try
            {
                var created = await _labTestService.OrderTestAsync(test);
                return Ok(created);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost("{id}/result")]
        public async Task<IActionResult> SubmitResult(int id, [FromBody] ResultSubmission request)
        {
            try
            {
                var success = await _labTestService.SubmitResultAsync(id, request.Result, request.PerformedBy);
                if (!success) return NotFound();
                return Ok(new { message = "Result submitted successfully" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("patient/{patientId}")]
        public async Task<ActionResult<IEnumerable<LabTest>>> GetPatientTests(int patientId)
        {
            try
            {
                var tests = await _labTestService.GetPatientTestsAsync(patientId);
                return Ok(tests);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }

    public class ResultSubmission
    {
        public string Result { get; set; } = string.Empty;
        public string PerformedBy { get; set; } = string.Empty;
    }
}
