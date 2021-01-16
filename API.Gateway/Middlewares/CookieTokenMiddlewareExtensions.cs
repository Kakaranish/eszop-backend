using Microsoft.AspNetCore.Builder;

namespace API.Gateway.Middlewares
{
    public static class CookieTokenMiddlewareExtensions
    {
        public static IApplicationBuilder UseCookieTokenMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<CookieTokenMiddleware>();
        }
    }
}