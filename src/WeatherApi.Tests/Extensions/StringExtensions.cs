using System.Net.Http;
using System.Net.Mime;
using System.Text;
using System.Text.Json;

namespace WeatherApi.Tests.Extensions
{
    public static class StringExtensions
    {
        public static StringContent ToJsonStringContent(this object body)
        {
            return new StringContent(JsonSerializer.Serialize(body), Encoding.UTF8, MediaTypeNames.Application.Json);
        }
    }
}