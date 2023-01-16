using Microsoft.AspNetCore.Mvc;
using MassTransit;

namespace ProviderService.Controllers;

[ApiController]
[Route("[controller]")]
public class WeatherForecastController : ControllerBase
{
    private static readonly string[] Summaries = new[]
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

    private readonly IPublishEndpoint _publishEndpoint;
    private readonly ILogger<WeatherForecastController> _logger;

    public WeatherForecastController(
        IPublishEndpoint publishEndpoint,
        ILogger<WeatherForecastController> logger
    )
    {
        _publishEndpoint = publishEndpoint;
        _logger = logger;
    }

    [HttpGet(Name = "GetWeatherForecast")]
    public IEnumerable<WeatherForecast> Get()
    {
        return Enumerable.Range(1, 5).Select(index => new WeatherForecast
        {
            Date = DateTime.Now.AddDays(index),
            TemperatureC = Random.Shared.Next(-20, 55),
            Summary = Summaries[Random.Shared.Next(Summaries.Length)]
        })
        .ToArray();
    }

    [HttpGet("[action]")]
    public IEnumerable<WeatherForecast> ListAsync()
    {
        return Enumerable.Range(1, 5).Select(index => new WeatherForecast
        {
            Date = DateTime.Now.AddDays(index),
            TemperatureC = Random.Shared.Next(-20, 55),
            Summary = Summaries[Random.Shared.Next(Summaries.Length)]
        })
        .ToArray();
    }

    public class PostData
    {
        public int Value { get; set; }
    }

    [HttpPost]
    public async Task<IActionResult> PostAsync(PostData input)
    {
        var output = new PostData
        {
            Value = input.Value > 0 ? input.Value * 2 : -1,
        };

        await _publishEndpoint.Publish(output);

        return StatusCode(201, output);
    }
}
