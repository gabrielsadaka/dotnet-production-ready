using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WeatherApi.Data.Entities;

namespace WeatherApi.Data.Repositories
{
    public class WeatherForecastsRepository : IWeatherForecastsRepository
    {
        private readonly WeatherContext _weatherContext;

        public WeatherForecastsRepository(WeatherContext weatherContext)
        {
            _weatherContext = weatherContext;
        }

        public Task<WeatherForecast> GetWeatherForecast(string city, DateTimeOffset forecastDate,
            CancellationToken ct = default)
        {
            return _weatherContext.WeatherForecasts
                .SingleOrDefaultAsync(x => x.City == city && x.ForecastDate == forecastDate, ct);
        }

        public async Task AddWeatherForecast(WeatherForecast weatherForecast, CancellationToken ct = default)
        {
            _weatherContext.WeatherForecasts.Add(weatherForecast);
            await _weatherContext.SaveChangesAsync(ct);
        }
    }
}