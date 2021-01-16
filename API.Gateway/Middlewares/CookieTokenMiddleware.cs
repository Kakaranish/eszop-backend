using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

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

            return _next(httpContext);
        }
    }
}
