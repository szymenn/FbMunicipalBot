using System.Collections.Generic;

namespace FbRestaurantsBot.Models.Messaging
{
    public class Entry
    {
        public string Id { get; set; }
        public List<FbRestaurantsBot.Models.Messaging.Messaging>  Messaging { get; set; }
    }
}