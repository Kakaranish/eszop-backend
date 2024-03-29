using Common.Utilities.Authentication;
using Common.Utilities.EventBus;
using Common.Utilities.EventBus.IntegrationEvents;
using Common.Utilities.Extensions;
using Common.Utilities.Helpers;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NotificationService.API.Application;
using NotificationService.API.Application.Hubs;
using NotificationService.API.Application.IntegrationEventHandlers;
using NotificationService.API.Application.Services;
using NotificationService.API.Extensions;
using NotificationService.Domain.Repositories;
using NotificationService.Infrastructure.DataAccess.Repositories;
using Serilog;

namespace NotificationService.API
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        public IWebHostEnvironment WebHostEnvironment { get; }

        public Startup(IConfiguration configuration, IWebHostEnvironment webHostEnvironment)
        {
            Configuration = configuration;
            WebHostEnvironment = webHostEnvironment;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddJwtAuthentication();
            services.AddHttpContextAccessor();
            services.ConfigureSignalR();

            services.Configure<NotificationSettings>(Configuration.GetSection("NotificationSettings"));

            services.AddMediatR(typeof(Startup).Assembly);

            services.ConfigureDbContext(WebHostEnvironment);
            services.ConfigureHealthchecks();

            var notificationOptions = Configuration.GetSection("NotificationSettings").Get<NotificationSettings>();
            //services.AddScheduler(builder =>
            //{
            //    builder.AddJob<CleanupJob>(configure: options =>
            //    {
            //        options.CronSchedule = notificationOptions.CleanupJobCronPattern;
            //    });
            //});

            services.AddSingleton<IConnectionManager, ConnectionManager>();
            services.AddSingleton<INotificationCache, NotificationCache>();
            services.AddScoped<INotificationRepository, NotificationRepository>();

            if (!EnvironmentHelpers.IsSeedingDatabase())
            {
                services.Configure<AzureEventBusConfig>(Configuration.GetSection("EventBus:AzureEventBus"));
                services.AddEventBus();
            }
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsCustomDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSerilogRequestLogging();

            app.UseRouting();

            app.UseAuthorization();
            app.UseAuthentication();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHealthChecks("/healthcheck");
                endpoints.MapHub<NotificationHub>("/hubs/notification");
            });

            app.UseEventHandling(async bus =>
            {
                await bus.SubscribeAsync<NotificationIntegrationEvent, NotificationIntegrationEventHandler>();
            });
        }
    }
}
