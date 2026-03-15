using FinancialBillingService.DTOs;
using FinancialBillingService.Interfaces;
using System.Text.Json;

namespace FinancialBillingService.Services
{
    public class BillingService : IBillingService
    {
        private readonly IServiceOrderRepository _serviceOrderRepository;
        private readonly IServiceRepository _serviceRepository;
        private readonly IHealthInsuranceRepository _healthInsuranceRepository;
        private readonly INationalInsuranceApiClient _nationalInsuranceApiClient;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;

        public BillingService(
            IServiceOrderRepository serviceOrderRepository,
            IServiceRepository serviceRepository,
            IHealthInsuranceRepository healthInsuranceRepository,
            INationalInsuranceApiClient nationalInsuranceApiClient,
            IHttpClientFactory httpClientFactory,
            IConfiguration configuration)
        {
            _serviceOrderRepository = serviceOrderRepository;
            _serviceRepository = serviceRepository;
            _healthInsuranceRepository = healthInsuranceRepository;
            _nationalInsuranceApiClient = nationalInsuranceApiClient;
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
        }
        public async Task<BillingInquiryDto> GetBillingInquiryAsync(int visitId)
        {
            var inquiry = new BillingInquiryDto
            {
                VisitID = visitId,
                TotalRawFee = 0
            };
            var serviceOrders = await _serviceOrderRepository.GetByVisitIdAsync(visitId);
            foreach (var order in serviceOrders)
            {
                var serviceDetail = await _serviceRepository.GetByIdAsync(order.ServiceID);
                string serviceName = serviceDetail?.ServiceName ?? $"Unknown Service {order.ServiceID}";

                inquiry.Services.Add(new ServiceOrderItemDto
                {
                    ServiceID = order.ServiceID,
                    ServiceName = serviceName,
                    UnitPrice = order.UnitPriceAtOrder,
                    Quantity = 1,
                    ItemType = "Service"
                });

                inquiry.TotalRawFee += order.UnitPriceAtOrder;
            }
            var prescriptionItems = await GetPrescriptionItemsFromPatientServiceAsync(visitId);
            foreach (var item in prescriptionItems)
            {
                inquiry.Services.Add(item);
                inquiry.TotalRawFee += item.UnitPrice * item.Quantity;
            }

            return inquiry;
        }
        private async Task<List<ServiceOrderItemDto>> GetPrescriptionItemsFromPatientServiceAsync(int visitId)
        {
            try
            {
                var baseUrl = _configuration["ServiceUrls:PatientSchedulingService"];
                if (string.IsNullOrWhiteSpace(baseUrl)) return new List<ServiceOrderItemDto>();

                var client = _httpClientFactory.CreateClient();
                var response = await client.GetAsync($"{baseUrl}/api/prescription/visit/{visitId}/items");

                if (!response.IsSuccessStatusCode) return new List<ServiceOrderItemDto>();

                var json = await response.Content.ReadAsStringAsync();
                var items = JsonSerializer.Deserialize<List<PrescriptionItemApiDto>>(json,
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                return items?.Select(i => new ServiceOrderItemDto
                {
                    ServiceID = i.MedicationId,
                    ServiceName = i.MedicationName,
                    UnitPrice = i.UnitPriceAtOrder,
                    Quantity = i.Quantity,
                    ItemType = "Medication"
                }).ToList() ?? new List<ServiceOrderItemDto>();
            }
            catch
            {
                return new List<ServiceOrderItemDto>(); 
            }
        }
        public async Task<InsuranceCalculationDto> CalculateInsuranceAsync(int visitId, int patientId)
        {
            var inquiry = await GetBillingInquiryAsync(visitId);
            decimal totalRawFee = inquiry.TotalRawFee;

            var insurance = await _healthInsuranceRepository.GetByPatientIdAsync(patientId);

            int coverageRate = 0;
            string cardNumber = "None";

            if (insurance != null && insurance.ExpiryDate >= DateTime.Now)
            {
                cardNumber = insurance.CardNumber;
                coverageRate = await _nationalInsuranceApiClient.GetCoverageRateAsync(cardNumber);

                if (insurance.CoverageRate != coverageRate)
                {
                    insurance.CoverageRate = coverageRate;
                    await _healthInsuranceRepository.UpdateAsync(insurance);
                }
            }

            decimal insuranceCoveredAmount = totalRawFee * (coverageRate / 100m);
            decimal coPaymentAmount = totalRawFee - insuranceCoveredAmount;

            return new InsuranceCalculationDto
            {
                VisitID = visitId,
                PatientID = patientId,
                CardNumber = cardNumber,
                CoverageRate = coverageRate,
                TotalRawFee = totalRawFee,
                InsuranceCoveredAmount = insuranceCoveredAmount,
                CoPaymentAmount = coPaymentAmount
            };
        }
        private class PrescriptionItemApiDto
        {
            public int MedicationId { get; set; }
            public string MedicationName { get; set; } = string.Empty;
            public int Quantity { get; set; }
            public decimal UnitPriceAtOrder { get; set; }
        }
    }
}
