using Newtonsoft.Json;

namespace FbMunicipalTransportBot.Models.Messaging
{
    [JsonObject("message")]
    public class Message
    {
        [JsonProperty("text")]
        public string Text { get; set; }
    }
}