using FinancialBillingService.ConfigModel;
using FinancialBillingService.DTOs;
using FinancialBillingService.Interfaces;
using FinancialBillingService.Model;
using FinancialBillingService.Utils;
using Microsoft.Extensions.Options;

namespace FinancialBillingService.Services
{
    public class PaymentProcessingService : IPaymentProcessingService
    {
        private readonly IBillingService _billingService;
        private readonly IInvoiceRepository _invoiceRepository;
        private readonly IServiceOrderRepository _serviceOrderRepository;
        private readonly VnPayConfig _vnPayConfig;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;

        public PaymentProcessingService(
            IBillingService billingService,
            IInvoiceRepository invoiceRepository,
            IServiceOrderRepository serviceOrderRepository,
            IOptions<VnPayConfig> vnPayConfig,
            IHttpClientFactory httpClientFactory,
            IConfiguration configuration)
        {
            _billingService = billingService;
            _invoiceRepository = invoiceRepository;
            _serviceOrderRepository = serviceOrderRepository;
            _vnPayConfig = vnPayConfig.Value;
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
        }
        public async Task<PaymentInfoDto> GetPaymentInfoForScreenAsync(int visitId, int patientId)
        {
            var inquiry = await _billingService.GetBillingInquiryAsync(visitId);
            var insuranceCalc = await _billingService.CalculateInsuranceAsync(visitId, patientId);
            string qrData = $"VIETQR|970436|0123456789|{insuranceCalc.CoPaymentAmount}|Thanh toan vien phi Visit {visitId}";

            return new PaymentInfoDto
            {
                VisitID = visitId,
                TotalAmountToPay = insuranceCalc.CoPaymentAmount,
                QrCodeData = qrData,
                Services = inquiry.Services
            };
        }
        public async Task<PaymentResponseDto> ProcessPaymentAsync(PaymentRequestDto request, int cashierId)
        {
            var insuranceCalc = await _billingService.CalculateInsuranceAsync(request.VisitID, request.PatientID);
            
            if (insuranceCalc.CoPaymentAmount < 0)
                throw new InvalidOperationException("Calculation error: Co-payment cannot be negative.");
            var invoice = new Invoice
            {
                VisitID = request.VisitID,
                PatientID = request.PatientID,
                PatientName = request.PatientName,
                CashierID = cashierId,
                TotalAmount = insuranceCalc.TotalRawFee,
                InsurancePaid = insuranceCalc.InsuranceCoveredAmount,
                PatientPaid = insuranceCalc.CoPaymentAmount,
                PaymentMethod = request.PaymentMethod,
                Status = "Paid",
                IssuedAt = DateTime.Now
            };

            var createdInvoice = await _invoiceRepository.CreateAsync(invoice);
            var serviceOrders = await _serviceOrderRepository.GetByVisitIdAsync(request.VisitID);
            foreach (var order in serviceOrders)
            {
                order.Status = "Paid";
                await _serviceOrderRepository.UpdateAsync(order);
            }
            await NotifyVisitStatusAsync(request.VisitID, "Paid/Pending Meds");

            return new PaymentResponseDto
            {
                InvoiceID = createdInvoice.InvoiceID,
                TotalAmountPaid = createdInvoice.PatientPaid,
                PaymentMethod = createdInvoice.PaymentMethod,
                Status = createdInvoice.Status,
                PaymentTime = createdInvoice.IssuedAt,
                Message = "Thanh toán thành công. Đã mở khóa quy trình phát thuốc."
            };
        }
        private async Task NotifyVisitStatusAsync(int visitId, string newStatus)
        {
            try
            {
                var baseUrl = _configuration["ServiceUrls:PatientSchedulingService"];
                if (string.IsNullOrWhiteSpace(baseUrl)) return; 

                var client = _httpClientFactory.CreateClient();
                var payload = new { VisitId = visitId, Status = newStatus };
                await client.PatchAsJsonAsync($"{baseUrl}/api/visitrecord/{visitId}/status", payload);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[WARN] Failed to notify PatientSchedulingService for Visit {visitId}: {ex.Message}");
            }
        }
        public Task<string> CreateVnPaymentUrlAsync(VnPaymentRequestDto request, HttpContext httpContext)
        {
            var timeZoneById = TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time");
            var timeNow = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, timeZoneById);
            var tick = DateTime.Now.Ticks.ToString();

