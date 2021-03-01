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

        public async Task<GetDeliveryMethodsForOffersResponse> GetDeliveryMethodsForOffers(
            GetDeliveryMethodsForOffersRequest request, CallContext context = default)
        {
            if (request.OfferIds == null || !request.OfferIds.Any())
                return new GetDeliveryMethodsForOffersResponse();

            var offers = await _offerRepository.GetMultipleWithIds(request.OfferIds);
            if (offers == null || offers.Count == 0) return new GetDeliveryMethodsForOffersResponse();

            var sellerId = offers.First().OwnerId;
            if (offers.Any(x => x.OwnerId != sellerId)) return new GetDeliveryMethodsForOffersResponse();

            var flattenDeliveryMethods = offers.Where(x => x.DeliveryMethods != null)
                .SelectMany(x => x.DeliveryMethods);
            var mergedDeliveryMethods = flattenDeliveryMethods.GroupBy(method => method.Name)
                .Select(group => group.OrderByDescending(method => method.Price).FirstOrDefault())
                .ToList();

            return new GetDeliveryMethodsForOffersResponse
            {
                DeliveryMethods = mergedDeliveryMethods.Select(method =>
                    new DeliveryMethodDto
                    {
                        Name = method.Name,
                        Price = method.Price
                    }
                )
            };
        }
    }
}
