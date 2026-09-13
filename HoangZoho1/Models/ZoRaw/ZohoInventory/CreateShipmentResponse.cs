using System;

namespace HoangZoho1.Models.ZoRaw.ZohoInventory
{

    public class CreateShipmentResponse
    {

        public int? code { get; set; }
        
        public string message { get; set; }
        
        public Shipmentorder shipmentorder { get; set; }
    
    }

    public class Shipmentorder
    {

        public string salesorder_id { get; set; }
        
        public string salesorder_number { get; set; }
        
        public string salesorder_date { get; set; }
        
        public string salesorder_fulfilment_status { get; set; }
        
        public string sales_channel { get; set; }
        
        public string sales_channel_formatted { get; set; }
        
        public string shipment_id { get; set; }
        
        public string shipment_number { get; set; }
        
        public string date { get; set; }
        
        public string shipment_status { get; set; }
        
        public string shipment_sub_status { get; set; }
        
        public string status { get; set; }
        
        public string detailed_status { get; set; }
        
        public string status_message { get; set; }
        
        public string carrier { get; set; }
        
        public string tracking_carrier_code { get; set; }
        
        public string service { get; set; }
        
        public string delivery_days { get; set; }
        
        public string source_id { get; set; }
        
        public string label_format { get; set; }
        
        public string source_name { get; set; }
        
        public bool? delivery_guarantee { get; set; }
        
        public string reference_number { get; set; }
        
        public string customer_id { get; set; }
        
        public string customer_name { get; set; }
        
        public ShipmentContactPersons[] contact_persons { get; set; }
        
        public bool? is_taxable { get; set; }
        
        public string tax_id { get; set; }
        
        public string tax_name { get; set; }
        
        public decimal? tax_percentage { get; set; }
        
        public string currency_id { get; set; }
        
        public string currency_code { get; set; }
        
        public string currency_symbol { get; set; }
        
        public decimal? exchange_rate { get; set; }
        
        public decimal? discount { get; set; }
        
        public bool? is_discount_before_tax { get; set; }
        
        public string discount_type { get; set; }
        
        public string estimate_id { get; set; }
        
        public string delivery_method { get; set; }
        
        public string delivery_method_id { get; set; }
        
        public string tracking_number { get; set; }
        
        public string tracking_link { get; set; }
        
        public string last_tracking_update_date { get; set; }
        
        public string expected_delivery_date { get; set; }
        
        public string shipment_delivered_date { get; set; }
        
        public object[] multipiece_shipments { get; set; }
        
        public string shipment_type { get; set; }
        
        public bool? is_carrier_shipment { get; set; }
        
        public bool? is_tracking_enabled { get; set; }
        
        public bool? is_forms_available { get; set; }
        
        public bool? is_email_notification_enabled { get; set; }
        
        public object[] tracking_statuses { get; set; }
        
        public object[] invoices { get; set; }
        
        public Line_Items[] line_items { get; set; }
        
        public decimal? shipping_charge { get; set; }
        
        public decimal? sub_total { get; set; }
        
        public decimal? tax_total { get; set; }
        
        public decimal? total { get; set; }
        
        public ShipmentTax[] taxes { get; set; }
        
        public int? price_precision { get; set; }
        
        public bool? is_emailed { get; set; }
        
        public ShipmentBillingAddress billing_address { get; set; }
        
        public ShipmentShippingAddress shipping_address { get; set; }
        
        public string notes { get; set; }
        
        public object[] custom_fields { get; set; }
        
        public Custom_Field_Hash custom_field_hash { get; set; }
        
        public string template_id { get; set; }
        
        public string template_name { get; set; }
        
        public string template_type { get; set; }
        
        public DateTime? created_time { get; set; }
        
        public DateTime? last_modified_time { get; set; }
        
        public ShipmentPackage[] packages { get; set; }
        
        public object[] documents { get; set; }
        
        public int? associated_packages_count { get; set; }
        
        public string created_by_id { get; set; }
        
        public string last_modified_by_id { get; set; }
        
        public bool? is_viewed_in_mail { get; set; }
        
        public string mail_first_viewed_time { get; set; }
        
        public string mail_last_viewed_time { get; set; }
    
    }

    public class ShipmentBillingAddress
    {
        
        public string address { get; set; }
        
        public string street2 { get; set; }
        
        public string city { get; set; }
        
        public string state { get; set; }
        
        public string zip { get; set; }
        
        public string country { get; set; }
        
        public string fax { get; set; }
        
        public string phone { get; set; }
        
        public string attention { get; set; }
    
    }

    public class ShipmentShippingAddress
    {
        public string company_name { get; set; }
        public string address { get; set; }
        public string street2 { get; set; }
        public string city { get; set; }
        public string state { get; set; }
        public string zip { get; set; }
        public string country { get; set; }
        public string fax { get; set; }
        public string phone { get; set; }
        public string attention { get; set; }
    }

    public class ShipmentContactPersons
    {
        public string first_name { get; set; }
        public string last_name { get; set; }
        public string email { get; set; }
        public string phone { get; set; }
        public string mobile { get; set; }
        public string department { get; set; }
    }

    public class Line_Items
    {
        public string line_item_id { get; set; }
        public string item_id { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public int? item_order { get; set; }
        public decimal? bcy_rate { get; set; }
        public decimal? rate { get; set; }
        public decimal? quantity { get; set; }
        public string unit { get; set; }
        public string tax_id { get; set; }
        public string tax_name { get; set; }
        public string tax_type { get; set; }
        public decimal? tax_percentage { get; set; }
        public decimal? item_total { get; set; }
        public bool? is_invoiced { get; set; }
        public object[] mapped_items { get; set; }
    }

    public class ShipmentTax
    {
        public string tax_name { get; set; }
        public decimal? tax_amount { get; set; }
    }

    public class ShipmentPackage
    {
        public string package_id { get; set; }
        public string package_number { get; set; }
        public decimal? package_quantity { get; set; }
        public string package_date { get; set; }
        public string notes { get; set; }
        public string terms { get; set; }
    }

}
