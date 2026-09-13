using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HoangZoho1.Models.Common
{

    public class ZohoDeleteResponse
    {

        public DeleteData[] data { get; set; }

    }

    public class DeleteData
    {

        public string code { get; set; }

        public DeleteDetails details { get; set; }

        public string message { get; set; }

        public string status { get; set; }

    }

    public class DeleteDetails
    {

        public string id { get; set; }

    }

}
