using Microsoft.AspNetCore.Http;

namespace Offers.API.Application.Types
{
    public class ImageToUpload
    {
        public string Id { get; init; }
        public IFormFile File { get; init; }
        public ImageMetadata Metadata { get; init; }
    }
}
