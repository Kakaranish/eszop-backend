using API.Gateway.Configuration;
using API.Gateway.Middlewares;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Serilog;

namespace API.Gateway
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddGlobalCors();

            services.AddOcelot(Configuration);
            services.AddSwaggerForOcelot(Configuration);

            services.AddHealthChecks()
                .AddCheck("HealthCheck", () => HealthCheckResult.Healthy());
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseSerilogRequestLogging();
            app.UseCors();
            app.UseCookieTokenMiddleware();
            app.UseWebSockets();

            app.UseRouting();
            app.UseEndpoints(endpoints => endpoints.MapHealthChecks("/healthcheck"));

            app.UseSwaggerForOcelotUI();
            app.UseOcelot().Wait();
        }
    }
}
