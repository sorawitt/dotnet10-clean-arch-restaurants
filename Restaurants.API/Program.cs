using Restaurants.API.Services;
using Restaurants.Application.Extensions;
using Restaurants.Infrastructure.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddApplication();
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
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddScoped<IWeatherForecastService, WeatherForecastService>();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

if (app.Environment.IsDevelopment())
{
    await app.Services.SeedDataAsync();
}

app.Run();
