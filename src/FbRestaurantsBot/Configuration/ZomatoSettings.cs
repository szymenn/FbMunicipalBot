using FbRestaurantsBot.Helpers;
using Newtonsoft.Json;

namespace FbRestaurantsBot.Configuration
{
    [JsonObject(Constants.ZomatoSettings)]
    public class ZomatoSettings
    {
        [JsonProperty("ApiKey")]
        public string ApiKey { get; set; }
    }
}