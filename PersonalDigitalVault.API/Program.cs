using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PersonalDigitalVault.API.Authentication.Services;
using PersonalDigitalVault.API.Authentication.Validators;
using PersonalDigitalVault.API.Data;
using PersonalDigitalVault.API.Models;
using PersonalDigitalVault.API.Repositories.Implementations;
using PersonalDigitalVault.API.Repositories.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// ==============================
// Add Services
// ==============================

// Controllers
builder.Services.AddControllers();

// Database - Entity Framework Core + SQL Server
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")
    ));

// ==============================
// Authentication Module DI
// ==============================

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IRoleRepository, RoleRepository>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();
builder.Services.AddScoped<RegisterRequestValidator>();

// OpenAPI
builder.Services.AddOpenApi();
builder.Services.AddSwaggerGen();

// ==============================
// Build Application
// ==============================

var app = builder.Build();

// ==============================
// Initialize Required Database Data
// ==============================

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider
        .GetRequiredService<ApplicationDbContext>();

    await DbInitializer.InitializeRolesAsync(dbContext);
}

// ==============================
// HTTP Request Pipeline
// ==============================

// OpenAPI available in Development
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Redirect HTTP to HTTPS
app.UseHttpsRedirection();

// Authorization
app.UseAuthorization();

// Map API Controllers
app.MapControllers();


// ==============================
// Run Application
// ==============================

app.Run();