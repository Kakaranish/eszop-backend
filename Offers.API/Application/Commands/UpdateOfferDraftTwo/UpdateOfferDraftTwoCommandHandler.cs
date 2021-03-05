using Common.Exceptions;
using Common.Extensions;
using MediatR;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Offers.API.DataAccess.Repositories;
using Offers.API.Domain;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Offers.API.Application.Commands.UpdateOfferDraftTwo
{
    public class UpdateOfferDraftTwoCommandHandler : IRequestHandler<UpdateOfferDraftTwoCommand>
    {
        private readonly HttpContext _httpContext;
        private readonly IOfferRepository _offerRepository;

        public UpdateOfferDraftTwoCommandHandler(IHttpContextAccessor httpContextAccessor,
            IOfferRepository offerRepository)
        {
            _httpContext = httpContextAccessor.HttpContext ??
                           throw new ArgumentNullException(nameof(httpContextAccessor.HttpContext));
            _offerRepository = offerRepository ?? throw new ArgumentNullException(nameof(offerRepository));
        }

        public async Task<Unit> Handle(UpdateOfferDraftTwoCommand request, CancellationToken cancellationToken)
        {
            var userId = _httpContext.User.Claims.ToTokenPayload().UserClaims.Id;
            var offerId = Guid.Parse(request.OfferId);

            var deliveryMethods = ExtractDeliveryMethods(request);

            var offer = await _offerRepository.GetByIdAsync(offerId);
            if (offer == null || offer.OwnerId != userId) throw new NotFoundException();

            offer.SetDeliveryMethods(deliveryMethods);

            _offerRepository.Update(offer);
            await _offerRepository.UnitOfWork.SaveChangesAndDispatchDomainEventsAsync(cancellationToken);

            return await Unit.Task;
        }

        private static IList<DeliveryMethod> ExtractDeliveryMethods(UpdateOfferDraftTwoCommand request)
        {
            var extractedDeliveryMethods = JsonConvert.DeserializeObject<IList<DeliveryMethod>>(request.DeliveryMethods)
                                       ?? throw new OffersDomainException($"'{request.DeliveryMethods}' is not parsable");

            return extractedDeliveryMethods;
        }
    }
}
