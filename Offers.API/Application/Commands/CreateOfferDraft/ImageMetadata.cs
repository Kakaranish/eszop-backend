using Newtonsoft.Json;

namespace Offers.API.Application.Commands.CreateOfferDraft
{
    public class ImageMetadata
    {
        [JsonProperty] public string ImageId { get; set; }
        [JsonProperty] public bool IsRemote { get; set; }
        [JsonProperty] public bool IsMain { get; set; }
        [JsonProperty] public int SortId { get; set; }
    }
}
