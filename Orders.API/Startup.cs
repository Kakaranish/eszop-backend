using Common.Authentication;
using Common.Extensions;
using Common.ServiceBus;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Orders.API
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
            services.AddHttpContextAccessor();
            services.AddLocalhostCorsPolicy();

            services.ConfigureUrls();

            services.AddJwtAuthentication();
            services.AddMediatR(typeof(Startup).Assembly);

            services.AddHealthChecks();

            services.AddRabbitMqEventBus();
            AddSubscriptions(services);
        }

        public void AddSubscriptions(IServiceCollection services)
        {
            using var serviceProvider = services.BuildServiceProvider();
            var eventBus = serviceProvider.GetRequiredService<IEventBus>();

            // PLACEHOLDER FOR SUBS
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsCustomDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseCors("LocalhostCorsPolicy");
            }

            app.UseExceptionHandler("/error");

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();
            app.UseAuthentication();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapHealthChecks("/healthcheck");
                endpoints.MapControllers();
            });
        }
    }
}
