using MediatR;
using Microsoft.AspNetCore.Mvc;
using Restaurants.Application.Restaurants.Commands.CreateRestaurant;
using Restaurants.Application.Restaurants.Commands.DeleteRestaurant;
using Restaurants.Application.Restaurants.Commands.UpdateRestaurant;
using Restaurants.Application.Restaurants.Dtos;
using Restaurants.Application.Restaurants.Queries.GetAllRestaurants;
using Restaurants.Application.Restaurants.Queries.GetRestaurantById;

[ApiController]
[Route("/api/restaurants")]
[Produces("application/json")]
public class RestaurantsController(IMediator mediator) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<RestaurantDto>>> GetAll()
    {
        var restaurants = await mediator.Send(new GetAllRestaurantsQuery());
        return Ok(restaurants);
    }

    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<RestaurantDto?>> GetRestaurant([FromRoute] int id)
    {
        var restaurant = await mediator.Send(new GetRestaurantByIdQuery(id));
        if (restaurant is null)
            return NotFound();
        return Ok(restaurant);
    }


    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteRestaurant([FromRoute] int id)
    {
        var isDeleted = await mediator.Send(new DeleteRestaurantCommand(id));
        if (isDeleted)
            return NoContent();
        return NotFound();
    }

    [HttpPost]
    [Consumes("application/json")]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<int>> CreateRestaurant([FromBody] CreateRestaurantCommand command)
    {
        var restaurantId = await mediator.Send(command);
        return Ok(restaurantId);
    }

    [HttpPatch("{id}")]
    [Consumes("application/json")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdateRestaurant([FromRoute] int id, [FromBody] UpdateRestaurantCommand command)
    {
        if (command is null)
            return BadRequest();

        if (id <= 0)
            return BadRequest();

        command.Id = id;
        var isUpdated = await mediator.Send(command);
        if (isUpdated)
            return NoContent();
        return NotFound();
    }
}
