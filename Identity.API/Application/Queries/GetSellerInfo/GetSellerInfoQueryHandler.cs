using Identity.API.DataAccess.Repositories;
using Identity.API.Domain;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Identity.API.Application.Queries.GetSellerInfo
{
    public class GetSellerInfoQueryHandler : IRequestHandler<GetSellerInfoQuery, SellerInfo>
    {
        private readonly ISellerInfoRepository _sellerInfoRepository;

        public GetSellerInfoQueryHandler(ISellerInfoRepository sellerInfoRepository)
        {
            _sellerInfoRepository = sellerInfoRepository ?? throw new ArgumentNullException(nameof(sellerInfoRepository));
        }

        public async Task<SellerInfo> Handle(GetSellerInfoQuery request, CancellationToken cancellationToken)
        {
            var sellerId = Guid.Parse(request.SellerId);
            var sellerInfo = await _sellerInfoRepository.GetByIdAsync(sellerId);

            return sellerInfo;
        }
    }
}
