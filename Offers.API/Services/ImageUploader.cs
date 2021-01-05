using Microsoft.AspNetCore.Http;
using Offers.API.Extensions;
using Offers.API.Services.Dto;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Offers.API.Services
{
    public class ImageUploader : IImageUploader
    {
        public const int MaxImageSizeInKb = 2048;
        private readonly IBlobStorage _blobStorage;

        public ImageUploader(IBlobStorage blobStorage)
        {
            _blobStorage = blobStorage ?? throw new ArgumentNullException(nameof(blobStorage));
        }

        public async Task<UploadedFileDto> UploadAsync(IFormFile imageFile)
        {
            if (!imageFile.IsImage()) return null;
            if (!imageFile.HasValidSize(MaxImageSizeInKb)) return null;

            var uploadFilename = Guid.NewGuid() + Path.GetExtension(imageFile.FileName);
            return await _blobStorage.UploadAsync(imageFile.OpenReadStream(), uploadFilename);
        }
    }
}
