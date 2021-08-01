using Offers.Domain.Aggregates.OfferAggregate;
using System.Collections.Generic;

namespace Offers.API.Application.Services
{
    public interface IRequestKeyValueInfoExtractor
    {
        IList<KeyValueInfo> Extract(string keyValueInfosStr);
    }
}
