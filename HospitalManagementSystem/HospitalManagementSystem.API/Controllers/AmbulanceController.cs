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
    public class AmbulanceController : ControllerBase
    {
        private readonly IAmbulanceService _ambulanceService;

        public AmbulanceController(IAmbulanceService ambulanceService)
        {
            _ambulanceService = ambulanceService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Ambulance>>> GetFleet()
        {
            try
            {
                var fleet = await _ambulanceService.GetFleetAsync();
                return Ok(fleet);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("logs/active")]
        public async Task<ActionResult<IEnumerable<AmbulanceLog>>> GetActiveLogs()
        {
            try
            {
                var logs = await _ambulanceService.GetActiveLogsAsync();
                return Ok(logs);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost("dispatch")]
        public async Task<ActionResult<AmbulanceLog>> Dispatch([FromBody] AmbulanceLog log)
        {
            try
            {
                var created = await _ambulanceService.DispatchAmbulanceAsync(log);
                return Ok(created);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost("logs/{id}/status")]
        public async Task<IActionResult> UpdateStatus(int id, [FromBody] UpdateStatusRequest request)
        {
            try
            {
                var success = await _ambulanceService.UpdateLogStatusAsync(id, request.Status);
                if (!success) return NotFound();
                return Ok(new { message = "Trip status updated successfully" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }

    public class UpdateStatusRequest
    {
        public string Status { get; set; } = string.Empty;
    }
}
