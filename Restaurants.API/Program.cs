using Microsoft.OpenApi;
using Restaurants.API.Middlewares;
using Restaurants.API.Services;
using Restaurants.Application.Extensions;
using Restaurants.Domain.Entities;
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
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

// Configure Swagger with JWT authentication
const string bearerScheme = "bearer";
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition(bearerScheme, new OpenApiSecurityScheme
    {
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        Description = "Put ONLY your JWT token here. Swagger will add 'Bearer ' automatically."
    });

    options.AddSecurityRequirement(document => new OpenApiSecurityRequirement
    {
        [new OpenApiSecuritySchemeReference(bearerScheme, document)] = new List<string>()
    });
});

// Register middleware
builder.Services.AddScoped<ErrorHandlingMiddleware>();

// Register authentication and authorization
builder.Services.AddAuthentication();
builder.Services.AddAuthorization();

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

// Register API-specific services
builder.Services.AddScoped<IWeatherForecastService, WeatherForecastService>();

var app = builder.Build();

// Configure the HTTP request pipeline
app.UseSerilogRequestLogging();

// Error handling must be early in the pipeline
app.UseMiddleware<ErrorHandlingMiddleware>();

// Swagger in all environments for easier testing
app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

// Authentication must come before authorization
app.UseAuthentication();
app.UseAuthorization();

// Map endpoints
app.MapGroup("api/identity").MapIdentityApi<User>();

app.MapControllers();

// Seed data in development
if (app.Environment.IsDevelopment())
{
    await app.Services.SeedDataAsync();
}

app.Run();
