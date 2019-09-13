using System.Collections.Generic;

namespace FbRestaurantsBot.Core.Dtos.Messaging
{
    public class WebHookRequest
    {
        public string Object { get; set; }
        public ICollection<Entry> Entry { get; set; }
    }
}