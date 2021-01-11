using MediatR;
using Offers.API.DataAccess.Repositories;
using Offers.API.Domain;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Offers.API.Application.Commands.CreatePredefinedDeliveryMethod
{
    public class CreatePredefinedDeliveryMethodCommandHandler :
        IRequestHandler<CreatePredefinedDeliveryMethodCommand, Guid>
    {
        private readonly IPredefinedDeliveryMethodRepository _deliveryMethodRepository;

        public CreatePredefinedDeliveryMethodCommandHandler(IPredefinedDeliveryMethodRepository deliveryMethodRepository)
        {
            _deliveryMethodRepository = deliveryMethodRepository ??
                                        throw new ArgumentNullException(nameof(deliveryMethodRepository));
        }

        public async Task<Guid> Handle(CreatePredefinedDeliveryMethodCommand request, CancellationToken cancellationToken)
        {
            var deliveryMethod = new PredefinedDeliveryMethod(request.Name, request.Description);

            _deliveryMethodRepository.Add(deliveryMethod);
            await _deliveryMethodRepository.UnitOfWork.SaveChangesAndDispatchDomainEventsAsync(cancellationToken);

            return deliveryMethod.Id;
        }
    }
}
