using Common.Extensions;
using Common.Types;
using MediatR;
using Offers.API.Application.Dto;
using Offers.API.DataAccess.Repositories;
using Offers.API.Extensions;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Offers.API.Application.Queries.GetFilteredOffers
{
    public class GetOffersQueryHandler : IRequestHandler<GetOffersQuery, Pagination<OfferDto>>
    {
        private readonly IOfferRepository _offerRepository;

        public GetOffersQueryHandler(IOfferRepository offerRepository)
        {
            _offerRepository = offerRepository ?? throw new ArgumentNullException(nameof(offerRepository));
        }

        public async Task<Pagination<OfferDto>> Handle(GetOffersQuery request, CancellationToken cancellationToken)
        {
            var offerFilter = request.OfferFilter;
            var offersPagination = await _offerRepository.GetFiltered(offerFilter);
            var offersDtoPagination = offersPagination.Transform(offers => offers.Select(offer => offer.ToDto()));

            return offersDtoPagination;
        }
    }
}
