using System;
using Google.Cloud.SecretManager.V1;
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
            var databasePasswordSecret = GetDatabasePasswordSecret(dbSettings);
            var connectionString = new NpgsqlConnectionStringBuilder
            {
                Host = !string.IsNullOrEmpty(dbSocketDir)
                    ? $"{dbSocketDir}/{instanceConnectionName}"
                    : dbSettings["HOST"],
                Username = dbSettings["USER"],
                Password = databasePasswordSecret,
                Database = dbSettings["NAME"],
                SslMode = SslMode.Disable,
                Pooling = true
            };

            services.AddDbContext<WeatherContext>(options =>
                options
                    .UseNpgsql(connectionString.ToString())
                    .UseSnakeCaseNamingConvention());
        }

        private static string GetDatabasePasswordSecret(IConfiguration dbSettings)
        {
            var googleProject = Environment.GetEnvironmentVariable("GOOGLE_PROJECT");

            if (string.IsNullOrEmpty(googleProject)) return dbSettings["PASSWORD"];

            var dbPasswordSecretId = dbSettings["PASSWORD_SECRET_ID"];
            var dbPasswordSecretVersion = dbSettings["PASSWORD_SECRET_VERSION"];

            var client = SecretManagerServiceClient.Create();

            var secretVersionName = new SecretVersionName(googleProject, dbPasswordSecretId, dbPasswordSecretVersion);

            var result = client.AccessSecretVersion(secretVersionName);

            return result.Payload.Data.ToStringUtf8();
        }
    }
}