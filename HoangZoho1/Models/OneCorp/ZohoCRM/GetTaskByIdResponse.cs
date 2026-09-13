using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HoangZoho1.Models.OneCorp.ZohoCRM
{

    public class GetTaskByIdResponse
    {
        public TaskDetails[] data { get; set; }
    }

    public class TaskDetails
    {

        public Owner Owner { get; set; }

        public string Description { get; set; }

        public DateTime? Closed_Time { get; set; }

        public bool? Send_Notification_Email { get; set; }

        public Modified_By Modified_By { get; set; }

        public string id { get; set; }

        public Who_Id Who_Id { get; set; }

        public string Status { get; set; }

        public DateTime? Modified_Time { get; set; }

        public string Due_Date { get; set; }

        public string Priority { get; set; }

        public DateTime? Created_Time { get; set; }

        public string Subject { get; set; }

        public What_Id What_Id { get; set; }

        public Created_By Created_By { get; set; }

        public object[] Tag { get; set; }

        public string LDS_Id { get; set; }

        [JsonProperty("$se_module")]
        public string What_Module { get; set; }

    }

}
