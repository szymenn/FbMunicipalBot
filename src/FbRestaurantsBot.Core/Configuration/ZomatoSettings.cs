using FbRestaurantsBot.Core.Helpers;
using Newtonsoft.Json;

namespace FbRestaurantsBot.Core.Configuration
{
    [JsonObject(Constants.ZomatoSettings)]
    public class ZomatoSettings
    {
        [JsonProperty("ApiKey")]
        public string ApiKey { get; set; }
    }
}