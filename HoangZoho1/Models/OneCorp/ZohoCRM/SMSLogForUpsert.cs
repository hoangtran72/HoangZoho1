using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HoangZoho1.Models.OneCorp.ZohoCRM
{

    [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
    public class SMSLogForUpsert
    {

        public string Related_Deal { get; set; }

        public string Contact_Email { get; set; }

        public string Contact_First_Name { get; set; }

        public string Contact_Last_Name { get; set; }

        public string Contact_Full_Name { get; set; }

        public string Contact_Mobile { get; set; }

        public string Message_Id { get; set; }

        public string Conversation_Id { get; set; }

        public string Related_Contact { get; set; }

        public string Related_Lead { get; set; }

        public string Sent_Time { get; set; }

        public string Status { get; set; }

        public string Template { get; set; }

        public string Owner { get; set; }

        public decimal? Price { get; set; }

        public string Direction { get; set; }

        public string OneCorp_Number { get; set; }

        public string Phone_Group { get; set; }

        public string Platform { get; set; }

    }

}
