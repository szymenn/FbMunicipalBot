using Newtonsoft.Json;

namespace FbRestaurantsBot.Core.Dtos.Restaurants
{
    public class RestaurantWrapper
    {
        [JsonProperty("restaurant")]
        public Restaurant Restaurant { get; set; }
    }
}