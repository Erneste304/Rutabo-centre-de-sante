using HospitalManagementSystem.Data.Entities;
using HospitalManagementSystem.Data.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HospitalManagementSystem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PatientFlowController : ControllerBase
    {
        private readonly IPatientFlowService _flowService;

        public PatientFlowController(IPatientFlowService flowService)
        {
            _flowService = flowService;
        }

        [HttpGet("visits")]
        public async Task<ActionResult<IEnumerable<PatientVisit>>> GetActiveVisits()
        {
            var visits = await _flowService.GetActiveVisitsAsync();
            return Ok(visits);
        }

        [HttpPost("start")]
        public async Task<ActionResult<PatientVisit>> StartVisit([FromBody] StartVisitRequest request)
        {
            var visit = await _flowService.StartVisitAsync(request.PatientId, request.VisitType, request.Complaint);
            return Ok(visit);
        }

        [HttpPost("triage")]
        public async Task<ActionResult<TriageRecord>> RecordTriage([FromBody] TriageRequest request)
        {
            var record = new TriageRecord
            {
                VisitId = request.VisitId,
                Temperature = request.Temperature,
                BloodPressureSystolic = request.BloodPressureSystolic,
                BloodPressureDiastolic = request.BloodPressureDiastolic,
                HeartRate = request.HeartRate,
                Notes = request.Notes,
                RecordedAt = request.RecordedAt ?? System.DateTime.UtcNow
            };
            var result = await _flowService.RecordTriageAsync(record);
            return Ok(result);
        }

        [HttpGet("insurance-types")]
        public async Task<ActionResult<IEnumerable<InsuranceType>>> GetInsuranceTypes()
        {
            var types = await _flowService.GetInsuranceTypesAsync();
            return Ok(types);
        }

        [HttpPut("status/{visitId}")]
        public async Task<IActionResult> UpdateStatus(int visitId, [FromBody] UpdateVisitStatusRequest request)
        {
            var success = await _flowService.UpdateVisitStatusAsync(visitId, request.Status, request.Priority);
            if (!success) return NotFound();
            return Ok();
        }
    }

    public class StartVisitRequest
    {
        public int PatientId { get; set; }
        public string VisitType { get; set; } = string.Empty;
        public string Complaint { get; set; } = string.Empty;
    }

    public class UpdateVisitStatusRequest
    {
        public string Status { get; set; } = string.Empty;
        public string? Priority { get; set; }
    }

    public class TriageRequest
    {
        public int VisitId { get; set; }
        public decimal? Temperature { get; set; }
        public int? BloodPressureSystolic { get; set; }
        public int? BloodPressureDiastolic { get; set; }
        public int? HeartRate { get; set; }
        public string? Notes { get; set; }
        public System.DateTime? RecordedAt { get; set; }
    }
}
