using System.Collections.Generic;

namespace HoangZoho1.Models.ZoRaw.ZohoInventory
{

    public class CreateShipmentRequest
    {

        public CreateShipmentRequest()
        {

            shipmentorder_custom_fields = new List<Custom_Field>();

        }
        public string date { get; set; }
        
        public string reference_number { get; set; }
        
        public string delivery_method { get; set; }
        
        public string tracking_number { get; set; }

        public string tracking_link { get; set; }

        public decimal? shipping_charge { get; set; }
        
        public decimal? exchange_rate { get; set; }
        
        public string notes { get; set; }

        public List<Custom_Field> shipmentorder_custom_fields { get; set; }

    }

    public class Custom_Field
    {

        public string customfield_id { get; set; }

        public string value { get; set; }

    }

}
