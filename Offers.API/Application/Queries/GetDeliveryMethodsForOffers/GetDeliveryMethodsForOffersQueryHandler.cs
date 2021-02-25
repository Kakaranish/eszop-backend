using Common.Dto;
using MediatR;
using Offers.API.DataAccess.Repositories;
using Offers.API.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Offers.API.Extensions;

namespace Offers.API.Application.Queries.GetDeliveryMethodsForOffers
{
    public class GetDeliveryMethodsForOffersQueryHandler : IRequestHandler<GetDeliveryMethodsForOffersQuery, IList<DeliveryMethodDto>>
    {
        private readonly IOfferRepository _offerRepository;

        public GetDeliveryMethodsForOffersQueryHandler(IOfferRepository offerRepository)
        {
            _offerRepository = offerRepository ?? throw new ArgumentNullException(nameof(offerRepository));
        }

        public async Task<IList<DeliveryMethodDto>> Handle(GetDeliveryMethodsForOffersQuery request, CancellationToken cancellationToken)
        {
            var offersIds = request.OfferIds.Select(id => Guid.Parse(id));

            var offers = await _offerRepository.GetMultipleWithIds(offersIds);
            var sellerId = offers.First().OwnerId;
            if (offers.Any(x => x.OwnerId != sellerId))
                throw new OffersDomainException("All offers must have the same seller");

            var flattenDeliveryMethods = offers.Where(x => x.DeliveryMethods != null)
                .SelectMany(x => x.DeliveryMethods);

            return flattenDeliveryMethods.GroupBy(method => method.Name)
                .Select(group => group.OrderByDescending(method => method.Price).FirstOrDefault().ToDto())
                .ToList();
        }
    }
}
