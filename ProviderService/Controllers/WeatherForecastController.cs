using Microsoft.AspNetCore.Mvc;
using MassTransit;
using DataTransfer;

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

    [HttpPost]
    public async Task<IActionResult> PostAsync(DataTransfer01 input)
    {
        var output = new DataTransfer01
        {
            Value = input.Value > 0 ? input.Value * 2 : -1,
        };

        await _publishEndpoint.Publish(output);

        return StatusCode(201, output);
    }
}
