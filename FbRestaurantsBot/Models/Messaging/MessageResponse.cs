using Newtonsoft.Json;

namespace FbMunicipalTransportBot.Models.Messaging
{
    public class MessageResponse
    {
        [JsonProperty("text")]
        public string Text { get; set; }
    }
}