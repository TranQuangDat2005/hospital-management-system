using Microsoft.EntityFrameworkCore;
using PharmacyMedicationService.Data;
using PharmacyMedicationService.Interfaces;
using PharmacyMedicationService.Model;
using PharmacyMedicationService.Repositories;
using PharmacyMedicationService.Services;
using System;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);


DotNetEnv.Env.Load();


builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "PharmacyMedicationService API", Version = "v1" });
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT"
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});


var jwtKey = Environment.GetEnvironmentVariable("JWT_KEY") ?? "ThisIsASecretKeyForJwtTokenGenerationWowThisIsLongEnoughToWork";
var jwtIssuer = Environment.GetEnvironmentVariable("JWT_ISSUER") ?? "HospitalManagementSystem";
var jwtAudience = Environment.GetEnvironmentVariable("JWT_AUDIENCE") ?? "HospitalManagementSystem";

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtIssuer,
            ValidAudience = jwtAudience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
        };
    });

builder.Services.AddAuthorization();


builder.Services.AddDbContext<PharmacyMedicationDbContext>(options =>
    options.UseInMemoryDatabase("PharmacyDb"));

builder.Services.AddScoped<IPrescriptionItemRepository, PrescriptionItemRepository>();
builder.Services.AddScoped<IPharmacyService, PharmacyService>();

var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();


using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<PharmacyMedicationDbContext>();
    
    Guid pId = Guid.Parse("11111111-1111-1111-1111-111111111111");
    Guid med1Id = Guid.NewGuid();
    Guid med2Id = Guid.NewGuid();

    context.Medications.AddRange(new Medication[]
    {
        new Medication { Id = med1Id, Name = "Panadol Extra", Unit = "Viên", UnitPrice = 5000, Instructions = "Uống sau ăn", Status = true },
        new Medication { Id = med2Id, Name = "Amoxicillin 500mg", Unit = "Viên", UnitPrice = 12000, Instructions = "Uống ngày 2 lần", Status = true }
    });

    context.PrescriptionItems.AddRange(new PrescriptionItem[]
    {
        new PrescriptionItem 
        { 
            Id = Guid.NewGuid(), 
            PrescriptionId = pId, 
            MedicationId = med1Id, 
            Quantity = 10, 
            Status = ItemStatus.PendingPayment, 
            UpdatedAt = DateTime.UtcNow 
        },
        new PrescriptionItem 
        { 
            Id = Guid.NewGuid(), 
            PrescriptionId = pId, 
            MedicationId = med2Id, 
            Quantity = 20, 
            Status = ItemStatus.PendingPayment, 
            UpdatedAt = DateTime.UtcNow 
        }
    });

    context.SaveChanges();
}

app.Run();
