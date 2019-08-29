using Newtonsoft.Json;

namespace FbMunicipalTransportBot.Models.Restaurants
{
    [JsonObject("restaurant")]
    public class Restaurant
    {
        [JsonProperty("name")]
        public string Name { get; set; }  
        [JsonProperty("url")]
        public string Url { get; set; }
        [JsonProperty("location")]
        public Location Location { get; set; }
    }
}