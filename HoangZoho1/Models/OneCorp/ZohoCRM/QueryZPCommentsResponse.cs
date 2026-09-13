using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HoangZoho1.Models.OneCorp.ZohoCRM
{

    public class QueryZPCommentsResponse
    {

        public QueryZPCommentsResponse()
        {

            data = new List<ZPComment>();

        }

        public List<ZPComment> data { get; set; }

        public Info info { get; set; }

    }

    public class Related_Deal
    {

        public string id { get; set; }

    }

    public class Related_Contact
    {

        public string id { get; set; }

    }

}
