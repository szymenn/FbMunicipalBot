using Newtonsoft.Json;

namespace FbRestaurantsBot.Core.Dtos.Messaging
{
    public class MessageResponse
    {
        [JsonProperty("text")]
        public string Text { get; set; }
    }
}