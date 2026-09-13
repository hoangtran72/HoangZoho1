using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HoangZoho1.Models.Oratto.Custom
{

    public class ZohoApiResponse
    {

        public string code { get; set; }

        public Details details { get; set; }

        public string message { get; set; }

    }

    public class Details
    {

        public string output { get; set; }

        public string output_type { get; set; }

        public string id { get; set; }

    }


}
