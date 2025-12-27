using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Exceptions;
using Restaurants.Domain.Repositories;

namespace Restaurants.Application.Restaurants.Commands.CreateDish;

public class CreateDishCommandHandler(
    ILogger<CreateDishCommandHandler> logger,
    IMapper mapper,
    IRestaurantsRepository restaurantsRepository,
    IDishesRepository dishesRepository
    ) : IRequestHandler<CreateDishCommand, int>
{
    public async Task<int> Handle(CreateDishCommand request, CancellationToken cancellationToken)
    {
        logger.LogInformation(
            "Creating dish {DishName} for restaurant {RestaurantId} at price {Price}",
            request.Name,
            request.RestaurantId,
            request.Price
        );
        var restaurant = await restaurantsRepository.GetByIdAsync(request.RestaurantId);
        if (restaurant == null)
            throw new NotFoundException($"Restaurant with ID {request.RestaurantId} not found.");

        var dish = mapper.Map<Dish>(request);
        var dishId = await dishesRepository.CreateAsync(dish);
        logger.LogInformation("Created dish {DishId} for restaurant {RestaurantId}", dishId, request.RestaurantId);
        return dishId;
    }
}
