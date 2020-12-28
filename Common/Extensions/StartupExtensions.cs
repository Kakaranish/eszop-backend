using System.Linq;
using Common.Types;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Common.Extensions
{
    public static class StartupExtensions
    {
        private static readonly string[] DevelopmentEnvironments = {
            "Development",
            "DevelopmentLocal"
        };

        public static IServiceCollection AddLocalhostCorsPolicy(this IServiceCollection services)
        {
            services.AddCors(corsOptions =>
            {
                corsOptions.AddPolicy("LocalhostCorsPolicy", builder =>
                {
                    builder.AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader();
                });
            });

            return services;
        }

        public static IServiceCollection ConfigureUrls(this IServiceCollection services)
        {
            using var servicesProvider = services.BuildServiceProvider();
            var configuration = servicesProvider.GetRequiredService<IConfiguration>();

            services.Configure<UrlsConfig>(configuration.GetSection("Urls"));
            
            return services;
        }
        
        public static bool IsCustomDevelopment(this IWebHostEnvironment env)
        {
            return DevelopmentEnvironments.Contains(env.EnvironmentName);
        }
    }
}
