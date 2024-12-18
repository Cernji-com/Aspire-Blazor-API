using AspireApp1.ApiService.Services;
using Microsoft.AspNetCore.Mvc;

namespace AspireApp1.ApiService.Controllers;

[ApiController]
[Route("api/[controller]")]
public class WeatherForecastController : ControllerBase
{
    private readonly IWeatherForecastService _weatherService;

    public WeatherForecastController(IWeatherForecastService weatherService)
    {
        _weatherService = weatherService;
    }

    [HttpGet("{days:int?}")]
    public IActionResult GetWeatherForecast([FromRoute] int days = 5)
    {
        var forecast = _weatherService.GetWeatherForecast(days);
        return Ok(forecast);
    }
}
