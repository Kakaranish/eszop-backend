using Common.Utilities.Exceptions;
using Common.Utilities.Extensions;
using MediatR;
using Microsoft.AspNetCore.Http;
using Offers.Domain.Repositories;
using Offers.Infrastructure.Dto;
using Offers.Infrastructure.Extensions;
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
            if (offer == null || offer.OwnerId != userId) throw new NotFoundException();

            return offer.ToOfferFullViewDto();
        }
    }
}
