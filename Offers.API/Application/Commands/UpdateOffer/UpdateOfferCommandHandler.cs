using MediatR;
using Offers.API.DataAccess.Repositories;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Offers.API.Application.Commands.UpdateOffer
{
    public class UpdateOfferCommandHandler : IRequestHandler<UpdateOfferCommand>
    {
        private readonly IOfferRepository _offerRepository;

        public UpdateOfferCommandHandler(IOfferRepository offerRepository)
        {
            _offerRepository = offerRepository ?? throw new ArgumentNullException(nameof(offerRepository));
        }

        public async Task<Unit> Handle(UpdateOfferCommand request, CancellationToken cancellationToken)
        {
            var offer = await _offerRepository.GetByIdAsync(Guid.Parse(request.OfferId));

            if (request.Name is not null) offer.SetName(request.Name);
            if (request.Description is not null) offer.SetDescription(request.Description);
            if (request.Price is not null) offer.SetPrice(request.Price.Value);

            var updated = await _offerRepository.UnitOfWork.SaveChangesAndDispatchDomainEventsAsync(cancellationToken);
            if(updated)
            {
                // TODO: Send integration event
            }

            return await Unit.Task;
        }
    }
}
