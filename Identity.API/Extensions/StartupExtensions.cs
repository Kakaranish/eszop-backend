using Common.Authentication;
using Common.Types.ErrorHandling.CustomFluentValidation;
using FluentValidation.AspNetCore;
using Identity.API.DataAccess.Repositories;
using Identity.API.Services;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;

namespace Identity.API.Extensions
{
    public static class StartupExtensions
    {
        public static IServiceCollection AddInternalServices(this IServiceCollection services)
        {
            services.AddSingleton<PasswordValidatorBase, PasswordValidator>();
            services.AddSingleton<IPasswordHasher, PasswordHasher>();

            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();

            services.AddSingleton<IAccessTokenService, AccessTokenService>();
            services.AddScoped<IRefreshTokenService, RefreshTokenService>();

            services.AddSingleton<IAccessTokenDecoder, AccessTokenDecoder>();
            services.AddSingleton<IRefreshTokenDecoder, RefreshTokenDecoder>();

            return services;
        }

        public static IServiceCollection AddFluentValidation(this IServiceCollection services)
        {
            services.AddControllers(options =>
                {
                    options.Filters.Add<CustomFluentValidationFailureActionFilter>();
                })
                .ConfigureApiBehaviorOptions(options =>
                {
                    options.SuppressModelStateInvalidFilter = true;
                })
                .AddFluentValidation(configuration =>
                {
                    var typesToExclude = new[] { typeof(PasswordValidator) };
                    configuration.RegisterValidatorsFromAssemblyContaining<Startup>(result =>
                        !typesToExclude.Contains(result.ValidatorType));
                });

            return services;
        }
    }
}