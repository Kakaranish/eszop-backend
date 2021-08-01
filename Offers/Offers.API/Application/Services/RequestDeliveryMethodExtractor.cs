using Newtonsoft.Json;
using Offers.Domain.Aggregates.OfferAggregate;
using Offers.Domain.Exceptions;
using System.Collections.Generic;

namespace Offers.API.Application.Services
{
    public class RequestDeliveryMethodExtractor : IRequestDeliveryMethodExtractor
    {
        public IList<DeliveryMethod> Extract(string deliveryMethodsStr)
        {
            var extractedDeliveryMethods = JsonConvert.DeserializeObject<IList<DeliveryMethod>>(deliveryMethodsStr)
                                           ?? throw new OffersDomainException($"'{deliveryMethodsStr}' is not parsable");

            return extractedDeliveryMethods;
        }
    }
}
