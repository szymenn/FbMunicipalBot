using Newtonsoft.Json;

namespace FbRestaurantsBot.Core.Dtos.Restaurants
{
    public class Location
    {
        [JsonProperty("address")]
        public string Address { get; set; }
        [JsonProperty("locality")]
        public string Locality { get; set; }
        [JsonProperty("city")]
        public string City { get; set; }
    }
}