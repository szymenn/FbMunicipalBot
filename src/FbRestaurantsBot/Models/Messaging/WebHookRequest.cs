
using System.Collections.Generic;

namespace FbRestaurantsBot.Models.Messaging
{
    public class WebHookRequest
    {
        public string Object { get; set; }
        public IEnumerable<Entry> Entry { get; set; }
    }
}