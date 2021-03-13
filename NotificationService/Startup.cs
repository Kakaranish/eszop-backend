using Common.Authentication;
using Common.EventBus;
using Common.EventBus.IntegrationEvents;
using Common.Extensions;
using Common.HealthCheck;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Logging;
using NotificationService.Application;
using NotificationService.Application.Hubs;
using NotificationService.Application.IntegrationEventHandlers;
using NotificationService.Application.Services;
using NotificationService.DataAccess;
using NotificationService.DataAccess.Repositories;
using Serilog;

namespace NotificationService
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddSignalR();
            services.AddJwtAuthentication();
            services.AddHttpContextAccessor();

            services.Configure<NotificationSettings>(Configuration.GetSection("NotificationSettings"));

            services.AddMediatR(typeof(Startup).Assembly);

            var connectionString = Configuration.GetConnectionString("SqlServer");
            services.AddDbContext<AppDbContext>(builder =>
                {
                    builder
                        .UseSqlServer(connectionString)
                        .UseLoggerFactory(LoggerFactory.Create(loggingBuilder => loggingBuilder.AddDebug()));
                },
                ServiceLifetime.Transient
            );
            services.AddHealthChecks()
                .AddCheck(
                    name: "SqlServerCheck",
                    instance: new SqlConnectionHealthCheck(connectionString),
                    failureStatus: HealthStatus.Unhealthy);

            var notificationOptions = Configuration.GetSection("NotificationSettings").Get<NotificationSettings>();
            services.AddScheduler(builder =>
            {
                builder.AddJob<CleanupJob>(configure: options =>
                {
                    options.CronSchedule = notificationOptions.CleanupJobCronPattern;
                });
            });

            services.AddSingleton<IConnectionManager, ConnectionManager>();
            services.AddSingleton<INotificationCache, NotificationCache>();
            services.AddTransient<INotificationRepository, NotificationRepository>();

            services.AddScoped<NotificationIntegrationEventHandler>();
            services.AddRabbitMqEventBus();
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

            SubscribeDirectly(app);
        }

        private static void SubscribeDirectly(IApplicationBuilder app)
        {
            var eventBus = app.ApplicationServices.GetRequiredService<IEventBus>();
            eventBus.SubscribeAsync<NotificationIntegrationEvent, NotificationIntegrationEventHandler>();
        }
    }
}
