using Newtonsoft.Json;
using Offers.Domain.Aggregates.OfferAggregate;
using Offers.Domain.Exceptions;
using System.Collections.Generic;

namespace Offers.API.Application.Services
{
    public class RequestKeyValueInfoExtractor : IRequestKeyValueInfoExtractor
    {
        public IList<KeyValueInfo> Extract(string keyValueInfosStr)
        {
            if (keyValueInfosStr == null) return null;

            var extractKeyValueInfos = JsonConvert.DeserializeObject<IList<KeyValueInfo>>(keyValueInfosStr)
                                       ?? throw new OffersDomainException($"'{keyValueInfosStr}' is not parsable");

            return extractKeyValueInfos;
        }
    }
}
