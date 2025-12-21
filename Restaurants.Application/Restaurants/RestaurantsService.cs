using Microsoft.Extensions.Logging;
using Restaurants.Application.Restaurants.Dtos;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Repositories;

namespace Restaurants.Application.Restaurants;

internal class RestaurantsService(IRestaurantsRepository repository, ILogger<RestaurantsService> logger) : IRestaurantsService
{
    public async Task<IEnumerable<RestaurantDto>> GetAllRestaurants()
    {
        logger.LogInformation("Getting all restaurants...");
        var restaurants = await repository.GetAllAsync();
        var restaurantsDtos = restaurants.Select(RestaurantDto.FromEntity);
        return restaurantsDtos!;
    }

    public async Task<RestaurantDto?> GetRestaurant(int id)
    {
        logger.LogInformation("Getting restaurant by id {id}", id);
        var restaurant = await repository.GetByIdAsync(id);
        var restaurantDto = RestaurantDto.FromEntity(restaurant);
        return restaurantDto;
    }
}