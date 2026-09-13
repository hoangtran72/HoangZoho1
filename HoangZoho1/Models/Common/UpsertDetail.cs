using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HoangZoho1.Models.Common
{

    public class UpsertDetail
    {

        public DateTime? Modified_Time { get; set; }

        public Modified_By Modified_By { get; set; }

        public DateTime? Created_Time { get; set; }

        public string id { get; set; }

        public Created_By Created_By { get; set; }

    }

    public class Created_By
    {

        public string name { get; set; }

        public string id { get; set; }

        public string email { get; set; }

    }

    public class Modified_By
    {

        public string name { get; set; }

        public string id { get; set; }

        public string email { get; set; }

    }

}
