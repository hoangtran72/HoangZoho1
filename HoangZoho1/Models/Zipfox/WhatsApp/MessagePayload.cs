using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HoangZoho1.Models.Zipfox.WhatsApp
{

    public class MessagePayload
    {

        [JsonProperty("object")]
        public string _object { get; set; }

        public MessageEntry[] entry { get; set; }

    }

    public class MessageEntry
    {

        public string id { get; set; }

        public MessageChange[] changes { get; set; }

    }

    public class MessageChange
    {

        public MessageValue value { get; set; }

        public string field { get; set; }

    }

    public class MessageValue
    {

        public string messaging_product { get; set; }

        public Metadata metadata { get; set; }

        public WhatsAppContact[] contacts { get; set; }

        public WhatsAppMessage[] messages { get; set; }

    }

    public class Metadata
    {

        public string display_phone_number { get; set; }

        public string phone_number_id { get; set; }

    }

    public class WhatsAppContact
    {

        public Profile profile { get; set; }

        public string wa_id { get; set; }

    }

    public class Profile
    {

        public string name { get; set; }

    }

    public class WhatsAppMessage
    {

        public string from { get; set; }

        public string id { get; set; }

        public string timestamp { get; set; }

        public Text text { get; set; }

        public Button button { get; set; }

        public string type { get; set; }

    }

    public class Text
    {

        public string body { get; set; }

    }

    public class Button
    {

        public string payload { get; set; }

        public string text { get; set; }

    }


}
