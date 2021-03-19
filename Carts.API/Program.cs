using Common.Logging;
using Common.Types;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Serilog;
using System;

namespace Carts.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            AppContext.SetSwitch(
                "System.Net.Http.SocketsHttpHandler.Http2UnencryptedSupport", true);

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
                    webBuilder.ConfigureKestrel((builderContext, options) =>
                    {
                        var listeningPorts = new ListeningPortsConfig();
                        builderContext.Configuration.GetSection("ListeningPorts").Bind(listeningPorts);

                        options.ListenAnyIP(listeningPorts.Api, o => o.Protocols = HttpProtocols.Http1);
                    });
                });
    }
}
