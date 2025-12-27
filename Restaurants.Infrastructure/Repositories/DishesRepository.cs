using Microsoft.EntityFrameworkCore;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Repositories;
using Restaurants.Infrastructure.Persistence;

namespace Restaurants.Infrastructure.Repositories;

internal class DishesRepository(RestaurantsDbContext dbContext) : IDishesRepository
{
    public async Task<int> CreateAsync(Dish entity)
    {
        dbContext.Dishes.Add(entity);
        await dbContext.SaveChangesAsync();
        return entity.Id;
    }

    public async Task DeleteAsync(Dish entity)
    {
        dbContext.Dishes.Remove(entity);
        await dbContext.SaveChangesAsync();
    }

    public Task DeleteAllForRestaurantAsync(int restaurantId)
    {
        return dbContext.Dishes
            .Where(dish => dish.RestaurantId == restaurantId)
            .ExecuteDeleteAsync();
    }
}
