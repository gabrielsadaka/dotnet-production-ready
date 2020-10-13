using System;
using WeatherApi.Domain;

namespace WeatherApi.Tests.Builders
{
    public class WeatherForecastBuilder
    {
        public Guid Id { get; private set; } = Guid.NewGuid();

        public string City { get; private set; }

        public DateTimeOffset ForecastDate { get; private set; }

        public decimal Forecast { get; private set; }

        public WeatherForecast Build()
        {
            return new WeatherForecast(Id, City, ForecastDate, Forecast);
        }

        public WeatherForecastBuilder WithId(Guid id)
        {
            Id = id;
            return this;
        }

        public WeatherForecastBuilder WithCity(string city)
        {
            City = city;
            return this;
        }

        public WeatherForecastBuilder WithForecastDate(DateTimeOffset forecastDate)
        {
            ForecastDate = forecastDate;
            return this;
        }

        public WeatherForecastBuilder WithForecast(decimal forecast)
        {
            Forecast = forecast;
            return this;
        }
    }
}