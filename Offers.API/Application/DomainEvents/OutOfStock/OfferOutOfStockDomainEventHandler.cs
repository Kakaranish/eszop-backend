using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Offers.API.Application.DomainEvents.OutOfStock
{
    public class OfferOutOfStockDomainEventHandler : INotificationHandler<OfferOutOfStockDomainEvent>
    {
        public Task Handle(OfferOutOfStockDomainEvent notification, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
