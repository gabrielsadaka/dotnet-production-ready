using System;
using System.Linq;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using WeatherApi.Data;
using WeatherApi.Data.Entities;

namespace WeatherApi.Tests.IntegrationTests
{
    public class WeatherApiWebApplicationFactory<TStartup>
        : WebApplicationFactory<TStartup> where TStartup : class
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                var descriptor = services.SingleOrDefault(
                    d => d.ServiceType ==
                         typeof(DbContextOptions<WeatherContext>));

                services.Remove(descriptor);

                services.AddDbContext<WeatherContext>(options => { options.UseInMemoryDatabase("weather-db"); });

                var sp = services.BuildServiceProvider();

                using (var scope = sp.CreateScope())
                {
                    var scopedServices = scope.ServiceProvider;
                    var db = scopedServices.GetRequiredService<WeatherContext>();
                    var logger = scopedServices
                        .GetRequiredService<ILogger<WeatherApiWebApplicationFactory<TStartup>>>();

                    db.Database.EnsureCreated();

                    try
                    {
                        InitializeDbForTests(db);
                    }
                    catch (Exception ex)
                    {
                        logger.LogError(ex, "An error occurred seeding the " +
                                            "database with test messages. Error: {Message}", ex.Message);
                    }
                }
            });
        }

        private void InitializeDbForTests(WeatherContext db)
        {
            db.WeatherForecasts.Add(new WeatherForecast
            {
                Id = Guid.NewGuid(),
                City = "Australia/Melbourne",
                ForecastDate = new DateTimeOffset(2020, 01, 02, 0, 0, 0, TimeSpan.Zero),
                Forecast = 23.35m
            });

            db.SaveChanges();
        }
    }
}