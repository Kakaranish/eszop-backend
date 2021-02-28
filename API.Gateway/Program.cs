using Common.Logging;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Serilog;
using System.IO;

namespace API.Gateway
{
    public class Program
    {
        public static void Main(string[] args)
        {
            LogConfiguration.Configure("Configuration/appsettings.json");

            Log.Logger.Information("----------------------------------------");
            Log.Logger.Information("---  Starting service  -----------------");
            Log.Logger.Information("----------------------------------------");

            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseSerilog()
                .ConfigureAppConfiguration((context, builder) =>
                {
                    builder
                        .SetBasePath(Path.Combine(context.HostingEnvironment.ContentRootPath, "Configuration"))
                        .AddJsonFile("appsettings.json")
                        .AddJsonFile($"appsettings.{context.HostingEnvironment.EnvironmentName}.json")
                        .AddJsonFile($"ocelot.{context.HostingEnvironment.EnvironmentName}.json")
                        .AddEnvironmentVariables();
                })
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
