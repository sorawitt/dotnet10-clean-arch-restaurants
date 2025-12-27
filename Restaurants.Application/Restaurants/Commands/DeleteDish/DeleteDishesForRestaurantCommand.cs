using MediatR;

namespace Restaurants.Application.Restaurants.Commands.DeleteDish;

public class DeleteDishesForRestaurantCommand(int restaurantId) : IRequest
{
    public int RestaurantId { get; } = restaurantId;
}
