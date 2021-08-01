using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace Common.Utilities.Authentication
{
    public static class Extensions
    {
        public static IServiceCollection AddJwtAuthentication(this IServiceCollection services)
        {
            IConfiguration configuration;
            using (var serviceProvider = services.BuildServiceProvider())
            {
                configuration = serviceProvider.GetRequiredService<IConfiguration>();
            }

            services.Configure<JwtConfig>(configuration.GetSection("JwtConfig"));

            var jwtConfig = new JwtConfig();
            configuration.GetSection("JwtConfig").Bind(jwtConfig);

            services
                .AddAuthentication()
                .AddJwtBearer(options =>
                {
                    var accessTokenSecret = Encoding.UTF8.GetBytes(jwtConfig.AccessTokenSecretKey);
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        IssuerSigningKey = new SymmetricSecurityKey(accessTokenSecret),
                        ValidIssuer = jwtConfig.Issuer,
                        ValidAudience = jwtConfig.Audience,
                        ValidAlgorithms = new List<string> { SecurityAlgorithms.HmacSha256 },
                        ClockSkew = TimeSpan.Zero
                    };
                    options.SaveToken = true;
                });

            services.AddAuthorization(options =>
            {
                options.AddPolicy("Admin", builder =>
                {
                    builder.RequireClaim("Role", "ADMIN", "SUPER_ADMIN");
                });
                options.AddPolicy("SuperAdmin", builder =>
                {
                    builder.RequireClaim("Role", "SUPER_ADMIN");
                });
            });

            services.AddSingleton<IAccessTokenDecoder, AccessTokenDecoder>();

            return services;
        }
    }
}
