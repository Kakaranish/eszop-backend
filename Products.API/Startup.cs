using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Products.API.DataAccess;
using Products.API.DataAccess.Repositories;
using System.Linq;
using Common.Authentication;
using MediatR;

namespace Products.API
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
            
            services.AddCors(corsOptions =>
            {
                corsOptions.AddPolicy("LocalhostCorsPolicy", builder =>
                {
                    builder.AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader();
                });
            });

            services.AddJwtAuthentication();
            services.AddHttpContextAccessor();
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
