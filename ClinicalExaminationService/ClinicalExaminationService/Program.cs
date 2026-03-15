using ClinicalExaminationService.Data;
using ClinicalExaminationService.Interfaces;
using ClinicalExaminationService.Reposities;
using ClinicalExaminationService.Model;
using ClinicalExaminationService.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ClinicalExamDbContext>(options =>
    options.UseInMemoryDatabase("ClinicalExamDb"));

builder.Services.AddScoped<IVisitRecordRepository, VisitRecordRepository>();
builder.Services.AddScoped<IVitalSignRepository, VitalSignRepository>();
builder.Services.AddScoped<IIcd10CatalogRepository, Icd10CatalogRepository>();
builder.Services.AddScoped<IMedicalExaminationRepository, MedicalExaminationRepository>();
builder.Services.AddScoped<IPrescriptionRepository, PrescriptionRepository>();
builder.Services.AddScoped<IServiceOrderRepository, ServiceOrderRepository>();
builder.Services.AddScoped<IFollowUpAppointmentRepository, FollowUpAppointmentRepository>();
builder.Services.AddScoped<IAssignmentScheduleRepository, AssignmentScheduleRepository>();
builder.Services.AddScoped<IExaminationService, ExaminationService>();

builder.Services.AddAuthentication("Bearer").AddJwtBearer("Bearer", options => { });
builder.Services.AddAuthorization();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<ClinicalExamDbContext>();
    context.Database.EnsureCreated();

    if (!context.VisitRecords.Any())
    {
        var mockVisitId = Guid.NewGuid();
        var mockVisit = new VisitRecord
        {
            Id = mockVisitId,
            PatientId = Guid.NewGuid(),
            ReceptionistId = Guid.NewGuid(),
            DepartmentId = Guid.NewGuid(),
            QueueNumber = 1,
            Reason = "Headache",
            Status = VisitStatus.Waiting,
            CheckInTime = DateTime.UtcNow
        };
        context.VisitRecords.Add(mockVisit);
        context.SaveChanges();
        Console.WriteLine($"[Seed] Created mock VisitRecord: {mockVisitId}");
    }
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();
