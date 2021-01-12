using MediatR;
using Offers.API.Application.Dto;
using Offers.API.DataAccess.Repositories;
using Offers.API.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Offers.API.Application.Queries.GetAllOffers
{
    public class GetAllOffersQueryHandler : IRequestHandler<GetAllOffersQuery, IList<OfferDto>>
    {
        private readonly IOfferRepository _offerRepository;

        public GetAllOffersQueryHandler(IOfferRepository offerRepository)
        {
            _offerRepository = offerRepository ?? throw new ArgumentNullException(nameof(offerRepository));
        }

        public async Task<IList<OfferDto>> Handle(GetAllOffersQuery request, CancellationToken cancellationToken)
        {
            var offers = await _offerRepository.GetAllAsync();
            return offers.Select(offer => offer.ToDto()).ToList();
        }
    }
}
