using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Moq;
using WeatherApi.Controllers;
using WeatherApi.Domain;
using WeatherApi.Services;
using WeatherApi.Tests.Builders;
using Xunit;

namespace WeatherApi.Tests.UnitTests.Controllers
{
    public class WeatherForecastsControllerTests
    {
        [Fact]
        public async Task GetReturnsOkResult()
        {
            var sut = new WeatherForecastsController(Mock.Of<IWeatherForecastsService>());
            var result = await sut.Get("Australia/Melbourne", new DateTimeOffset(2020, 1, 2, 3, 5, 1, TimeSpan.Zero));

            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async Task GetReturnsCorrectForecast()
        {
            var id = Guid.NewGuid();
            const string city = "Australia/Melbourne";
            var forecastDate = new DateTimeOffset(2020, 1, 2, 3, 5, 1, TimeSpan.Zero);
            var expectedWeatherForecast = new WeatherForecastBuilder()
                .WithId(id)
                .WithCity(city)
                .WithForecastDate(forecastDate)
                .WithForecast(10.23m)
                .Build();
            var stubWeatherForecastsService = new Mock<IWeatherForecastsService>();
            stubWeatherForecastsService
                .Setup(x => x.GetWeatherForecast(city, forecastDate, It.IsAny<CancellationToken>()))
                .ReturnsAsync(expectedWeatherForecast);

            var sut = new WeatherForecastsController(stubWeatherForecastsService.Object);
            var result = await sut.Get(city, forecastDate);

            var actualWeatherForecast = (WeatherForecast) ((OkObjectResult) result).Value;
            Assert.Equal(id, actualWeatherForecast.Id);
            Assert.Equal(city, actualWeatherForecast.City);
            Assert.Equal(forecastDate, actualWeatherForecast.ForecastDate);
        }
    }
}