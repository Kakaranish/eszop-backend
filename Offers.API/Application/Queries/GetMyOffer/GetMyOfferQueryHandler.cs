using Common.Extensions;
using MediatR;
using Microsoft.AspNetCore.Http;
using Offers.API.Application.Dto;
using Offers.API.DataAccess.Repositories;
using Offers.API.Extensions;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Offers.API.Application.Queries.GetMyOffer
{
    public class GetMyOfferQueryHandler : IRequestHandler<GetMyOfferQuery, OfferFullViewDto>
    {
        private readonly HttpContext _httpContext;
        private readonly IOfferRepository _offerRepository;

        public GetMyOfferQueryHandler(IHttpContextAccessor httpContextAccessor, IOfferRepository offerRepository)
        {
            _httpContext = httpContextAccessor.HttpContext ??
                           throw new ArgumentNullException(nameof(httpContextAccessor.HttpContext));
            _offerRepository = offerRepository ?? throw new ArgumentNullException(nameof(offerRepository));
        }

        public async Task<OfferFullViewDto> Handle(GetMyOfferQuery request, CancellationToken cancellationToken)
        {
            var userId = _httpContext.User.Claims.ToTokenPayload().UserClaims.Id;
            var offerId = Guid.Parse(request.OfferId);

            var offer = await _offerRepository.GetByIdAsync(offerId);
            if (offer == null) return null;

            return offer.OwnerId == userId
                ? offer.ToOfferFullViewDto()
                : null;
        }
    }
}
