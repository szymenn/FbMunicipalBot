using Newtonsoft.Json;

namespace FbRestaurantsBot.Models.Restaurants
{
    public class RestaurantWrapper
    {
        [JsonProperty("restaurant")]
        public Restaurant Restaurant { get; set; }
    }
}