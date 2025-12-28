using Restaurants.API.Extensions;
using Restaurants.Application.Extensions;
using Restaurants.Infrastructure.Extensions;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Configure Serilog early to capture startup errors
builder.Host.UseSerilog((context, services, configuration) =>
{
    configuration
        .ReadFrom.Configuration(context.Configuration)
        .ReadFrom.Services(services);
});

// Add services to the container
builder.AddPresentation();

// Register application layers
builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

// Configure AutoMapper
builder.Services.AddAutoMapper(
    cfg =>
    {
        var licenseKey = builder.Configuration["AutoMapper:LicenseKey"];
        if (!string.IsNullOrWhiteSpace(licenseKey))
        {
            cfg.LicenseKey = licenseKey;
        }
    },
    typeof(Restaurants.Application.Extensions.ServiceCollectionExtensions).Assembly
);

var app = builder.Build();

// Configure the HTTP request pipeline
app.UsePresentation();

// Seed data in development
if (app.Environment.IsDevelopment())
{
    await app.Services.SeedDataAsync();
}

app.Run();
