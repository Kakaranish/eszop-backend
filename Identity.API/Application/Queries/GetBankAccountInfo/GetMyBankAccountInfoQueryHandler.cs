using Common.Dto;
using Common.Extensions;
using Identity.API.DataAccess.Repositories;
using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Identity.API.Application.Queries.GetBankAccountInfo
{
    public class GetMyBankAccountInfoQueryHandler : IRequestHandler<GetMyBankAccountInfoQuery, BankAccountInfoDto>
    {
        private readonly ISellerInfoRepository _sellerInfoRepository;
        private readonly HttpContext _httpContext;

        public GetMyBankAccountInfoQueryHandler(ISellerInfoRepository sellerInfoRepository,
            IHttpContextAccessor httpContextAccessor)
        {
            _sellerInfoRepository = sellerInfoRepository ?? throw new ArgumentNullException(nameof(sellerInfoRepository));
            _httpContext = httpContextAccessor.HttpContext ??
                           throw new ArgumentNullException(nameof(httpContextAccessor.HttpContext));
        }

        public async Task<BankAccountInfoDto> Handle(GetMyBankAccountInfoQuery request, CancellationToken cancellationToken)
        {
            var userId = _httpContext.User.Claims.ToTokenPayload().UserClaims.Id;

            var sellerInfo = await _sellerInfoRepository.GetByUserIdAsync(userId);
            if (sellerInfo == null) return null;

            return new BankAccountInfoDto { AccountNumber = sellerInfo.BankAccountNumber };
        }
    }
}
