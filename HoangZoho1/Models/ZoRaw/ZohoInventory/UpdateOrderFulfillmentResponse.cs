using System;

namespace HoangZoho1.Models.ZoRaw.ZohoInventory
{

    public class UpdateOrderFulfillmentResponse
    {
        
        public int? code { get; set; }
        
        public string message { get; set; }
        
        public Module_Record module_record { get; set; }
    
    }

    public class Module_Record
    {

        public string module_record_id { get; set; }
        
        public string module_api_name { get; set; }
        
        public DateTime? last_modified_time { get; set; }
        
        public string last_modified_time_formatted { get; set; }
        
        public DateTime? created_time { get; set; }
        
        public string created_time_formatted { get; set; }
        
        public string created_by_id { get; set; }
        
        public string last_modified_by_id { get; set; }
        
        public string status { get; set; }
        
        public string status_formatted { get; set; }
        
        public string cf_customer { get; set; }
        
        public string cf_customer_formatted { get; set; }
        
        public string cf_po_due_date { get; set; }
        
        public string cf_po_due_date_formatted { get; set; }
        
        public string cf_reference { get; set; }
        
        public string cf_reference_formatted { get; set; }
        
        public string cf_tracking_url { get; set; }
        
        public string cf_tracking_url_formatted { get; set; }
        
        public string cf_shipment_status { get; set; }
        
        public string cf_shipment_status_formatted { get; set; }
        
        public string cf_order_date { get; set; }
        
        public string cf_order_date_formatted { get; set; }
        
        public string cf_order { get; set; }
        
        public string cf_order_formatted { get; set; }
        
        public string cf_order_id { get; set; }
        
        public string cf_order_id_formatted { get; set; }
        
        public string cf_order_type { get; set; }
        
        public string cf_order_type_formatted { get; set; }
        
        public string cf_carrier { get; set; }
        
        public string cf_carrier_formatted { get; set; }
        
        public string cf_shipping_address { get; set; }
        
        public string cf_shipping_address_formatted { get; set; }
        
        public string cf_b2b_overdue_flag { get; set; }
        
        public string cf_b2b_overdue_flag_formatted { get; set; }
        
        public string cf_billing_address { get; set; }
        
        public string cf_billing_address_formatted { get; set; }
        
        public string cf_tracking { get; set; }
        
        public string cf_tracking_formatted { get; set; }
        
        public string cf_print_package_slip { get; set; }
        
        public string cf_print_package_slip_formatted { get; set; }
        
        public string cf_book_shipment_url { get; set; }
        
        public string cf_book_shipment_url_formatted { get; set; }
        
        public string cf_step_4_print_shipping_label { get; set; }
        
        public string cf_step_4_print_shipping_label_formatted { get; set; }
        
        public Cf_Cm_Item_Table[] cf_cm_item_table { get; set; }
        
        public string cf_cm_item_table_formatted { get; set; }
        
        public string cf_order_status { get; set; }
        
        public string cf_order_status_formatted { get; set; }
        
        public string cf_payment_terms { get; set; }
        
        public string cf_payment_terms_formatted { get; set; }
        
        public string cf_scan_order_url { get; set; }
        
        public string cf_scan_order_url_formatted { get; set; }
        public string record_created_by { get; set; }
        
        public string record_created_by_formatted { get; set; }
        
        public string record_last_modified_by { get; set; }
        
        public string record_last_modified_by_formatted { get; set; }
        
        public string record_name { get; set; }
        
        public string record_name_formatted { get; set; }
        
        public string cf_delivery_method { get; set; }
        
        public string cf_delivery_method_formatted { get; set; }
    
    }

    public class Cf_Cm_Item_Table
    {
        
        public string cf_location_formatted { get; set; }
        
        public string cf_unit_formatted { get; set; }
        
        public string cf_quantity_formatted { get; set; }
        
        public decimal? cf_quantity { get; set; }
        
        public string cf_item_details_formatted { get; set; }
        
        public string cf_location { get; set; }
        
        public string cf_unit { get; set; }
        
        public string cf_item_details { get; set; }
    
    }

}
