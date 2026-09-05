using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;
using PersonalDigitalVault.API.Administration.Services;
using PersonalDigitalVault.API.Authentication.Helpers;
using PersonalDigitalVault.API.Authentication.Services;
using PersonalDigitalVault.API.Authentication.Validators;
using PersonalDigitalVault.API.Data;
using PersonalDigitalVault.API.Models;
using PersonalDigitalVault.API.PublicSharing.Services.Implementations;
using PersonalDigitalVault.API.PublicSharing.Services.Interfaces;
using PersonalDigitalVault.API.PublicSharing.Validators;
using PersonalDigitalVault.API.Repositories.Implementations;
using PersonalDigitalVault.API.Repositories.Interfaces;
using PersonalDigitalVault.API.SecureVault.Services;
using System.Text;

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
builder.Services.AddScoped<LoginRequestValidator>();
builder.Services.AddScoped<ForgotPasswordRequestValidator>();
builder.Services.AddScoped<JwtTokenHelper>();
builder.Services.AddScoped<IPasswordResetTokenRepository, PasswordResetTokenRepository>();
builder.Services.AddScoped<PasswordResetTokenHelper>();
builder.Services.AddScoped<IEmailService, EmailService>();

// ==============================
// Administration Module DI
// ==============================

builder.Services.AddScoped<IAdminUserRepository, AdminUserRepository>();
builder.Services.AddScoped<IAdminUserService, AdminUserService>();

// ==============================
// Public Sharing Module DI
// ==============================

builder.Services.AddScoped<IShareLinkRepository, ShareLinkRepository>();
builder.Services.AddScoped<IDocumentRepository, DocumentRepository>();

builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<IShareService, ShareService>();

builder.Services.AddScoped<ShareLinkValidator>();

// ==============================
// Secure Vault Module DI
// ==============================

builder.Services.AddScoped<IFolderRepository, FolderRepository>();
builder.Services.AddScoped<IFolderService, FolderService>();


// ==============================
// JWT Authentication
// ==============================

var jwtKey = builder.Configuration["Jwt:Key"]
    ?? throw new InvalidOperationException(
        "JWT key is not configured.");

var jwtIssuer = builder.Configuration["Jwt:Issuer"]
    ?? throw new InvalidOperationException(
        "JWT issuer is not configured.");

var jwtAudience = builder.Configuration["Jwt:Audience"]
    ?? throw new InvalidOperationException(
        "JWT audience is not configured.");

builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters =
            new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,

                ValidIssuer = jwtIssuer,
                ValidAudience = jwtAudience,

                IssuerSigningKey =
                    new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(jwtKey)),

                ClockSkew = TimeSpan.Zero
            };
    });

// OpenAPI
builder.Services.AddOpenApi();
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer",
        new OpenApiSecurityScheme
        {
            Name = "Authorization",
            Type = SecuritySchemeType.Http,
            Scheme = "bearer",
            BearerFormat = "JWT",
            In = ParameterLocation.Header,
            Description = "Enter your JWT token."
        });

    options.AddSecurityRequirement(document =>
        new OpenApiSecurityRequirement
        {
            [new OpenApiSecuritySchemeReference("Bearer", document)] = []
        });
});

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

// Authentication
app.UseAuthentication();

// Authorization
app.UseAuthorization();

// Map API Controllers
app.MapControllers();


// ==============================
// Run Application
// ==============================

app.Run();