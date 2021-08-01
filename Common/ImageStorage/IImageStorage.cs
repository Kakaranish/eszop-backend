using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Common.Utilities.ImageStorage
{
    public interface IImageStorage
    {
        Task<UploadedFileDto> UploadAsync(IFormFile imageFile);
        Task<bool> DeleteAsync(string blobName);
    }
}
