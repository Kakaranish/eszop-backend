using Common.Exceptions;
using Common.Extensions;
using Common.ImageStorage;
using MediatR;
using Microsoft.AspNetCore.Http;
using Offers.API.DataAccess.Repositories;
using Offers.API.Domain;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Offers.API.Application.Commands.RemoveOfferDraft
{
    public class RemoveOfferDraftCommandHandler : IRequestHandler<RemoveOfferDraftCommand>
    {
        private readonly HttpContext _httpContext;
        private readonly IOfferRepository _offerRepository;
        private readonly IImageStorage _imageStorage;

        public RemoveOfferDraftCommandHandler(IHttpContextAccessor httpContextAccessor, IOfferRepository offerRepository,
            IImageStorage imageStorage)
        {
            _httpContext = httpContextAccessor.HttpContext ??
                           throw new ArgumentNullException(nameof(httpContextAccessor.HttpContext));
            _offerRepository = offerRepository ?? throw new ArgumentNullException(nameof(offerRepository));
            _imageStorage = imageStorage ?? throw new ArgumentNullException(nameof(imageStorage));
        }

        public async Task<Unit> Handle(RemoveOfferDraftCommand request, CancellationToken cancellationToken)
        {
            var userId = _httpContext.User.Claims.ToTokenPayload().UserClaims.Id;
            var offerId = Guid.Parse(request.OfferId);

            var offer = await _offerRepository.GetByIdAsync(offerId);
            if (offer == null || offer.OwnerId != userId) throw new NotFoundException();

            if (!offer.IsDraft)
                throw new OffersDomainException($"Offer {offerId} is not draft");

            foreach (var offerImage in offer.Images)
            {
                await _imageStorage.DeleteAsync(offerImage.Filename);
            }

            _offerRepository.Remove(offer);
            await _offerRepository.UnitOfWork.SaveChangesAndDispatchDomainEventsAsync(cancellationToken);

            return await Unit.Task;
        }
    }
}
