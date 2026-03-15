using FinancialBillingService.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FinancialBillingService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class BillingController : ControllerBase
    {
        private readonly IBillingService _billingService;

        public BillingController(IBillingService billingService)
        {
            _billingService = billingService;
        }

        
        [HttpGet("visit/{visitId}/inquiry")]
        public async Task<IActionResult> GetBillingInquiry(int visitId)
        {
            var inquiry = await _billingService.GetBillingInquiryAsync(visitId);
            if (inquiry == null || inquiry.Services.Count == 0)
            {
                return NotFound(new { message = $"No services found for Visit ID {visitId}." });
            }

            return Ok(inquiry);
        }

        
        [HttpPost("visit/{visitId}/calculate-insurance")]
        public async Task<IActionResult> CalculateInsurance(int visitId, [FromQuery] int patientId)
        {
            if (patientId <= 0)
            {
                return BadRequest("Patient ID is required to calculate insurance.");
            }

            var calculationResult = await _billingService.CalculateInsuranceAsync(visitId, patientId);
            return Ok(calculationResult);
        }
    }
}
