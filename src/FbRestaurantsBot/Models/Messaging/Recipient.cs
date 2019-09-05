using Newtonsoft.Json;

namespace FbRestaurantsBot.Models.Messaging
{
    [JsonObject("recipient")]
    public class Recipient
    {
        [JsonProperty("id")]
        public string Id { get; set; }
    }
}