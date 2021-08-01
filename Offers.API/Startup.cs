using Common.Grpc;
using Common.Grpc.Services.OrdersService;
using Common.Utilities.Authentication;
using Common.Utilities.ErrorHandling;
using Common.Utilities.EventBus;
using Common.Utilities.EventBus.IntegrationEvents;
using Common.Utilities.Extensions;
using Common.Utilities.Helpers;
using Common.Utilities.ImageStorage;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Offers.API.Application.DomainEventHandlers.ActiveOfferChanged.Reducers;
using Offers.API.Application.IntegrationEventHandlers;
using Offers.API.Application.Services;
using Offers.Domain.Exceptions;
using Offers.Domain.Repositories;
using Offers.Infrastructure.DataAccess;
using Offers.Infrastructure.DataAccess.Repositories;
using Offers.Infrastructure.Grpc;
using ProtoBuf.Grpc.Server;
using Serilog;
using System.Globalization;

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
            services.AddRequestLocalization(options =>
            {
                options.DefaultRequestCulture = new RequestCulture(CultureInfo.InvariantCulture);
            });

            services.AddControllers();
            services.AddHttpContextAccessor();
            services.AddCodeFirstGrpc();
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Offers.API",
                    Version = "v1"
                });
            });

            services.ReadServicesEndpoints();
            services.AddJwtAuthentication();
            services.AddMediatR(typeof(Startup).Assembly);

            services.AddBlobStorage();
            services.AddSingleton<IImageStorage, ImageStorage>();

            var connectionString = services.GetSqlServerConnectionString();
            services.AddDbContext<AppDbContext>(builder =>
                builder
                    .UseSqlServer(connectionString)
                    .UseLazyLoadingProxies()
                    .UseLoggerFactory(LoggerFactory.Create(loggingBuilder => loggingBuilder.AddDebug()))
            );
            services.AddHealthChecks()
                .AddSqlServer(connectionString);

            AssemblyScanner.FindValidatorsInAssembly(typeof(Startup).Assembly)
                .ForEach(item => services.AddScoped(item.InterfaceType, item.ValidatorType));
            services.AddScoped(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

            services.AddScoped<IRequestOfferImagesProcessor, RequestOfferImagesProcessor>();
            services.AddScoped<IRequestOfferImagesMetadataExtractor, RequestOfferImagesMetadataExtractor>();
            services.AddScoped<IRequestDeliveryMethodExtractor, RequestDeliveryMethodExtractor>();
            services.AddScoped<IRequestKeyValueInfoExtractor, RequestKeyValueInfoExtractor>();

            services.AddScoped<IOfferRepository, OfferRepository>();
            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<IPredefinedDeliveryMethodRepository, PredefinedDeliveryMethodRepository>();

            services.AddEventDispatching<DomainEventReducer>();
            services.AddScoped<IOfferDomainEventReducer, OfferDomainEventReducer>();

            services.AddExceptionHandling<OffersDomainException>();

            services.AddScoped<IGrpcServiceClientFactory<IOrdersService>, GrpcServiceClientFactory<IOrdersService>>();

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

            app.UseRequestLocalization();

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

            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "Offers.API v1");
            });

            app.UseEventHandling(async bus =>
            {
                await bus.SubscribeAsync<OrderStartedIntegrationEvent, OrderStartedIntegrationEventHandler>();
                await bus.SubscribeAsync<OrderCancelledIntegrationEvent, OrderCancelledIntegrationEventHandler>();
                await bus.SubscribeAsync<UserLockedIntegrationEvent, UserLockedIntegrationEventHandler>();
            });
        }
    }
}
