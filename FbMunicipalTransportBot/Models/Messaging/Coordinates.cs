using Newtonsoft.Json;

namespace FbMunicipalTransportBot.Models.Messaging
{
    public class Coordinates
    {
        [JsonProperty("lat")]
        public double Latitude { get; set; }
        [JsonProperty("long")]
        public double Longitude { get; set; }
    }
}