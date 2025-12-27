using MediatR;
using Microsoft.AspNetCore.Mvc;
using Restaurants.Application.Dishes.Dtos;
using Restaurants.Application.Restaurants.Commands.CreateDish;
using Restaurants.Application.Restaurants.Commands.DeleteDish;
using Restaurants.Application.Restaurants.Queries.GetDishByIdForRestaurant;
using Restaurants.Application.Restaurants.Queries.GetDishesForRestaurant;

namespace Restaurants.API.Controllers;

[Route("api/restaurants/{restaurantId}/dishes")]
[ApiController]
[Produces("application/json")]
public class DishesController(IMediator mediator) : ControllerBase
{
    [HttpPost]
    [Consumes("application/json")]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<int>> CreateDish([FromRoute] int restaurantId, [FromBody] CreateDishCommand command)
    {
        command.RestaurantId = restaurantId;
        var dishId = await mediator.Send(command);
        return Ok(dishId);
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<IEnumerable<DishDto>>> GetAllForRestaurant([FromRoute] int restaurantId)
    {
        var dishes = await mediator.Send(new GetDishesForRestaurantQuery(restaurantId));
        return Ok(dishes);
    }

    [HttpDelete]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteAllForRestaurant([FromRoute] int restaurantId)
    {
        if (restaurantId <= 0)
            return BadRequest();

        await mediator.Send(new DeleteDishesForRestaurantCommand(restaurantId));
        return NoContent();
    }

    [HttpGet("{dishId}")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<DishDto>> GetByIdForRestaurant([FromRoute] int restaurantId, [FromRoute] int dishId)
    {
        var dish = await mediator.Send(new GetDishByIdForRestaurantQuery(restaurantId, dishId));
        return Ok(dish);
    }

    [HttpDelete("{dishId}")]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteByIdForRestaurant([FromRoute] int restaurantId, [FromRoute] int dishId)
    {
        if (restaurantId <= 0 || dishId <= 0)
            return BadRequest();

        await mediator.Send(new DeleteDishCommand(restaurantId, dishId));
        return NoContent();
    }
}
