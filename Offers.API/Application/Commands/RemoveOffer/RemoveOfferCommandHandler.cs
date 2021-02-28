using Common.Exceptions;
using Common.Extensions;
using MediatR;
using Microsoft.AspNetCore.Http;
using Offers.API.DataAccess.Repositories;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Offers.API.Application.Commands.RemoveOffer
{
    public class RemoveOfferCommandHandler : IRequestHandler<RemoveOfferCommand>
    {
        private readonly IOfferRepository _offerRepository;
        private readonly HttpContext _httpContext;

        public RemoveOfferCommandHandler(IHttpContextAccessor httpContextAccessor, IOfferRepository offerRepository)
        {
            _httpContext = httpContextAccessor.HttpContext ??
                           throw new ArgumentNullException(nameof(httpContextAccessor.HttpContext));
            _offerRepository = offerRepository ?? throw new ArgumentNullException(nameof(offerRepository));
        }

        public async Task<Unit> Handle(RemoveOfferCommand request, CancellationToken cancellationToken)
        {
            var offerId = Guid.Parse(request.OfferId);
            var offer = await _offerRepository.GetByIdAsync(offerId);
            if (offer == null) throw new NotFoundException();

            var userClaims = _httpContext.User.Claims.ToTokenPayload().UserClaims;
            var userId = userClaims.Id;
            const string adminRole = "ADMIN";

            if (offer.OwnerId != userId && userClaims.Role != adminRole) throw new NotFoundException();

            offer.MarkAsRemoved();

            _offerRepository.Update(offer);
            await _offerRepository.UnitOfWork.SaveChangesAndDispatchDomainEventsAsync(cancellationToken);

            return await Unit.Task;
        }
    }
}
