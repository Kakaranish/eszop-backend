using Common.Authentication;
using Common.Types;
using Identity.API.DataAccess;
using Identity.API.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using System.Collections.Generic;
using Common.Extensions;
using Common.HealthCheck;

namespace Identity.API
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
            services.AddHttpContextAccessor();

            services.AddLocalhostCorsPolicy();
            
            services.Configure<JwtConfig>(Configuration.GetSection("JwtConfig"));
            services.AddJwtAuthentication();

            var sqlServerConnectionString = Configuration.GetConnectionString("SqlServer");
            services.AddDbContext<AppDbContext>(builder => builder.UseSqlServer(sqlServerConnectionString));

            var redisConnectionString = Configuration.GetConnectionString("Redis");
            services.AddDistributedRedisCache(options =>
            {
                options.Configuration = redisConnectionString;
            });

            services.AddHealthChecks()
                .AddCheck(
                    name: "SqlServerCheck",
                    instance: new SqlConnectionHealthCheck(sqlServerConnectionString),
                    failureStatus: HealthStatus.Unhealthy)
                .AddCheck(
                    name: "RedisCheck",
                    instance: new RedisConnectionHealthCheck(redisConnectionString),
                    failureStatus: HealthStatus.Unhealthy);

            services.AddFluentValidation();
            services.AddInternalServices();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (IsDevelopmentEnv(env))
            {
                app.UseDeveloperExceptionPage();
                app.UseCors("LocalhostCorsPolicy");
            }

            app.UseExceptionHandler(a => a.Run(async context =>
            {
                var exceptionHandlerPathFeature = context.Features.Get<IExceptionHandlerPathFeature>();
                var exception = exceptionHandlerPathFeature.Error;

                if (exception is DomainException)
                {
                    await context.Response.WriteAsJsonAsync(new
                    {
                        Error = exception.Message
                    });
                }
            }));

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapHealthChecks("/healthcheck");
                endpoints.MapControllers();
            });
        }

        private static bool IsDevelopmentEnv(IWebHostEnvironment env)
        {
            var developmentEnvironments = new List<string> { "Development", "DevelopmentLocal" };
            return developmentEnvironments.Contains(env.EnvironmentName);
        }
    }
}
