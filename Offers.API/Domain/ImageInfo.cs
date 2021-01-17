using System;

namespace Offers.API.Domain
{
    public class ImageInfo
    {
        public Guid Id { get; set; }
        public int? SortId { get; private set; }
        public string Filename { get; private set; }
        public string ContainerName { get; private set; }

        protected ImageInfo()
        {
        }

        public ImageInfo(string filename, string containerName, int? sortId = default)
        {
            Id = Guid.NewGuid();
            SetFilename(filename);
            SetContainerName(containerName);
            SetSortId(sortId);
        }

        public void SetFilename(string filename)
        {
            ValidateFilename(filename);
            Filename = filename;
        }

        public void SetContainerName(string containerName)
        {
            ValidateContainerName(containerName);
            ContainerName = containerName;
        }

        public void SetSortId(int? sortId)
        {
            SortId = sortId;
        }

        #region Validation

        private void ValidateFilename(string filename)
        {
            if (string.IsNullOrWhiteSpace(filename))
                throw new OffersDomainException($"{nameof(filename)} cannot be null or whitespace");
        }

        private void ValidateContainerName(string containerName)
        {
            if (string.IsNullOrWhiteSpace(containerName))
                throw new OffersDomainException($"{nameof(containerName)} cannot be null or whitespace");
        }

        #endregion
    }
}
