using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HoangZoho1.Models.RestaurantEquipmentOnline.JustCall
{

    public class GetListOfSMSResponse
    {

        public string status { get; set; }

        public int count { get; set; }

        public SMSDetails[] data { get; set; }

        public string correlation_id { get; set; }

    }

    public class SMSDetails
    {

        public SMSDetails()
        {
            mms = new List<Mms>();
        }

        public int? id { get; set; }

        public string client_number { get; set; }

        public string justcall_number { get; set; }

        public string body { get; set; }

        public string direction { get; set; }

        public string is_mms { get; set; }

        public List<Mms> mms { get; set; }

        public string contact_name { get; set; }

        public string datetime { get; set; }

        public string agent_name { get; set; }

        public int? agent_id { get; set; }

        public string delivery_status { get; set; }

        public bool is_deleted { get; set; }

    }

}
