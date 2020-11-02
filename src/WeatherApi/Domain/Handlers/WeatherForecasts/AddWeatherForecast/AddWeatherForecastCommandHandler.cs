using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using WeatherApi.Data.Entities;
using WeatherApi.Data.Repositories;

namespace WeatherApi.Domain.Handlers.WeatherForecasts.AddWeatherForecast
{
    public class
        AddWeatherForecastCommandHandler : IRequestHandler<AddWeatherForecastCommand, AddWeatherForecastResponse>
    {
        private readonly IWeatherForecastsRepository _weatherForecastsRepository;

        public AddWeatherForecastCommandHandler(IWeatherForecastsRepository weatherForecastsRepository)
        {
            _weatherForecastsRepository = weatherForecastsRepository;
        }

        public async Task<AddWeatherForecastResponse> Handle(AddWeatherForecastCommand request,
            CancellationToken cancellationToken)
        {
            var id = Guid.NewGuid();

            await _weatherForecastsRepository.AddWeatherForecast(new WeatherForecast(id,
                request.City, request.ForecastDate, request.Forecast), cancellationToken);

            return new AddWeatherForecastResponse(id);
        }
    }
}