using Common.Utilities.Extensions;
using Identity.API.Application.Dto;
using Identity.API.Extensions;
using Identity.Domain.Repositories;
using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Identity.API.Application.Queries.GetSellerMe
{
    public class GetSellerInfoMeQueryHandler : IRequestHandler<GetSellerMeQuery, SellerInfoDto>
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ISellerInfoRepository _sellerInfoRepository;

        public GetSellerInfoMeQueryHandler(IHttpContextAccessor httpContextAccessor, ISellerInfoRepository sellerInfoRepository)
        {
            _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
            _sellerInfoRepository = sellerInfoRepository ?? throw new ArgumentNullException(nameof(sellerInfoRepository));
        }

        public async Task<SellerInfoDto> Handle(GetSellerMeQuery request, CancellationToken cancellationToken)
        {
            var userId = _httpContextAccessor.HttpContext.User.Claims.ToTokenPayload().UserClaims.Id;
            var sellerInfo = await _sellerInfoRepository.GetByUserIdAsync(userId);

            return sellerInfo.ToDto();
        }
    }
}
