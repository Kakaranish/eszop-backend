using Common.Exceptions;
using Common.Extensions;
using MediatR;
using Microsoft.AspNetCore.Http;
using Offers.API.DataAccess.Repositories;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Offers.API.Application.Commands.PublishOffer
{
    public class PublishOfferCommandHandler : IRequestHandler<PublishOfferCommand>
    {
        private readonly HttpContext _httpContext;
        private readonly IOfferRepository _offerRepository;

        public PublishOfferCommandHandler(IHttpContextAccessor httpContextAccessor,
            IOfferRepository offerRepository)
        {
            _httpContext = httpContextAccessor.HttpContext ??
                           throw new ArgumentNullException(nameof(httpContextAccessor.HttpContext));
            _offerRepository = offerRepository ?? throw new ArgumentNullException(nameof(offerRepository));
        }

        public async Task<Unit> Handle(PublishOfferCommand request, CancellationToken cancellationToken)
        {
            var userId = _httpContext.User.Claims.ToTokenPayload().UserClaims.Id;
            var offerId = Guid.Parse(request.OfferId);

            var offer = await _offerRepository.GetByIdAsync(offerId);
            if (offer == null || offer.OwnerId != userId) throw new NotFoundException("Offer");

            offer.SetPublished();

            _offerRepository.Update(offer);
            await _offerRepository.UnitOfWork.SaveChangesAndDispatchDomainEventsAsync(cancellationToken);

            return await Unit.Task;
        }
    }
}
