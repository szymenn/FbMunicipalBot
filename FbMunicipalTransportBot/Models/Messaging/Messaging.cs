using System;

namespace FbMunicipalTransportBot.Models.Messaging
{
    public class Messaging
    {
        public Sender Sender { get; set; }
        public Recipient Recipient { get; set; }
        public long Timestamp { get; set; }
        public Message Message { get; set; }
        public Postback Postback { get; set; }
    }
}