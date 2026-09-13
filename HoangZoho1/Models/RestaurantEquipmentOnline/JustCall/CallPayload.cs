using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HoangZoho1.Models.RestaurantEquipmentOnline.JustCall
{

    public class CallPayload
    {

        public CallData data { get; set; }

    }

    public class CallData
    {

        public string type { get; set; }

        public string subject { get; set; }

        public string description { get; set; }

        public string direction { get; set; }

        public string called_via { get; set; }

        public string contact_name { get; set; }

        public string contact_number { get; set; }

        public string contact_email { get; set; }

        public string recording_url { get; set; }

        public string call_status { get; set; }

        public string call_duration { get; set; }

        public int? call_duration_sec { get; set; }

        public int? is_contact { get; set; }

        public Forwarded_Number forwarded_number { get; set; }

        public string signature { get; set; }

        public string datetime { get; set; }

        public string agent_name { get; set; }

        public int? agent_id { get; set; }

        public string requestid { get; set; }

        public string recordingmp3 { get; set; }

        public string callinfo { get; set; }

        public string call_sid { get; set; }

        public string callid { get; set; }

        public Ivr ivr { get; set; }

        public string missed_call_type { get; set; }

        public string notes { get; set; }

    }

    public class Forwarded_Number
    {

        public string number { get; set; }

        public string reason { get; set; }

        public int reason_code { get; set; }

    }

    public class Ivr
    {

        public string digit { get; set; }

        public string digit_description { get; set; }

    }

}
