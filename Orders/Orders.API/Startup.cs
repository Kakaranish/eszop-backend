using Common.Grpc;
using Common.Grpc.Services.IdentityService;
using Common.Grpc.Services.OffersService;
using Common.Utilities.Authentication;
using Common.Utilities.ErrorHandling;
using Common.Utilities.EventBus;
using Common.Utilities.EventBus.IntegrationEvents;
using Common.Utilities.Extensions;
using Common.Utilities.Helpers;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Orders.API.Application.IntegrationEvents;
using Orders.Domain.Exceptions;
using Orders.Domain.Repositories;
using Orders.Infrastructure.DataAccess;
using Orders.Infrastructure.DataAccess.Repositories;
using Orders.Infrastructure.Grpc;
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

            var connectionString = services.GetSqlServerConnectionString();
            services.AddDbContext<AppDbContext>(builder =>
                    builder
                        .UseSqlServer(connectionString)
                        .UseLazyLoadingProxies()
                        .UseLoggerFactory(LoggerFactory.Create(loggingBuilder => loggingBuilder.AddDebug())) // DEBUG PURPOSES
            );
            services.AddHealthChecks()
                .AddSqlServer(connectionString);

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
