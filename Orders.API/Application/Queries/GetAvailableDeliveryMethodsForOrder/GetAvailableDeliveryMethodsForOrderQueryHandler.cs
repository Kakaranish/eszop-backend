using Common.Dto;
using Common.Extensions;
using Common.Types;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Orders.API.DataAccess.Repositories;
using Orders.API.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Orders.API.Application.Queries.GetAvailableDeliveryMethodsForOrder
{
    public class GetAvailableDeliveryMethodsForOrderQueryHandler
        : IRequestHandler<GetAvailableDeliveryMethodsForOrderQuery, IList<DeliveryMethodDto>>
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IOrderRepository _orderRepository;
        private readonly HttpContext _httpContext;
        private readonly UrlsConfig _urlsConfig;

        public GetAvailableDeliveryMethodsForOrderQueryHandler(IHttpClientFactory httpClientFactory,
            IHttpContextAccessor httpContextAccessor, IOrderRepository orderRepository, IOptions<UrlsConfig> options)
        {
            _httpClientFactory = httpClientFactory ??
                                 throw new ArgumentNullException(nameof(httpClientFactory));
            _httpContext = httpContextAccessor.HttpContext ??
                           throw new ArgumentNullException(nameof(httpContextAccessor.HttpContext));
            _orderRepository = orderRepository ?? throw new ArgumentNullException(nameof(orderRepository));
            _urlsConfig = options?.Value ?? throw new ArgumentNullException(nameof(options.Value));
        }

        public async Task<IList<DeliveryMethodDto>> Handle(GetAvailableDeliveryMethodsForOrderQuery request, CancellationToken cancellationToken)
        {
            var userId = _httpContext.User.Claims.ToTokenPayload().UserClaims.Id;
            var orderId = Guid.Parse(request.OrderId);

            var order = await _orderRepository.GetByIdAsync(orderId);
            if (order.BuyerId != userId)
                throw new OrdersDomainException("There is no such order");

            var offerIds = order.OrderItems.Select(x => x.OfferId).ToList();
            var queryParams = PrepareQueryParams(offerIds);
            var uri = $"{_urlsConfig.Offers}/api/delivery-methods/offers?{queryParams}";

            var httpClient = _httpClientFactory.CreateClient();
            var accessToken = _httpContext.Request.Headers["Authorization"].ToString().Split(' ')[1];
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            var response = await httpClient.GetAsync(uri,cancellationToken);
            var contentStr = await response.Content.ReadAsStringAsync(cancellationToken);
            var deliveryMethods = JsonConvert.DeserializeObject<IList<DeliveryMethodDto>>(contentStr);

            return deliveryMethods;
        }

        private static string PrepareQueryParams(IList<Guid> offerIds)
        {
            if (offerIds == null || offerIds.Count == 0) return string.Empty;

            var queryParamsBuilder = new StringBuilder();
            for (var i = 0; i < offerIds.Count; i++)
            {
                var offerId = offerIds[i];
                queryParamsBuilder.Append($"ids={offerId}");

                if (i + 1 != offerIds.Count) queryParamsBuilder.Append("&");
            }

            return queryParamsBuilder.ToString();
        }
    }
}
