using System;

namespace WeatherApi.Domain.Handlers.WeatherForecasts.GetWeatherForecast
{
    public class GetWeatherForecastResponse
    {
        public GetWeatherForecastResponse(Guid id, string city, DateTimeOffset forecastDate, decimal forecast)
        {
            Id = id;
            City = city;
            ForecastDate = forecastDate;
            Forecast = forecast;
        }

        public Guid Id { get; }
        public string City { get; }
        public DateTimeOffset ForecastDate { get; }
        public decimal Forecast { get; }
    }
}