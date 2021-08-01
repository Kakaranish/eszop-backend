using Common.Exceptions;
using Common.Extensions;
using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Threading;
using System.Threading.Tasks;
using Offers.Domain.Repositories;

namespace Offers.API.Application.Commands.EndOffer
{
    public class EndOfferCommandHandler : IRequestHandler<EndOfferCommand>
    {
        private readonly IOfferRepository _offerRepository;
        private readonly HttpContext _httpContext;

        public EndOfferCommandHandler(IHttpContextAccessor httpContextAccessor, IOfferRepository offerRepository)
        {
            _httpContext = httpContextAccessor.HttpContext ??
                           throw new ArgumentNullException(nameof(httpContextAccessor.HttpContext));
            _offerRepository = offerRepository ?? throw new ArgumentNullException(nameof(offerRepository));
        }

        public async Task<Unit> Handle(EndOfferCommand request, CancellationToken cancellationToken)
        {
            var offerId = Guid.Parse(request.OfferId);
            var offer = await _offerRepository.GetByIdAsync(offerId);
            if (offer == null) throw new NotFoundException();

            var userClaims = _httpContext.User.Claims.ToTokenPayload().UserClaims;
            var userId = userClaims.Id;
            const string adminRole = "admin";
            if (offer.OwnerId != userId && userClaims.Role != adminRole) throw new NotFoundException();

            offer.EndOffer();

            _offerRepository.Update(offer);
            await _offerRepository.UnitOfWork.SaveChangesAndDispatchDomainEventsAsync(cancellationToken);

            return await Unit.Task;
        }
    }
}
