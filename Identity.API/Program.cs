using Common.Logging;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace Identity.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            LogConfiguration.Configure();

            Log.Logger.Information("----------------------------------------");
            Log.Logger.Information("---  Starting service  -----------------");
            Log.Logger.Information("----------------------------------------");

            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseSerilog()
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                    webBuilder.ConfigureKestrel(options =>
                    {
                        options.ListenAnyIP(6000, o => o.Protocols = HttpProtocols.Http1);
                        options.ListenLocalhost(6001, o => o.Protocols = HttpProtocols.Http2);
                    });
                });
    }
}
