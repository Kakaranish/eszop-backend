using Offers.API.Domain;

namespace Offers.API.Services.Dto
{
    public class UploadedFileDto
    {
        public string Uri { get; init; }
        public string Filename { get; init; }
        public string ContainerName { get; init; }
    }

    public static class UploadedFileDtoExtensions
    {
        public static ImageInfo ToImageInfo(this UploadedFileDto uploadedFileDto)
        {
            if (uploadedFileDto == null) return null;
            return new ImageInfo(uploadedFileDto.Filename, uploadedFileDto.ContainerName, uploadedFileDto.Uri);
        }
    }
}
