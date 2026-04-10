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
    public class BillingsController : ControllerBase
    {
        private readonly IBillingService _billingService;

        public BillingsController(IBillingService billingService)
        {
            _billingService = billingService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Billing>>> GetBillings()
        {
            try
            {
                var billings = await _billingService.GetAllBillingsAsync();
                return Ok(billings);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Billing>> GetBilling(int id)
        {
            try
            {
                var billing = await _billingService.GetBillingByIdAsync(id);
                if (billing == null)
                    return NotFound();
                
                return Ok(billing);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost]
        public async Task<ActionResult<Billing>> CreateBilling([FromBody] Billing billing)
        {
            try
            {
                var createdBilling = await _billingService.CreateBillingAsync(billing);
                return CreatedAtAction(nameof(GetBilling), new { id = createdBilling.BillId }, createdBilling);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost("{id}/payments")]
        public async Task<ActionResult<Payment>> AddPayment(int id, [FromBody] Payment payment)
        {
            try
            {
                var createdPayment = await _billingService.AddPaymentAsync(id, payment);
                return Ok(createdPayment);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("patient/{patientId}")]
        public async Task<ActionResult<IEnumerable<Billing>>> GetPatientBillings(int patientId)
        {
            try
            {
                var billings = await _billingService.GetBillingsByPatientIdAsync(patientId);
                return Ok(billings);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
