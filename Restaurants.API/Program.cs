using Microsoft.OpenApi;
using Restaurants.API.Middlewares;
using Restaurants.API.Services;
using Restaurants.Application.Extensions;
using Restaurants.Domain.Entities;
using Restaurants.Infrastructure.Extensions;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
const string schemeId = "bearer";
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition(schemeId, new OpenApiSecurityScheme
    {
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Put ONLY your JWT token here. Swagger will add 'Bearer ' automatically."
    });

    options.AddSecurityRequirement(document => new OpenApiSecurityRequirement
    {
        [new OpenApiSecuritySchemeReference(schemeId, document)] = new List<string>()
    });
});
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddScoped<ErrorHandlingMiddleware>();
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
builder.Host.UseSerilog((context, services, configuration) =>
{
    configuration
        .ReadFrom.Configuration(context.Configuration)
        .ReadFrom.Services(services);
});
var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseSerilogRequestLogging();
app.UseMiddleware<ErrorHandlingMiddleware>();

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.MapIdentityApi<User>();

app.UseAuthorization();

app.MapControllers();

if (app.Environment.IsDevelopment())
{
    await app.Services.SeedDataAsync();
}

app.Run();
