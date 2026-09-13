using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HoangZoho1.Models.RestaurantEquipmentOnline.ZohoCRM
{

    [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
    public class JustCallLogForCreation
    {

        public string Call_Duration { get; set; }

        public string Owner { get; set; }

        public string Forward_Reason { get; set; }

        public string Email { get; set; }

        public string Description { get; set; }

        public string SMS_Id { get; set; }

        public string Delivery_Status { get; set; }

        public string Direction { get; set; }

        public string Forward_Number { get; set; }

        public string Contact_Number { get; set; }

        public string Call_Status { get; set; }

        public string MMS_Content { get; set; }

        public string JustCall_Number { get; set; }

        public string Recording_URL { get; set; }

        public bool? Is_MMS { get; set; }

        public string Subject { get; set; }

        public string Contact_Name { get; set; }

        public string Call_Id { get; set; }

        public int Call_Duration_in_seconds { get; set; }

        public string Log_Time { get; set; }

        public string SMS_Content { get; set; }

        public string Log_Type { get; set; }

        public string Call_Type { get; set; }

        public string Missed_Call_Type { get; set; }

        public string Call_Notes { get; set; }

        public string IVR_Digit { get; set; }

        public string IVR_Digit_Description { get; set; }

    }

}
