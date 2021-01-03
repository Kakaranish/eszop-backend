using MediatR;
using Offers.API.DataAccess.Repositories;
using Offers.API.Domain;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Offers.API.Application.Queries.GetAllOffers
{
    public class GetAllOffersQueryHandler : IRequestHandler<GetAllOffersQuery, IList<Offer>>
    {
        private readonly IOfferRepository _offerRepository;

        public GetAllOffersQueryHandler(IOfferRepository offerRepository)
        {
            _offerRepository = offerRepository ?? throw new ArgumentNullException(nameof(offerRepository));
        }

        public async Task<IList<Offer>> Handle(GetAllOffersQuery request, CancellationToken cancellationToken)
        {
            return await _offerRepository.GetAllAsync();
        }
    }
}
