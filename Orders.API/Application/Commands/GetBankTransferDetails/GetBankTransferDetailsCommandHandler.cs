using Common.Exceptions;
using Common.Extensions;
using Common.Grpc.Services.OffersService.Requests.GetBankAccountNumber;
using MediatR;
using Microsoft.AspNetCore.Http;
using Orders.API.Application.Dto;
using Orders.API.DataAccess.Repositories;
using Orders.API.Grpc;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Orders.API.Application.Commands.GetBankTransferDetails
{
    public class GetBankTransferDetailsCommandHandler
        : IRequestHandler<GetBankTransferDetailsCommand, BankTransferDetailsDto>
    {
        private readonly HttpContext _httpContext;
        private readonly IOrderRepository _orderRepository;
        private readonly IOffersServiceClientFactory _offersServiceClientFactory;

        public GetBankTransferDetailsCommandHandler(IHttpContextAccessor httpContextAccessor,
            IOrderRepository orderRepository, IOffersServiceClientFactory offersServiceClientFactory)
        {
            _httpContext = httpContextAccessor.HttpContext ??
                           throw new ArgumentNullException(nameof(httpContextAccessor.HttpContext));
            _orderRepository = orderRepository ?? throw new ArgumentNullException(nameof(orderRepository));
            _offersServiceClientFactory = offersServiceClientFactory ?? throw new ArgumentNullException(nameof(offersServiceClientFactory));
        }

        public async Task<BankTransferDetailsDto> Handle(GetBankTransferDetailsCommand request, CancellationToken cancellationToken)
        {
            var orderId = Guid.Parse(request.OrderId);
            var order = await _orderRepository.GetByIdAsync(orderId);
            if (order == null) throw new NotFoundException("Order");

            var userClaims = _httpContext.User.Claims.ToTokenPayload().UserClaims;
            if (userClaims.Role.ToUpperInvariant() != "ADMIN" &&
                userClaims.Id != order.BuyerId && userClaims.Id != order.SellerId)
            {
                throw new NotFoundException("Order");
            }

            var offersServiceClient = _offersServiceClientFactory.Create();

            var anyOfferId = order.OrderItems.First().OfferId;
            var grpcRequest = new GetBankAccountNumberRequest { OfferId = anyOfferId };
            var grpcResponse = await offersServiceClient.GetBankAccount(grpcRequest);

            var bankTransferDetails = new BankTransferDetailsDto
            {
                Title = $"Order {orderId}",
                TransferAmount = order.TotalPriceWithDelivery,
                AccountNumber = grpcResponse.BankAccountNumber
            };

            return bankTransferDetails;
        }
    }
}
