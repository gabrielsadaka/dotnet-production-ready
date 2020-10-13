using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WeatherApi.Domain;

namespace WeatherApi.Data.Repositories
{
    public class WeatherForecastsRepository : IWeatherForecastsRepository
    {
        private readonly WeatherContext _weatherContext;

        public WeatherForecastsRepository(WeatherContext weatherContext)
        {
            _weatherContext = weatherContext;
        }

        public Task<WeatherForecast> GetWeatherForecast(string city, DateTimeOffset forecastDate, CancellationToken ct = default)
        {
            return _weatherContext.WeatherForecasts
                .SingleOrDefaultAsync(x => x.City == city && x.ForecastDate == forecastDate, ct);
        }
    }
}