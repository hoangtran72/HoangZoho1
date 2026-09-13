using HoangZoho1.Models.ZoRaw.Freightcom;
using Newtonsoft.Json;

namespace HoangZoho1.Models.ZoRaw.Freightcom
{

    public class CreateFreightcomShipmentRequest
    {

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string unique_id { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string payment_method_id { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string service_id { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public FreightcomShipmentDetails details { get; set; }
    
    }

}
