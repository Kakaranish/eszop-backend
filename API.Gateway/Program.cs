using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace API.Gateway
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();
            
            host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
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
