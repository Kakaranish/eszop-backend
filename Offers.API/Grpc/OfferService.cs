using Common.Grpc.Services;
using Common.Grpc.Services.Types;
using Offers.API.DataAccess.Repositories;
using ProtoBuf.Grpc;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Offers.API.Grpc
{
    public class OfferService : IOffersService
    {
        private readonly IOfferRepository _offerRepository;

        public OfferService(IOfferRepository offerRepository)
        {
            _offerRepository = offerRepository ?? throw new ArgumentNullException(nameof(offerRepository));
        }

        public async Task<GetBankAccountNumberResponse> GetBankAccount(GetBankAccountNumberRequest request, CallContext context = default)
        {
            var offer = await _offerRepository.GetByIdAsync(request.OfferId);
            return new GetBankAccountNumberResponse { BankAccountNumber = offer.BankAccountNumber };
        }
    }
}
