using System;

namespace WeatherApi.Data.Entities
{
    public class WeatherForecast
    {
        public Guid Id { get; set; }
        public string? City { get; set; }
        public DateTimeOffset ForecastDate { get; set; }
        public decimal Forecast { get; set; }
    }
}