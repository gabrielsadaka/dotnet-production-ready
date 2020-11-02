using System;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using WeatherApi.Data.Entities;
using WeatherApi.Data.Repositories;
using WeatherApi.Domain.Handlers.WeatherForecasts.GetWeatherForecast;
using WeatherApi.Exceptions;
using Xunit;

namespace WeatherApi.Tests.UnitTests.Domain.Handlers.WeatherForecasts.GetWeatherForecasts
{
    public class GetWeatherForecastQueryHandlerTests
    {
        [Fact]
        public async Task HandleReturnsCorrectForecast()
        {
            const string city = "Australia/Melbourne";
            var forecastDate = new DateTimeOffset(2020, 1, 3, 3, 2, 4, TimeSpan.Zero);
            var expectedWeatherForecast = new WeatherForecast(Guid.NewGuid(), city, forecastDate, 10.23m);
            var getWeatherForecastQuery = new GetWeatherForecastQuery
            {
                City = city,
                ForecastDate = forecastDate
            };

            var stubWeatherForecastsRepository = new Mock<IWeatherForecastsRepository>();
            stubWeatherForecastsRepository
                .Setup(x => x.GetWeatherForecast(city, forecastDate, It.IsAny<CancellationToken>()))
                .ReturnsAsync(expectedWeatherForecast);

            var sut = new GetWeatherForecastQueryHandler(stubWeatherForecastsRepository.Object);
            var actualWeatherForecast = await sut.Handle(getWeatherForecastQuery, CancellationToken.None);

            Assert.Equal(city, actualWeatherForecast.City);
            Assert.Equal(forecastDate, actualWeatherForecast.ForecastDate);
        }

        [Fact]
        public async Task HandleWhenForecastDoesNotExistThrowsNotFoundException()
        {
            const string city = "Australia/Melbourne";
            var forecastDate = new DateTimeOffset(2020, 1, 3, 3, 2, 4, TimeSpan.Zero);
            var getWeatherForecastQuery = new GetWeatherForecastQuery
            {
                City = city,
                ForecastDate = forecastDate
            };
            var stubWeatherForecastsRepository = new Mock<IWeatherForecastsRepository>();

            var sut = new GetWeatherForecastQueryHandler(stubWeatherForecastsRepository.Object);
            await Assert.ThrowsAsync<NotFoundException>(() =>
                sut.Handle(getWeatherForecastQuery, CancellationToken.None));
        }
    }
}