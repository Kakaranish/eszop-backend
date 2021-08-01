using System.Collections.Generic;
using Offers.Domain;
using Offers.Domain.Aggregates.OfferAggregate;

namespace Offers.API.Application.Services
{
    public interface IRequestKeyValueInfoExtractor
    {
        IList<KeyValueInfo> Extract(string keyValueInfosStr);
    }
}
