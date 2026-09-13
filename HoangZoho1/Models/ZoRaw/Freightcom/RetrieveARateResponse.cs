using Newtonsoft.Json;
using System.Collections.Generic;

namespace HoangZoho1.Models.ZoRaw.Freightcom
{

    public class RetrieveARateResponse
    {

        public FreightcomStatus status { get; set; }
        
        public List<FreightcomRate> rates { get; set; }
    
    }

    public class FreightcomStatus
    {

        public bool? done { get; set; }
        
        public int? total { get; set; }
        
        public int? complete { get; set; }
    
    }

    public class FreightcomRate
    {

        public string service_id { get; set; }
        
        public FreightcomValidUntil valid_until { get; set; }
        
        public FreightcomTotal total { get; set; }

        [JsonProperty("base")]
        public FreightcomBase _base { get; set; }
        
        public FreightcomSurcharge[] surcharges { get; set; }
        
        public FreightcomTax[] taxes { get; set; }
        
        public int? transit_time_days { get; set; }
        
        public bool? transit_time_not_available { get; set; }
        
        public string carrier_name { get; set; }
        
        public string service_name { get; set; }
    
    }

    public class FreightcomValidUntil
    {
        
        public int? year { get; set; }
        
        public int? month { get; set; }
        
        public int? day { get; set; }
    
    }

    public class FreightcomTotal
    {
        
        public string value { get; set; }
        
        public string currency { get; set; }
    
    }

    public class FreightcomBase
    {
        
        public string value { get; set; }
        
        public string currency { get; set; }
    
    }

    public class FreightcomSurcharge
    {
        
        public string type { get; set; }
        
        public FreightcomAmount amount { get; set; }
    
    }

    public class FreightcomAmount
    {
        
        public string value { get; set; }
        
        public string currency { get; set; }
    
    }

    public class FreightcomTax
    {

        public string type { get; set; }
        
        public FreightcomAmount amount { get; set; }
    
    }

}
