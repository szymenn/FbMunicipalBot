using Newtonsoft.Json;

namespace FbMunicipalTransportBot.Configuration
{
    [JsonObject("FacebookSettings")]
    public class FacebookSettings
    {
        [JsonProperty("Secret")]
        public string Secret { get; set; }
        [JsonProperty("VerifyToken")]
        public string VerifyToken { get; set; }
        [JsonProperty("Mode")]
        public string Mode { get; set; }
        [JsonProperty("Object")]
        public string Object { get; set; }
    }
}