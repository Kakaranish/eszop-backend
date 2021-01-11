using MediatR;
using Offers.API.DataAccess.Repositories;
using Offers.API.Domain;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Offers.API.Application.Commands.RemovePredefinedDeliveryMethod
{
    public class RemovePredefinedDeliveryMethodCommandHandler : IRequestHandler<RemovePredefinedDeliveryMethodCommand>
    {
        private readonly IPredefinedDeliveryMethodRepository _deliveryMethodRepository;

        public RemovePredefinedDeliveryMethodCommandHandler(IPredefinedDeliveryMethodRepository deliveryMethodRepository)
        {
            _deliveryMethodRepository = deliveryMethodRepository ??
                                        throw new ArgumentNullException(nameof(deliveryMethodRepository));
        }

        public async Task<Unit> Handle(RemovePredefinedDeliveryMethodCommand request, CancellationToken cancellationToken)
        {
            var deliveryMethodId = Guid.Parse(request.DeliveryMethodId);

            var deliveryMethod = await _deliveryMethodRepository.GetById(deliveryMethodId);
            if (deliveryMethod == null)
                throw new OffersDomainException($"Predefined delivery method {deliveryMethodId} not found");

            _deliveryMethodRepository.Remove(deliveryMethod);
            await _deliveryMethodRepository.UnitOfWork.SaveChangesAndDispatchDomainEventsAsync(cancellationToken);

            return await Unit.Task;
        }
    }
}
