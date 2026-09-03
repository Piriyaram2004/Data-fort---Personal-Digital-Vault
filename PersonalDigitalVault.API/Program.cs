using Microsoft.EntityFrameworkCore;
using PersonalDigitalVault.API.Data;

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

// OpenAPI
builder.Services.AddOpenApi();
builder.Services.AddSwaggerGen();

// ==============================
// Build Application
// ==============================

var app = builder.Build();


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