using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using WeatherApi.Controllers;
using WeatherApi.Domain.Handlers.WeatherForecasts.AddWeatherForecast;
using WeatherApi.Domain.Handlers.WeatherForecasts.GetWeatherForecast;
using Xunit;

namespace WeatherApi.Tests.UnitTests.Controllers
{
    public class WeatherForecastsControllerTests
    {
        [Fact]
        public async Task GetReturnsOkResult()
        {
            var getWeatherForecastQuery = new GetWeatherForecastQuery
            {
                City = "Australia/Melbourne",
                ForecastDate = new DateTimeOffset(2020, 1, 2, 3, 5, 1, TimeSpan.Zero)
            };
            var sut = new WeatherForecastsController(Mock.Of<IMediator>());
            var result = await sut.Get(getWeatherForecastQuery);

            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async Task GetReturnsCorrectForecast()
        {
            var id = Guid.NewGuid();
            const string city = "Australia/Melbourne";
            var forecastDate = new DateTimeOffset(2020, 1, 2, 3, 5, 1, TimeSpan.Zero);
            var getWeatherForecastQuery = new GetWeatherForecastQuery
            {
                City = city,
                ForecastDate = forecastDate
            };
            var expectedWeatherForecast = new GetWeatherForecastResponse(id, city, forecastDate, 10.23m);
            var stubMediator = new Mock<IMediator>();
            stubMediator
                .Setup(x => x.Send(getWeatherForecastQuery, It.IsAny<CancellationToken>()))
                .ReturnsAsync(expectedWeatherForecast);

            var sut = new WeatherForecastsController(stubMediator.Object);
            var result = await sut.Get(getWeatherForecastQuery);

            var actualWeatherForecast = (GetWeatherForecastResponse) ((OkObjectResult) result).Value;
            Assert.Equal(id, actualWeatherForecast.Id);
            Assert.Equal(city, actualWeatherForecast.City);
            Assert.Equal(forecastDate, actualWeatherForecast.ForecastDate);
        }

        [Fact]
        public async Task PostReturnsOkResult()
        {
            var sut = new WeatherForecastsController(Mock.Of<IMediator>());
            var addWeatherForecastCommand = new AddWeatherForecastCommand
            {
                City = "Australia/Melbourne",
                ForecastDate = new DateTimeOffset(2020, 1, 2, 3, 5, 1, TimeSpan.Zero),
                Forecast = 10.32m
            };
            var result = await sut.Post(addWeatherForecastCommand);

            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async Task PostReturnsCreatedId()
        {
            var id = Guid.NewGuid();
            var addWeatherForecastCommand = new AddWeatherForecastCommand
            {
                City = "Australia/Melbourne",
                ForecastDate = new DateTimeOffset(2020, 1, 2, 3, 5, 1, TimeSpan.Zero),
                Forecast = 12.23m
            };
            var expectedWeatherForecastResponse = new AddWeatherForecastResponse(id);
            var stubMediator = new Mock<IMediator>();
            stubMediator
                .Setup(x => x.Send(addWeatherForecastCommand, It.IsAny<CancellationToken>()))
                .ReturnsAsync(expectedWeatherForecastResponse);

            var sut = new WeatherForecastsController(stubMediator.Object);
            var result = await sut.Post(addWeatherForecastCommand);

            var actualWeatherForecastResponse = (AddWeatherForecastResponse) ((OkObjectResult) result).Value;
            Assert.Equal(id, actualWeatherForecastResponse.Id);
        }
    }
}