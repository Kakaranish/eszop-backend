using Common.Authentication;
using Identity.API.DataAccess;
using Identity.API.DataAccess.Repositories;
using Identity.API.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Identity.API.Extensions
{
    public static class StartupExtensions
    {
        public static void SeedTestDb(this IApplicationBuilder applicationBuilder)
        {
            var dbInitializer = applicationBuilder.ApplicationServices.GetRequiredService<IDbInitializer>();
            dbInitializer.SeedData();
        }

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

            services.AddScoped<IUserService, UserService>();

            return services;
        }
    }
}
