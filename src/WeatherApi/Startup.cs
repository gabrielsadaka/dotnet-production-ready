using Hellang.Middleware.ProblemDetails;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Npgsql;
using WeatherApi.Data;
using WeatherApi.Data.Repositories;
using WeatherApi.Exceptions;
using WeatherApi.Services;

namespace WeatherApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IWebHostEnvironment webHostEnvironment)
        {
            Configuration = configuration;
            WebHostEnvironment = webHostEnvironment;
        }

        public IConfiguration Configuration { get; }
        public IWebHostEnvironment WebHostEnvironment { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            using var loggerFactory = LoggerFactory.Create(builder =>
            {
                builder.AddConsole();
            });
            var logger = loggerFactory.CreateLogger("Startup");
            services.AddControllers();

            services.AddScoped<IWeatherForecastsRepository, WeatherForecastsRepository>();
            services.AddScoped<IWeatherForecastsService, WeatherForecastsService>();

            var dbSettings = Configuration.GetSection("WEATHERDB");
            var dbSocketDir = dbSettings["SocketPath"];
            var instanceConnectionName = dbSettings["InstanceConnectionName"];
            var connectionString = new NpgsqlConnectionStringBuilder
            {
                Host = !string.IsNullOrEmpty(dbSocketDir)
                    ? $"{dbSocketDir}/{instanceConnectionName}"
                    : dbSettings["Host"],
                Username = dbSettings["User"],
                Password = dbSettings["Password"],
                Database = dbSettings["Name"],
                SslMode = SslMode.Disable,
                Pooling = true
            };
            logger.LogInformation($"Connecting to {connectionString.Host}");

            services.AddDbContext<WeatherContext>(options =>
                options
                    .UseNpgsql(connectionString.ToString())
                    .UseSnakeCaseNamingConvention());

            services.AddProblemDetails(opts =>
            {
                opts.ShouldLogUnhandledException = (ctx, ex, pb) => WebHostEnvironment.IsDevelopment();
                opts.IncludeExceptionDetails = (ctx, ex) => WebHostEnvironment.IsDevelopment();
                opts.MapToStatusCode<NotFoundException>(StatusCodes.Status404NotFound);
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public static void Configure(IApplicationBuilder app)
        {
            app.UseProblemDetails();

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}