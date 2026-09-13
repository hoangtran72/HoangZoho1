using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HoangZoho1.Models.RestaurantEquipmentOnline.ZohoCRM
{


    public class SearchJustCallLogsResponse
    {

        public JustCallLogDetails[] data { get; set; }

        public Info info { get; set; }

    }

    public class JustCallLogDetails
    {

        public string Call_Duration { get; set; }

        public Owner Owner { get; set; }

        public string Forward_Reason { get; set; }

        public string Email { get; set; }

        public string Description { get; set; }

        public string SMS_Id { get; set; }

        public string Delivery_Status { get; set; }

        public string Direction { get; set; }

        public string Name { get; set; }

        public DateTime Last_Activity_Time { get; set; }

        public Modified_By Modified_By { get; set; }

        public string Forward_Number { get; set; }

        public string Contact_Number { get; set; }

        public string id { get; set; }

        public string Call_Status { get; set; }

        public string MMS_Content { get; set; }

        public string JustCall_Number { get; set; }

        public string Recording_URL { get; set; }

        public DateTime? Modified_Time { get; set; }

        public DateTime? Created_Time { get; set; }

        public bool? Is_MMS { get; set; }

        public string Subject { get; set; }

        public string Contact_Name { get; set; }

        public string Call_Id { get; set; }

        public int Call_Duration_in_seconds { get; set; }

        public DateTime? Log_Time { get; set; }

        public Created_By Created_By { get; set; }

        public string SMS_Content { get; set; }

    }

}
