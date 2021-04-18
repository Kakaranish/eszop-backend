using Common.Authentication;
using Common.ErrorHandling;
using Common.EventBus;
using Common.Extensions;
using Common.Helpers;
using FluentValidation;
using Identity.API.DataAccess;
using Identity.API.DataAccess.Repositories;
using Identity.API.Domain;
using Identity.API.Grpc;
using Identity.API.Services;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using ProtoBuf.Grpc.Server;
using Serilog;

namespace Identity.API
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
                    Title = "Identity.API",
                    Version = "v1"
                });
            });

            services.Configure<JwtConfig>(Configuration.GetSection("JwtConfig"));
            services.AddJwtAuthentication();
            services.AddMediatR(typeof(Startup).Assembly);

            var sqlServerConnectionString = services.GetSqlServerConnectionString();
            services.AddDbContext<AppDbContext>(builder =>
                builder
                    .UseSqlServer(sqlServerConnectionString)
                    .UseLazyLoadingProxies()
                    .UseLoggerFactory(LoggerFactory.Create(loggingBuilder => loggingBuilder.AddDebug()))
            );

            var redisConnectionString = Configuration.GetRedisConnectionString();
            services.AddDistributedRedisCache(options => options.Configuration = redisConnectionString);

            services.AddHealthChecks()
                .AddSqlServer(sqlServerConnectionString)
                .AddRedis(redisConnectionString);

            AssemblyScanner.FindValidatorsInAssembly(typeof(Startup).Assembly)
                .ForEach(item => services.AddScoped(item.InterfaceType, item.ValidatorType));
            services.AddScoped(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

            services.AddExceptionHandling<IdentityDomainException>();

            services.AddSingleton<PasswordValidatorBase, PasswordValidator>();
            services.AddSingleton<IPasswordHasher, PasswordHasher>();

            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();

            services.AddSingleton<IAccessTokenService, AccessTokenService>();
            services.AddScoped<IRefreshTokenService, RefreshTokenService>();

            services.AddSingleton<IAccessTokenDecoder, AccessTokenDecoder>();
            services.AddSingleton<IRefreshTokenDecoder, RefreshTokenDecoder>();

            services.AddScoped<ISellerInfoRepository, SellerInfoRepository>();

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

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapHealthChecks("/healthcheck");
                endpoints.MapControllers();
                endpoints.MapGrpcService<IdentityService>();
            });

            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "Identity.API v1");
            });
        }
    }
}
