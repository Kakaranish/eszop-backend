using Identity.API.DataAccess.Repositories;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Identity.API.Application.Queries.GetBankAccountNumber
{
    public class GetBankAccountNumberQueryHandler : IRequestHandler<GetBankAccountNumberQuery, string>
    {
        private readonly ISellerInfoRepository _sellerInfoRepository;

        public GetBankAccountNumberQueryHandler(ISellerInfoRepository sellerInfoRepository)
        {
            _sellerInfoRepository = sellerInfoRepository ?? throw new ArgumentNullException(nameof(sellerInfoRepository));
        }

        public async Task<string> Handle(GetBankAccountNumberQuery request, CancellationToken cancellationToken)
        {
            var sellerId = Guid.Parse(request.SellerId);
            var sellerInfo = await _sellerInfoRepository.GetByUserIdAsync(sellerId);
            if (sellerInfo == null) return null;

            return !string.IsNullOrWhiteSpace(sellerInfo.BankAccountNumber)
                ? sellerInfo.BankAccountNumber
                : null;
        }
    }
}
