using Microsoft.AspNetCore.Http;
using Offers.API.Services.Dto;
using System.Threading.Tasks;

namespace Offers.API.Services
{
    public interface IImageStorage
    {
        Task<UploadedFileDto> UploadAsync(IFormFile imageFile);
        Task<bool> DeleteAsync(string blobName);
    }
}
