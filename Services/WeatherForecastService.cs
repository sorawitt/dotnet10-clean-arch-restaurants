namespace Restaurants.API.Services;

public interface IWeatherForecastService
{
    IEnumerable<WeatherForecast> Get(int count, int minTemperatureC, int maxTemperatureC);
}

public class WeatherForecastService : IWeatherForecastService
{
    private static readonly string[] Summaries =
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild",
        "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

    public IEnumerable<WeatherForecast> Get(int count, int minTemperatureC, int maxTemperatureC)
    {
        if (count <= 0)
        {
            return Array.Empty<WeatherForecast>();
        }

        if (minTemperatureC > maxTemperatureC)
        {
            return Array.Empty<WeatherForecast>();
        }

        var now = DateTime.UtcNow;
        var maxExclusive = maxTemperatureC == int.MaxValue ? maxTemperatureC : maxTemperatureC + 1;

        return Enumerable.Range(1, count).Select(index => new WeatherForecast
        {
            Date = DateOnly.FromDateTime(now.AddDays(index)),
            TemperatureC = Random.Shared.Next(minTemperatureC, maxExclusive),
            Summary = Summaries[Random.Shared.Next(Summaries.Length)]
        }).ToArray();
    }
}
