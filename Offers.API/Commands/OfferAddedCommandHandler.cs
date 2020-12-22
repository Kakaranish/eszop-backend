using System;
using System.Threading;
using System.Threading.Tasks;
using Common.Extensions;
using MediatR;
using Microsoft.AspNetCore.Http;
using Offers.API.DataAccess.Repositories;
using Offers.API.Domain;

namespace Offers.API.Commands
{
    public class OfferAddedCommandHandler : IRequestHandler<OfferAddedCommand>
    {
        private readonly IOfferRepository _offerRepository;
        private readonly HttpContext _httpContext;

        public OfferAddedCommandHandler(IHttpContextAccessor httpContextAccessor,
            IOfferRepository offerRepository)
        {
            _httpContext = httpContextAccessor?.HttpContext ??
                           throw new ArgumentNullException(nameof(httpContextAccessor));
            _offerRepository = offerRepository ?? throw new ArgumentNullException(nameof(offerRepository));
        }

        public async Task<Unit> Handle(OfferAddedCommand command, CancellationToken cancellationToken)
        {
            var tokenPayload = _httpContext.User.Claims.ToTokenPayload();

            var offer = new Offer
            {
                CreatedAt = DateTime.UtcNow,
                Description = command.Description,
                EndsAt = DateTime.UtcNow.AddDays(14),
                Name = command.Name,
                OwnerId = tokenPayload.UserClaims.Id,
                Price = command.Price
            };

            await _offerRepository.AddAsync(offer);

            return await Task.FromResult(Unit.Value);
        }
    }
}
