using System;

// ReSharper disable AutoPropertyCanBeMadeGetOnly.Local

namespace WeatherApi.Domain
{
    public class WeatherForecast
    {
        public WeatherForecast(Guid id, string city, DateTimeOffset forecastDate, decimal forecast)
        {
            Id = id;
            City = city;
            ForecastDate = forecastDate;
            Forecast = forecast;
        }

        public Guid Id { get; private set; }
        public string City { get; private set; }
        public DateTimeOffset ForecastDate { get; private set; }
        public decimal Forecast { get; private set; }
    }
}