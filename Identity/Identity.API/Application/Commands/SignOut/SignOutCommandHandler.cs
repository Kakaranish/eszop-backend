using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Identity.API.Application.Commands.SignOut
{
    public class SignOutCommandHandler : IRequestHandler<SignOutCommand>
    {
        private readonly HttpContext _httpContext;

        public SignOutCommandHandler(IHttpContextAccessor httpContextAccessor)
        {
            _httpContext = httpContextAccessor.HttpContext ??
                           throw new ArgumentNullException(nameof(httpContextAccessor.HttpContext));
        }

        public Task<Unit> Handle(SignOutCommand request, CancellationToken cancellationToken)
        {
            _httpContext.Response.Cookies.Delete("accessToken");
            _httpContext.Response.Cookies.Delete("refreshToken");
            _httpContext.Response.Cookies.Delete("accessTokenExp");

            return Unit.Task;
        }
    }
}
