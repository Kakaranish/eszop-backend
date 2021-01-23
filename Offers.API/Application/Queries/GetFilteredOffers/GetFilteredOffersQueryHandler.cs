using Common.Extensions;
using Common.Types;
using MediatR;
using Offers.API.Application.Dto;
using Offers.API.DataAccess;
using Offers.API.DataAccess.Repositories;
using Offers.API.Extensions;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Offers.API.Application.Queries.GetFilteredOffers
{
    public class GetFilteredOffersQueryHandler : IRequestHandler<GetFilteredOffersQuery, Pagination<OfferDto>>
    {
        private readonly IOfferRepository _offerRepository;

        public GetFilteredOffersQueryHandler(IOfferRepository offerRepository)
        {
            _offerRepository = offerRepository ?? throw new ArgumentNullException(nameof(offerRepository));
        }

        public async Task<Pagination<OfferDto>> Handle(GetFilteredOffersQuery request, CancellationToken cancellationToken)
        {
            var filter = new OfferFilter(request.FromPrice, request.ToPrice, request.Category);
            var pageDetails = new PageDetails(request.PageIndex, request.PageSize);
            var offersPagination = await _offerRepository.GetFiltered(filter, pageDetails);
            var offersDtoPagination = offersPagination.Transform(offers => offers.Select(offer => offer.ToDto()));

            return offersDtoPagination;
        }
    }
}
