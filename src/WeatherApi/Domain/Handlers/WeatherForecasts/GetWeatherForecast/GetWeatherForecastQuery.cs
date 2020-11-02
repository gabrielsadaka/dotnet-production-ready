using System;
using MediatR;

namespace WeatherApi.Domain.Handlers.WeatherForecasts.GetWeatherForecast
{
    public class GetWeatherForecastQuery : IRequest<GetWeatherForecastResponse>
    {
        public string City { get; set; } = "";
        public DateTimeOffset ForecastDate { get; set; }
    }
}