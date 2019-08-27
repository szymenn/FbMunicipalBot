using FbMunicipalTransportBot.Models.Messaging;
using Newtonsoft.Json;

namespace FbMunicipalTransportBot.Models
{
    public class Response
    {
        [JsonProperty("recipient")]
        public Recipient Recipient;
        [JsonProperty("message")]
        public Message Message;
    }
}