using Newtonsoft.Json;

namespace FbRestaurantsBot.Models.Messaging
{
    public class Response
    {
        [JsonProperty("recipient")]
        public Recipient Recipient;
        [JsonProperty("message")]
        public MessageResponse Message;
    }
}