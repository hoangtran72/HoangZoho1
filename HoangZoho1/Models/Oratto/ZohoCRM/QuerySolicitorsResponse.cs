using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HoangZoho1.Models.Oratto.ZohoCRM
{


    public class QuerySolicitorsResponse
    {

        public QuerySolicitorData[] data { get; set; }

        public Info info { get; set; }

    }

    public class Info
    {

        public int? count { get; set; }
        
        public bool more_records { get; set; }
    
    }

    public class QuerySolicitorData
    {

        public string First_Name { get; set; }

        public string Last_Name { get; set; }

        public string Law_Firm_Name { get; set; }

        public string Title { get; set; }

        public bool? Oratto_Lawyer_Shortlist { get; set; }

        public string Matter_Expertise { get; set; }

        public string Sub_Categories { get; set; }

        public string id { get; set; }

    }

}
