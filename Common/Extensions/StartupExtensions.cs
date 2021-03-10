using Common.ErrorHandling;
using Common.EventBus;
using Common.Types;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;

namespace Common.Extensions
{
    public static class StartupExtensions
    {
        private static readonly string[] DevelopmentEnvironments = {
            "Development",
            "DevelopmentLocal"
        };

        public static IServiceCollection ReadServicesEndpoints(this IServiceCollection services)
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

        public static IServiceCollection AddEventDispatching(this IServiceCollection services)
        {
            services.AddEventDispatching<DefaultEventReducer>();

            return services;
        }
        public static IServiceCollection AddEventDispatching<TReducer>(this IServiceCollection services) where TReducer : class, IEventReducer
        {
            services.AddScoped<IEventDispatcher, EventDispatcher>();
            services.AddScoped<IEventReducer, TReducer>();

            return services;
        }
    }
}
