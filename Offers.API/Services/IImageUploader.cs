using Microsoft.AspNetCore.Http;
using Offers.API.Services.Dto;
using System.Threading.Tasks;

namespace Offers.API.Services
{
    public interface IImageUploader
    {
        Task<UploadedFileDto> UploadAsync(IFormFile imageFile);
    }
}
