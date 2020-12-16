using Microsoft.AspNetCore.Http;
using System;
using System.Net;
using System.Threading.Tasks;

namespace Common.Authentication
{
    public class AccessTokenValidatorMiddleware : IMiddleware
    {
        private readonly IAccessTokenDecoder _accessTokenDecoder;

        public AccessTokenValidatorMiddleware(IAccessTokenDecoder accessTokenDecoder)
        {
            _accessTokenDecoder = accessTokenDecoder ?? throw new ArgumentNullException(nameof(accessTokenDecoder));
        }

        public Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;

            return Task.CompletedTask;
        }
    }
}
