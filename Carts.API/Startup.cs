using System.Collections;
using Carts.API.DataAccess;
using Carts.API.DataAccess.Repositories;
using Common.Authentication;
using Common.Extensions;
using Common.HealthCheck;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Carts.API
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

            services.AddJwtAuthentication();
            services.AddMediatR(typeof(Startup).Assembly);

            var connectionString = Configuration.GetConnectionString("SqlServer");
            services.AddDbContext<AppDbContext>(builder => builder.UseSqlServer(connectionString));
            services.AddHealthChecks()
                .AddCheck(
                    name: "SqlServerCheck",
                    instance: new SqlConnectionHealthCheck(connectionString),
                    failureStatus: HealthStatus.Unhealthy);

            services.AddScoped<ICartRepository, CartRepository>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            var developmentEnvironments = new[]
            {
                "Development",
                "DevelopmentLocal"
            };

            if (((IList)developmentEnvironments).Contains(env.EnvironmentName))
            {
                app.UseDeveloperExceptionPage();
                app.UseCors("LocalhostCorsPolicy");
            }

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
