using Common.Authentication;
using Common.ErrorHandling;
using Common.EventBus;
using Common.EventBus.IntegrationEvents;
using Common.Extensions;
using Common.HealthCheck;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Logging;
using Offers.API.Application.DomainEvents.Reducers;
using Offers.API.Application.IntegrationEventHandlers;
using Offers.API.Application.Services;
using Offers.API.DataAccess;
using Offers.API.DataAccess.Repositories;
using Offers.API.Domain;
using Offers.API.Extensions;
using Offers.API.Grpc;
using Offers.API.Services;
using ProtoBuf.Grpc.Server;
using Serilog;

namespace Offers.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddHttpContextAccessor();
            services.AddCodeFirstGrpc();

            services.AddJwtAuthentication();
            services.AddMediatR(typeof(Startup).Assembly);

            services.Configure<AzureStorageConfig>(Configuration.GetSection("AzureStorage"));
            services.AddBlobStorage();
            services.AddSingleton<IImageStorage, ImageStorage>();

            var connectionString = Configuration.GetConnectionString("SqlServer");
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

            services.AddScoped<IRequestOfferImagesProcessor, RequestOfferImagesProcessor>();
            services.AddScoped<IRequestOfferImagesMetadataExtractor, RequestOfferImagesMetadataExtractor>();

            services.AddScoped<IOfferRepository, OfferRepository>();
            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<IPredefinedDeliveryMethodRepository, PredefinedDeliveryMethodRepository>();

            services.AddEventDispatching<DomainEventReducer>();
            services.AddScoped<IOfferDomainEventReducer, OfferDomainEventReducer>();

            services.AddExceptionHandling<OffersDomainException>();

            services
                .AddRabbitMqEventBus()
                .Subscribe<OrderStartedIntegrationEvent, OrderStartedIntegrationEventHandler>()
                .Subscribe<OrderCancelledIntegrationEvent, OrderCancelledIntegrationEventHandler>();
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
                endpoints.MapGrpcService<OfferService>();
            });
        }
    }
}
