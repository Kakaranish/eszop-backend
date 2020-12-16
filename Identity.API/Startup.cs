using Common.Authentication;
using Common.Types;
using Common.Types.ErrorHandling.CustomFluentValidation;
using FluentValidation.AspNetCore;
using Identity.API.DataAccess;
using Identity.API.Extensions;
using Identity.API.HealthCheck;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using System.Collections.Generic;

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
            services.AddControllers(options =>
                {
                    options.Filters.Add<CustomFluentValidationFailureActionFilter>();
                })
                .ConfigureApiBehaviorOptions(options =>
                {
                    options.SuppressModelStateInvalidFilter = true;
                })
                .AddFluentValidation(configuration =>
                {
                    configuration.RegisterValidatorsFromAssemblyContaining<Startup>();
                });

            services.AddHttpContextAccessor();

            services.AddCors(corsOptions =>
            {
                corsOptions.AddPolicy("LocalhostCorsPolicy", builder =>
                {
                    builder.AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader();
                });
            });

            services.Configure<JwtConfig>(Configuration.GetSection("JwtConfig"));
            services.AddJwtAuthentication();

            var connectionString = Configuration.GetConnectionString("SqlServer");
            services.AddDbContext<AppDbContext>(builder => builder.UseSqlServer(connectionString));

            services.AddHealthChecks()
                .AddCheck(
                    name: "SqlServerCheck",
                    instance: new SqlConnectionHealthCheck(connectionString),
                    failureStatus: HealthStatus.Unhealthy);

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
