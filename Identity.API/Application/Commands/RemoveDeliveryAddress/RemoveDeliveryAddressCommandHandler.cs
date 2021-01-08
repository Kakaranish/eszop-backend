using Common.Extensions;
using Identity.API.DataAccess.Repositories;
using Identity.API.Domain;
using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Identity.API.Application.Commands.RemoveDeliveryAddress
{
    public class RemoveDeliveryAddressCommandHandler : IRequestHandler<RemoveDeliveryAddressCommand>
    {
        private readonly IDeliveryAddressRepository _deliveryAddressRepository;
        private readonly HttpContext _httpContext;

        public RemoveDeliveryAddressCommandHandler(IHttpContextAccessor httpContextAccessor,
            IDeliveryAddressRepository deliveryAddressRepository)
        {
            _httpContext = httpContextAccessor.HttpContext ??
                           throw new ArgumentNullException(nameof(httpContextAccessor.HttpContext));
            _deliveryAddressRepository = deliveryAddressRepository ??
                                         throw new ArgumentNullException(nameof(deliveryAddressRepository));
        }

        public async Task<Unit> Handle(RemoveDeliveryAddressCommand request, CancellationToken cancellationToken)
        {
            var userId = _httpContext.User.Claims.ToTokenPayload().UserClaims.Id;
            var deliveryAddressId = Guid.Parse(request.DeliveryAddressId);
            var deliveryAddress = await _deliveryAddressRepository.GetById(deliveryAddressId);

            if (deliveryAddress == null || deliveryAddress.UserId != userId)
                throw new IdentityDomainException($"There is no {deliveryAddressId} delivery address");

            if (deliveryAddress.User.PrimaryDeliveryAddressId == deliveryAddressId)
                deliveryAddress.User.RemovePrimaryDeliveryAddress();

            _deliveryAddressRepository.Remove(deliveryAddress);

            await _deliveryAddressRepository.UnitOfWork.SaveChangesAndDispatchDomainEventsAsync(cancellationToken);

            return await Unit.Task;
        }
    }
}
