using Common.Utilities.Extensions;
using Identity.Domain.Repositories;
using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Identity.API.Application.Queries.CanSell
{
    public class CanSellQueryHandler : IRequestHandler<CanSellQuery, bool>
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ISellerInfoRepository _sellerInfoRepository;

        public CanSellQueryHandler(IHttpContextAccessor httpContextAccessor, ISellerInfoRepository sellerInfoRepository)
        {
            _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
            _sellerInfoRepository = sellerInfoRepository ?? throw new ArgumentNullException(nameof(sellerInfoRepository));
        }

        public async Task<bool> Handle(CanSellQuery request, CancellationToken cancellationToken)
        {
            var userId = _httpContextAccessor.HttpContext.User.Claims.ToTokenPayload().UserClaims.Id;
            var sellerInfo = await _sellerInfoRepository.GetByUserIdAsync(userId);

            return sellerInfo != null;
        }
    }
}
