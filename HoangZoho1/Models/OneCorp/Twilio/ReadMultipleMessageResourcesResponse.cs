using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HoangZoho1.Models.OneCorp.Twilio
{

    public class ReadMultipleMessageResourcesResponse
    {

        public string first_page_uri { get; set; }

        public int? end { get; set; }

        public string previous_page_uri { get; set; }

        public TwilioMessage[] messages { get; set; }

        public string uri { get; set; }

        public int? page_size { get; set; }

        public int? start { get; set; }

        public string next_page_uri { get; set; }

        public int? page { get; set; }

    }

    public class TwilioMessage
    {

        public string body { get; set; }

        public string num_segments { get; set; }

        public string direction { get; set; }

        public string from { get; set; }

        public string date_updated { get; set; }

        public string price { get; set; }

        public string error_message { get; set; }

        public string uri { get; set; }

        public string account_sid { get; set; }

        public string num_media { get; set; }

        public string to { get; set; }

        public string date_created { get; set; }

        public string status { get; set; }

        public string sid { get; set; }

        public string date_sent { get; set; }

        public object messaging_service_sid { get; set; }

        public int? error_code { get; set; }

        public string price_unit { get; set; }

        public string api_version { get; set; }

        public Subresource_Uris subresource_uris { get; set; }

    }

    public class Subresource_Uris
    {

        public string media { get; set; }

        public string feedback { get; set; }

    }

}
