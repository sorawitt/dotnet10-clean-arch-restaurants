using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Restaurants.Domain.Constants;
using Restaurants.Domain.Entities;

namespace Restaurants.Infrastructure.Persistence;

internal class RestaurantSeeder(RestaurantsDbContext dbContext)
{
    public async Task SeedAsync()
    {
        if (await dbContext.Database.CanConnectAsync())
        {
            if (!await dbContext.Restaurants.AnyAsync())
            {
                var restaurants = GetRestaurants();
                dbContext.Restaurants.AddRange(restaurants);
                await dbContext.SaveChangesAsync();
            }

            if (!await dbContext.Roles.AnyAsync())
            {
                var roles = GetRoles();
                dbContext.Roles.AddRange(roles);
                await dbContext.SaveChangesAsync();
            }
        }
    }

    private IEnumerable<IdentityRole> GetRoles()
    {
        List<IdentityRole> roles = [
            CreateRole(UserRoles.User),
            CreateRole(UserRoles.Owner),
            CreateRole(UserRoles.Admin),
        ];
        return roles;
    }

    private static IdentityRole CreateRole(string roleName)
    {
        return new IdentityRole(roleName)
        {
            NormalizedName = roleName.ToUpperInvariant()
        };
    }

    private static IEnumerable<Restaurant> GetRestaurants()
    {
        return new List<Restaurant>
        {
            new()
            {
                Name = "Pizza Palace",
                Description = "Neapolitan pizza, salads, and desserts.",
                Category = "Italian",
                HasDelivery = true,
                ContactEmail = "contact@pizzapalace.example",
                ContactNumber = "123-456-789",
                Address = new Address
                {
                    City = "Bangkok",
                    Street = "Sukhumvit 21",
                    PostalCode = "10110",
                },
                Dishes = new List<Dish>
                {
                    new()
                    {
                        Name = "Margherita",
                        Description = "Tomato, mozzarella, basil.",
                        Price = 12.50m,
                    },
                    new()
                    {
                        Name = "Pepperoni",
                        Description = "Spicy pepperoni and mozzarella.",
                        Price = 14.00m,
                    },
                },
            },
            new()
            {
                Name = "Sushi Square",
                Description = "Fresh sushi and sashimi sets.",
                Category = "Japanese",
                HasDelivery = false,
                ContactEmail = "hello@sushisquare.example",
                ContactNumber = "987-654-321",
                Address = new Address
                {
                    City = "Bangkok",
                    Street = "Silom 10",
                    PostalCode = "10500",
                },
                Dishes = new List<Dish>
                {
                    new()
                    {
                        Name = "Salmon Set",
                        Description = "Salmon nigiri with miso soup.",
                        Price = 18.00m,
                    },
                    new()
                    {
                        Name = "Tuna Roll",
                        Description = "Tuna maki roll.",
                        Price = 9.50m,
                    },
                },
            },
        };
    }
}
