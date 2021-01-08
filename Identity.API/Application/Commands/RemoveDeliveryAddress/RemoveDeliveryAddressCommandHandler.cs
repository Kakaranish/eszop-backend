﻿using Common.Extensions;
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
        private readonly IUserRepository _userRepository;
        private readonly HttpContext _httpContext;

        public RemoveDeliveryAddressCommandHandler(IHttpContextAccessor httpContextAccessor,
            IDeliveryAddressRepository deliveryAddressRepository, IUserRepository userRepository)
        {
            _httpContext = httpContextAccessor.HttpContext ??
                           throw new ArgumentNullException(nameof(httpContextAccessor.HttpContext));
            _deliveryAddressRepository = deliveryAddressRepository ??
                                         throw new ArgumentNullException(nameof(deliveryAddressRepository));
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
        }

        public async Task<Unit> Handle(RemoveDeliveryAddressCommand request, CancellationToken cancellationToken)
        {
            var userId = _httpContext.User.Claims.ToTokenPayload().UserClaims.Id;
            var deliveryAddressId = Guid.Parse(request.DeliveryAddressId);
            var deliveryAddress = await _deliveryAddressRepository.GetById(deliveryAddressId);

            if (deliveryAddress == null || deliveryAddress.UserId != userId)
                throw new IdentityDomainException($"There is no {deliveryAddressId} delivery address");

            _deliveryAddressRepository.Remove(deliveryAddress);

            var user = await _userRepository.GetByIdAsync(userId);
            if (user.PrimaryDeliveryAddressId == deliveryAddressId) user.RemovePrimaryDeliveryAddress();

            await _deliveryAddressRepository.UnitOfWork.SaveChangesAndDispatchDomainEventsAsync(cancellationToken);

            return await Unit.Task;
        }
    }
}
