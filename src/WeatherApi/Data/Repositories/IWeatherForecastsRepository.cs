using System;
using System.Threading;
using System.Threading.Tasks;
using WeatherApi.Data.Entities;

namespace WeatherApi.Data.Repositories
{
    public interface IWeatherForecastsRepository
    {
        Task<WeatherForecast> GetWeatherForecast(string city, DateTimeOffset forecastDate,
            CancellationToken ct = default);
    }
}