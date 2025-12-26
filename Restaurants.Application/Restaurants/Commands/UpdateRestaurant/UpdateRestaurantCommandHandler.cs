using MediatR;
using Microsoft.Extensions.Logging;
using Restaurants.Domain.Repositories;

namespace Restaurants.Application.Restaurants.Commands.UpdateRestaurant;

public class UpdateRestaurantCommandHandler(
    ILogger<UpdateRestaurantCommandHandler> logger,
    IRestaurantsRepository repository) : IRequestHandler<UpdateRestaurantCommand, bool>
{
    public async Task<bool> Handle(UpdateRestaurantCommand request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Update Restaurant");
        var restaurant = await repository.GetByIdAsync(request.Id);
        if (restaurant is null)
            return false;

        if (request.Name is not null)
        {
            restaurant.Name = request.Name;
        }

        if (request.Description is not null)
        {
            restaurant.Description = request.Description;
        }

        if (request.HasDelivery.HasValue)
        {
            restaurant.HasDelivery = request.HasDelivery.Value;
        }

        await repository.UpdateRestaurant(restaurant);
        return true;
    }
}
