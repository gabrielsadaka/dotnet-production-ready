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

        public async Task<WeatherForecast> GetWeatherForecast(string city, DateTimeOffset forecastDate,
            CancellationToken ct = default)
        {
            var weatherForecast = await _weatherForecastsRepository.GetWeatherForecast(city, forecastDate, ct);
            return new WeatherForecast(weatherForecast.Id, weatherForecast.City, weatherForecast.ForecastDate,
                weatherForecast.Forecast);
        }
    }
}