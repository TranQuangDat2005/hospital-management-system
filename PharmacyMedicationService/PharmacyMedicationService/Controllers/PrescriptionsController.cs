using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PharmacyMedicationService.DTOs;
using PharmacyMedicationService.Interfaces;

namespace PharmacyMedicationService.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class PrescriptionsController : ControllerBase
    {
        private readonly IPharmacyService _pharmacyService;

        public PrescriptionsController(IPharmacyService pharmacyService)
        {
            _pharmacyService = pharmacyService;
        }

        [HttpGet("queue")]
        [Authorize(Roles = "Pharmacist,Admin")]
        public async Task<IActionResult> GetPrescriptionQueue()
        {
            var queue = await _pharmacyService.GetPrescriptionQueueAsync();
            return Ok(queue);
        }

        [HttpGet("{prescriptionId}")]
        [Authorize(Roles = "Pharmacist,Doctor,Admin")]
        public async Task<IActionResult> GetPrescriptionDetail(Guid prescriptionId)
        {
            var detail = await _pharmacyService.GetPrescriptionDetailAsync(prescriptionId);
            if (detail == null)
                return NotFound(new { message = "Prescription not found." });
            return Ok(detail);
        }

        [HttpGet("{prescriptionId}/items")]
        [Authorize(Roles = "Pharmacist,Doctor,Admin")]
        public async Task<IActionResult> GetPrescriptionItems(Guid prescriptionId)
        {
            var items = await _pharmacyService.GetPrescriptionItemsAsync(prescriptionId);
            return Ok(items);
        }

        
        [HttpPatch("items/status")]
        public async Task<IActionResult> UpdateItemStatus([FromBody] UpdateItemStatusDto request)
        {
            try
            {
                var success = await _pharmacyService.UpdateItemStatusAsync(request);
                if (!success)
                    return NotFound(new { message = "Items not found." });

                return Ok(new { message = "Status updated successfully." });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message }); 
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred.", details = ex.Message });
            }
        }
    }
}
