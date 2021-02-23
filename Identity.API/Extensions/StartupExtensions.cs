using Common.Authentication;
using Identity.API.DataAccess.Repositories;
using Identity.API.Services;
using Microsoft.Extensions.DependencyInjection;

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

            services.AddScoped<IProfileInfoRepository, ProfileInfoRepository>();
            services.AddScoped<ISellerInfoRepository, SellerInfoRepository>();

            return services;
        }
    }
}