using System;
using System.IO;
using System.Threading.Tasks;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.Extensions.Options;

namespace Common.Utilities.ImageStorage
{
    public class AzureBlobStorage : IBlobStorage
    {
        private readonly AzureStorageConfig _azureConfig;
        private readonly BlobContainerClient _containerClient;
        private bool _ensured = false;

        public AzureBlobStorage(IOptions<AzureStorageConfig> azureOptions)
        {
            _azureConfig = azureOptions?.Value ?? throw new ArgumentNullException(nameof(azureOptions.Value));
            _containerClient = new BlobContainerClient(_azureConfig.ConnectionString, _azureConfig.ContainerName);
        }

        public string ContainerName => _azureConfig.ContainerName;

        public async Task<UploadedFileDto> UploadAsync(Stream content, string blobName)
        {
            await EnsureContainerExistsAsync();

            var blobClient = _containerClient.GetBlobClient(blobName);
            await blobClient.UploadAsync(content);

            return new UploadedFileDto
            {
                Uri = blobClient.Uri.ToString(),
                ContainerName = ContainerName,
                Filename = blobName
            };
        }

        public async Task<bool> DeleteAsync(string blobName)
        {
            await EnsureContainerExistsAsync();

            var blobClient = _containerClient.GetBlobClient(blobName);
            var result = await blobClient.DeleteIfExistsAsync(DeleteSnapshotsOption.IncludeSnapshots);

            return result.Value;
        }

        public async Task<Stream> DownloadAsync(string blobName)
        {
            await EnsureContainerExistsAsync();

            var blobClient = _containerClient.GetBlobClient(blobName);
            var downloadInfo = await blobClient.DownloadAsync();

            return downloadInfo.Value.Content;
        }

        private async Task EnsureContainerExistsAsync()
        {
            if (_ensured) return;

            if (!await _containerClient.ExistsAsync())
            {
                throw new InvalidOperationException($"Blob container '{ContainerName}' does not exist");
            }

            _ensured = true;
        }
    }
}
