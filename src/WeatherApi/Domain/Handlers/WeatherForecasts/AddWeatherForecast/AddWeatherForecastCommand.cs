using System;
using MediatR;

namespace WeatherApi.Domain.Handlers.WeatherForecasts.AddWeatherForecast
{
    public class AddWeatherForecastCommand : IRequest<AddWeatherForecastResponse>
    {
        public string City { get; set; } = "";
        public DateTimeOffset ForecastDate { get; set; }
        public decimal Forecast { get; set; }
    }
}