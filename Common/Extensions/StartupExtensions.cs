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
    }
}
