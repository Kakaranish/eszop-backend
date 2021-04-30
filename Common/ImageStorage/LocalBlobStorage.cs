using System;
using System.IO;
using System.Threading.Tasks;

namespace Common.ImageStorage
{
    public class LocalBlobStorage : IBlobStorage
    {
        private readonly string _uploadDir;

        public string ContainerName => "wwwroot";

        public LocalBlobStorage(string uploadDir)
        {
            if (!Directory.Exists(uploadDir))
                throw new DirectoryNotFoundException($"{uploadDir} does not exist");
            
            _uploadDir = uploadDir ?? throw new ArgumentNullException(nameof(uploadDir));
        }

        public async Task<UploadedFileDto> UploadAsync(Stream content, string blobName)
        {
            var path = Path.Combine(_uploadDir, blobName);
            await using var stream = new FileStream(path, FileMode.Create);
            await content.CopyToAsync(stream);

            return new UploadedFileDto
            {
                Uri = $"/{ContainerName}/{blobName}",
                ContainerName = ContainerName,
                Filename = blobName
            };
        }

        public Task<bool> DeleteAsync(string blobName)
        {
            var path = Path.Combine(_uploadDir, blobName);

            var fileExists = File.Exists(path);
            if (fileExists) File.Delete(blobName);

            return Task.FromResult(fileExists);
        }

        public Task<Stream> DownloadAsync(string blobName)
        {
            var path = Path.Combine(_uploadDir, blobName);
            if (!File.Exists(path)) return null;

            var file = new FileStream(path, FileMode.Open);
            return Task.FromResult((Stream)file);
        }
    }
}
