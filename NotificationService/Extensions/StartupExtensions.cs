using Common.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Text.Json;

namespace NotificationService.Extensions
{
    public static class StartupExtensions
    {
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
    }
}
