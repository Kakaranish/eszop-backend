using Common.Grpc.Services.IdentityService;
using Common.Grpc.Services.IdentityService.Requests.GetBankAccountNumber;
using Identity.Domain.Repositories;
using System;
using System.Threading.Tasks;

namespace Identity.Infrastructure.Grpc
{
    public class IdentityService : IIdentityService
    {
        private readonly ISellerInfoRepository _sellerInfoRepository;

        public IdentityService(ISellerInfoRepository sellerInfoRepository)
        {
            _sellerInfoRepository = sellerInfoRepository ?? throw new ArgumentNullException(nameof(sellerInfoRepository));
        }

        public async Task<GetBankAccountNumberResponse> GetBankAccount(GetBankAccountNumberRequest request)
        {
            var sellerInfo = await _sellerInfoRepository.GetByUserIdAsync(request.UserId);
            var response = new GetBankAccountNumberResponse { BankAccountNumber = sellerInfo?.BankAccountNumber };

            return response;
        }
    }
}
