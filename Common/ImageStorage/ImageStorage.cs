using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Common.ImageStorage
{
    public class ImageStorage : IImageStorage
    {
        public const int MaxImageSizeInKb = 2048;
        private readonly IBlobStorage _blobStorage;

        public ImageStorage(IBlobStorage blobStorage)
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

        public async Task<bool> DeleteAsync(string blobName)
        {
            return await _blobStorage.DeleteAsync(blobName);
        }
    }
}
