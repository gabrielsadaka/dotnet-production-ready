using Hellang.Middleware.ProblemDetails;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using WeatherApi.Data;
using WeatherApi.Data.Repositories;
using WeatherApi.Exceptions;
using WeatherApi.Services;

namespace WeatherApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IWebHostEnvironment environment)
        {
            Configuration = configuration;
            Environment = environment;
        }

        public IConfiguration Configuration { get; }
        public IWebHostEnvironment Environment { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            services.AddScoped<IWeatherForecastsRepository, WeatherForecastsRepository>();
            services.AddScoped<IWeatherForecastsService, WeatherForecastsService>();

            var dbSettings = Configuration.GetSection("WEATHERDB");
            services.AddDbContext<WeatherContext>(options =>
                options
                    .UseNpgsql(
                        $"Host={dbSettings["HOST"]};Database={dbSettings["DB"]};Username={dbSettings["USER"]};Password={dbSettings["PASSWORD"]}")
                    .UseSnakeCaseNamingConvention());

            services.AddProblemDetails(opts =>
            {
                opts.ShouldLogUnhandledException = (ctx, ex, pb) => Environment.IsDevelopment();
                opts.IncludeExceptionDetails = (ctx, ex) => Environment.IsDevelopment();
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