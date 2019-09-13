using Newtonsoft.Json;

namespace FbRestaurantsBot.Core.Dtos.Messaging
{
    public class Response
    {
        [JsonProperty("recipient")]
        public Recipient Recipient { get; set; }
        [JsonProperty("message")]
        public MessageResponse Message { get; set; }
    }
}