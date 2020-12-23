using System;
using System.Threading;
using System.Threading.Tasks;
using Common.Extensions;
using MediatR;
using Microsoft.AspNetCore.Http;
using Offers.API.DataAccess.Repositories;
using Offers.API.Domain;

namespace Offers.API.Application.Commands.CreateOffer
{
    public class CreateOfferCommandHandler : IRequestHandler<CreateOfferCommand, Guid>
    {
        private readonly IOfferRepository _offerRepository;
        private readonly HttpContext _httpContext;

        public CreateOfferCommandHandler(IHttpContextAccessor httpContextAccessor,
            IOfferRepository offerRepository)
        {
            _httpContext = httpContextAccessor?.HttpContext ??
                           throw new ArgumentNullException(nameof(httpContextAccessor));
            _offerRepository = offerRepository ?? throw new ArgumentNullException(nameof(offerRepository));
        }

        public async Task<Guid> Handle(CreateOfferCommand command, CancellationToken cancellationToken)
        {
            var tokenPayload = _httpContext.User.Claims.ToTokenPayload();

            var offer = new Offer(
                ownerId: tokenPayload.UserClaims.Id, 
                name: command.Name, 
                description: command.Description, 
                price: command.Price
            );

            await _offerRepository.AddAsync(offer);

            return await Task.FromResult(offer.Id);
        }
    }
}
