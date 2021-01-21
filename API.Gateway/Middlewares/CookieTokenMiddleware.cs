using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace API.Gateway.Middlewares
{
    public class CookieTokenMiddleware
    {
        private readonly RequestDelegate _next;

        public CookieTokenMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public Task Invoke(HttpContext httpContext)
        {
            if (httpContext.Request.Cookies.TryGetValue("accessToken", out var accessToken))
            {
                httpContext.Request.Headers.Add("Authorization", $"Bearer {accessToken}");
            }

            if (httpContext.Request.Cookies.TryGetValue("refreshToken", out var refreshToken))
            {
                httpContext.Request.Headers.Add("refreshToken", refreshToken);
            }

            return _next(httpContext);
        }
    }
}
