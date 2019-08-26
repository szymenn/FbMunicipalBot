using System;
using System.Collections.Generic;

namespace FbMunicipalTransportBot.Models
{
    public class Entry
    {
        public string Id { get; set; }
        public List<Messaging.Messaging>  Messaging { get; set; }
    }
}