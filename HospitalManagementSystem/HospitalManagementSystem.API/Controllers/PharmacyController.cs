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
    public class PharmacyController : ControllerBase
    {
        private readonly IPrescriptionService _prescriptionService;
        private readonly IInventoryService _inventoryService;

        public PharmacyController(IPrescriptionService prescriptionService, IInventoryService inventoryService)
        {
            _prescriptionService = prescriptionService;
            _inventoryService = inventoryService;
        }

        [HttpGet("prescriptions")]
        public async Task<ActionResult<IEnumerable<Prescription>>> GetActivePrescriptions()
        {
            try
            {
                var prescriptions = await _prescriptionService.GetActivePrescriptionsAsync();
                return Ok(prescriptions);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost("prescriptions/{id}/dispense")]
        public async Task<IActionResult> DispensePrescription(int id)
        {
            try
            {
                var success = await _prescriptionService.DispensePrescriptionAsync(id);
                if (!success) return NotFound(new { message = "Prescription not found or already dispensed" });
                return Ok(new { message = "Prescription dispensed successfully" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("medicines")]
        public async Task<ActionResult<IEnumerable<Medicine>>> GetMedicines()
        {
            try
            {
                var medicines = await _inventoryService.GetAllMedicinesAsync();
                return Ok(medicines);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
