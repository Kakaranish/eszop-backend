using Newtonsoft.Json;
using Offers.Domain.Exceptions;

namespace Offers.Domain.Aggregates.OfferAggregate
{
    public class KeyValueInfo
    {
        [JsonProperty] public string Key { get; private set; }
        [JsonProperty] public string Value { get; private set; }

        [JsonConstructor]
        public KeyValueInfo(string key, string value)
        {
            SetKey(key);
            SetValue(value);
        }

        public void SetKey(string key)
        {
            ValidateKey(key);
            Key = key;
        }

        public void SetValue(string value)
        {
            ValidateValue(value);
            Value = value;
        }

        #region Validation

        public void ValidateKey(string key)
        {
            if (string.IsNullOrWhiteSpace(key))
                throw new OffersDomainException($"{nameof(Key)} cannot be null or whitespace");
        }

        public void ValidateValue(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new OffersDomainException($"{nameof(Value)} cannot be null or whitespace");
        }

        #endregion
    }
}
