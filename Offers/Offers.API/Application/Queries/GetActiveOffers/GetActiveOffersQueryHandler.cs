using Common.Domain.Types;
using Common.Utilities.Extensions;
using MediatR;
using Offers.Domain.Repositories;
using Offers.Infrastructure.Dto;
using Offers.Infrastructure.Extensions;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Offers.API.Application.Queries.GetActiveOffers
{
    public class GetActiveOffersQueryHandler : IRequestHandler<GetActiveOffersQuery, Pagination<OfferListPreviewDto>>
    {
        private readonly IOfferRepository _offerRepository;

        public GetActiveOffersQueryHandler(IOfferRepository offerRepository)
        {
            _offerRepository = offerRepository ?? throw new ArgumentNullException(nameof(offerRepository));
        }

        public async Task<Pagination<OfferListPreviewDto>> Handle(GetActiveOffersQuery request, CancellationToken cancellationToken)
        {
            var offerFilter = request.OfferFilter;
            var offersPagination = await _offerRepository.GetAllActiveAsync(offerFilter);
            var offersDtoPagination = offersPagination.Transform(offers =>
                offers.Select(offer => offer.ToOfferListPreviewDto()));

            return offersDtoPagination;
        }
    }
}
