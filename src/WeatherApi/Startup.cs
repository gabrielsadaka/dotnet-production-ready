using Hellang.Middleware.ProblemDetails;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using WeatherApi.Exceptions;
using WeatherApi.Infrastructure.ServiceCollectionExtensions;
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
            services.AddControllers();

            services.AddScoped<IWeatherForecastsService, WeatherForecastsService>();

            services.ConfigureDataAccess(Configuration);

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