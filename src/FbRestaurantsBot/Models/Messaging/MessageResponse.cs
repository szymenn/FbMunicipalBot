using Newtonsoft.Json;

namespace FbRestaurantsBot.Models.Messaging
{
    public class MessageResponse
    {
        [JsonProperty("text")]
        public string Text { get; set; }
    }
}