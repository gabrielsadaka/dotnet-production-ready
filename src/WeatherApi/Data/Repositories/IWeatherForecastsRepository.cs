using System;
using WeatherApi.Domain;

namespace WeatherApi.Data.Repositories
{
    public interface IWeatherForecastsRepository
    {
        WeatherForecast GetForecast(string city, DateTimeOffset forecastDate);
    }
}