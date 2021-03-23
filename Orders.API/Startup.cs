using Common.Authentication;
using Common.ErrorHandling;
using Common.EventBus;
using Common.EventBus.IntegrationEvents;
using Common.Extensions;
using Common.Grpc;
using Common.Grpc.Services.IdentityService;
using Common.Grpc.Services.OffersService;
using Common.HealthCheck;
using Common.Helpers;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Orders.API.Application.IntegrationEvents;
using Orders.API.DataAccess;
using Orders.API.DataAccess.Repositories;
using Orders.API.Domain;
using Orders.API.Grpc;
using ProtoBuf.Grpc.Server;
using Serilog;

namespace Orders.API
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
            services.AddHttpContextAccessor();
            services.AddCodeFirstGrpc();
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Orders.API",
                    Version = "v1"
                });
            });

            services.ReadServicesEndpoints();

            services.AddJwtAuthentication();
            services.AddMediatR(typeof(Startup).Assembly);

            var connectionString = Configuration.GetConnectionString("SqlServer");
            services.AddDbContext<AppDbContext>(builder =>
                    builder
                        .UseSqlServer(connectionString)
                        .UseLazyLoadingProxies()
                        .UseLoggerFactory(LoggerFactory.Create(loggingBuilder => loggingBuilder.AddDebug())) // DEBUG PURPOSES
            );
            services.AddHealthChecks()
                .AddCheck(
                    name: "SqlServerCheck",
                    instance: new SqlConnectionHealthCheck(connectionString),
                    failureStatus: HealthStatus.Unhealthy);

            AssemblyScanner.FindValidatorsInAssembly(typeof(Startup).Assembly)
                .ForEach(item => services.AddScoped(item.InterfaceType, item.ValidatorType));
            services.AddScoped(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

            services.AddScoped<IOrderRepository, OrderRepository>();
            services.AddScoped<IGrpcServiceClientFactory<IOffersService>, GrpcServiceClientFactory<IOffersService>>();
            services.AddScoped<IGrpcServiceClientFactory<IIdentityService>, GrpcServiceClientFactory<IIdentityService>>();

            services.AddExceptionHandling<OrdersDomainException>();

            if (!EnvironmentHelpers.IsSeedingDatabase())
            {
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

            app.UseExceptionHandler("/error");

            app.UseRouting();

            app.UseAuthorization();
            app.UseAuthentication();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapHealthChecks("/healthcheck");
                endpoints.MapControllers();
                endpoints.MapGrpcService<OrdersService>();
            });

            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "Orders.API v1");
            });

            app.UseEventHandling(async bus =>
            {
                await bus.SubscribeAsync<OfferBecameUnavailableIntegrationEvent, OfferBecameUnavailableIntegrationEventHandler>();
            });
        }
    }
}
