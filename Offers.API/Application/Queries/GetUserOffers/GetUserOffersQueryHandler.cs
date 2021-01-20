using Common.Extensions;
using MediatR;
using Microsoft.AspNetCore.Http;
using Offers.API.Application.Dto;
using Offers.API.DataAccess.Repositories;
using Offers.API.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Offers.API.Application.Queries.GetUserOffers
{
    public class GetUserOffersQueryHandler : IRequestHandler<GetUserOffersQuery, IList<OfferDto>>
    {
        private readonly IOfferRepository _offerRepository;
        private readonly HttpContext _httpContext;

        public GetUserOffersQueryHandler(IHttpContextAccessor httpContextAccessor, IOfferRepository offerRepository)
        {
            _httpContext = httpContextAccessor.HttpContext ??
                           throw new ArgumentNullException(nameof(httpContextAccessor.HttpContext));
            _offerRepository = offerRepository ?? throw new ArgumentNullException(nameof(offerRepository));
        }

        public async Task<IList<OfferDto>> Handle(GetUserOffersQuery request, CancellationToken cancellationToken)
        {
            var userId = _httpContext.User.Claims.ToTokenPayload().UserClaims.Id;

            var offers = await _offerRepository.GetAllByUserIdAsync(userId);

            return offers.Select(x => x.ToDto()).ToList();
        }
    }
}
