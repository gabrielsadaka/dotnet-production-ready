using System;
using WeatherApi.Domain.Handlers.WeatherForecasts.AddWeatherForecast;
using Xunit;

namespace WeatherApi.Tests.UnitTests.Domain.Handlers.WeatherForecasts.AddWeatherForecast
{
    public class AddWeatherForecastResponseTests
    {
        [Fact]
        public void ConstructorReturnsAddWeatherForecastResponse()
        {
            var id = Guid.NewGuid();

            var sut = new AddWeatherForecastResponse(id);

            Assert.Equal(id, sut.Id);
        }
    }
}