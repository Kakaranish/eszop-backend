using Common.Utilities.Extensions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NotificationService.Infrastructure.DataAccess;
using System.Text.Json;

namespace NotificationService.API.Extensions
{
    public static class StartupExtensions
    {
        public static IServiceCollection ConfigureDbContext(this IServiceCollection services, IWebHostEnvironment webHostEnvironment)
        {
            var connectionString = services.GetSqlServerConnectionString();

            var dbContextScope = webHostEnvironment.IsCustomDevelopment()
                ? ServiceLifetime.Scoped
                : ServiceLifetime.Scoped;

            services.AddDbContext<AppDbContext>(builder =>
                {
                    builder
                        .UseSqlServer(connectionString)
                        .UseLoggerFactory(LoggerFactory.Create(loggingBuilder => loggingBuilder.AddDebug()));
                },
                contextLifetime: dbContextScope,
                optionsLifetime: dbContextScope
            );

            return services;
        }

        public static IServiceCollection ConfigureSignalR(this IServiceCollection services)
        {
            using var servicesProvider = services.BuildServiceProvider();
            var configuration = servicesProvider.GetRequiredService<IConfiguration>();

            var signalRBuilder = services.AddSignalR()
                .AddJsonProtocol(options => options.PayloadSerializerOptions.DictionaryKeyPolicy = JsonNamingPolicy.CamelCase);

            if (configuration.GetValue<bool>("IsScaledOutService"))
            {
                var redisConnectionString = configuration.GetRedisConnectionString();
                signalRBuilder.AddStackExchangeRedis(redisConnectionString);
            }

            return services;
        }

        public static IServiceCollection ConfigureHealthchecks(this IServiceCollection services)
        {
            using var servicesProvider = services.BuildServiceProvider();
            var configuration = servicesProvider.GetRequiredService<IConfiguration>();

            var sqlServerConnectionStr = configuration.GetSqlServerConnectionString();
            var healthcheckBuilder = services.AddHealthChecks()
                .AddSqlServer(sqlServerConnectionStr);

            if (configuration.GetValue<bool>("IsScaledOutService"))
            {
                var redisConnectionStr = configuration.GetRedisConnectionString();
                healthcheckBuilder.AddRedis(redisConnectionStr);
            }

            return services;
        }
    }
}
