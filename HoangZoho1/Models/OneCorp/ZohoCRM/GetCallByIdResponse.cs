using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HoangZoho1.Models.OneCorp.ZohoCRM
{

    public class GetCallByIdResponse
    {

        public CallDetails[] data { get; set; }

    }

    public class CallDetails
    {

        public string Call_Duration { get; set; }

        public Owner Owner { get; set; }

        public string Description { get; set; }

        public Modified_By Modified_By { get; set; }

        public string id { get; set; }

        public string Caller_ID { get; set; }

        public string Call_Status { get; set; }

        public string Call_Type { get; set; }

        public string Call_Purpose { get; set; }

        public string Call_Agenda { get; set; }

        public string Call_Result { get; set; }

        public Who_Id Who_Id { get; set; }

        public DateTime? Modified_Time { get; set; }

        public DateTime? Created_Time { get; set; }

        public DateTime? Call_Start_Time { get; set; }

        public string Subject { get; set; }

        public string Dialled_Number { get; set; }

        [JsonProperty("$se_module")]
        public string se_module { get; set; }

        public What_Id What_Id { get; set; }

        public int? Call_Duration_in_seconds { get; set; }

        public Created_By Created_By { get; set; }

        public string Reminder { get; set; }

        public string[] Tag { get; set; }

    }

    public class What_Id
    {
        public string name { get; set; }
        public string id { get; set; }
    }

    public class Who_Id
    {
        public string name { get; set; }
        public string id { get; set; }
    }

}
