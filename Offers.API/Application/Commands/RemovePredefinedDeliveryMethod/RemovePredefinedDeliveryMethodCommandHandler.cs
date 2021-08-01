using Common.Exceptions;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using Offers.Domain.Repositories;

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
            if (deliveryMethod == null) throw new NotFoundException();

            _deliveryMethodRepository.Remove(deliveryMethod);
            await _deliveryMethodRepository.UnitOfWork.SaveChangesAndDispatchDomainEventsAsync(cancellationToken);

            return await Unit.Task;
        }
    }
}
