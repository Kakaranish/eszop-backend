using Serilog;
using Serilog.Events;
using System;
using System.IO;
using System.Reflection;

namespace Common.Logging
{
    public static class LogConfiguration
    {
        public static void Configure(string logDir = null)
        {
            var logPath = GetLogPath(logDir);

            Log.Logger = new LoggerConfiguration()
                .Enrich.WithMachineName()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
                .WriteTo.Console(
                    outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u4}] {Message:lj}{NewLine}{Exception}",
                    restrictedToMinimumLevel: LogEventLevel.Information)
                .WriteTo.File(logPath, rollingInterval: RollingInterval.Day)
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
