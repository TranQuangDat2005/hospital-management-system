using FinancialBillingService.DTOs;
using FinancialBillingService.Interfaces;
using FinancialBillingService.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FinancialBillingService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class InvoiceController : ControllerBase
    {
        private readonly IInvoiceIssuanceService _invoiceIssuanceService;
        private readonly IInvoiceRepository _invoiceRepository;

        public InvoiceController(IInvoiceIssuanceService invoiceIssuanceService, IInvoiceRepository invoiceRepository)
        {
            _invoiceIssuanceService = invoiceIssuanceService;
            _invoiceRepository = invoiceRepository;
        }

        
        [HttpGet("history")]
        public async Task<IActionResult> SearchPaymentHistory(
            [FromQuery] int? patientId,
            [FromQuery] DateTime? dateFrom,
            [FromQuery] DateTime? dateTo,
            [FromQuery] int? invoiceId,
            [FromQuery] string? paymentMethod)
        {
            var invoices = await _invoiceRepository.SearchAsync(patientId, dateFrom, dateTo, invoiceId, paymentMethod);

            var invoiceList = invoices.ToList();
            if (!invoiceList.Any())
                return NotFound(new { message = "No records found." });

            var baseUrl = $"{Request.Scheme}://{Request.Host}";
            var result = invoiceList.Select(i => new PaymentHistoryResponseDto
            {
                InvoiceID = i.InvoiceID,
                VisitID = i.VisitID,
                PatientID = i.PatientID,
                PatientName = i.PatientName,
                TotalAmount = i.TotalAmount,
                InsurancePaid = i.InsurancePaid,
                PatientPaid = i.PatientPaid,
                PaymentMethod = i.PaymentMethod,
                Status = i.Status,
                IssuedAt = i.IssuedAt,
                PdfUrl = $"{baseUrl}/api/invoice/{i.InvoiceID}/pdf"
            });

            return Ok(result);
        }

        
        [HttpGet("{id}/pdf")]
        public async Task<IActionResult> DownloadInvoicePdf(int id)
        {
            try
            {
                var pdfBytes = await _invoiceIssuanceService.GenerateInvoicePdfAsync(id);
                return File(pdfBytes, "application/pdf", $"Invoice_{id}.pdf");
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error generating PDF invoice", error = ex.Message });
            }
        }
    }
}
