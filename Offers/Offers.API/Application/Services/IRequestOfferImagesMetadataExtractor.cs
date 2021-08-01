using Offers.API.Application.Types;
using System.Collections.Generic;

namespace Offers.API.Application.Services
{
    public interface IRequestOfferImagesMetadataExtractor
    {
        Dictionary<string, ImageMetadata> Extract(string imagesMetadata);
    }
}
