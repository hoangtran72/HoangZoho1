using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HoangZoho1.Models.Twilio
{
    public class SendSMSResponse
    {
        public string account_sid { get; set; }

        public string api_version { get; set; }

        public string body { get; set; }

        public string date_created { get; set; }

        public string date_sent { get; set; }

        public string date_updated { get; set; }

        public string direction { get; set; }

        public object error_code { get; set; }

        public object error_message { get; set; }

        public string from { get; set; }

        public string messaging_service_sid { get; set; }

        public string num_media { get; set; }

        public string num_segments { get; set; }

        public object price { get; set; }

        public object price_unit { get; set; }

        public string sid { get; set; }

        public string status { get; set; }

        public Subresource_Uris subresource_uris { get; set; }

        public string to { get; set; }

        public string uri { get; set; }

    }

    public class Subresource_Uris
    {
        public string media { get; set; }
    }
}
