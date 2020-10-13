using System;
using System.Threading;
using System.Threading.Tasks;
using WeatherApi.Data.Repositories;
using WeatherApi.Domain;

namespace WeatherApi.Services
{
    public class WeatherForecastsService : IWeatherForecastsService
    {
        private readonly IWeatherForecastsRepository _weatherForecastsRepository;

        public WeatherForecastsService(IWeatherForecastsRepository weatherForecastsRepository)
        {
            _weatherForecastsRepository = weatherForecastsRepository;
        }

        public Task<WeatherForecast> GetWeatherForecast(string city, DateTimeOffset forecastDate,
            CancellationToken ct = default)
        {
            return _weatherForecastsRepository.GetWeatherForecast(city, forecastDate, ct);
        }
    }
}