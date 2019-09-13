using System.Collections.Generic;
using Newtonsoft.Json;

namespace FbRestaurantsBot.Core.Dtos.Restaurants
{
    public class Nearby
    {
        [JsonProperty("nearby_restaurants")]
        public ICollection<RestaurantWrapper> Restaurants { get; set; }
    }
}