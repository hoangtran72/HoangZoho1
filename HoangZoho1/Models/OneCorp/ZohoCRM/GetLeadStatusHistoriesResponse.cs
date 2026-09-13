using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HoangZoho1.Models.OneCorp.ZohoCRM
{

    public class GetLeadStatusHistoriesResponse
    {

        public StatusDetails[] data { get; set; }

        public Info info { get; set; }

    }

    public class StatusDetails
    {

        public DateTime? Modified_Time { get; set; }

        public string Email { get; set; }

        public object Rating { get; set; }

        public int? Duration_Days { get; set; }

        public string Mobile { get; set; }

        public DateTime? Last_Activity_Time { get; set; }

        public string Lead_Status { get; set; }

        public Full_Name Full_Name { get; set; }

        public Modified_By Modified_By { get; set; }

        public string id { get; set; }

    }

    public class Full_Name
    {

        public string name { get; set; }

        public string id { get; set; }

    }

}
