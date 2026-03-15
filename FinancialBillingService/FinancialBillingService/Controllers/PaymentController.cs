using FinancialBillingService.DTOs;
using FinancialBillingService.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FinancialBillingService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class PaymentController : ControllerBase
    {
        private readonly IPaymentProcessingService _paymentProcessingService;

        public PaymentController(IPaymentProcessingService paymentProcessingService)
        {
            _paymentProcessingService = paymentProcessingService;
        }

        [HttpGet("visit/{visitId}/screen-info")]
        public async Task<IActionResult> GetPaymentInfoForScreen(int visitId, [FromQuery] int patientId)
        {
            if (patientId <= 0)
            {
                return BadRequest("Patient ID is required.");
            }

            try
            {
                var info = await _paymentProcessingService.GetPaymentInfoForScreenAsync(visitId, patientId);
                return Ok(info);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = $"Error generating payment info: {ex.Message}" });
            }
        }

        [HttpPost("process")]
        public async Task<IActionResult> ProcessPayment([FromBody] PaymentRequestDto request)
        {
            if (request == null || request.VisitID <= 0 || request.PatientID <= 0)
                return BadRequest("Invalid payment request.");
                
            var claimValue = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? User.FindFirstValue("sub");
            int cashierId = int.TryParse(claimValue, out var parsed) ? parsed : 0;

            try
            {
                var result = await _paymentProcessingService.ProcessPaymentAsync(request, cashierId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = $"Payment processing failed: {ex.Message}" });
            }
        }

        [HttpPost("vnpay/create-url")]
        [AllowAnonymous] 
        public async Task<IActionResult> CreateVnPaymentUrl([FromBody] VnPaymentRequestDto request)
        {
            if (request == null || request.VisitID <= 0 || request.Amount <= 0)
            {
                return BadRequest("Invalid request data.");
            }

            var url = await _paymentProcessingService.CreateVnPaymentUrlAsync(request, HttpContext);
            return Ok(new { paymentUrl = url });
        }

        [HttpGet("vnpay/callback")]
        [AllowAnonymous] 
        public async Task<IActionResult> VnPaymentCallback()
        {
            var response = await _paymentProcessingService.ExecuteVnPaymentCallbackAsync(Request.Query);
            if (response.Success)
            {
                return Ok(response);
            }
            
            return BadRequest(response);
        }
    }
}
