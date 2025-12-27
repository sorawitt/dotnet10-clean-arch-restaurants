using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Restaurants.Application.Dishes.Dtos;
using Restaurants.Domain.Exceptions;
using Restaurants.Domain.Repositories;

namespace Restaurants.Application.Restaurants.Queries.GetDishByIdForRestaurant;

public class GetDishByIdForRestaurantQueryHandler(
    ILogger<GetDishByIdForRestaurantQueryHandler> logger,
    IMapper mapper,
    IRestaurantsRepository restaurantsRepository) : IRequestHandler<GetDishByIdForRestaurantQuery, DishDto>
{
    public async Task<DishDto> Handle(GetDishByIdForRestaurantQuery request, CancellationToken cancellationToken)
    {
        logger.LogInformation(
            "Retrieving dish {DishId} for restaurant {RestaurantId}",
            request.DishId,
            request.RestaurantId
        );
        var restaurant = await restaurantsRepository.GetByIdAsync(request.RestaurantId);
        if (restaurant == null)
            throw new NotFoundException($"Restaurant with ID {request.RestaurantId} not found.");

        var dish = restaurant.Dishes.FirstOrDefault(item => item.Id == request.DishId);
        if (dish == null)
            throw new NotFoundException($"Dish with ID {request.DishId} not found for restaurant {request.RestaurantId}.");
        var result = mapper.Map<DishDto>(dish);
        return result;
    }
}
