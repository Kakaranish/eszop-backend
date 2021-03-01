using Common.Exceptions;
using Common.Extensions;
using Common.Grpc.Services;
using Common.Grpc.Services.Types;
using Common.Types;
using Grpc.Net.Client;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Orders.API.Application.Dto;
using Orders.API.DataAccess.Repositories;
using ProtoBuf.Grpc.Client;
using System;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Orders.API.Application.Commands.GetBankTransferDetails
{
    public class GetBankTransferDetailsCommandHandler
        : IRequestHandler<GetBankTransferDetailsCommand, BankTransferDetailsDto>
    {
        private readonly IOrderRepository _orderRepository;
        private readonly HttpContext _httpContext;
        private readonly UrlsConfig _urlsConfig;

        public GetBankTransferDetailsCommandHandler(IHttpContextAccessor httpContextAccessor,
            IOrderRepository orderRepository, IOptions<UrlsConfig> options)
        {
            _httpContext = httpContextAccessor.HttpContext ??
                           throw new ArgumentNullException(nameof(httpContextAccessor.HttpContext));
            _orderRepository = orderRepository ?? throw new ArgumentNullException(nameof(orderRepository));
            _urlsConfig = options?.Value ?? throw new ArgumentNullException(nameof(options.Value));
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

            using var channel = GrpcChannel.ForAddress($"{_urlsConfig.Offers}");
            var client = channel.CreateGrpcService<IOffersService>();
            var grpcRequest = new GetBankAccountNumberRequest { OfferId = order.OrderItems.First().OfferId };
            var grpcResponse = await client.GetBankAccount(grpcRequest);

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
