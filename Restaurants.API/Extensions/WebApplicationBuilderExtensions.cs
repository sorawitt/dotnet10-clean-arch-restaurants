using Microsoft.AspNetCore.Identity;
using Microsoft.OpenApi;
using Restaurants.API.Middlewares;
using Restaurants.API.Services;

namespace Restaurants.API.Extensions;

public static class WebApplicationBuilderExtensions
{
    public static WebApplicationBuilder AddPresentation(this WebApplicationBuilder builder)
    {
        AddControllers(builder.Services);
        AddSwagger(builder.Services);
        AddAuthentication(builder.Services);
        AddApiServices(builder.Services);

        return builder;
    }

    private static void AddControllers(IServiceCollection services)
    {
        services.AddControllers();
        services.AddEndpointsApiExplorer();
    }

    private static void AddSwagger(IServiceCollection services)
    {
        // Configure Swagger with bearer token authentication
        var bearerScheme = IdentityConstants.BearerScheme;
        services.AddSwaggerGen(options =>
        {
            options.AddSecurityDefinition(bearerScheme, new OpenApiSecurityScheme
            {
                Type = SecuritySchemeType.Http,
                Scheme = "bearer",
                Description = "Put ONLY your access token here. Swagger will add 'Bearer ' automatically."
            });

            options.AddSecurityRequirement(document => new OpenApiSecurityRequirement
            {
                [new OpenApiSecuritySchemeReference(bearerScheme, document)] = new List<string>()
            });
        });
    }

    private static void AddAuthentication(IServiceCollection services)
    {
        // Identity API endpoints register the bearer scheme; only set defaults here.
        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = IdentityConstants.BearerScheme;
            options.DefaultChallengeScheme = IdentityConstants.BearerScheme;
            options.DefaultScheme = IdentityConstants.BearerScheme;
        });
        services.AddAuthorization();
    }

    private static void AddApiServices(IServiceCollection services)
    {
        services.AddScoped<ErrorHandlingMiddleware>();
        services.AddScoped<IWeatherForecastService, WeatherForecastService>();
    }
}
