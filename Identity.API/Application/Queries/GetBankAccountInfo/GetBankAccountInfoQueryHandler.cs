using System;
using System.Threading;
using System.Threading.Tasks;
using Common.Dto;
using Identity.API.DataAccess.Repositories;
using MediatR;

namespace Identity.API.Application.Queries.GetBankAccountInfo
{
    public class GetBankAccountInfoQueryHandler : IRequestHandler<GetBankAccountInfoQuery, BankAccountInfoDto>
    {
        private readonly ISellerInfoRepository _sellerInfoRepository;

        public GetBankAccountInfoQueryHandler(ISellerInfoRepository sellerInfoRepository)
        {
            _sellerInfoRepository = sellerInfoRepository ?? throw new ArgumentNullException(nameof(sellerInfoRepository));
        }

        public async Task<BankAccountInfoDto> Handle(GetBankAccountInfoQuery request, CancellationToken cancellationToken)
        {
            var sellerId = Guid.Parse(request.SellerId);
            var sellerInfo = await _sellerInfoRepository.GetByUserIdAsync(sellerId);
            if (sellerInfo == null) return null;

            return new BankAccountInfoDto
            {
                AccountNumber = sellerInfo.BankAccountNumber
            };
        }
    }
}
