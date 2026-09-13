using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HoangZoho1.Models.RestaurantEquipmentOnline.JustCall
{

    public class GetUserByIdResponse
    {

        public string status { get; set; }

        public Data data { get; set; }

        public string correlation_id { get; set; }

    }

    public class Data
    {

        public int? agent_id { get; set; }

        public int? owner_id { get; set; }

        public string firstname { get; set; }

        public string lastname { get; set; }

        public string email { get; set; }

        public int? on_call { get; set; }

        public string last_login { get; set; }

        public int? availability { get; set; }

        public Number[] numbers { get; set; }

    }

    public class Number
    {

        public int? id { get; set; }

        public string friendly_name { get; set; }

        public string phone { get; set; }

        public string custom_name { get; set; }

        public int? agent_id { get; set; }

        public Capabilities capabilities { get; set; }

    }

    public class Capabilities
    {

        public string sms { get; set; }

        public string mms { get; set; }

        public string fax { get; set; }

        public string voice { get; set; }

    }

}
