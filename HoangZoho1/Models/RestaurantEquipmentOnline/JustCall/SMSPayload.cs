using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HoangZoho1.Models.RestaurantEquipmentOnline.JustCall
{

    public class SMSPayload
    {

        public SmsData data { get; set; }

    }

    public class SmsData
    {

        public SmsData()
        {
            mms = new List<Mms>();
        }

        public string type { get; set; }

        public string direction { get; set; }

        public string justcall_number { get; set; }

        public string contact_name { get; set; }

        public string contact_number { get; set; }

        public string contact_email { get; set; }

        public int? is_contact { get; set; }

        public string content { get; set; }

        public string signature { get; set; }

        public string datetime { get; set; }

        public string delivery_status { get; set; }

        public string requestid { get; set; }

        public int? messageid { get; set; }

        public string is_mms { get; set; }

        public List<Mms> mms { get; set; }

        public string agent_name { get; set; }

        public int? agent_id { get; set; }

    }

    public class Mms
    {

        public string media_url { get; set; }

        public string content_type { get; set; }

    }

}
