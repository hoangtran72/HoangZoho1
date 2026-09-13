using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HoangZoho1.Models.Zipfox.ZohoCRM
{

    public class QueryRFQsResponse
    {

        public RFQData[] data { get; set; }

        public Info info { get; set; }

    }

    public class RFQData
    {

        public string id { get; set; }

        public string Name { get; set; }

    }

}
