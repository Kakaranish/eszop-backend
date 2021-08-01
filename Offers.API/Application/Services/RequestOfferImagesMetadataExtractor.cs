using Newtonsoft.Json;
using Offers.API.Application.Types;
using System.Collections.Generic;
using System.Linq;
using Offers.Domain;
using Offers.Domain.Exceptions;

namespace Offers.API.Application.Services
{
    public class RequestOfferImagesMetadataExtractor : IRequestOfferImagesMetadataExtractor
    {
        public Dictionary<string, ImageMetadata> Extract(string imagesMetadata)
        {
            var imagesMetadataList = JsonConvert.DeserializeObject<IList<ImageMetadata>>(imagesMetadata);
            var metadataDict = imagesMetadataList.ToDictionary(x => x.ImageId);

            if (imagesMetadataList == null)
                throw new OffersDomainException("Invalid images metadata");
            if (imagesMetadataList.Count == 0)
                throw new OffersDomainException("Min number of images is 1");
            if (imagesMetadataList.Count > 5)
                throw new OffersDomainException("Max number of images is 5");

            var mainImages = imagesMetadataList.Where(x => x.IsMain).ToList();
            if (mainImages.Count == 0)
                throw new OffersDomainException("No main image indicated");
            if (mainImages.Count > 1)
                throw new OffersDomainException("Possible only 1 main image");

            return metadataDict;
        }
    }
}
