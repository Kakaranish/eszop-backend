using System.Linq;
using Common.Authentication;
using Common.Extensions;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Offers.API.DataAccess;
using Offers.API.DataAccess.Repositories;

namespace Offers.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddHttpContextAccessor();
            services.AddLocalhostCorsPolicy();

            services.AddJwtAuthentication();
            services.AddMediatR(typeof(Startup).Assembly);

            var connectionString = Configuration.GetConnectionString("SqlServer");
            services.AddDbContext<AppDbContext>(builder =>
                builder.UseSqlServer(connectionString));

            services.AddScoped<IOfferRepository, OfferRepository>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            var developmentEnvironments = new[]
            {
                "Development",
                "DevelopmentLocal"
            };

            if (developmentEnvironments.Contains(env.EnvironmentName))
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
                endpoints.MapControllers();
            });
        }
    }
}
