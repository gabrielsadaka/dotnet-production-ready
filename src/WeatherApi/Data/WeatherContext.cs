using Microsoft.EntityFrameworkCore;
using WeatherApi.Data.Entities;

namespace WeatherApi.Data
{
    public class WeatherContext : DbContext
    {
        public WeatherContext(DbContextOptions<WeatherContext> dbContextOptions)
            : base(dbContextOptions)
        {
        }

        public DbSet<WeatherForecast> WeatherForecasts { get; set; }
    }
}