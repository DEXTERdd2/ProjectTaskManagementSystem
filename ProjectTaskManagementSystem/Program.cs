using System.Text;
using GamePlanBackend.Infrastructure.Auth.JWT;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using ProjectManagementSystem.Domain.Interference;
using ProjectManagementSystem.Infrastructure.Data;
using ProjectManagementSystem.Infrastructure.Services.ProjectService;
using ProjectManagementSystem.Infrastructure.Services.TaskService;

var builder = WebApplication.CreateBuilder(args);

// ----------------------
// Database Configuration
// ----------------------
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
);

// Register Services with DI
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IProjectService, ProjectService>();
builder.Services.AddScoped<ITaskService, TaskService>();

// ----------------------
// Controllers
// ----------------------
builder.Services.AddControllers();

// ----------------------
// Swagger Configuration
// ----------------------
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Project Management System API",
        Version = "v1"
    });

    // ?? JWT Setup in Swagger UI
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "Paste ONLY your JWT token (without 'Bearer ' prefix)",
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

// ----------------------
// CORS Policy
// ----------------------
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigin",
        policy => policy.AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader());
});

// ----------------------
// JWT Configuration
// ----------------------
var jwtSection = builder.Configuration.GetSection("Jwt");
builder.Services.Configure<JWTSettings>(jwtSection);

var secretKey = builder.Configuration["Jwt:SecretKey"];
if (string.IsNullOrEmpty(secretKey) || secretKey.Length < 16)
{
    throw new ArgumentException("Secret key is invalid. Please ensure it is at least 16 characters long.");
}

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey))
        };
    });

// ----------------------
// Build Application
// ----------------------
var app = builder.Build();

// ----------------------
// Middleware Pipeline
// ----------------------
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// ? Add CORS
app.UseCors("AllowAllOrigin");

// ? Add Authentication & Authorization
app.UseAuthentication();
app.UseAuthorization();

// ? Map Controllers
app.MapControllers();

// Redirect root URL to Swagger
app.MapGet("/", context =>
{
    context.Response.Redirect("/swagger/index.html", permanent: false);
    return Task.CompletedTask;
});

app.Run();
