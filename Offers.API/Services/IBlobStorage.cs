using System.IO;
using System.Threading.Tasks;

namespace Offers.API.Services
{
    public interface IBlobStorage
    {
        string ContainerName { get; }
        Task UploadAsync(Stream content, string blobName);
        Task<Stream> DownloadAsync(string blobName);
    }
}
