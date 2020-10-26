using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;
using WeatherApi.Data;
using WeatherApi.Data.Repositories;

namespace WeatherApi.Infrastructure.ServiceCollectionExtensions
{
    public static class DataAccessExtensions
    {
        public static void ConfigureDataAccess(this IServiceCollection services, IConfiguration configuration)
        {
            if (configuration == null) throw new ArgumentNullException(nameof(configuration));

            ConfigureDbContext(services, configuration);

            services.AddScoped<IWeatherForecastsRepository, WeatherForecastsRepository>();
        }

        private static void ConfigureDbContext(IServiceCollection services, IConfiguration configuration)
        {
            var dbSettings = configuration.GetSection("WEATHERDB");
            var dbSocketDir = dbSettings["SOCKET_PATH"];
            var instanceConnectionName = dbSettings["INSTANCE_CONNECTION_NAME"];
            var connectionString = new NpgsqlConnectionStringBuilder
            {
                Host = !string.IsNullOrEmpty(dbSocketDir)
                    ? $"{dbSocketDir}/{instanceConnectionName}"
                    : dbSettings["HOST"],
                Username = dbSettings["USER"],
                Password = dbSettings["PASSWORD"],
                Database = dbSettings["NAME"],
                SslMode = SslMode.Disable,
                Pooling = true
            };

            services.AddDbContext<WeatherContext>(options =>
                options
                    .UseNpgsql(connectionString.ToString())
                    .UseSnakeCaseNamingConvention());
        }
    }
}