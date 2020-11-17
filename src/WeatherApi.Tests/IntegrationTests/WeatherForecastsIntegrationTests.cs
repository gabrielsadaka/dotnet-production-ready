using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.WebUtilities;
using WeatherApi.Tests.Extensions;
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
            using var response = await client.GetAsync(new Uri(url, UriKind.Relative));

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
            using var response = await client.GetAsync(new Uri(url, UriKind.Relative));
            var responseContent = await response.Content.ReadAsStringAsync();
            var responseObj = JsonSerializer.Deserialize<object>(responseContent) as JsonElement?;

            Assert.Equal(city, responseObj?.GetProperty("city").ToString());
            Assert.Equal(forecastDate, responseObj?.GetProperty("forecastDate").ToString());
            Assert.Equal(23.35m,
                decimal.Parse(responseObj?.GetProperty("forecast").ToString()!, CultureInfo.InvariantCulture));
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
            using var response = await client.GetAsync(new Uri(url, UriKind.Relative));

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task PostReturnsSuccess()
        {
            const string city = "Australia/Melbourne";
            const string forecastDate = "2020-02-05T00:00:00.000Z";
            const decimal forecast = 23.35m;
            var body = new
            {
                city,
                forecastDate,
                forecast
            };

            using var client = _factory.CreateClient();
            using var jsonBody = body.ToJsonStringContent();
            using var response = await client.PostAsync(new Uri("/api/weather-forecasts", UriKind.Relative), jsonBody);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task PostReturnsIdOfNewWeatherForecast()
        {
            const string city = "Australia/Melbourne";
            const string forecastDate = "2020-04-22T00:00:00.000Z";
            const decimal forecast = 23.35m;
            var body = new
            {
                city,
                forecastDate,
                forecast
            };

            using var client = _factory.CreateClient();
            using var jsonBody = body.ToJsonStringContent();
            using var response = await client.PostAsync(new Uri("/api/weather-forecasts", UriKind.Relative), jsonBody);
            var responseContent = await response.Content.ReadAsStringAsync();
            var responseObj = JsonSerializer.Deserialize<object>(responseContent) as JsonElement?;

            var id = responseObj?.GetProperty("id").ToString();
            Assert.NotNull(id);
            Assert.True(Guid.Parse(id!) != Guid.Empty);
        }
    }
}