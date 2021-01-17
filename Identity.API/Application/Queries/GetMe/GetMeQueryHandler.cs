using Common.Extensions;
using Identity.API.Application.Dto;
using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Identity.API.Application.Queries.GetMe
{
    public class GetMeQueryHandler : IRequestHandler<GetMeQuery, MeDto>
    {
        private readonly HttpContext _httpContext;

        public GetMeQueryHandler(IHttpContextAccessor httpContextAccessor)
        {
            _httpContext = httpContextAccessor.HttpContext ??
                           throw new ArgumentNullException(nameof(httpContextAccessor.HttpContext));
        }

        public Task<MeDto> Handle(GetMeQuery request, CancellationToken cancellationToken)
        {
            var userClaims = _httpContext.User.Claims.ToTokenPayload().UserClaims;

            return Task.FromResult(new MeDto
            {
                Email = userClaims.Email,
                Role = userClaims.Role
            });
        }
    }
}
