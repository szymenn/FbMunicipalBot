using FbMunicipalTransportBot.Helpers;
using Newtonsoft.Json;

namespace FbMunicipalTransportBot.Configuration
{
    [JsonObject(Constants.ZomatoSettings)]
    public class ZomatoSettings
    {
        [JsonProperty("ApiKey")]
        public string ApiKey { get; set; }
    }
}