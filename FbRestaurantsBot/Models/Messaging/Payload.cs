using Newtonsoft.Json;

namespace FbMunicipalTransportBot.Models.Messaging
{
    public class Payload
    {
        [JsonProperty("coordinates")]
        public Coordinates Coordinates { get; set; }
    }
}