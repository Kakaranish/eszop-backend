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
            var offersPagination = await _offerRepository.GetByUserIdAsync(sellerId, request.OfferFilter);

            var offersDtoPagination = offersPagination.Transform(offers =>
                offers.Select(offer => offer.ToOfferListPreviewDto()));

            return offersDtoPagination;
        }
    }
}
