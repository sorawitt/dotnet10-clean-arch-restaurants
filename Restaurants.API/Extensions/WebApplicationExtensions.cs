using Microsoft.Extensions.Configuration;
using Restaurants.API.Middlewares;
using Restaurants.Domain.Entities;
using Serilog;

namespace Restaurants.API.Extensions;

public static class WebApplicationExtensions
{
    public static WebApplication UsePresentation(this WebApplication app)
    {
        app.UseSerilogRequestLogging();

        // Error handling must be early in the pipeline
        app.UseMiddleware<ErrorHandlingMiddleware>();

        if (!app.Environment.IsDevelopment())
        {
            app.UseHsts();
        }

        app.UseHttpsRedirection();

        if (IsSwaggerEnabled(app))
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        // Authentication must come before authorization
        app.UseAuthentication();
        app.UseAuthorization();

        // Map endpoints
        app.MapGroup("api/identity").MapIdentityApi<User>();
        app.MapControllers();

        return app;
    }

    private static bool IsSwaggerEnabled(WebApplication app)
    {
        return app.Configuration.GetValue("Swagger:Enabled", app.Environment.IsDevelopment());
    }
}
