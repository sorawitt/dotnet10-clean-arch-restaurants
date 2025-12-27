using MediatR;
using Microsoft.Extensions.Logging;
using Restaurants.Domain.Exceptions;
using Restaurants.Domain.Repositories;

namespace Restaurants.Application.Restaurants.Commands.DeleteDish;

public class DeleteDishCommandHandler(
    ILogger<DeleteDishCommandHandler> logger,
    IRestaurantsRepository restaurantsRepository,
    IDishesRepository dishesRepository) : IRequestHandler<DeleteDishCommand>
{
    public async Task Handle(DeleteDishCommand request, CancellationToken cancellationToken)
    {
        logger.LogInformation(
            "Deleting dish {DishId} for restaurant {RestaurantId}",
            request.DishId,
            request.RestaurantId
        );

        var restaurant = await restaurantsRepository.GetByIdAsync(request.RestaurantId);
        if (restaurant is null)
            throw new NotFoundException($"Restaurant with ID {request.RestaurantId} not found.");

        var dish = restaurant.Dishes.FirstOrDefault(item => item.Id == request.DishId);
        if (dish is null)
            throw new NotFoundException(
                $"Dish with ID {request.DishId} not found for restaurant {request.RestaurantId}."
            );

        await dishesRepository.Delete(dish);
        logger.LogInformation(
            "Deleted dish {DishId} for restaurant {RestaurantId}",
            request.DishId,
            request.RestaurantId
        );
    }
}
