using System.Collections.Generic;
using Newtonsoft.Json;

namespace FbRestaurantsBot.Core.Dtos.Messaging
{
    [JsonObject("message")]
    public class Message
    {
        [JsonProperty("text")]
        public string Text { get; set; }
        [JsonProperty("attachments")]
        public List<Attachment> Attachments { get; set; }
    }
}