using Common.Types;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Common.Extensions
{
    public static class StartupExtensions
    {
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
    }
}
