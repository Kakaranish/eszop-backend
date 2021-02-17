using Common.Extensions;
using Identity.API.Application.Dto;
using Identity.API.DataAccess.Repositories;
using Identity.API.Extensions;
using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Identity.API.Application.Queries.GetProfileInfo
{
    public class GetProfileInfoQueryHandler : IRequestHandler<GetProfileInfoQuery, ProfileInfoDto>
    {
        private readonly IProfileInfoRepository _profileInfoRepository;
        private readonly HttpContext _httpContext;

        public GetProfileInfoQueryHandler(IProfileInfoRepository profileInfoRepository,
            IHttpContextAccessor httpContextAccessor)
        {
            _profileInfoRepository = profileInfoRepository ?? throw new ArgumentNullException(nameof(profileInfoRepository));
            _httpContext = httpContextAccessor.HttpContext ??
                           throw new ArgumentNullException(nameof(httpContextAccessor.HttpContext));
        }

        public async Task<ProfileInfoDto> Handle(GetProfileInfoQuery request, CancellationToken cancellationToken)
        {
            var userId = _httpContext.User.Claims.ToTokenPayload().UserClaims.Id;
            var profileInfo = await _profileInfoRepository.GetByUserIdAsync(userId);

            return profileInfo.ToDto();
        }
    }
}
