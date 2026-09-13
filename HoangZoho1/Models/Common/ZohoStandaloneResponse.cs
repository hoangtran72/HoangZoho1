using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HoangZoho1.Models.Common
{

    public class ZohoStandaloneResponse<T>
    {

        public string code { get; set; }
        
        public Details<T> details { get; set; }
        
        public string message { get; set; }
    
    }

    public class Details<T>
    {

        public Details()
        {

            userMessage = new List<string>();

        }

        public T output { get; set; }
        
        public List<string> userMessage { get; set; }
        
        public string output_type { get; set; }
        
        public string id { get; set; }
    
    }

}
