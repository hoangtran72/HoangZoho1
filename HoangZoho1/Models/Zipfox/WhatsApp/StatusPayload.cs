using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HoangZoho1.Models.Zipfox.WhatsApp
{

    public class StatusPayload
    {

        public string _object { get; set; }
        
        public StatusEntry[] entry { get; set; }
    
    }

    public class StatusEntry
    {

        public string id { get; set; }

        public StatusChange[] changes { get; set; }

    }

    public class StatusChange
    {

        public StatusValue value { get; set; }

        public string field { get; set; }

    }

    public class StatusValue
    {

        public string messaging_product { get; set; }

        public Metadata metadata { get; set; }

        public Status[] statuses { get; set; }

    }

    public class Status
    {

        public string id { get; set; }

        public string status { get; set; }

        public string timestamp { get; set; }

        public string recipient_id { get; set; }

        public Conversation conversation { get; set; }

        public Pricing pricing { get; set; }

    }

    public class Conversation
    {
        public string id { get; set; }
        public Origin origin { get; set; }
    }

    public class Origin
    {

        public string type { get; set; }

    }

    public class Pricing
    {

        public bool billable { get; set; }

        public string pricing_model { get; set; }

        public string category { get; set; }

    }

}
