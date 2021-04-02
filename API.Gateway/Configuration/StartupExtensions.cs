using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace API.Gateway.Configuration
{
    public static class StartupExtensions
    {
        public static IServiceCollection AddGlobalCors(this IServiceCollection services)
        {
            var clientUrl = services.GetClientUrl();
            services.AddCors(options =>
            {
                options.AddDefaultPolicy(builder =>
                {
                    builder
                        .WithOrigins(clientUrl)
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowCredentials();
                });
            });

            return services;
        }

        private static string GetClientUrl(this IServiceCollection services)
        {
            var serviceProvider = services.BuildServiceProvider();
            var configuration = serviceProvider.GetRequiredService<IConfiguration>();

            const string connStrEnvVarName = "ESZOP_CLIENT_URI";
            var clientUri = Environment.GetEnvironmentVariable(connStrEnvVarName);
            if (string.IsNullOrWhiteSpace(clientUri))
                clientUri = configuration.GetValue<string>("ClientUrl");
            if (string.IsNullOrWhiteSpace(clientUri))
                throw new InvalidOperationException("ClientUrl provided neither in env variable nor appsettings");

            return clientUri;
        }
    }
}
