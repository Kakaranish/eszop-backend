using System;
using System.Linq;
using Common.ErrorHandling;
using Common.Types;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Common.Extensions
{
    public static class StartupExtensions
    {
        private static readonly string[] DevelopmentEnvironments = {
            "Development",
            "DevelopmentLocal"
        };

        public static IServiceCollection ConfigureUrls(this IServiceCollection services)
        {
            using var servicesProvider = services.BuildServiceProvider();
            var configuration = servicesProvider.GetRequiredService<IConfiguration>();

            services.Configure<UrlsConfig>(configuration.GetSection("Urls"));
            
            return services;
        }

        public static IServiceCollection ConfigureServicesEndpoints(this IServiceCollection services)
        {
            using var servicesProvider = services.BuildServiceProvider();
            var configuration = servicesProvider.GetRequiredService<IConfiguration>();

            services.Configure<ServicesEndpointsConfig>(configuration.GetSection("Endpoints"));

            return services;
        }

        public static bool IsCustomDevelopment(this IWebHostEnvironment env)
        {
            return DevelopmentEnvironments.Contains(env.EnvironmentName);
        }

        public static IServiceCollection AddExceptionHandling<TDomainException>(this IServiceCollection services) where TDomainException : Exception
        {
            var serviceProvider = services.BuildServiceProvider();
            var loggerFactory = serviceProvider.GetRequiredService<ILoggerFactory>();
            var exceptionHandler = new ExceptionHandler<TDomainException>(loggerFactory);

            services.AddSingleton(exceptionHandler);

            return services;
        }
    }
}
