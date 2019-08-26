
using System.Collections.Generic;

namespace FbMunicipalTransportBot.Models
{
    public class WebHookRequest
    {
        public string Object { get; set; }
        public ICollection<Entry> Entry { get; set; }
    }
}