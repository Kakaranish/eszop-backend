using Common.Extensions;
using MediatR;
using Microsoft.AspNetCore.Http;
using Orders.API.Application.Dto;
using Orders.API.DataAccess.Repositories;
using Orders.API.Domain;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Orders.API.Application.Commands.GetBankTransferDetails
{
    public class GetBankTransferDetailsCommandHandler
        : IRequestHandler<GetBankTransferDetailsCommand, BankTransferDetailsDto>
    {
        private readonly IOrderRepository _orderRepository;
        private readonly HttpContext _httpContext;

        public GetBankTransferDetailsCommandHandler(IHttpContextAccessor httpContextAccessor,
            IOrderRepository orderRepository)
        {
            _httpContext = httpContextAccessor.HttpContext ??
                           throw new ArgumentNullException(nameof(httpContextAccessor.HttpContext));
            _orderRepository = orderRepository ?? throw new ArgumentNullException(nameof(orderRepository));
        }

        public async Task<BankTransferDetailsDto> Handle(GetBankTransferDetailsCommand request, CancellationToken cancellationToken)
        {
            var orderId = Guid.Parse(request.OrderId);
            var order = await _orderRepository.GetByIdAsync(orderId);
            if (order == null) throw new OrdersDomainException($"Order {orderId} not found");

            var userClaims = _httpContext.User.Claims.ToTokenPayload().UserClaims;
            if (userClaims.Role.ToLowerInvariant() != "admin" &&
                userClaims.Id != order.BuyerId && userClaims.Id != order.SellerId)
            {
                throw new OrdersDomainException($"Order {orderId} not found");
            }

            var bankTransferDetails = new BankTransferDetailsDto
            {
                Title = $"Order {orderId}",
                TransferAmount = order.TotalPrice,
                AccountNumber = "1234 5678" // TEMP
            };

            return bankTransferDetails;
        }
    }
}
