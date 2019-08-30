using Newtonsoft.Json;

namespace FbRestaurantsBot.Models.Messaging
{
    public class Attachment
    {
        [JsonProperty("type")]
        public string Type { get; set; }
        [JsonProperty("payload")]
        public Payload Payload { get; set; }
    }
}