using Carts.API.DataAccess.Repositories;
using Carts.API.Domain;
using Carts.API.Extensions;
using Common.Dto;
using Common.Extensions;
using Common.Types;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Carts.API.Application.Commands.FinalizeCart
{
    public class FinalizeCartCommandHandler : IRequestHandler<FinalizeCartCommand, Guid>
    {
        private readonly ILogger<FinalizeCartCommandHandler> _logger;
        private readonly ICartRepository _cartRepository;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly HttpContext _httpContext;
        private readonly UrlsConfig _urlsConfig;

        public FinalizeCartCommandHandler(ILogger<FinalizeCartCommandHandler> logger,
            IHttpContextAccessor httpContextAccessor, ICartRepository cartRepository,
            IHttpClientFactory httpClientFactory, IOptions<UrlsConfig> options)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _httpContext = httpContextAccessor.HttpContext ??
                           throw new ArgumentNullException(nameof(httpContextAccessor.HttpContext));
            _cartRepository = cartRepository ?? throw new ArgumentNullException(nameof(cartRepository));
            _httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
            _urlsConfig = options?.Value ?? throw new ArgumentNullException(nameof(options.Value));
        }

        public async Task<Guid> Handle(FinalizeCartCommand request, CancellationToken cancellationToken)
        {
            var userId = _httpContext.User.Claims.ToTokenPayload().UserClaims.Id;
            var cart = await _cartRepository.GetOrCreateByUserIdAsync(userId);
            if (cart.IsEmpty)
            {
                var msg = $"Cart {cart.Id} cannot be finalized because it's empty";
                _logger.LogError(msg);
                throw new CartsDomainException(msg);
            }

            var httpClient = _httpClientFactory.CreateClient();
            var accessToken = _httpContext.Request.Headers["Authorization"].ToString().Split(' ')[1];
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            var uri = $"{_urlsConfig.Orders}/api/order";
            var content = new StringContent(JsonConvert.SerializeObject(cart.ToDto()), Encoding.UTF8, "application/json");
            var response = await httpClient.PostAsync(uri, content, cancellationToken);
            var responseContent = await response.Content.ReadAsStringAsync(cancellationToken);
            var orderCreatedDto = JsonConvert.DeserializeObject<OrderCreatedDto>(responseContent);

            _logger.LogInformation($"Order {orderCreatedDto.OrderId} created from cart {cart.Id}");

            _cartRepository.Remove(cart);
            await _cartRepository.UnitOfWork.SaveChangesAndDispatchDomainEventsAsync(cancellationToken);

            _logger.LogInformation($"Removed cart {cart.Id}");

            return orderCreatedDto.OrderId;
        }
    }
}
