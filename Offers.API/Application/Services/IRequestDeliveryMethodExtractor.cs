using Offers.Domain.Aggregates.OfferAggregate;
using System.Collections.Generic;

namespace Offers.API.Application.Services
{
    public interface IRequestDeliveryMethodExtractor
    {
        IList<DeliveryMethod> Extract(string deliveryMethodsStr);
    }
}
