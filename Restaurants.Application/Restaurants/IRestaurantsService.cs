using Microsoft.Extensions.Logging;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Repositories;

namespace Restaurants.Application.Restaurants;

public interface IRestaurantsService
{
    public Task<IEnumerable<Restaurant>> GetAllRestaurants();
    public Task<Restaurant?> GetRestaurant(int id);
}