using System.Threading;
using System.Threading.Tasks;
using MediatR;
using WeatherApi.Data.Repositories;
using WeatherApi.Exceptions;

namespace WeatherApi.Domain.Handlers.WeatherForecasts.GetWeatherForecast
{
    public class GetWeatherForecastQueryHandler : IRequestHandler<GetWeatherForecastQuery, GetWeatherForecastResponse>
    {
        private readonly IWeatherForecastsRepository _weatherForecastsRepository;

        public GetWeatherForecastQueryHandler(IWeatherForecastsRepository weatherForecastsRepository)
        {
            _weatherForecastsRepository = weatherForecastsRepository;
        }

        public async Task<GetWeatherForecastResponse> Handle(GetWeatherForecastQuery request,
            CancellationToken cancellationToken)
        {
            var weatherForecast =
                await _weatherForecastsRepository.GetWeatherForecast(request.City, request.ForecastDate,
                    cancellationToken);

            if (weatherForecast == null) throw new NotFoundException();

            return new GetWeatherForecastResponse(weatherForecast.Id, weatherForecast.City,
                weatherForecast.ForecastDate.UtcDateTime,
                weatherForecast.Forecast);
        }
    }
}