            var vnpay = new VnPayLibrary();
            vnpay.AddRequestData("vnp_Version", _vnPayConfig.Version);
            vnpay.AddRequestData("vnp_Command", _vnPayConfig.Command);
            vnpay.AddRequestData("vnp_TmnCode", _vnPayConfig.TmnCode);
            vnpay.AddRequestData("vnp_Amount", ((long)(request.Amount * 100)).ToString()); 
            
            vnpay.AddRequestData("vnp_CreateDate", timeNow.ToString("yyyyMMddHHmmss"));
            vnpay.AddRequestData("vnp_CurrCode", "VND");
            var ipAddress = httpContext.Connection.RemoteIpAddress?.ToString() ?? "127.0.0.1";
            if(ipAddress == "::1") ipAddress = "127.0.0.1";
            vnpay.AddRequestData("vnp_IpAddr", ipAddress);

            vnpay.AddRequestData("vnp_Locale", "vn");
            
            vnpay.AddRequestData("vnp_OrderInfo", $"Thanh toan vien phi Visit {request.VisitID}");
            vnpay.AddRequestData("vnp_OrderType", "other"); 
            vnpay.AddRequestData("vnp_ReturnUrl", _vnPayConfig.ReturnUrl);
            
            var txnRef = $"{request.VisitID}_{request.PatientID}_{tick}"; 
            vnpay.AddRequestData("vnp_TxnRef", txnRef);

            var paymentUrl = vnpay.CreateRequestUrl(_vnPayConfig.BaseUrl, _vnPayConfig.HashSecret);

            return Task.FromResult(paymentUrl);
        }
        public async Task<VnPaymentResponseDto> ExecuteVnPaymentCallbackAsync(IQueryCollection collections)
        {
            var vnpay = new VnPayLibrary();
            foreach (var (key, value) in collections)
            {
                if (!string.IsNullOrEmpty(key) && key.StartsWith("vnp_"))
                {
                    vnpay.AddResponseData(key, value.ToString());
                }
            }

            var vnp_orderId = vnpay.GetResponseData("vnp_TxnRef");
            var vnp_TransactionId = vnpay.GetResponseData("vnp_TransactionNo");
            var vnp_SecureHash = collections.FirstOrDefault(p => p.Key == "vnp_SecureHash").Value.ToString();
            var vnp_ResponseCode = vnpay.GetResponseData("vnp_ResponseCode");
            var vnp_OrderInfo = vnpay.GetResponseData("vnp_OrderInfo");

            bool checkSignature = vnpay.ValidateSignature(vnp_SecureHash, _vnPayConfig.HashSecret);
            if (!checkSignature)
            {
                return new VnPaymentResponseDto
                {
                    Success = false,
                    Message = "Invalid Signature"
                };
            }

            bool success = vnp_ResponseCode == "00";

            if (success)
            {
                var segments = vnp_orderId.Split('_');
                if (segments.Length >= 2
                    && int.TryParse(segments[0], out int visitId)
                    && int.TryParse(segments[1], out int patientId))
                {
                    var serviceOrders = await _serviceOrderRepository.GetByVisitIdAsync(visitId);
                    foreach (var order in serviceOrders)
                    {
                        if (order.Status != "Paid")
                        {
                            order.Status = "Paid";
                            await _serviceOrderRepository.UpdateAsync(order);
                        }
                    }
                    var existingInvoice = await _invoiceRepository.GetByVisitIdAsync(visitId);
                    if (existingInvoice == null)
                    {
                        var insuranceCalc = await _billingService.CalculateInsuranceAsync(visitId, patientId);
                        var newInvoice = new Invoice
                        {
                            VisitID = visitId,
                            PatientID = patientId,
                            PatientName = string.Empty, 
                            CashierID = 0, 
                            TotalAmount = insuranceCalc.TotalRawFee,
                            InsurancePaid = insuranceCalc.InsuranceCoveredAmount,
                            PatientPaid = insuranceCalc.CoPaymentAmount,
                            PaymentMethod = "VNPAY",
                            Status = "Paid",
                            IssuedAt = DateTime.Now
                        };
                        await _invoiceRepository.CreateAsync(newInvoice);
                    }
                    await NotifyVisitStatusAsync(visitId, "Paid/Pending Meds");
                }
            }

            return new VnPaymentResponseDto
            {
                Success = success,
                PaymentMethod = "VNPAY",
                OrderDescription = vnp_OrderInfo,
                OrderId = vnp_orderId,
                TransactionId = vnp_TransactionId,
                Token = vnp_SecureHash,
                VnPayResponseCode = vnp_ResponseCode
            };
        }
    }
}
