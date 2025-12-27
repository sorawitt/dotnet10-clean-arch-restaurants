using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Repositories;

namespace Restaurants.Application.Restaurants.Commands.CreateRestaurant;

public class CreateRestaurantCommandHandler(
    ILogger<CreateRestaurantCommandHandler> logger,
    IMapper mapper,
    IRestaurantsRepository repository) : IRequestHandler<CreateRestaurantCommand, int>
{
    public async Task<int> Handle(CreateRestaurantCommand request, CancellationToken cancellationToken)
    {
        logger.LogInformation(
            "Creating restaurant {RestaurantName} ({Category})",
            request.Name,
            request.Category
        );
        var restaurant = mapper.Map<Restaurant>(request);
        var id = await repository.CreateRestaurant(restaurant);
        logger.LogInformation("Created restaurant {RestaurantId}", id);
        return id;
    }
}
