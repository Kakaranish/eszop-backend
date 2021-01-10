using Common.Dto;
using Common.Extensions;
using Common.Types;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Orders.API.Application.Dto;
using Orders.API.DataAccess.Repositories;
using Orders.API.Domain;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;

namespace Orders.API.Application.Commands.GetBankTransferDetails
{
    public class GetBankTransferDetailsCommandHandler
        : IRequestHandler<GetBankTransferDetailsCommand, BankTransferDetailsDto>
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly HttpContext _httpContext;
        private readonly UrlsConfig _urlsConfig;

        public GetBankTransferDetailsCommandHandler(IHttpContextAccessor httpContextAccessor,
            IOrderRepository orderRepository, IHttpClientFactory httpClientFactory, IOptions<UrlsConfig> options)
        {
            _httpContext = httpContextAccessor.HttpContext ??
                           throw new ArgumentNullException(nameof(httpContextAccessor.HttpContext));
            _orderRepository = orderRepository ?? throw new ArgumentNullException(nameof(orderRepository));
            _httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
            _urlsConfig = options?.Value ?? throw new ArgumentNullException(nameof(options.Value));
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

            var bankAccountInfoDto = await GetSellerBankAccountInfo(order.SellerId, cancellationToken);
            if (bankAccountInfoDto?.AccountNumber == null)
                throw new OrdersDomainException("Bank account number is not provided by seller");

            var bankTransferDetails = new BankTransferDetailsDto
            {
                Title = $"Order {orderId}",
                TransferAmount = order.TotalPrice,
                AccountNumber = bankAccountInfoDto.AccountNumber
            };

            return bankTransferDetails;
        }

        private async Task<BankAccountInfoDto> GetSellerBankAccountInfo(Guid sellerId, CancellationToken cancellationToken)
        {
            var uri = $"{_urlsConfig.Identity}/api/seller/{sellerId}/bank-account";
            var httpClient = _httpClientFactory.CreateClient();
            var accessToken = _httpContext.Request.Headers["Authorization"].ToString().Split(' ')[1];
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            var response = await httpClient.GetAsync(uri, cancellationToken);
            var responseContent = await response.Content.ReadAsStringAsync(cancellationToken);
            var bankAccountInfoDto = JsonConvert.DeserializeObject<BankAccountInfoDto>(responseContent);

            return bankAccountInfoDto;
        }
    }
}
