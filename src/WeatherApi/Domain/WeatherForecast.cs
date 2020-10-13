using System;

namespace WeatherApi.Domain
{
    public class WeatherForecast
    {
        public WeatherForecast(string city, DateTimeOffset forecastDate, decimal forecast)
        {
            City = city;
            ForecastDate = forecastDate;
            Forecast = forecast;
        }

        public string City { get; }
        public DateTimeOffset ForecastDate { get; }
        public decimal Forecast { get; }
    }
}