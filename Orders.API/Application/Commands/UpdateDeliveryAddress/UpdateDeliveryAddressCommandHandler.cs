using Common.Extensions;
using MediatR;
using Microsoft.AspNetCore.Http;
using Orders.API.DataAccess.Repositories;
using Orders.API.Domain;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Orders.API.Application.Commands.UpdateDeliveryAddress
{
    public class UpdateDeliveryAddressCommandHandler : IRequestHandler<UpdateDeliveryAddressCommand>
    {
        private readonly HttpContext _httpContext;
        private readonly IOrderRepository _orderRepository;

        public UpdateDeliveryAddressCommandHandler(IHttpContextAccessor httpContextAccessor, IOrderRepository orderRepository)
        {
            _httpContext = httpContextAccessor.HttpContext ??
                           throw new ArgumentNullException(nameof(httpContextAccessor.HttpContext));
            _orderRepository = orderRepository ?? throw new ArgumentNullException(nameof(orderRepository));
        }

        public async Task<Unit> Handle(UpdateDeliveryAddressCommand request, CancellationToken cancellationToken)
        {
            var userId = _httpContext.User.Claims.ToTokenPayload().UserClaims.Id;
            var orderId = Guid.Parse(request.OrderId);

            var order = await _orderRepository.GetByIdAsync(orderId);
            if (order == null || order.BuyerId != userId)
                throw new OrdersDomainException($"There is no order with id {orderId}");

            var deliveryAddress = new DeliveryAddress(request.FirstName, request.LastName, request.PhoneNumber,
                request.Country, request.City, request.ZipCode, request.Street);
            order.SetDeliveryAddress(deliveryAddress);

            _orderRepository.Update(order);
            await _orderRepository.UnitOfWork.SaveChangesAndDispatchDomainEventsAsync(cancellationToken);

            return await Unit.Task;
        }
    }
}
