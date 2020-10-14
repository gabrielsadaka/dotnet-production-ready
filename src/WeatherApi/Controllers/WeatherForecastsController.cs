using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WeatherApi.Services;

namespace WeatherApi.Controllers
{
    [ApiController]
    [Route("/api/weather-forecasts")]
    public class WeatherForecastsController : ControllerBase
    {
        private readonly IWeatherForecastsService _weatherForecastsService;

        public WeatherForecastsController(IWeatherForecastsService weatherForecastsService)
        {
            _weatherForecastsService = weatherForecastsService;
        }

        public async Task<IActionResult> Get(string city, DateTimeOffset forecastDate, CancellationToken ct = default)
        {
            var weatherForecast = await _weatherForecastsService.GetWeatherForecast(city, forecastDate, ct);
            return Ok(weatherForecast);
        }
    }
}