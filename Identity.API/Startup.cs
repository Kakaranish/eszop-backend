using Common.Authentication;
using Common.Extensions;
using Common.HealthCheck;
using Identity.API.DataAccess;
using Identity.API.Domain;
using Identity.API.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;

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
            services.AddControllers();
            services.AddHttpContextAccessor();
            services.AddLocalhostCorsPolicy();

            services.AddJwtAuthentication();
            services.Configure<JwtConfig>(Configuration.GetSection("JwtConfig"));

            var sqlServerConnectionString = Configuration.GetConnectionString("SqlServer");
            services.AddDbContext<AppDbContext>(builder => builder.UseSqlServer(sqlServerConnectionString));

            var redisConnectionString = Configuration.GetConnectionString("Redis");
            services.AddDistributedRedisCache(options => options.Configuration = redisConnectionString);

            services.AddHealthChecks()
                .AddCheck(
                    name: "SqlServerCheck",
                    instance: new SqlConnectionHealthCheck(sqlServerConnectionString),
                    failureStatus: HealthStatus.Unhealthy)
                .AddCheck(
                    name: "RedisCheck",
                    instance: new RedisConnectionHealthCheck(redisConnectionString),
                    failureStatus: HealthStatus.Unhealthy);

            services.AddExceptionHandling<IdentityDomainException>();

            services.AddFluentValidation();
            services.AddInternalServices();
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

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapHealthChecks("/healthcheck");
                endpoints.MapControllers();
            });
        }
    }
}
