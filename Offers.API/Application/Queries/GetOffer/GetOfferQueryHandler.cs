using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Offers.API.DataAccess.Repositories;
using Offers.API.Domain;

namespace Offers.API.Application.Queries.GetOffer
{
    public class GetOfferQueryHandler : IRequestHandler<GetOfferQuery, Offer>
    {
        private readonly IOfferRepository _offerRepository;

        public GetOfferQueryHandler(IOfferRepository offerRepository)
        {
            _offerRepository = offerRepository ?? throw new ArgumentNullException(nameof(offerRepository));
        }

        public async Task<Offer> Handle(GetOfferQuery request, CancellationToken cancellationToken)
        {
            var offerId = Guid.Parse(request.OfferId);
            return await _offerRepository.GetByIdAsync(offerId);
        }
    }
}
