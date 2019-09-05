using Newtonsoft.Json;

namespace FbRestaurantsBot.Models.Messaging
{
    public class Payload
    {
        [JsonProperty("coordinates")]
        public Coordinates Coordinates { get; set; }
    }
}