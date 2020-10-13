using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WeatherApi.Data;
using WeatherApi.Data.Repositories;
using WeatherApi.Tests.Builders;
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
            var expectedWeatherForecast = new WeatherForecastBuilder()
                .WithId(id)
                .WithCity(city)
                .WithForecastDate(forecastDate)
                .WithForecast(10.23m)
                .Build();

            var dbContextOptions = new DbContextOptionsBuilder<WeatherContext>()
                .UseInMemoryDatabase("weather-db")
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
    }
}