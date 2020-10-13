using System;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using WeatherApi.Data.Repositories;
using WeatherApi.Services;
using WeatherApi.Tests.Builders;
using Xunit;

namespace WeatherApi.Tests.UnitTests.Services
{
    public class WeatherForecastsServiceTests
    {
        [Fact]
        public async Task GetWeatherForecastReturnsCorrectForecast()
        {
            const string city = "Australia/Melbourne";
            var forecastDate = new DateTimeOffset(2020, 1, 3, 3, 2, 4, TimeSpan.Zero);
            var expectedWeatherForecast = new WeatherForecastBuilder()
                .WithCity(city)
                .WithForecastDate(forecastDate)
                .WithForecast(10.23m)
                .Build();
            var stubWeatherForecastsRepository = new Mock<IWeatherForecastsRepository>();
            stubWeatherForecastsRepository
                .Setup(x => x.GetWeatherForecast(city, forecastDate, It.IsAny<CancellationToken>()))
                .ReturnsAsync(expectedWeatherForecast);

            var sut = new WeatherForecastsService(stubWeatherForecastsRepository.Object);
            var actualWeatherForecast = await sut.GetWeatherForecast(city, forecastDate);

            Assert.Equal(city, actualWeatherForecast.City);
            Assert.Equal(forecastDate, actualWeatherForecast.ForecastDate);
        }
    }
}