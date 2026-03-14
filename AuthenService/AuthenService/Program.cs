using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using User_Authentication_Service.Data;
using User_Authentication_Service.Helpers;
using User_Authentication_Service.Interfaces;
using User_Authentication_Service.Middleware;
using User_Authentication_Service.Repositories;
using User_Authentication_Service.Services;

var builder = WebApplication.CreateBuilder(args);

// ─── Database ────────────────────────────────────────────────────────────────
builder.Services.AddDbContext<UserAuthenDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("PostgreConnection")));

// ─── JWT Authentication ──────────────────────────────────────────────────────
var jwtSettings = builder.Configuration.GetSection("JwtSettings");
var secretKey   = jwtSettings["SecretKey"] ?? throw new InvalidOperationException("JWT SecretKey chưa cấu hình!");

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme    = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey         = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey)),
        ValidateIssuer           = true,
        ValidIssuer              = jwtSettings["Issuer"],
        ValidateAudience         = true,
        ValidAudience            = jwtSettings["Audience"],
        ValidateLifetime         = true,
        ClockSkew                = TimeSpan.Zero  // Không cho phép dung sai thời gian
    };

    options.Events = new JwtBearerEvents
    {
        OnChallenge = async context =>
        {
            context.HandleResponse();
            context.Response.StatusCode  = StatusCodes.Status401Unauthorized;
            context.Response.ContentType = "application/json";
            await context.Response.WriteAsJsonAsync(new
            {
                message = "Bạn cần đăng nhập để truy cập tài nguyên này."
            });
        },
        OnForbidden = async context =>
        {
            context.Response.StatusCode  = StatusCodes.Status403Forbidden;
            context.Response.ContentType = "application/json";
            await context.Response.WriteAsJsonAsync(new
            {
                message = "Bạn không có quyền truy cập tài nguyên này."
            });
        }
    };
});

builder.Services.AddAuthorization();

// ─── Dependency Injection ─────────────────────────────────────────────────────
builder.Services.AddSingleton<JwtHelper>();                              // Singleton vì có blacklist in-memory

builder.Services.AddScoped<IUserRepository,         UserRepository>();
builder.Services.AddScoped<IPermissionRepository,   PermissionRepository>();
builder.Services.AddScoped<IDepartmentRepository,   DepartmentRepository>();
builder.Services.AddScoped<IAuthService,            AuthService>();
builder.Services.AddScoped<IUserService,            UserService>();
builder.Services.AddScoped<IDepartmentService,      DepartmentService>();
builder.Services.AddScoped<IPermissionService,      PermissionService>();

// ─── Controllers & Swagger ───────────────────────────────────────────────────
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title   = "Hospital Management - Auth Service",
        Version = "v1",
        Description = "API xác thực và quản lý người dùng bệnh viện"
    });

    // Thêm Bearer JWT vào Swagger UI
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name         = "Authorization",
        Type         = SecuritySchemeType.Http,
        Scheme       = "bearer",
        BearerFormat = "JWT",
        In           = ParameterLocation.Header,
        Description  = "Nhập JWT token. Ví dụ: eyJhbGciOiJIUzI1NiIs..."
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id   = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

// ─── Build App ───────────────────────────────────────────────────────────────
var app = builder.Build();

// ─── Middleware Pipeline ─────────────────────────────────────────────────────
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Auth Service v1");
        c.RoutePrefix = "swagger";
    });
}

app.UseHttpsRedirection();

// Thứ tự quan trọng: Authentication → Authorization → PermissionMiddleware
app.UseAuthentication();
app.UseAuthorization();
app.UsePermissionMiddleware();   // Kiểm tra quyền động từ DB

app.MapControllers();

app.Run();
