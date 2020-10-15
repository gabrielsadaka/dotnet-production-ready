using System;

namespace WeatherApi.Domain
{
    public class WeatherForecast
    {
        public WeatherForecast(Guid id, string? city, DateTimeOffset forecastDate, decimal forecast)
        {
            Id = id;
            City = city;
            ForecastDate = forecastDate;
            Forecast = forecast;
        }

        public Guid Id { get; }
        public string? City { get; }
        public DateTimeOffset ForecastDate { get; }
        public decimal Forecast { get; }
    }
}