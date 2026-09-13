using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HoangZoho1.Models.Zipfox.ZohoCRM
{

    public class QueryWhatsAppLogsResponse
    {
        
        public QueryLogData[] data { get; set; }

        public QueryInfo info { get; set; }

    }

    public class QueryInfo
    {

        public int? count { get; set; }

        public bool? more_records { get; set; }

    }

    public class QueryLogData
    {

        public string id { get; set; }
        
        public string Name { get; set; }

    }

}
