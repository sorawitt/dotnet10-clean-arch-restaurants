using MediatR;
using Restaurants.Application.Dishes.Dtos;

namespace Restaurants.Application.Restaurants.Queries.GetDishesForRestaurant;

public class GetDishesForRestaurantQuery(int restaurantId) : IRequest<IEnumerable<DishDto>>
{
    public int RestaurantId { get; } = restaurantId;
}
