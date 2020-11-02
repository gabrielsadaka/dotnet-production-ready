using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using WeatherApi.Domain.Handlers.WeatherForecasts.AddWeatherForecast;
using WeatherApi.Domain.Handlers.WeatherForecasts.GetWeatherForecast;

namespace WeatherApi.Controllers
{
    [ApiController]
    [Route("/api/weather-forecasts")]
    public class WeatherForecastsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public WeatherForecastsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] GetWeatherForecastQuery query, CancellationToken ct = default)
        {
            var response = await _mediator.Send(query, ct);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> Post(AddWeatherForecastCommand command, CancellationToken ct = default)
        {
            var response = await _mediator.Send(command, ct);
            return Ok(response);
        }
    }
}