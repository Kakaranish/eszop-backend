using MediatR;
using Offers.API.Application.Dto;
using Offers.API.DataAccess.Repositories;
using Offers.API.Extensions;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Offers.API.Application.Queries.GetOffer
{
    public class GetOfferQueryHandler : IRequestHandler<GetOfferQuery, OfferDto>
    {
        private readonly IOfferRepository _offerRepository;

        public GetOfferQueryHandler(IOfferRepository offerRepository)
        {
            _offerRepository = offerRepository ?? throw new ArgumentNullException(nameof(offerRepository));
        }

        public async Task<OfferDto> Handle(GetOfferQuery request, CancellationToken cancellationToken)
        {
            var offerId = Guid.Parse(request.OfferId);
            var offer = await _offerRepository.GetByIdAsync(offerId);

            return offer.ToDto();
        }
    }
}
