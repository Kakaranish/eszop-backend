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

namespace Offers.API.Application.Queries.GetSellerOffers
{
    public class GetSellerOffersQueryHandler : IRequestHandler<GetSellerOffersQuery, Pagination<OfferListPreviewDto>>
    {
        private readonly IOfferRepository _offerRepository;

        public GetSellerOffersQueryHandler(IOfferRepository offerRepository)
        {
            _offerRepository = offerRepository ?? throw new ArgumentNullException(nameof(offerRepository));
        }

        public async Task<Pagination<OfferListPreviewDto>> Handle(GetSellerOffersQuery request, CancellationToken cancellationToken)
        {
            var sellerId = Guid.Parse(request.SellerId);
            var offersPagination = await _offerRepository.GetAllActiveByUserIdAsync(sellerId, request.OfferFilter);

            var offersDtoPagination = offersPagination.Transform(offers =>
                offers.Select(offer => offer.ToOfferListPreviewDto()));

            return offersDtoPagination;
        }
    }
}
