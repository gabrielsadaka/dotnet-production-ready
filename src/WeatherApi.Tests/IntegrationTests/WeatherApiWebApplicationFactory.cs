using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using WeatherApi.Data;
using WeatherApi.Data.Entities;

namespace WeatherApi.Tests.IntegrationTests
{
    public class WeatherApiWebApplicationFactory<TStartup>
        : WebApplicationFactory<TStartup> where TStartup : class
    {
        protected override IHostBuilder CreateHostBuilder()
        {
            return base.CreateHostBuilder()
                .UseEnvironment(Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "local");
        }

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            if (builder == null) throw new ArgumentNullException(nameof(builder));
            builder.ConfigureServices(services =>
            {
                var sp = services.BuildServiceProvider();

                using var scope = sp.CreateScope();

                var scopedServices = scope.ServiceProvider;
                var db = scopedServices.GetRequiredService<WeatherContext>();
                var logger = scopedServices
                    .GetRequiredService<ILogger<WeatherApiWebApplicationFactory<TStartup>>>();

                try
                {
                    InitializeDbForTests(db);
                }
#pragma warning disable CA1031
                catch (Exception ex)
#pragma warning restore CA1031
                {
                    logger.LogError(ex, "An error occurred seeding the " +
                                        "database with test messages. Error: {Message}", ex.Message);
                }
            });
        }

        private static void InitializeDbForTests(WeatherContext db)
        {
            db.WeatherForecasts.RemoveRange(db.WeatherForecasts);

            db.SaveChanges();

            db.WeatherForecasts.Add(new WeatherForecast(Guid.NewGuid(), "Australia/Melbourne",
                new DateTimeOffset(2020, 01, 02, 0, 0, 0, TimeSpan.Zero), 23.35m));

            db.SaveChanges();
        }
    }
}