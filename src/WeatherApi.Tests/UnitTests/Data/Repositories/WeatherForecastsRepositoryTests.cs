using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WeatherApi.Data;
using WeatherApi.Data.Entities;
using WeatherApi.Data.Repositories;
using Xunit;

namespace WeatherApi.Tests.UnitTests.Data.Repositories
{
    public class WeatherForecastsRepositoryTests
    {
        [Fact]
        public async Task GetWeatherForecastReturnsCorrectForecast()
        {
            var id = Guid.NewGuid();
            const string city = "Australia/Melbourne";
            var forecastDate = new DateTimeOffset(2020, 1, 3, 3, 2, 4, TimeSpan.Zero);
            var expectedWeatherForecast = new WeatherForecast(id, city, forecastDate, 10.23m);

            var dbContextOptions = new DbContextOptionsBuilder<WeatherContext>()
                .UseInMemoryDatabase(nameof(GetWeatherForecastReturnsCorrectForecast))
                .Options;

            await using var context = new WeatherContext(dbContextOptions);
            await context.WeatherForecasts.AddAsync(expectedWeatherForecast);
            await context.SaveChangesAsync();

            var sut = new WeatherForecastsRepository(context);
            var actualWeatherForecast = await sut.GetWeatherForecast(city, forecastDate);

            Assert.Equal(id, actualWeatherForecast.Id);
            Assert.Equal(city, actualWeatherForecast.City);
            Assert.Equal(forecastDate, actualWeatherForecast.ForecastDate);
        }

        [Fact]
        public async Task AddWeatherForecastSavesForecastSuccessfully()
        {
            var id = Guid.NewGuid();
            const string city = "Australia/Melbourne";
            var forecastDate = new DateTimeOffset(2020, 1, 3, 3, 2, 4, TimeSpan.Zero);
            const decimal forecast = 10.23m;
            var newWeatherForecast = new WeatherForecast(id, city, forecastDate, forecast);

            var dbContextOptions = new DbContextOptionsBuilder<WeatherContext>()
                .UseInMemoryDatabase(nameof(AddWeatherForecastSavesForecastSuccessfully))
                .Options;

            await using var context = new WeatherContext(dbContextOptions);

            var sut = new WeatherForecastsRepository(context);
            await sut.AddWeatherForecast(newWeatherForecast);

            var savedWeatherForecast =
                await context.WeatherForecasts.SingleOrDefaultAsync(x => x.Id == newWeatherForecast.Id);

            Assert.NotNull(savedWeatherForecast);
            Assert.Equal(id, savedWeatherForecast.Id);
            Assert.Equal(city, savedWeatherForecast.City);
            Assert.Equal(forecastDate, savedWeatherForecast.ForecastDate);
            Assert.Equal(forecast, savedWeatherForecast.Forecast);
        }
    }
}