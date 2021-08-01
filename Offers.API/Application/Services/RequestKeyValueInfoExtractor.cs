using Newtonsoft.Json;
using System.Collections.Generic;
using Offers.Domain;
using Offers.Domain.Aggregates.OfferAggregate;
using Offers.Domain.Exceptions;

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
