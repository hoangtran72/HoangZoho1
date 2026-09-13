using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HoangZoho1.Models.Zipfox.ZohoCRM
{

    [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
    public class WhatsAppLogForCreation
    {

        public string Message_Id { get; set; }

        public string Owner { get; set; }

        public string Direction { get; set; }

        public string Status { get; set; }

        public string From_Phone_Number { get; set; }

        public string To_Phone_Number { get; set; }

        public string Conversation_Id { get; set; }

        public string Related_Contact { get; set; }

        public string Sent_Time { get; set; }

        public string Read_Time { get; set; }

        public string Received_Time { get; set; }

        public string Message_Content { get; set; }

    }

}
