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
    public IEnumerable<WeatherForecast> Get()
    {
        return _service.Get();
    }

    [HttpGet("first")]
    public ActionResult<WeatherForecast> GetFirst()
    {
        var result = _service.Get().FirstOrDefault();
        if (result is null)
        {
            return NotFound();
        }

        return Ok(result);
    }
}
