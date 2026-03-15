using FinancialBillingService.Interfaces;
using FinancialBillingService.Model;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using Microsoft.Extensions.Configuration;

namespace FinancialBillingService.Services
{
    public class InvoiceIssuanceService : IInvoiceIssuanceService
    {
        private readonly IInvoiceRepository _invoiceRepository;
        private readonly IBillingService _billingService;
        private readonly IConfiguration _configuration;

        public InvoiceIssuanceService(IInvoiceRepository invoiceRepository, IBillingService billingService, IConfiguration configuration)
        {
            _invoiceRepository = invoiceRepository;
            _billingService = billingService;
            _configuration = configuration;
        }

        public async Task<byte[]> GenerateInvoicePdfAsync(int invoiceId)
        {
            var invoice = await _invoiceRepository.GetByIdAsync(invoiceId);
            if (invoice == null)
            {
                throw new KeyNotFoundException($"Invoice {invoiceId} not found");
            }
            var inquiry = await _billingService.GetBillingInquiryAsync(invoice.VisitID);

            var document = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.Margin(2, Unit.Centimetre);
                    page.PageColor(Colors.White);
                    page.DefaultTextStyle(x => x.FontSize(11).FontFamily("Arial"));

                    page.Header().Element(header => ComposeHeader(header, invoice));
                    page.Content().Element(content => ComposeContent(content, invoice, inquiry));
                    page.Footer().Element(ComposeFooter);
                });
            });

            return document.GeneratePdf();
        }

        private void ComposeHeader(IContainer container, Invoice invoice)
        {
            string logoPath = Path.Combine(Directory.GetCurrentDirectory(), "Assets", "logo.png");
            
            var hospitalName = _configuration["HospitalInfo:Name"] ?? "BỆNH VIỆN ĐA KHOA QUỐC TẾ HMS";
            var hospitalAddress = _configuration["HospitalInfo:Address"] ?? "Địa chỉ: 123 Đường Sức Khỏe, Quận Y Tế, TP. HCM";
            var hospitalPhoneAndWebsite = _configuration["HospitalInfo:PhoneAndWebsite"] ?? "Điện thoại: 1900 123 456 | Website: www.hms.com.vn";

            container.Row(row =>
            {
                if (System.IO.File.Exists(logoPath))
                {
                    row.ConstantItem(80).Image(logoPath);
                }

                row.RelativeItem().PaddingLeft(System.IO.File.Exists(logoPath) ? 20 : 0).Column(column =>
                {
                    column.Item().Text(hospitalName).FontSize(20).SemiBold().FontColor(Colors.Blue.Darken2);
                    column.Item().Text(hospitalAddress);
                    column.Item().Text(hospitalPhoneAndWebsite);
                });

                row.ConstantItem(150).Column(column =>
                {
                    column.Item().Text("HÓA ĐƠN GTGT (TEST)").FontSize(16).Bold().FontColor(Colors.Red.Medium);
                    column.Item().Text($"Mã hóa đơn: INV-{invoice.InvoiceID:D6}");
                    column.Item().Text($"Ngày phát hành: {invoice.IssuedAt:dd/MM/yyyy HH:mm}");
                });
            });
        }

        private void ComposeContent(IContainer container, Invoice invoice, DTOs.BillingInquiryDto inquiry)
        {
            container.PaddingVertical(1, Unit.Centimetre).Column(column =>
            {
                column.Item().PaddingBottom(10).Column(infoInfo =>
                {
                    infoInfo.Item().Text("THÔNG TIN THANH TOÁN").FontSize(14).SemiBold().Underline();
                    infoInfo.Item().PaddingTop(5).Text($"Mã lượt khám (Visit ID): {invoice.VisitID}");
                    infoInfo.Item().Text($"Mã bệnh nhân (Patient ID): {invoice.PatientID}"); 
                    if (!string.IsNullOrEmpty(invoice.PatientName))
                        infoInfo.Item().Text($"Tên bệnh nhân: {invoice.PatientName}").SemiBold();
                    infoInfo.Item().Text($"Trạng thái: {invoice.Status.ToUpper()}").Bold().FontColor(invoice.Status == "Paid" ? Colors.Green.Medium : Colors.Red.Medium);
                    infoInfo.Item().Text($"Nhân viên thu ngân (Cashier ID): {invoice.CashierID}");
                });
                column.Item().PaddingBottom(5).Text("CHI TIẾT DỊCH VỤ / THUỐC").FontSize(14).SemiBold().Underline();
                
                column.Item().Table(table =>
                {
                    table.ColumnsDefinition(columns =>
                    {
                        columns.ConstantColumn(30); 
                        columns.RelativeColumn(); 
                        columns.ConstantColumn(100); 
                    });
                    table.Header(header =>
                    {
                        header.Cell().BorderBottom(1).PaddingBottom(5).Text("#").SemiBold();
                        header.Cell().BorderBottom(1).PaddingBottom(5).Text("Tên Dịch vụ").SemiBold();
                        header.Cell().BorderBottom(1).PaddingBottom(5).AlignRight().Text("Đơn giá (VND)").SemiBold();
                    });
                    int index = 1;
                    foreach (var service in inquiry.Services)
                    {
                        var typeLabel = service.ItemType == "Medication" ? "[Thuốc] " : "";
                        table.Cell().PaddingVertical(5).Text(index.ToString());
                        table.Cell().PaddingVertical(5).Text($"{typeLabel}{service.ServiceName}");
                        table.Cell().PaddingVertical(5).AlignRight().Text($"{service.UnitPrice * service.Quantity:N0}");
                        index++;
                    }
                });
                column.Item().PaddingTop(20).Table(table =>
                {
                    table.ColumnsDefinition(columns =>
                    {
                        columns.RelativeColumn(3);
                        columns.RelativeColumn(1);
                    });

                    table.Cell().AlignRight().Text("Tổng cộng: ").SemiBold();
                    table.Cell().AlignRight().Text($"{invoice.TotalAmount:N0} VND");

                    table.Cell().AlignRight().Text("Bảo hiểm chi trả: ").SemiBold();
                    table.Cell().AlignRight().Text($"- {invoice.InsurancePaid:N0} VND");

                    table.Cell().AlignRight().Text("Bệnh nhân phải trả (Co-payment): ").SemiBold().FontSize(12).FontColor(Colors.Red.Medium);
                    table.Cell().AlignRight().Text($"{invoice.PatientPaid:N0} VND").SemiBold().FontSize(12).FontColor(Colors.Red.Medium);
                });
                column.Item().PaddingTop(30).Column(paymentCol => 
                {
                    paymentCol.Item().Text("XÁC NHẬN THANH TOÁN").FontSize(14).SemiBold().Underline();
                    paymentCol.Item().PaddingTop(5).Text($"Phương thức thanh toán: {invoice.PaymentMethod}");
                    paymentCol.Item().Text("Thời gian xử lý: " + invoice.IssuedAt.ToString("dd/MM/yyyy HH:mm:ss"));
                });
            });
        }

        private void ComposeFooter(IContainer container)
        {
            container.AlignCenter().Text(x =>
            {
                x.Span("Trang ");
                x.CurrentPageNumber();
                x.Span(" / ");
                x.TotalPages();
                x.Span("\nĐây là hóa đơn điện tử (Test) được tạo tự động từ hệ thống HMS.");
            });
        }
    }
}
