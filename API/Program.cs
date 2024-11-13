using API.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Configure Kestrel to use specific SSL/TLS protocols
builder.WebHost.ConfigureKestrel(options =>
{
    options.ConfigureHttpsDefaults(httpsOptions =>
    {
        // Set the supported SSL protocols to TLS 1.2 and TLS 1.3
        httpsOptions.SslProtocols = System.Security.Authentication.SslProtocols.Tls12 | System.Security.Authentication.SslProtocols.Tls13;
    });
});

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddDbContext<DataContext>(opt => 
{
    opt.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection"));
});




// Configure the HTTP request pipeline.

// Add CORS with specific configuration
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowLocalhost", policy =>
    {
        policy.AllowAnyHeader()
              .AllowAnyMethod()
              .WithOrigins("http://localhost:4200", "https://localhost:4200")
              .AllowCredentials();  // Optional, if you need to allow credentials (cookies, headers)
    });
});

var app = builder.Build();

// Apply CORS policy globally
app.UseCors("AllowLocalhost");

app.MapControllers();

app.Run();
