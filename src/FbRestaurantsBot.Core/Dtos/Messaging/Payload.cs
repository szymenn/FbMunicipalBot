using Newtonsoft.Json;

namespace FbRestaurantsBot.Core.Dtos.Messaging
{
    public class Payload
    {
        [JsonProperty("coordinates")]
        public Coordinates Coordinates { get; set; }
    }
}