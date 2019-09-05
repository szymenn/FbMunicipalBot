using System.Collections.Generic;
using Newtonsoft.Json;

namespace FbRestaurantsBot.Models.Messaging
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