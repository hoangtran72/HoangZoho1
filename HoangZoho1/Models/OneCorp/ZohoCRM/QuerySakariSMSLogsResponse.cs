using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HoangZoho1.Models.OneCorp.ZohoCRM
{

    public class QuerySakariSMSLogsResponse
    {

        public LogData[] data { get; set; }
        
        public Info info { get; set; }
    
    }

    public class LogData
    {

        public string id { get; set; }
        
        public string Name { get; set; }
    
    }

}
