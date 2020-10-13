using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WeatherApi.Controllers;
using Xunit;

namespace WeatherApi.Tests.UnitTests.Controllers
{
    public class WeatherForecastsControllerTests
    {
        [Fact]
        public async Task GetReturnsOkResult()
        {
            var sut = new WeatherForecastsController();
            var result = await sut.Get();

            Assert.IsType<OkResult>(result);
        }
    }
}