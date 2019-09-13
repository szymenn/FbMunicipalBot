using Newtonsoft.Json;

namespace FbRestaurantsBot.Core.Dtos.Messaging
{
    [JsonObject("recipient")]
    public class Recipient
    {
        [JsonProperty("id")]
        public string Id { get; set; }
    }
}