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
    public class BloodBankController : ControllerBase
    {
        private readonly IBloodBankService _bloodBankService;

        public BloodBankController(IBloodBankService bloodBankService)
        {
            _bloodBankService = bloodBankService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<BloodBank>>> GetStock()
        {
            try
            {
                var stock = await _bloodBankService.GetBloodStockAsync();
                return Ok(stock);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost("update")]
        public async Task<IActionResult> UpdateStock([FromBody] UpdateBloodRequest request)
        {
            try
            {
                var success = await _bloodBankService.UpdateStockAsync(request.BloodType, request.Delta);
                if (!success) return NotFound();
                return Ok(new { message = "Stock updated successfully" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }

    public class UpdateBloodRequest
    {
        public string BloodType { get; set; } = string.Empty;
        public int Delta { get; set; }
    }
}
