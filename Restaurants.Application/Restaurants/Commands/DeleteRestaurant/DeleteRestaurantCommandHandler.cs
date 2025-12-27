using MediatR;
using Microsoft.Extensions.Logging;
using Restaurants.Domain.Repositories;

namespace Restaurants.Application.Restaurants.Commands.DeleteRestaurant;

public class DeleteRestaurantCommandHandler(
    ILogger<DeleteRestaurantCommandHandler> logger,
    IRestaurantsRepository repository) : IRequestHandler<DeleteRestaurantCommand, bool>
{
    public async Task<bool> Handle(DeleteRestaurantCommand request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Deleting restaurant {RestaurantId}", request.Id);
        var restaurant = await repository.GetByIdAsync(request.Id);
        if (restaurant is null)
        {
            logger.LogWarning("Restaurant {RestaurantId} not found for delete", request.Id);
            return false;
        }

        await repository.DeleteRestaurant(restaurant);
        logger.LogInformation("Deleted restaurant {RestaurantId}", request.Id);
        return true;
    }
}
