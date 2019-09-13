using Newtonsoft.Json;

namespace FbRestaurantsBot.Core.Dtos.Messaging
{
    public class Coordinates
    {
        [JsonProperty("lat")]
        public double Latitude { get; set; }
        [JsonProperty("long")]
        public double Longitude { get; set; }
    }
}