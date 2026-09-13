using System;
using System.Collections.Generic;

namespace HoangZoho1.Models.ZoRaw.Stallion
{

    public class CreateStallionShipmentResponse
    {

        public bool? success { get; set; }
        
        public CreateShipment shipment { get; set; }
        
        public CreateShipmentRate rate { get; set; }
        
        public string label { get; set; }
        
        public string tracking_code { get; set; }
        
        public string message { get; set; }
    
    }

    public class CreateShipment
    {
        
        public string return_name { get; set; }
        
        public string ship_code { get; set; }
        
        public object batch_id { get; set; }
        
        public object closeout_id { get; set; }
        
        public string name { get; set; }
        
        public string address1 { get; set; }
        
        public object address2 { get; set; }
        
        public string city { get; set; }
        
        public object email { get; set; }
        
        public string postal_code { get; set; }
        
        public string province_code { get; set; }
        
        public string country_code { get; set; }
        
        public object phone { get; set; }
        
        public string package_contents { get; set; }
        
        public float? value { get; set; }
        
        public string currency { get; set; }
        
        public float? length { get; set; }
        
        public float? width { get; set; }
        
        public float? height { get; set; }
        
        public string size_unit { get; set; }
        
        public string package_type { get; set; }
        
        public string postage_type { get; set; }
        
        public string region { get; set; }
        
        public string status { get; set; }
        
        public float? weight { get; set; }
        
        public string weight_unit { get; set; }
        
        public string tracking_code { get; set; }
        
        public float? rate { get; set; }
        
        public float? tax { get; set; }
        
        public float? total_paid { get; set; }
        
        public List<CreateShipmentItem> items { get; set; }
        
        public bool? purchase_label { get; set; }
        
        public bool? insured { get; set; }
        
        public object store { get; set; }
        
        public object order_id { get; set; }
        
        public object display_order_id { get; set; }
        
        public DateTime? created_at { get; set; }
    
    }

    public class CreateShipmentItem
    {
        
        public object sku { get; set; }
        
        public float? value { get; set; }
        
        public string currency { get; set; }
        
        public int quantity { get; set; }
        
        public string description { get; set; }

    }

    public class CreateShipmentRate
    {
        
        public int? postage_type_id { get; set; }
        
        public string postage_type { get; set; }
        
        public int? trackable { get; set; }
        
        public string package_type { get; set; }
        
        public string base_rate { get; set; }
        
        public object[] add_ons { get; set; }
        
        public string rate { get; set; }
        
        public string gst { get; set; }
        
        public string pst { get; set; }
        
        public string hst { get; set; }
        
        public string qst { get; set; }
        
        public string tax { get; set; }
        
        public string duty { get; set; }
        
        public string duty_tax { get; set; }
        
        public string total { get; set; }
        
        public string currency { get; set; }
        
        public string delivery_days { get; set; }
        
        public CreateShipmentInductionAddress induction_address { get; set; }
        
        public CreateShipmentReturnAddress return_address { get; set; }

    }

    public class CreateShipmentInductionAddress
    {
        
        public string name { get; set; }
        
        public object company { get; set; }
        
        public string address1 { get; set; }
        
        public string address2 { get; set; }
        
        public string city { get; set; }
        
        public string province_code { get; set; }
        
        public string postal_code { get; set; }
        
        public string country_code { get; set; }
        
        public string phone { get; set; }
        
        public string email { get; set; }
        
        public bool? is_residential { get; set; }
    
    }

    public class CreateShipmentReturnAddress
    {
        
        public string name { get; set; }
        
        public object company { get; set; }
        
        public string address1 { get; set; }
        
        public string address2 { get; set; }
        
        public string city { get; set; }
        
        public string province_code { get; set; }
        
        public string postal_code { get; set; }
        
        public string country_code { get; set; }
        
        public string phone { get; set; }
        
        public string email { get; set; }
        
        public bool? is_residential { get; set; }

    }

}
