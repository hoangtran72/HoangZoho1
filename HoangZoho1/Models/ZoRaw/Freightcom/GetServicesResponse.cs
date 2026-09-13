using System.Collections.Generic;

namespace HoangZoho1.Models.ZoRaw.Freightcom
{

    public class GetServicesResponse
    {
        
        public List<FreightcomService> Property1 { get; set; }
    
    }

    public class FreightcomService
    {

        public string id { get; set; }
        
        public string carrier_name { get; set; }
        
        public string service_name { get; set; }
    
    }

}
