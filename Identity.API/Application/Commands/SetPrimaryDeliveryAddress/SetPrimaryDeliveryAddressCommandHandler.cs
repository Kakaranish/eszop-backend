using Common.Extensions;
using Identity.API.DataAccess.Repositories;
using Identity.API.Domain;
using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Identity.API.Application.Commands.SetPrimaryDeliveryAddress
{
    public class SetPrimaryDeliveryAddressCommandHandler : IRequestHandler<SetPrimaryDeliveryAddressCommand>
    {
        private readonly HttpContext _httpContext;
        private readonly IDeliveryAddressRepository _deliveryAddressRepository;
        private readonly IUserRepository _userRepository;

        public SetPrimaryDeliveryAddressCommandHandler(IHttpContextAccessor httpContextAccessor,
            IDeliveryAddressRepository deliveryAddressRepository, IUserRepository userRepository)
        {
            _httpContext = httpContextAccessor.HttpContext ??
                           throw new ArgumentNullException(nameof(httpContextAccessor.HttpContext));
            _deliveryAddressRepository = deliveryAddressRepository ??
                                         throw new ArgumentNullException(nameof(deliveryAddressRepository));
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
        }

        public async Task<Unit> Handle(SetPrimaryDeliveryAddressCommand request, CancellationToken cancellationToken)
        {
            var userId = _httpContext.User.Claims.ToTokenPayload().UserClaims.Id;
            var deliveryAddressId = Guid.Parse(request.DeliveryAddressId);
            var deliveryAddress = await _deliveryAddressRepository.GetById(deliveryAddressId);
            if (deliveryAddress == null || deliveryAddress.UserId != userId)
                throw new IdentityDomainException($"There is no {deliveryAddressId} delivery address");

            var user = await _userRepository.GetByIdAsync(userId);
            if (user.PrimaryDeliveryAddressId == deliveryAddressId) return await Unit.Task;

            user.SetPrimaryDeliveryAddress(deliveryAddress);

            _userRepository.Update(user);
            await _userRepository.UnitOfWork.SaveChangesAndDispatchDomainEventsAsync(cancellationToken);

            return await Unit.Task;
        }
    }
}
