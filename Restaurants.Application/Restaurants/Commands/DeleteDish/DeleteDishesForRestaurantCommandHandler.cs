using MediatR;
using Microsoft.Extensions.Logging;
using Restaurants.Domain.Exceptions;
using Restaurants.Domain.Repositories;

namespace Restaurants.Application.Restaurants.Commands.DeleteDish;

public class DeleteDishesForRestaurantCommandHandler(
    ILogger<DeleteDishesForRestaurantCommandHandler> logger,
    IRestaurantsRepository restaurantsRepository,
    IDishesRepository dishesRepository) : IRequestHandler<DeleteDishesForRestaurantCommand>
{
    public async Task Handle(DeleteDishesForRestaurantCommand request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Deleting all dishes for restaurant {RestaurantId}", request.RestaurantId);

        var restaurantExists = await restaurantsRepository.ExistsAsync(request.RestaurantId);
        if (!restaurantExists)
            throw new NotFoundException($"Restaurant with ID {request.RestaurantId} not found.");

        await dishesRepository.DeleteAllForRestaurantAsync(request.RestaurantId);
        logger.LogInformation("Deleted all dishes for restaurant {RestaurantId}", request.RestaurantId);
    }
}
