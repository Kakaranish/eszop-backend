using System;
using System.IO;
using System.Reflection;
using Common.Utilities.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Common.Utilities.ImageStorage
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
            else ConfigureForLocalBlobStorage(services, configuration);

            return services;
        }

        private static void ConfigureForAzureBlobStorage(IServiceCollection services, IConfiguration configuration)
        {
            const string connStrEnvVarName = "ESZOP_AZURE_STORAGE_CONN_STR";
            const string appsettingsPath = "AzureStorage:ConnectionString";
            var connectionStr = configuration.GetRequiredConfigValue(connStrEnvVarName, appsettingsPath);

            var containerName = configuration.GetValue<string>("AzureStorage:ContainerName");
            services.Configure<AzureStorageConfig>(config =>
            {
                config.ConnectionString = connectionStr;
                config.ContainerName = containerName;
            });

            services.AddSingleton<IBlobStorage, AzureBlobStorage>();
        }

        private static void ConfigureForLocalBlobStorage(IServiceCollection services, IConfiguration configuration)
        {
            const string envVarName = "ESZOP_LOCAL_BLOB_STORAGE_WWWROOT_DIR";
            const string appsettingsPath = "LocalBlobStorageWwwRootDir";

            var uploadDir = Environment.GetEnvironmentVariable(envVarName);
            if (string.IsNullOrWhiteSpace(uploadDir))
                uploadDir = configuration.GetValue<string>(appsettingsPath);
            if (string.IsNullOrWhiteSpace(uploadDir))
                uploadDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

            services.AddSingleton<IBlobStorage>(new LocalBlobStorage(uploadDir));
        }
    }
}
