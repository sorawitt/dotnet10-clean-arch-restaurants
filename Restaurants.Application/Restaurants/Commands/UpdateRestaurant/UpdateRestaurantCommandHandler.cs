using MediatR;
using Microsoft.Extensions.Logging;
using Restaurants.Domain.Exceptions;
using Restaurants.Domain.Repositories;

namespace Restaurants.Application.Restaurants.Commands.UpdateRestaurant;

public class UpdateRestaurantCommandHandler(
    ILogger<UpdateRestaurantCommandHandler> logger,
    IRestaurantsRepository repository) : IRequestHandler<UpdateRestaurantCommand>
{
    public async Task Handle(UpdateRestaurantCommand request, CancellationToken cancellationToken)
    {
        var updatedFields = new List<string>(3);
        if (request.Name is not null)
        {
            updatedFields.Add(nameof(request.Name));
        }

        if (request.Description is not null)
        {
            updatedFields.Add(nameof(request.Description));
        }

        if (request.HasDelivery.HasValue)
        {
            updatedFields.Add(nameof(request.HasDelivery));
        }

        logger.LogInformation(
            "Updating restaurant {RestaurantId} with fields {UpdatedFields}",
            request.Id,
            updatedFields
        );
        var restaurant = await repository.GetByIdAsync(request.Id);
        if (restaurant is null)
        {
            throw new NotFoundException($"Restaurant with ID {request.Id} not found.");
        }

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
        logger.LogInformation("Updated restaurant {RestaurantId}", request.Id);
    }
}
