using Microsoft.Extensions.Configuration;
using Serilog;
using Serilog.Events;
using System;
using System.IO;
using System.Reflection;

namespace Common.Logging
{
    public static class LogConfiguration
    {
        public static void Configure()
        {
            Configure("appsettings.json");
        }

        public static void Configure(string appSettingsPath)
        {
            var configuration = new ConfigurationBuilder()
                .AddJsonFile(appSettingsPath)
                .Build();

            var logsDir = configuration.GetValue<string>("LogsDir");
            if (string.IsNullOrWhiteSpace(logsDir)) logsDir = Environment.GetEnvironmentVariable("ESZOP_LOGS_DIR");
            if (string.IsNullOrWhiteSpace(logsDir)) Directory.GetCurrentDirectory();

            var logPath = GetLogPath(logsDir);

            ConfigureLogger(logPath);
        }

        private static void ConfigureLogger(string logPath)
        {
            Log.Logger = new LoggerConfiguration()
                .Enrich.WithMachineName()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
                .MinimumLevel.Verbose()
                .WriteTo.Console(
                    restrictedToMinimumLevel: LogEventLevel.Information,
                    outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u4}] {Message:lj}{NewLine}{Exception}")
                .WriteTo.File(logPath,
                    restrictedToMinimumLevel: LogEventLevel.Debug,
                    outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u4}] {Message:lj}{NewLine}{Exception}",
                    rollingInterval: RollingInterval.Day)
                .CreateLogger();
        }

        private static string GetLogPath(string providedLogDir)
        {
            var assembly = Assembly.GetEntryAssembly() ?? throw new ArgumentNullException(nameof(Assembly.GetEntryAssembly));
            var assemblyName = assembly.GetName().Name;

            var logFilename = $"eszop-{assemblyName.Replace(".", "")}-log-.txt";
            var logDir = providedLogDir ?? Path.GetDirectoryName(assembly.Location);
            return Path.Combine(logDir ?? string.Empty, logFilename);
        }
    }
}
