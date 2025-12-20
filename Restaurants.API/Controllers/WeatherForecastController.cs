using Microsoft.AspNetCore.Mvc;
using Restaurants.API.Services;

namespace Restaurants.API.Controllers;

[ApiController]
[Route("[controller]")]
public class WeatherForecastController : ControllerBase
{
    private readonly IWeatherForecastService _service;

    public WeatherForecastController(IWeatherForecastService service)
    {
        _service = service;
    }

    [HttpGet]
    public ActionResult<IEnumerable<WeatherForecast>> Get(
        [FromQuery] int count = 5,
        [FromQuery] int minTemperatureC = -20,
        [FromQuery] int maxTemperatureC = 55)
    {
        if (count <= 0)
        {
            return BadRequest("count must be greater than 0.");
        }

        if (minTemperatureC > maxTemperatureC)
        {
            return BadRequest("minTemperatureC must be less than or equal to maxTemperatureC.");
        }

        return Ok(_service.Get(count, minTemperatureC, maxTemperatureC));
    }
}
