using Newtonsoft.Json;
using System.Collections.Generic;
using Offers.Domain;
using Offers.Domain.Aggregates.OfferAggregate;
using Offers.Domain.Exceptions;

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
