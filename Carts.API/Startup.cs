using Carts.API.Application.IntegrationEventsHandlers;
using Carts.API.DataAccess;
using Carts.API.DataAccess.Repositories;
using Carts.API.Domain;
using Common.Authentication;
using Common.ErrorHandling;
using Common.EventBus;
using Common.EventBus.IntegrationEvents;
using Common.Extensions;
using Common.Grpc;
using Common.Grpc.Services.OffersService;
using Common.Grpc.Services.OrdersService;
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
using Serilog;

namespace Carts.API
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
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Carts.API",
                    Version = "v1"
                });
            });

            services.ReadServicesEndpoints();

            services.AddJwtAuthentication();
            services.AddMediatR(typeof(Startup).Assembly);

            var connectionString = services.GetSqlServerConnectionString();
            services.AddDbContext<AppDbContext>(builder =>
                builder
                    .UseSqlServer(connectionString)
                    .UseLazyLoadingProxies()
                    .UseLoggerFactory(LoggerFactory.Create(loggingBuilder => loggingBuilder.AddDebug()))
            );
            services.AddHealthChecks()
                .AddCheck(
                    name: "SqlServerCheck",
                    instance: new SqlConnectionHealthCheck(connectionString),
                    failureStatus: HealthStatus.Unhealthy);

            AssemblyScanner.FindValidatorsInAssembly(typeof(Startup).Assembly)
                .ForEach(item => services.AddScoped(item.InterfaceType, item.ValidatorType));
            services.AddScoped(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

            services.AddScoped<ICartRepository, CartRepository>();
            services.AddScoped<ICartItemRepository, CartItemRepository>();

            services.AddScoped<IGrpcServiceClientFactory<IOffersService>, GrpcServiceClientFactory<IOffersService>>();
            services.AddScoped<IGrpcServiceClientFactory<IOrdersService>, GrpcServiceClientFactory<IOrdersService>>();

            services.AddExceptionHandling<CartsDomainException>();

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
            });

            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "Carts.API v1");
            });

            app.UseEventHandling(async eventBus =>
            {
                await eventBus.SubscribeAsync<ActiveOfferChangedIntegrationEvent, ActiveOfferChangedIntegrationEventHandler>();
                await eventBus.SubscribeAsync<OfferBecameUnavailableIntegrationEvent, OfferBecameUnavailableIntegrationEventHandler>();
            });
        }
    }
}
