using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Common.ImageStorage
{
    public static class Extensions
    {
        public static IServiceCollection AddBlobStorage(this IServiceCollection services)
        {
            var serviceProvider = services.BuildServiceProvider();
            var configuration = serviceProvider.GetRequiredService<IConfiguration>();

            var useAzureBlobStorage = bool.TryParse(
                configuration.GetSection("UseAzureBlobStorage").Value, out var valueResult) && valueResult;

            if (useAzureBlobStorage) ConfigureForAzureBlobStorage(services, configuration);
            else services.AddSingleton<IBlobStorage, LocalBlobStorage>();

            return services;
        }

        private static void ConfigureForAzureBlobStorage(IServiceCollection services, IConfiguration configuration)
        {
            const string connStrEnvVarName = "ESZOP_AZURE_STORAGE_CONN_STR";
            
            var connectionStr = Environment.GetEnvironmentVariable(connStrEnvVarName);
            if (string.IsNullOrWhiteSpace(connectionStr))
                connectionStr = configuration.GetValue<string>("AzureStorage:ConnectionString");
            if (string.IsNullOrWhiteSpace(connectionStr))
                throw new InvalidOperationException("Connection string provided neither in env variable nor appsettings");

            var containerName = configuration.GetValue<string>("AzureStorage:ContainerName");

            services.Configure<AzureStorageConfig>(config =>
            {
                config.ConnectionString = connectionStr;
                config.ContainerName = containerName;
            });

            services.AddSingleton<IBlobStorage, AzureBlobStorage>();
        }
    }
}
