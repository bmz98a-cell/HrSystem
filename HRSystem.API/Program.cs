using FluentValidation;
using FluentValidation.AspNetCore;
using HRSystem.API.Data;
using HRSystem.API.Middleware;
using HRSystem.API.Models;
using HRSystem.API.Services;
using HRSystem.API.Validators;
using HRSystem.API.Validators.HRSystem.API.Validators;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;



var builder = WebApplication.CreateBuilder(args);
//builder.Services.AddScoped<ILicenseService, LicenseService>();

// =====================
// Services
// =====================
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<ITenantProvider, TenantProvider>();
builder.Services.AddScoped<IEmployeeService, EmployeeService>();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddHttpContextAccessor();

builder.Services.AddScoped<ITenantProvider, TenantProvider>();

//builder.Services.AddScoped<ILicenseService, LicenseService>();
builder.Services.AddScoped<BranchService>();

builder.Services.AddControllers();

builder.Services.AddValidatorsFromAssemblyContaining<EmployeeValidator>();
builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.InvalidModelStateResponseFactory = context =>
    {
        var errors = context.ModelState
    .Where(x => x.Value != null && x.Value.Errors.Count > 0)
    .SelectMany(x => x.Value!.Errors)
    .Select(x => x.ErrorMessage)
    .ToList();

        var response = new
        {
            message = "Validation failed",
            errors = errors
        };

        return new BadRequestObjectResult(response);
    };
});

builder.Services.AddScoped<IBranchService, BranchService>();

builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddValidatorsFromAssemblyContaining<CreateBranchValidator>();


// 🔥 Tenant
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<TenantProvider>();
builder.Services.AddScoped<IBranchService, BranchService>();


// =====================
// Swagger + JWT
// =====================
builder.Services.AddSingleton<IConfiguration>(builder.Configuration);

builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Enter JWT Token like: Bearer {your token}"
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
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


// =====================
// 🔐 JWT Config
// =====================

var jwtSettings = builder.Configuration.GetSection("Jwt");

var keyString = jwtSettings["Key"];

if (string.IsNullOrEmpty(keyString))
    throw new Exception("JWT Key is missing in appsettings.json");

var key = Encoding.UTF8.GetBytes(keyString);

// =====================
// Authentication
// =====================

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,

        ValidIssuer = jwtSettings["Issuer"],
        ValidAudience = jwtSettings["Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(key)
    };
});

// =====================
// Authorization (Permissions)
// =====================

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("CreateEmployee",
        policy => policy.RequireClaim("permission", "CreateEmployee"));

    options.AddPolicy("ViewEmployees",
        policy => policy.RequireClaim("permission", "ViewEmployees"));
});

// =====================
// Database
// =====================

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// =====================
// App Build
// =====================

var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

// =====================
// Middleware
// =====================

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseDeveloperExceptionPage();
app.UseAuthentication();

//app.UseMiddleware<LicenseMiddleware>();   // 👈 ضيف هذا
//app.UseMiddleware<TenantMiddleware>();    // 👈 عندك من قبل
app.UseAuthorization();
//app.UseMiddleware<LicenseMiddleware>();
app.UseMiddleware<ExceptionMiddleware>();

// =====================
// Seed Data (مطور فقط)
// =====================

using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

    context.Database.Migrate(); // 🔥 مهم

    // Tenant
    if (!context.Tenants.Any())
    {
        context.Tenants.Add(new Tenant
        {
            Name = "Main Tenant"
        });

        context.SaveChanges();
    }

    // User
    if (!context.Users.Any())
    {
        var passwordHash = BCrypt.Net.BCrypt.HashPassword("1234");

        context.Users.Add(new User
        {
            Username = "admin",
            PasswordHash = passwordHash,
            TenantId = 1
        });

        context.SaveChanges();
    }
}

// =====================
// Endpoints
// =====================

app.UseExceptionHandler(errorApp =>
{
    errorApp.Run(async context =>
    {
        var exception = context.Features
            .Get<Microsoft.AspNetCore.Diagnostics.IExceptionHandlerFeature>()?
            .Error;

        if (exception is UnauthorizedAccessException)
        {
            context.Response.StatusCode = 401;
            await context.Response.WriteAsync("Unauthorized");
            return;
        }

        context.Response.StatusCode = 500;
        await context.Response.WriteAsync("Server Error");
    });
});

app.MapControllers();
//app.UseMiddleware<LicenseMiddleware>();

app.Run();