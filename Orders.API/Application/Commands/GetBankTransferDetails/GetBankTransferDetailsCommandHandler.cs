using Common.Exceptions;
using Common.Extensions;
using Common.Grpc;
using Common.Grpc.Services.IdentityService;
using Common.Grpc.Services.IdentityService.Requests.GetBankAccountNumber;
using Common.Types;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Orders.API.Application.Dto;
using Orders.API.DataAccess.Repositories;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Orders.API.Application.Commands.GetBankTransferDetails
{
    public class GetBankTransferDetailsCommandHandler
        : IRequestHandler<GetBankTransferDetailsCommand, BankTransferDetailsDto>
    {
        private readonly HttpContext _httpContext;
        private readonly IOrderRepository _orderRepository;
        private readonly IGrpcServiceClientFactory<IIdentityService> _grpcServiceClientFactory;
        private readonly ServicesEndpointsConfig _endpointsConfig;

        public GetBankTransferDetailsCommandHandler(IHttpContextAccessor httpContextAccessor,
            IOrderRepository orderRepository, IGrpcServiceClientFactory<IIdentityService> grpcServiceClientFactory,
            IOptions<ServicesEndpointsConfig> options)
        {
            _httpContext = httpContextAccessor.HttpContext ??
                           throw new ArgumentNullException(nameof(httpContextAccessor.HttpContext));
            _orderRepository = orderRepository ?? throw new ArgumentNullException(nameof(orderRepository));
            _grpcServiceClientFactory = grpcServiceClientFactory ?? throw new ArgumentNullException(nameof(grpcServiceClientFactory));
            _endpointsConfig = options.Value ?? throw new ArgumentNullException(nameof(options.Value));
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

            var identityServiceClient = _grpcServiceClientFactory.Create(
                _endpointsConfig.Identity.Grpc.ToString());
            var grpcRequest = new GetBankAccountNumberRequest { UserId = order.SellerId };
            var grpcResponse = await identityServiceClient.GetBankAccount(grpcRequest);

            var bankTransferDetails = new BankTransferDetailsDto
            {
                Title = $"Order {orderId}",
                TransferAmount = order.CalculateTotalPrice(),
                AccountNumber = grpcResponse.BankAccountNumber
            };

            return bankTransferDetails;
        }
    }
}
