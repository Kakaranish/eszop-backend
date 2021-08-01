using Common.Domain.Types;
using Common.Utilities.Extensions;
using MediatR;
using Microsoft.AspNetCore.Http;
using Offers.Domain.Repositories;
using Offers.Infrastructure.Dto;
using Offers.Infrastructure.Extensions;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Offers.API.Application.Queries.GetMyOffers
{
    public class GetMyOffersQueryHandler : IRequestHandler<GetMyOffersQuery, Pagination<OfferListPreviewDto>>
    {
        private readonly IOfferRepository _offerRepository;
        private readonly HttpContext _httpContext;

        public GetMyOffersQueryHandler(IHttpContextAccessor httpContextAccessor, IOfferRepository offerRepository)
        {
            _httpContext = httpContextAccessor.HttpContext ??
                           throw new ArgumentNullException(nameof(httpContextAccessor.HttpContext));
            _offerRepository = offerRepository ?? throw new ArgumentNullException(nameof(offerRepository));
        }

        public async Task<Pagination<OfferListPreviewDto>> Handle(GetMyOffersQuery request, CancellationToken cancellationToken)
        {
            var userId = _httpContext.User.Claims.ToTokenPayload().UserClaims.Id;

            var offersPagination = await _offerRepository.GetAllByUserIdAsync(userId, request.OfferFilter);
            var offersDtoPagination = offersPagination.Transform(offers =>
                offers.Select(offer => offer.ToOfferListPreviewDto()));

            return offersDtoPagination;
        }
    }
}
