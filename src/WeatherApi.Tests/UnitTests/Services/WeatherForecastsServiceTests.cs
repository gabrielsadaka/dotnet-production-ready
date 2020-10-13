using WeatherApi.Services;
using Xunit;

namespace WeatherApi.Tests.UnitTests.Services
{
    public class WeatherForecastsServiceTests
    {
        [Fact]
        public void GetWeatherForecastReturnsNonNullForecast()
        {
            var sut = new WeatherForecastsService();
            var weatherForecast = sut.GetWeatherForecast();
            Assert.NotNull(weatherForecast);
        }
    }
}