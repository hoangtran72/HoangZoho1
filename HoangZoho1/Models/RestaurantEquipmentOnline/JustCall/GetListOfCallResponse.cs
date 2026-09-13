using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HoangZoho1.Models.RestaurantEquipmentOnline.JustCall
{

    public class GetListOfCallResponse
    {

        public string status { get; set; }

        public int? count { get; set; }

        public CallDetails[] data { get; set; }

        public string correlation_id { get; set; }

    }

    public class CallDetails
    {

        public int id { get; set; }

        public string contact_number { get; set; }

        public string contact_name { get; set; }

        public string justcall_number { get; set; }

        public string type { get; set; }

        public string status { get; set; }

        public string time { get; set; }

        public string time_utc { get; set; }

        public string duration { get; set; }

        public string friendly_duration { get; set; }

        public string notes { get; set; }

        public string rating { get; set; }

        public string disposition_code { get; set; }

        public string missed_call_type { get; set; }

        public string recording { get; set; }

        public string justcall_agent { get; set; }

        public int agent_id { get; set; }

        public string call_info_url { get; set; }

        public Ivr ivr { get; set; }

        public string direction { get; set; }

        public string recording_child { get; set; }

    }

}
