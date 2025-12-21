using Restaurants.Application.Restaurants.Dtos;

namespace Restaurants.Application.Restaurants;

public interface IRestaurantsService
{
    public Task<IEnumerable<RestaurantDto>> GetAllRestaurants();
    public Task<RestaurantDto?> GetRestaurant(int id);
}