using System;

namespace WeatherApi.Domain.Handlers.WeatherForecasts.AddWeatherForecast
{
    public class AddWeatherForecastResponse
    {
        public AddWeatherForecastResponse(Guid id)
        {
            Id = id;
        }

        public Guid Id { get; }
    }
}