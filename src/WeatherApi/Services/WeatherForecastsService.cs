using WeatherApi.Domain;

namespace WeatherApi.Services
{
    public class WeatherForecastsService
    {
        public WeatherForecast GetWeatherForecast()
        {
            return new WeatherForecast();
        }
    }
}