using DotNetEnv;
using FinancialBillingService.ConfigModel;
using FinancialBillingService.Data;
using FinancialBillingService.Interfaces;
using FinancialBillingService.Repositories;
using FinancialBillingService.Services;
using Microsoft.EntityFrameworkCore;

Env.Load(); 

var builder = WebApplication.CreateBuilder(args);
builder.Services.Configure<VnPayConfig>(builder.Configuration.GetSection("VnPayConfig"));

builder.Services.AddDbContext<FinancialBillingDbContext>(options =>
    options.UseInMemoryDatabase("TestDb"));

builder.Services.AddHttpClient(); 

builder.Services.AddScoped<IServiceRepository, ServiceRepository>();
builder.Services.AddScoped<IInvoiceRepository, InvoiceRepository>();
builder.Services.AddScoped<IServiceOrderRepository, ServiceOrderRepository>();
builder.Services.AddScoped<IHealthInsuranceRepository, HealthInsuranceRepository>();
builder.Services.AddHttpClient<INationalInsuranceApiClient, NationalInsuranceApiClient>()
    .ConfigurePrimaryHttpMessageHandler(() =>
    {
        var handler = new HttpClientHandler();
        
        var proxyUrl = Environment.GetEnvironmentVariable("InsuranceApi__ProxyUrl");
        if (!string.IsNullOrEmpty(proxyUrl))
        {
            var proxy = new System.Net.WebProxy
            {
                Address = new Uri(proxyUrl),
                BypassProxyOnLocal = bool.TryParse(Environment.GetEnvironmentVariable("InsuranceApi__ProxyBypassLocal"), out var bypass) && bypass
            };
            handler.Proxy = proxy;
            handler.UseProxy = true;
        }

        return handler;
    });
builder.Services.AddScoped<IBillingService, BillingService>();
builder.Services.AddScoped<IPaymentProcessingService, PaymentProcessingService>();
builder.Services.AddScoped<IInvoiceIssuanceService, InvoiceIssuanceService>();
builder.Services.AddScoped<IServiceCatalogService, ServiceCatalogService>();

QuestPDF.Settings.License = QuestPDF.Infrastructure.LicenseType.Community;

builder.Services.AddAuthentication("Bearer").AddJwtBearer("Bearer", options => { });
builder.Services.AddAuthorization();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<FinancialBillingDbContext>();
    context.Database.EnsureCreated();

    if (!context.Invoice.Any())
    {
        context.Services.Add(new FinancialBillingService.Model.Service { ServiceID = 1, ServiceName = "Khám tổng quát", Category = "Khám bệnh", Price = 150000, Status = "Active" });
        context.Services.Add(new FinancialBillingService.Model.Service { ServiceID = 2, ServiceName = "Siêu âm bụng", Category = "CLS", Price = 250000, Status = "Active" });
        
        context.ServiceOrders.Add(new FinancialBillingService.Model.ServiceOrder { OrderID = 1, VisitID = 101, ServiceID = 1, DoctorID = 1, UnitPriceAtOrder = 150000, OrderTime = DateTime.Now, Status = "Completed" });
        context.ServiceOrders.Add(new FinancialBillingService.Model.ServiceOrder { OrderID = 2, VisitID = 101, ServiceID = 2, DoctorID = 2, UnitPriceAtOrder = 250000, OrderTime = DateTime.Now, Status = "Completed" });

        var mockInvoice = new FinancialBillingService.Model.Invoice { VisitID = 101, PatientID = 1, PatientName = "Nguyễn Văn A (Demo)", CashierID = 999, TotalAmount = 400000, InsurancePaid = 320000, PatientPaid = 80000, PaymentMethod = "VNPAY", Status = "Paid", IssuedAt = DateTime.Now };
        context.Invoice.Add(mockInvoice);
        
        context.SaveChanges();
        Console.WriteLine($"[DEBUG] Seeded Invoice with ID: {mockInvoice.InvoiceID}");
    }
}
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapControllers();

app.Run();
