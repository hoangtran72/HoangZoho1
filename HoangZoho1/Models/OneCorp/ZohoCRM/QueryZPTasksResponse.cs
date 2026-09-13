using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HoangZoho1.Models.OneCorp.ZohoCRM
{

    public class QueryZPTasksResponse
    {

        public QueryZPTasksResponse()
        {

            data = new List<ZPTask>();

        }

        public List<ZPTask> data { get; set; }

        public Info info { get; set; }

    }

    public class ZPTask
    {

        public string id { get; set; }

        public string Project_Name { get; set; }

        public string Project_URL { get; set; }

        public string Task_Name { get; set; }

        public string Task_URL { get; set; }

        public string Task_Status { get; set; }

        public string Task_Status_Color_Code { get; set; }

    }

}
