using System;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using WeatherApi.Data.Entities;
using WeatherApi.Data.Repositories;
using WeatherApi.Domain.Handlers.WeatherForecasts.AddWeatherForecast;
using Xunit;

namespace WeatherApi.Tests.UnitTests.Domain.Handlers.WeatherForecasts.AddWeatherForecast
{
    public class AddWeatherForecastCommandHandlerTests
    {
        [Fact]
        public async Task AddWeatherForecastSavesForecastSuccessfully()
        {
            var addWeatherForecastCommand = new AddWeatherForecastCommand
            {
                City = "Australia/Melbourne",
                ForecastDate = new DateTimeOffset(2020, 1, 3, 3, 2, 4, TimeSpan.Zero),
                Forecast = 10.23m
            };
            var mockWeatherForecastsRepository = new Mock<IWeatherForecastsRepository>();

            var sut = new AddWeatherForecastCommandHandler(mockWeatherForecastsRepository.Object);
            var addWeatherForecastResponse = await sut.Handle(addWeatherForecastCommand, CancellationToken.None);

            mockWeatherForecastsRepository
                .Verify(x => x.AddWeatherForecast(It.IsAny<WeatherForecast>(), It.IsAny<CancellationToken>()));
            Assert.True(addWeatherForecastResponse.Id != default);
        }
    }
}