using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace Common.ImageStorage
{
    public interface IImageStorage
    {
        Task<UploadedFileDto> UploadAsync(IFormFile imageFile);
        Task<bool> DeleteAsync(string blobName);
    }
}
