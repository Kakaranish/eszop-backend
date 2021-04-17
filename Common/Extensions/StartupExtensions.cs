using Common.Domain.EventDispatching;
using Common.ErrorHandling;
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
            "DevHostLocal",
            "DevHostCloud",
            "DevDockerLocal",
            "DevDockerCloud"
        };

        public static string GetSqlServerConnectionString(this IServiceCollection services)
        {
            using var servicesProvider = services.BuildServiceProvider();
            var configuration = servicesProvider.GetRequiredService<IConfiguration>();

            return configuration.GetSqlServerConnectionString();
        }

        public static string GetSqlServerConnectionString(this IConfiguration configuration)
        {
            const string connStrEnvVarName = "ESZOP_SQLSERVER_CONN_STR";
            var connectionStr = Environment.GetEnvironmentVariable(connStrEnvVarName);
            if (string.IsNullOrWhiteSpace(connectionStr))
                connectionStr = configuration.GetConnectionString("SqlServer");
            if (string.IsNullOrWhiteSpace(connectionStr))
                throw new InvalidOperationException("Connection string provided neither in env variable nor appsettings");

            return connectionStr;
        }

        public static string GetRedisConnectionString(this IServiceCollection services)
        {
            var serviceProvider = services.BuildServiceProvider();
            var configuration = serviceProvider.GetRequiredService<IConfiguration>();

            return configuration.GetRedisConnectionString();
        }

        public static string GetRedisConnectionString(this IConfiguration configuration)
        {
            const string connStrEnvVarName = "ESZOP_REDIS_CONN_STR";
            var connectionStr = Environment.GetEnvironmentVariable(connStrEnvVarName);
            if (string.IsNullOrWhiteSpace(connectionStr))
                connectionStr = configuration.GetConnectionString("Redis");
            if (string.IsNullOrWhiteSpace(connectionStr))
                throw new InvalidOperationException("Connection string provided neither in env variable nor appsettings");

            return connectionStr;
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
