using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Identity.API.Extensions
{
    public static class StartupExtensions
    {
        public static string GetRedisConnectionString(this IServiceCollection services)
        {
            var serviceProvider = services.BuildServiceProvider();
            var configuration = serviceProvider.GetRequiredService<IConfiguration>();

            const string connStrEnvVarName = "ESZOP_REDIS_CONN_STR";
            var connectionStr = Environment.GetEnvironmentVariable(connStrEnvVarName);
            if (string.IsNullOrWhiteSpace(connectionStr))
                connectionStr = configuration.GetConnectionString("Redis");
            if (string.IsNullOrWhiteSpace(connectionStr))
                throw new InvalidOperationException("Connection string provided neither in env variable nor appsettings");

            return connectionStr;
        }
    }
}