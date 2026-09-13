using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HoangZoho1.Models.Zipfox.ZohoCRM
{

    public class QueryContactsResponse
    {

        public QueryContactData[] data { get; set; }

        public Info info { get; set; }

    }

    public class QueryContactData
    {

        public string First_Name { get; set; }

        public string Last_Name { get; set; }

        public string id { get; set; }

    }


}
