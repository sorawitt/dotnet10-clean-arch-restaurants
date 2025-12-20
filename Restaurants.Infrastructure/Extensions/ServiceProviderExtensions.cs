using Microsoft.Extensions.DependencyInjection;
using Restaurants.Infrastructure.Persistence;

namespace Restaurants.Infrastructure.Extensions;

public static class ServiceProviderExtensions
{
    public static async Task SeedDataAsync(this IServiceProvider services)
    {
        using var scope = services.CreateScope();
        var seeder = scope.ServiceProvider.GetRequiredService<RestaurantSeeder>();
        await seeder.SeedAsync();
    }
}
