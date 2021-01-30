using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Common.Extensions;
using Common.Types;
using MediatR;
using Offers.API.Application.Dto;
using Offers.API.DataAccess.Repositories;
using Offers.API.Extensions;

namespace Offers.API.Application.Queries.GetActiveOffers
{
    public class GetActiveOffersQueryHandler : IRequestHandler<GetActiveOffersQuery, Pagination<OfferDto>>
    {
        private readonly IOfferRepository _offerRepository;

        public GetActiveOffersQueryHandler(IOfferRepository offerRepository)
        {
            _offerRepository = offerRepository ?? throw new ArgumentNullException(nameof(offerRepository));
        }

        public async Task<Pagination<OfferDto>> Handle(GetActiveOffersQuery request, CancellationToken cancellationToken)
        {
            var offerFilter = request.OfferFilter;
            var offersPagination = await _offerRepository.GetAllActiveAsync(offerFilter);
            var offersDtoPagination = offersPagination.Transform(offers => offers.Select(offer => offer.ToDto()));

            return offersDtoPagination;
        }
    }
}
