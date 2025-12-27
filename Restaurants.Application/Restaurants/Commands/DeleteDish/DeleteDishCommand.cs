using MediatR;

namespace Restaurants.Application.Restaurants.Commands.DeleteDish;

public class DeleteDishCommand(int restaurantId, int dishId) : IRequest
{
    public int RestaurantId { get; } = restaurantId;
    public int DishId { get; } = dishId;
}
