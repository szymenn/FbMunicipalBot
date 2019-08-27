using Newtonsoft.Json;

namespace FbMunicipalTransportBot.Models.Messaging
{
    [JsonObject("recipient")]
    public class Recipient
    {
        [JsonProperty("id")]
        public string Id { get; set; }
    }
}