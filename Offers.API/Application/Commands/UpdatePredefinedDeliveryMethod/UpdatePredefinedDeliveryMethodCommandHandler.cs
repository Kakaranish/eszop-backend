using Common.Exceptions;
using MediatR;
using Offers.API.DataAccess.Repositories;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Offers.API.Application.Commands.UpdatePredefinedDeliveryMethod
{
    public class UpdatePredefinedDeliveryMethodCommandHandler : IRequestHandler<UpdatePredefinedDeliveryMethodCommand>
    {
        private readonly IPredefinedDeliveryMethodRepository _deliveryMethodRepository;

        public UpdatePredefinedDeliveryMethodCommandHandler(IPredefinedDeliveryMethodRepository deliveryMethodRepository)
        {
            _deliveryMethodRepository = deliveryMethodRepository ?? throw new ArgumentNullException(nameof(deliveryMethodRepository));
        }

        public async Task<Unit> Handle(UpdatePredefinedDeliveryMethodCommand request, CancellationToken cancellationToken)
        {
            var deliveryMethodId = Guid.Parse(request.DeliveryMethodId);

            var deliveryMethod = await _deliveryMethodRepository.GetById(deliveryMethodId);
            if (deliveryMethod == null) throw new NotFoundException();

            deliveryMethod.SetName(request.Name);
            deliveryMethod.SetPrice(request.Price);

            _deliveryMethodRepository.Update(deliveryMethod);
            await _deliveryMethodRepository.UnitOfWork.SaveChangesAndDispatchDomainEventsAsync(cancellationToken);

            return await Unit.Task;
        }
    }
}
