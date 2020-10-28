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
            var dbSettings = configuration.GetSection("WeatherDb");
            var dbSocketDir = dbSettings["SocketPath"];
            var instanceConnectionName = dbSettings["InstanceConnectionName"];
            var databasePasswordSecret = GetDatabasePasswordSecret(dbSettings);
            var connectionString = new NpgsqlConnectionStringBuilder
            {
                Host = !string.IsNullOrEmpty(dbSocketDir)
                    ? $"{dbSocketDir}/{instanceConnectionName}"
                    : dbSettings["Host"],
                Username = dbSettings["User"],
                Password = databasePasswordSecret,
                Database = dbSettings["Name"],
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

            if (string.IsNullOrEmpty(googleProject)) return dbSettings["Password"];

            var dbPasswordSecretId = dbSettings["PasswordSecretId"];
            var dbPasswordSecretVersion = dbSettings["PasswordSecretVersion"];

            var client = SecretManagerServiceClient.Create();

            var secretVersionName = new SecretVersionName(googleProject, dbPasswordSecretId, dbPasswordSecretVersion);

            var result = client.AccessSecretVersion(secretVersionName);

            return result.Payload.Data.ToStringUtf8();
        }
    }
}