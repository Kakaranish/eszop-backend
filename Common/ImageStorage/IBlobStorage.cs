using System.IO;
using System.Threading.Tasks;

namespace Common.ImageStorage
{
    public interface IBlobStorage
    {
        string ContainerName { get; }
        Task<UploadedFileDto> UploadAsync(Stream content, string blobName);
        Task<bool> DeleteAsync(string blobName);
        Task<Stream> DownloadAsync(string blobName);
    }
}
