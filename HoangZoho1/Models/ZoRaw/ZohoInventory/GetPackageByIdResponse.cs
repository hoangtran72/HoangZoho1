using System;

namespace HoangZoho1.Models.ZoRaw.ZohoInventory
{

    public class GetPackageByIdResponse
    {
        
        public int? code { get; set; }
        
        public string message { get; set; }
        
        public InventoryPackage package { get; set; }
    
    }

    public class InventoryPackage
    {

        public string salesorder_id { get; set; }
        
        public string salesorder_number { get; set; }
        
        public string salesorder_date { get; set; }
        
        public string sales_channel { get; set; }
        
        public string sales_channel_formatted { get; set; }
        
        public string salesorder_fulfilment_status { get; set; }
        
        public string package_id { get; set; }
        
        public string package_number { get; set; }
        
        public string shipment_id { get; set; }
        
        public string shipment_number { get; set; }
        
        public string date { get; set; }
        
        public string shipping_date { get; set; }
        
        public string delivery_method { get; set; }
        
        public string delivery_method_id { get; set; }
        
        public string tracking_number { get; set; }
        
        public string tracking_link { get; set; }
        
        public string expected_delivery_date { get; set; }
        
        public string shipment_delivered_date { get; set; }
        
        public string status { get; set; }
        
        public string detailed_status { get; set; }
        
        public string status_message { get; set; }
        
        public string carrier { get; set; }
        
        public string service { get; set; }
        
        public string delivery_days { get; set; }
        
        public bool? delivery_guarantee { get; set; }
        
        public float? total_quantity { get; set; }
        
        public string customer_id { get; set; }
        
        public string customer_name { get; set; }
        
        public string email { get; set; }
        
        public string phone { get; set; }
        
        public string mobile { get; set; }
        
        public object[] contact_persons { get; set; }
        
        public string created_by_id { get; set; }
        
        public string last_modified_by_id { get; set; }
        
        public string notes { get; set; }
        
        public string terms { get; set; }
        
        public PackageLineItems[] line_items { get; set; }
        
        public bool? includes_picklist_tracking_info { get; set; }
        
        public bool? is_emailed { get; set; }
        
        public object[] custom_fields { get; set; }
        
        public Custom_Field_Hash custom_field_hash { get; set; }
        
        public object[] shipmentorder_custom_fields { get; set; }
        
        public PackageBillingAddress billing_address { get; set; }
        
        public PackageShippingAddress shipping_address { get; set; }
        
        public object[] picklists { get; set; }
        
        public string template_id { get; set; }
        
        public string template_name { get; set; }
        
        public string template_type { get; set; }
        
        public DateTime? created_time { get; set; }
        
        public DateTime? last_modified_time { get; set; }
        
        public PackageShipmentOrder shipment_order { get; set; }
    
    }

    public class PackageBillingAddress
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

    public class PackageShippingAddress
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

    public class PackageShipmentOrder
    {
        
        public string shipment_id { get; set; }
        
        public string shipment_number { get; set; }
        
        public string shipping_date { get; set; }
        
        public string delivery_method { get; set; }
        
        public string delivery_method_id { get; set; }
        
        public string notes { get; set; }
        
        public object[] multipiece_shipments { get; set; }
        
        public string shipment_type { get; set; }
        
        public Associated_Packages[] associated_packages { get; set; }
        
        public int? associated_packages_count { get; set; }
        
        public string tracking_number { get; set; }
        
        public string tracking_link { get; set; }
        
        public string expected_delivery_date { get; set; }
        
        public string shipment_delivered_date { get; set; }
        
        public string status { get; set; }
        
        public string shipment_status { get; set; }
        
        public string shipment_sub_status { get; set; }
        
        public string detailed_status { get; set; }
        
        public string status_message { get; set; }
        
        public string carrier { get; set; }
        
        public string service { get; set; }
        
        public string delivery_days { get; set; }
        
        public bool? delivery_guarantee { get; set; }
        
        public float? shipment_rate { get; set; }
        
        public object[] shipmentorder_custom_fields { get; set; }
        
        public bool? is_carrier_shipment { get; set; }
        
        public bool? can_show_tracking { get; set; }
        
        public bool? is_tracking_enabled { get; set; }
        
        public bool? is_forms_available { get; set; }
        
        public string source_id { get; set; }
        
        public string source_name { get; set; }
        
        public object[] tracking_statuses { get; set; }
    
    }

    public class Associated_Packages
    {
        
        public string package_id { get; set; }
        
        public string package_number { get; set; }
    
    }

    public class PackageLineItems
    {

        public string line_item_id { get; set; }
        
        public string so_line_item_id { get; set; }
        
        public string item_id { get; set; }
        
        public string picklist_item_id { get; set; }
        
        public string picklist_number { get; set; }
        
        public string sku { get; set; }
        
        public string name { get; set; }
        
        public string description { get; set; }
        
        public string salesorder_id { get; set; }
        
        public string salesorder_number { get; set; }
        
        public int? item_order { get; set; }
        
        public string item_type { get; set; }
        
        public decimal? quantity { get; set; }
        
        public string unit { get; set; }
        
        public string image_name { get; set; }
        
        public string image_type { get; set; }
        
        public string image_document_id { get; set; }
        
        public bool? is_invoiced { get; set; }
        
        public object[] item_custom_fields { get; set; }
        
        public PackageBatch[] batches { get; set; }
        
        public bool? track_batch_number { get; set; }
        
        public bool? track_batch_for_package { get; set; }
        
        public bool? is_storage_location_enabled { get; set; }
        
        public string location_id { get; set; }
        
        public string location_name { get; set; }
        
        public bool? is_combo_product { get; set; }
        
        public string combo_type { get; set; }
        
        public object[] mapped_items { get; set; }
    
    }

    public class PackageBatch
    {
        
        public string expiry_date_formatted { get; set; }
        
        public string batch_number { get; set; }
        
        public string batch_id { get; set; }
        
        public string batch_in_id { get; set; }
        
        public string expiry_date { get; set; }
        
        public string internal_batch_number { get; set; }
        
        public string external_batch_number { get; set; }
        
        public float? base_unit_out_quantity { get; set; }
        
        public float? in_quantity { get; set; }
        
        public string status_formatted { get; set; }
        
        public string manufacturer_date_formatted { get; set; }
        
        public string location_id { get; set; }
        
        public float? balance_quantity { get; set; }
        
        public string manufacturer_date { get; set; }
        
        public string batch_out_id { get; set; }
        
        public float? out_quantity { get; set; }
        
        public string status { get; set; }
    
    }

}
