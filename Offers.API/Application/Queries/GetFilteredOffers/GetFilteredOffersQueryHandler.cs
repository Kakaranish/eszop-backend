using Common.Types;
using MediatR;
using Offers.API.Domain;
using System;
using System.Threading;
using System.Threading.Tasks;
using Offers.API.DataAccess;
using Offers.API.DataAccess.Repositories;

namespace Offers.API.Application.Queries.GetFilteredOffers
{
    public class GetFilteredOffersQueryHandler : IRequestHandler<GetFilteredOffersQuery, Pagination<Offer>>
    {
        private readonly IOfferRepository _offerRepository;

        public GetFilteredOffersQueryHandler(IOfferRepository offerRepository)
        {
            _offerRepository = offerRepository ?? throw new ArgumentNullException(nameof(offerRepository));
        }

        public async Task<Pagination<Offer>> Handle(GetFilteredOffersQuery request, CancellationToken cancellationToken)
        {
            var filter = new OfferFilter(request.FromPrice, request.ToPrice, request.Category);
            var pageDetails = new PageDetails(request.PageIndex, request.PageSize);
            var offersPagination = await _offerRepository.GetFiltered(filter, pageDetails);
            
            return offersPagination;
        }
    }
}
