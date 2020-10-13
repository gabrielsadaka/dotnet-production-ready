using System;
using WeatherApi.Data.Repositories;
using WeatherApi.Domain;

namespace WeatherApi.Services
{
    public class WeatherForecastsService
    {
        private readonly IWeatherForecastsRepository _weatherForecastsRepository;

        public WeatherForecastsService(IWeatherForecastsRepository weatherForecastsRepository)
        {
            _weatherForecastsRepository = weatherForecastsRepository;
        }

        public WeatherForecast GetWeatherForecast(string city, DateTimeOffset forecastDate)
        {
            return _weatherForecastsRepository.GetForecast(city, forecastDate);
        }
    }
}