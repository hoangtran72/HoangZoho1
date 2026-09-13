using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HoangZoho1.Models.Lumicare.ZohoCRM
{


    public class SearchDistributorsResponse
    {

        public DistributorData[] data { get; set; }

        public Info info { get; set; }

    }

    public class DistributorData
    {
        public Owner Owner { get; set; }

        public string Name { get; set; }

        public DateTime? Last_Activity_Time { get; set; }

        public Modified_By Modified_By { get; set; }

        public string id { get; set; }

        public string Password { get; set; }

        public DateTime? Modified_Time { get; set; }

        public DateTime? Created_Time { get; set; }

        public string Username { get; set; }

        public Created_By Created_By { get; set; }
        
        public object[] Tag { get; set; }

    }

}
