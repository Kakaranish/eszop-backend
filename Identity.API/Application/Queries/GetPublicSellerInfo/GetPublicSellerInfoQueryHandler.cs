using System;
using System.Threading;
using System.Threading.Tasks;
using Common.Dto;
using Identity.API.DataAccess.Repositories;
using MediatR;

namespace Identity.API.Application.Queries.GetPublicSellerInfo
{
    public class GetPublicSellerInfoQueryHandler : IRequestHandler<GetPublicSellerInfoQuery, PublicSellerInfoDto>
    {
        private readonly ISellerInfoRepository _sellerInfoRepository;

        public GetPublicSellerInfoQueryHandler(ISellerInfoRepository sellerInfoRepository)
        {
            _sellerInfoRepository = sellerInfoRepository ?? throw new ArgumentNullException(nameof(sellerInfoRepository));
        }

        public async Task<PublicSellerInfoDto> Handle(GetPublicSellerInfoQuery request, CancellationToken cancellationToken)
        {
            var sellerId = Guid.Parse(request.SellerId);
            var sellerInfo = await _sellerInfoRepository.GetByUserIdAsync(sellerId);
            if (sellerInfo == null) return null;

            return new PublicSellerInfoDto
            {
                ContactEmail = sellerInfo.ContactEmail,
                PhoneNumber = sellerInfo.PhoneNumber,
                AdditionalInfo = sellerInfo.AdditionalInfo
            };
        }
    }
}
