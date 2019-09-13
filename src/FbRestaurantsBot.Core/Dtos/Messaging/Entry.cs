using System.Collections.Generic;

namespace FbRestaurantsBot.Core.Dtos.Messaging
{
    public class Entry
    {
        public string Id { get; set; }
        public List<Messaging>  Messaging { get; set; }
    }
}