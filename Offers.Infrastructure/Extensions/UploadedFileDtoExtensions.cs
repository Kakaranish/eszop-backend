using Common.ImageStorage;
using Offers.Domain.Aggregates.OfferAggregate;

namespace Offers.Infrastructure.Extensions
{
    public static class UploadedFileDtoExtensions
    {
        public static ImageInfo ToImageInfo(this UploadedFileDto uploadedFileDto)
        {
            if (uploadedFileDto == null) return null;
            return new ImageInfo(uploadedFileDto.Filename, uploadedFileDto.ContainerName, uploadedFileDto.Uri);
        }
    }
}
