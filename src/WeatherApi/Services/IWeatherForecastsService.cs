using System;
using System.Threading;
using System.Threading.Tasks;
using WeatherApi.Domain;

namespace WeatherApi.Services
{
    public interface IWeatherForecastsService
    {
        Task<WeatherForecast> GetWeatherForecast(string city, DateTimeOffset forecastDate,
            CancellationToken ct = default);
    }
}