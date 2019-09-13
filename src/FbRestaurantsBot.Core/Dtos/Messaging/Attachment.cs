using Newtonsoft.Json;

namespace FbRestaurantsBot.Core.Dtos.Messaging
{
    public class Attachment
    {
        [JsonProperty("type")]
        public string Type { get; set; }
        [JsonProperty("payload")]
        public Payload Payload { get; set; }
    }
}