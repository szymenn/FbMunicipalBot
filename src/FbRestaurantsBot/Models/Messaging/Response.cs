using Newtonsoft.Json;

namespace FbRestaurantsBot.Models.Messaging
{
    public class Response
    {
        [JsonProperty("recipient")]
        public Recipient Recipient { get; set; }
        [JsonProperty("message")]
        public MessageResponse Message { get; set; }
    }
}