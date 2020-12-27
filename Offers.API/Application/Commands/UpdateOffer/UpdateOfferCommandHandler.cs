using MediatR;
using Offers.API.DataAccess.Repositories;
using System;
using System.Threading;
using System.Threading.Tasks;
using Common.Extensions;
using Common.IntegrationEvents;
using Common.ServiceBus;
using Common.Types;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Offers.API.Application.Commands.UpdateOffer
{
    public class UpdateOfferCommandHandler : IRequestHandler<UpdateOfferCommand>
    {
        private readonly ILogger<UpdateOfferCommandHandler> _logger;
        private readonly IOfferRepository _offerRepository;
        private readonly IEventBus _eventBus;
        private readonly HttpContext _httpContext;

        public UpdateOfferCommandHandler(ILogger<UpdateOfferCommandHandler> logger, 
            IHttpContextAccessor httpContextAccessor, IOfferRepository offerRepository, IEventBus eventBus)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _httpContext = httpContextAccessor.HttpContext ??
                           throw new ArgumentNullException(nameof(httpContextAccessor.HttpContext));
            _offerRepository = offerRepository ?? throw new ArgumentNullException(nameof(offerRepository));
            _eventBus = eventBus ?? throw new ArgumentNullException(nameof(eventBus));
        }

        public async Task<Unit> Handle(UpdateOfferCommand request, CancellationToken cancellationToken)
        {
            var userId = _httpContext.User.Claims.ToTokenPayload().UserClaims.Id;
            var offer = await _offerRepository.GetByIdAsync(Guid.Parse(request.OfferId));
            if (offer is null || offer.OwnerId != userId)
            {
                var msg = $"Offer {request.OfferId} not found";
                _logger.LogError(msg);
                throw new OfferDomainException(msg);
            }

            var @event = new OfferChangedEvent(offer.Id)
            {
                Name = new ChangeState<string>(offer.Name, request.Name),
                Description = new ChangeState<string>(offer.Description, request.Description),
                Price = new ChangeState<decimal?>(offer.Price, request.Price)
            };

            if (@event.Name.Changed) offer.SetName(request.Name);
            if (@event.Description.Changed) offer.SetDescription(request.Description);
            if (@event.Price.Changed) offer.SetPrice(request.Price.Value);

            var shouldEventBePublished = await _offerRepository.UnitOfWork
                .SaveChangesAndDispatchDomainEventsAsync(cancellationToken);
            if(shouldEventBePublished)
            {
                await _eventBus.PublishAsync(@event);
                _logger.LogInformation($"Published {nameof(OfferChangedEvent)} integration event");
            }

            return await Unit.Task;
        }
    }
}
