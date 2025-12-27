using MediatR;
using Microsoft.Extensions.Logging;
using Restaurants.Domain.Exceptions;
using Restaurants.Domain.Repositories;

namespace Restaurants.Application.Restaurants.Commands.DeleteRestaurant;

public class DeleteRestaurantCommandHandler(
    ILogger<DeleteRestaurantCommandHandler> logger,
    IRestaurantsRepository repository) : IRequestHandler<DeleteRestaurantCommand>
{
    public async Task Handle(DeleteRestaurantCommand request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Deleting restaurant {RestaurantId}", request.Id);
        var restaurant = await repository.GetByIdAsync(request.Id);
        if (restaurant is null)
        {
            throw new NotFoundException($"Restaurant with ID {request.Id} not found.");
        }

        await repository.DeleteAsync(restaurant);
        logger.LogInformation("Deleted restaurant {RestaurantId}", request.Id);
    }
}
