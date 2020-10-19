using System.Collections.Generic;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.WebUtilities;
using Xunit;

namespace WeatherApi.Tests.IntegrationTests
{
    public class WeatherForecastsIntegrationTests
        : IClassFixture<WeatherApiWebApplicationFactory<Startup>>
    {
        private readonly WeatherApiWebApplicationFactory<Startup> _factory;

        public WeatherForecastsIntegrationTests(WeatherApiWebApplicationFactory<Startup> factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task GetReturnsSuccess()
        {
            const string city = "Australia/Melbourne";
            const string forecastDate = "2020-01-02T00:00:00.000Z";
            var url = QueryHelpers.AddQueryString("/api/weather-forecasts", new Dictionary<string, string>
            {
                {"city", city},
                {"forecastDate", forecastDate}
            });

            using var client = _factory.CreateClient();
            using var response = await client.GetAsync(url);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task GetReturnsCorrectWeather()
        {
            const string city = "Australia/Melbourne";
            const string forecastDate = "2020-01-02T00:00:00+00:00";
            var url = QueryHelpers.AddQueryString("/api/weather-forecasts", new Dictionary<string, string>
            {
                {"city", city},
                {"forecastDate", forecastDate}
            });

            using var client = _factory.CreateClient();
            using var response = await client.GetAsync(url);
            var responseContent = await response.Content.ReadAsStringAsync();
            var responseObj = JsonSerializer.Deserialize<object>(responseContent) as JsonElement?;

            Assert.Equal(city, responseObj?.GetProperty("city").ToString());
            Assert.Equal(forecastDate, responseObj?.GetProperty("forecastDate").ToString());
            Assert.Equal(23.35m, decimal.Parse(responseObj?.GetProperty("forecast").ToString()!));
        }

        [Fact]
        public async Task GetWhenWeatherForecastDoesNotExistReturnsNotFoundStatus()
        {
            const string city = "Australia/FakeCity";
            const string forecastDate = "2020-01-02T00:00:00+00:00";
            var url = QueryHelpers.AddQueryString("/api/weather-forecasts", new Dictionary<string, string>
            {
                {"city", city},
                {"forecastDate", forecastDate}
            });

            using var client = _factory.CreateClient();
            using var response = await client.GetAsync(url);

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }
    }
}