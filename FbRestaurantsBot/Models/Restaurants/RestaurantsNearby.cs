using System.Collections.Generic;
using Newtonsoft.Json;

namespace FbRestaurantsBot.Models.Restaurants
{
    [JsonObject("nearby_restaurants")]
    public class RestaurantsNearby
    {
        public ICollection<Restaurant> Restaurants { get; set; }
    }
}