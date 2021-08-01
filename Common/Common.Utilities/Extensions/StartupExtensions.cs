using System;
using System.Linq;
using Common.Domain.EventDispatching;
using Common.Utilities.ErrorHandling;
using Common.Utilities.Types;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Common.Utilities.Extensions
{
    public static class StartupExtensions
    {
        private static readonly string[] DevelopmentEnvironments = {
            "DevHostLocal",
            "DevHostCloud",
            "DevDockerLocal",
            "DevDockerCloud"
        };

        public static string GetRequiredConfigValue(this IConfiguration configuration,
            string environmentVariableName, string appsettingsPath)
        {
            var configValue = Environment.GetEnvironmentVariable(environmentVariableName);
            if (string.IsNullOrWhiteSpace(configValue))
                configValue = configuration.GetValue<string>(appsettingsPath);
            if (string.IsNullOrWhiteSpace(configValue))
            {
                var errorMsg = "Configuration value provided neither in " +
                               $"environment variable ({environmentVariableName}) nor appsettings ({appsettingsPath})";
                throw new InvalidOperationException(errorMsg);
            }

            return configValue;
        }

        public static string GetSqlServerConnectionString(this IServiceCollection services)
        {
            using var servicesProvider = services.BuildServiceProvider();
            var configuration = servicesProvider.GetRequiredService<IConfiguration>();

            return configuration.GetSqlServerConnectionString();
        }

        public static string GetSqlServerConnectionString(this IConfiguration configuration)
        {
            return configuration.GetRequiredConfigValue("ESZOP_SQLSERVER_CONN_STR", "ConnectionStrings:SqlServer");
        }

        public static string GetRedisConnectionString(this IServiceCollection services)
        {
            var serviceProvider = services.BuildServiceProvider();
            var configuration = serviceProvider.GetRequiredService<IConfiguration>();

            return configuration.GetRedisConnectionString();
        }

        public static string GetRedisConnectionString(this IConfiguration configuration)
        {
            return configuration.GetRequiredConfigValue("ESZOP_REDIS_CONN_STR", "ConnectionStrings:Redis");
        }

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

        public static IServiceCollection AddEventDispatching<TReducer>(this IServiceCollection services) where TReducer : class, IEventReducer
        {
            services.AddScoped<IEventDispatcher, EventDispatcher>();
            services.AddScoped<IEventReducer, TReducer>();

            return services;
        }
    }
}
