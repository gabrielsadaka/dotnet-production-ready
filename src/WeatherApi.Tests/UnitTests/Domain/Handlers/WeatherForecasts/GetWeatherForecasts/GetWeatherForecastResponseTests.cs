using System;
using WeatherApi.Domain.Handlers.WeatherForecasts.GetWeatherForecast;
using Xunit;

namespace WeatherApi.Tests.UnitTests.Domain.Handlers.WeatherForecasts.GetWeatherForecasts
{
    public class GetWeatherForecastResponseTests
    {
        [Fact]
        public void ConstructorReturnsGetWeatherForecastResponse()
        {
            var id = Guid.NewGuid();
            const string city = "Australia/Melbourne";
            var forecastDate = new DateTimeOffset(2020, 1, 2, 3, 5, 1, TimeSpan.Zero);
            const decimal forecast = 12.43m;

            var sut = new GetWeatherForecastResponse(id, city, forecastDate, forecast);

            Assert.Equal(id, sut.Id);
            Assert.Equal(city, sut.City);
            Assert.Equal(forecastDate, sut.ForecastDate);
            Assert.Equal(forecast, sut.Forecast);
        }
    }
}