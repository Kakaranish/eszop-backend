using Newtonsoft.Json;
using Offers.Domain.Exceptions;
using System;

namespace Offers.Domain.Aggregates.OfferAggregate
{
    public class ImageInfo : ICloneable
    {
        [JsonProperty] public Guid Id { get; set; }
        [JsonProperty] public string Uri { get; private set; }
        [JsonProperty] public string Filename { get; private set; }
        [JsonProperty] public string ContainerName { get; private set; }
        [JsonProperty] public int? SortId { get; private set; }
        [JsonProperty] public bool IsMain { get; private set; }

        [JsonConstructor]
        protected ImageInfo()
        {
        }

        public ImageInfo(string filename, string containerName, string uri)
        {
            Id = Guid.NewGuid();
            SetUri(uri);
            SetFilename(filename);
            SetContainerName(containerName);
        }

        private void SetUri(string uri)
        {
            ValidateUri(uri);
            Uri = uri;
        }

        private void SetFilename(string filename)
        {
            ValidateFilename(filename);
            Filename = filename;
        }

        private void SetContainerName(string containerName)
        {
            ValidateContainerName(containerName);
            ContainerName = containerName;
        }

        public void SetSortId(int? sortId)
        {
            SortId = sortId;
        }

        public void SetIsMain(bool value)
        {
            IsMain = value;
        }

        public object Clone()
        {
            return MemberwiseClone();
        }

        #region Validation

        private void ValidateUri(string uri)
        {
            if (string.IsNullOrWhiteSpace(uri))
                throw new OffersDomainException($"{nameof(uri)} cannot be null or whitespace");
        }

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
