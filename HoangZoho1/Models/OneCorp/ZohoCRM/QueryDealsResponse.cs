using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HoangZoho1.Models.OneCorp.ZohoCRM
{

    public class QueryDealsResponse
    {

        public DealData[] data { get; set; }
        
        public Info info { get; set; }
    
    }

    public class DealData
    {

        public string Deal_Name { get; set; }
        
        public string id { get; set; }
    
    }

}
