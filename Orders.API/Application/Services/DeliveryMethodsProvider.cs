using Common.Dto;
using Common.Types;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Orders.API.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Orders.API.Application.Services
{
    public class DeliveryMethodsProvider : IDeliveryMethodsProvider
    {
        private readonly UrlsConfig _urlsConfig;
        private readonly IHttpClientFactory _httpClientFactory;

        public DeliveryMethodsProvider(IOptions<UrlsConfig> options, IHttpClientFactory httpClientFactory)
        {
            _urlsConfig = options.Value ?? throw new ArgumentNullException(nameof(options.Value));
            _httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
        }

        public async Task<IList<DeliveryMethodDto>> Get(Order order)
        {
            var offerIds = order.OrderItems.Select(x => x.OfferId).ToList();
            var queryParams = PrepareQueryParams(offerIds);
            var uri = $"{_urlsConfig.Offers}/api/delivery-methods/offers?{queryParams}";

            var httpClient = _httpClientFactory.CreateClient();

            var response = await httpClient.GetAsync(uri);
            var contentStr = await response.Content.ReadAsStringAsync();
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
