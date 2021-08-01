using Newtonsoft.Json;

namespace Offers.API.Application.Types
{
    public class ImageMetadata
    {
        [JsonProperty] public string ImageId { get; init; }
        [JsonProperty] public bool IsRemote { get; init; }
        [JsonProperty] public bool IsMain { get; init; }
        [JsonProperty] public int SortId { get; init; }
    }
}
