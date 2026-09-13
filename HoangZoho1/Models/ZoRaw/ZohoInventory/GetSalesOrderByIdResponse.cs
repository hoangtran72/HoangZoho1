using System;
using System.Collections.Generic;

namespace HoangZoho1.Models.ZoRaw.ZohoInventory
{

    public class GetSalesOrderByIdResponse
    {

        public int? code { get; set; }

        public string message { get; set; }

        public SalesOrder salesorder { get; set; }

    }

    public class SalesOrder
    {

        public string salesorder_id { get; set; }

        public bool? is_viewed_in_mail { get; set; }

        public string mail_first_viewed_time { get; set; }

        public string mail_last_viewed_time { get; set; }

        public object[] documents { get; set; }

        public string zcrm_potential_id { get; set; }

        public string zcrm_potential_name { get; set; }

        public string salesorder_number { get; set; }

        public string date { get; set; }

        public string offline_created_date_with_time { get; set; }

        public string tracking_url { get; set; }

        public bool? has_discount { get; set; }

        public string status { get; set; }

        public string color_code { get; set; }

        public string current_sub_status_id { get; set; }

        public string current_sub_status { get; set; }

        public object[] sub_statuses { get; set; }

        public string shipment_date { get; set; }

        public string reference_number { get; set; }

        public string customer_id { get; set; }

        public string customer_name { get; set; }

        public object[] contact_persons { get; set; }

        public object[] contact_persons_associated { get; set; }

        public Contact_Person_Details[] contact_person_details { get; set; }

        public string source { get; set; }

        public string contact_category { get; set; }

        public string tax_treatment { get; set; }

        public bool? is_taxable { get; set; }

        public string tax_id { get; set; }

        public string tax_name { get; set; }

        public float? tax_percentage { get; set; }

        public bool? has_shipping_address { get; set; }

        public string currency_id { get; set; }

        public string currency_code { get; set; }

        public string currency_symbol { get; set; }

        public float? exchange_rate { get; set; }

        public bool? is_discount_before_tax { get; set; }

        public string discount_type { get; set; }

        public string estimate_id { get; set; }

        public string delivery_method { get; set; }

        public string delivery_method_id { get; set; }

        public bool? is_inclusive_tax { get; set; }

        public string tax_rounding { get; set; }

        public string tax_override_preference { get; set; }

        public string order_status { get; set; }

        public string invoiced_status { get; set; }

        public string paid_status { get; set; }

        public string shipped_status { get; set; }

        public string sales_channel { get; set; }

        public string sales_channel_formatted { get; set; }

        public string account_identifier { get; set; }

        public string integration_id { get; set; }

        public bool? is_dropshipped { get; set; }

        public bool? is_backordered { get; set; }

        public bool? is_manually_fulfilled { get; set; }

        public bool? can_manually_fulfill { get; set; }

        public bool? has_qty_cancelled { get; set; }

        public Shipping_Details shipping_details { get; set; }

        public string created_by_email { get; set; }

        public string created_by_name { get; set; }

        public string branch_id { get; set; }

        public string branch_name { get; set; }

        public string location_id { get; set; }

        public string location_name { get; set; }

        public float? total_quantity { get; set; }

        public SalesOrderLineItems[] line_items { get; set; }

        public string entity_tags { get; set; }

        public string submitter_id { get; set; }

        public string approver_id { get; set; }

        public string submitted_date { get; set; }

        public string submitted_by { get; set; }

        public string submitted_by_name { get; set; }

        public string submitted_by_email { get; set; }

        public string submitted_by_photo_url { get; set; }

        public float? price_precision { get; set; }

        public bool? is_emailed { get; set; }

        public bool? has_unconfirmed_line_item { get; set; }

        public object[] picklists { get; set; }

        public object[] purchaseorders { get; set; }

        public SalesOrderLocation[] locations { get; set; }

        public string billing_address_id { get; set; }

        public Billing_Address billing_address { get; set; }

        public string shipping_address_id { get; set; }

        public Shipping_Address shipping_address { get; set; }

        public bool? is_test_order { get; set; }

        public string notes { get; set; }

        public string terms { get; set; }

        public int? payment_terms { get; set; }

        public string payment_terms_label { get; set; }

        public object[] custom_fields { get; set; }

        public Custom_Field_Hash custom_field_hash { get; set; }

        public string template_id { get; set; }

        public string template_name { get; set; }

        public string page_width { get; set; }

        public string page_height { get; set; }

        public string orientation { get; set; }

        public string template_type { get; set; }

        public DateTime? created_time { get; set; }

        public DateTime? last_modified_time { get; set; }

        public string created_by_id { get; set; }

        public string created_date { get; set; }

        public string last_modified_by_id { get; set; }

        public string attachment_name { get; set; }

        public bool? can_send_in_mail { get; set; }

        public string salesperson_id { get; set; }

        public string salesperson_name { get; set; }

        public string merchant_id { get; set; }

        public string merchant_name { get; set; }

        public string pickup_location_id { get; set; }

        public string discount { get; set; }

        public float? discount_applied_on_amount { get; set; }

        public bool? is_adv_tracking_in_package { get; set; }

        public string shipping_charge_tax_id { get; set; }

        public string shipping_charge_tax_name { get; set; }

        public string shipping_charge_tax_type { get; set; }

        public string shipping_charge_tax_percentage { get; set; }

        public string shipping_charge_tax_exemption_id { get; set; }

        public string shipping_charge_tax_exemption_code { get; set; }

        public string shipping_charge_tax { get; set; }

        public string bcy_shipping_charge_tax { get; set; }

        public float? shipping_charge_exclusive_of_tax { get; set; }

        public float? shipping_charge_inclusive_of_tax { get; set; }

        public string shipping_charge_tax_formatted { get; set; }

        public string shipping_charge_exclusive_of_tax_formatted { get; set; }

        public string shipping_charge_inclusive_of_tax_formatted { get; set; }

        public float? shipping_charge { get; set; }

        public float? bcy_shipping_charge { get; set; }

        public float? adjustment { get; set; }

        public float? bcy_adjustment { get; set; }

        public string adjustment_description { get; set; }

        public float? roundoff_value { get; set; }

        public string transaction_rounding_type { get; set; }

        public float? sub_total { get; set; }

        public float? bcy_sub_total { get; set; }

        public float? sub_total_inclusive_of_tax { get; set; }

        public float? sub_total_exclusive_of_discount { get; set; }

        public float? discount_total { get; set; }

        public float? bcy_discount_total { get; set; }

        public float? discount_percent { get; set; }

        public float? tax_total { get; set; }

        public float? bcy_tax_total { get; set; }

        public float? total { get; set; }

        public string computation_type { get; set; }

        public float? bcy_total { get; set; }

        public Tax[] taxes { get; set; }

        public string tds_calculation_type { get; set; }

        public Package[] packages { get; set; }

        public So_Cycle_Preference so_cycle_preference { get; set; }

        public object[] invoices { get; set; }

        public bool? can_show_kit_return { get; set; }

        public bool? is_kit_partial_return { get; set; }

        public object[] salesreturns { get; set; }

        public object[] payments { get; set; }

        public object[] creditnotes { get; set; }

        public object[] refunds { get; set; }

        public Contact contact { get; set; }

        public float? balance { get; set; }

        public object[] approvers_list { get; set; }

        public bool? is_scheduled_for_quick_shipment_create { get; set; }

    }

    public class Shipping_Details
    {
    }

    public class Billing_Address
    {

        public string address { get; set; }

        public string street2 { get; set; }

        public string city { get; set; }

        public string state { get; set; }

        public string zip { get; set; }

        public string country { get; set; }

        public string country_code { get; set; }

        public string state_code { get; set; }

        public string fax { get; set; }

        public string phone { get; set; }

        public string attention { get; set; }

    }

    public class Shipping_Address
    {

        public string company_name { get; set; }

        public string address { get; set; }

        public string street2 { get; set; }

        public string city { get; set; }

        public string state { get; set; }

        public string zip { get; set; }

        public string country { get; set; }

        public string country_code { get; set; }

        public string state_code { get; set; }

        public string fax { get; set; }

        public string phone { get; set; }

        public string attention { get; set; }

    }

    public class Custom_Field_Hash
    {
    }

    public class So_Cycle_Preference
    {

        public bool? is_feature_enabled { get; set; }

        public string socycle_status { get; set; }

        public bool? can_create_invoice { get; set; }

        public bool? can_create_package { get; set; }

        public bool? can_create_shipment { get; set; }

        public Shipment_Preference shipment_preference { get; set; }

        public Invoice_Preference invoice_preference { get; set; }

    }

    public class Shipment_Preference
    {

        public string default_carrier { get; set; }

        public bool? send_notification { get; set; }

        public bool? deliver_shipments { get; set; }

    }

    public class Invoice_Preference
    {

        public bool? mark_as_sent { get; set; }

        public bool? record_payment { get; set; }

        public string payment_mode_id { get; set; }

        public string payment_account_id { get; set; }
    }

    public class Contact
    {

        public string contact_number { get; set; }

        public float? customer_balance { get; set; }

        public float? credit_limit { get; set; }

        public float? unused_customer_credits { get; set; }

        public bool? is_credit_limit_migration_completed { get; set; }

    }

    public class Contact_Person_Details
    {

        public string phone { get; set; }

        public string mobile { get; set; }

        public string last_name { get; set; }

        public string contact_person_id { get; set; }

        public string first_name { get; set; }

        public string email { get; set; }

    }

    public class SalesOrderLineItems
    {

        public string line_item_id { get; set; }

        public string variant_id { get; set; }

        public bool? track_batch_number { get; set; }

        public string item_id { get; set; }

        public bool? is_returnable { get; set; }

        public string product_id { get; set; }

        public string attribute_name1 { get; set; }

        public string attribute_name2 { get; set; }

        public string attribute_name3 { get; set; }

        public string attribute_option_name1 { get; set; }

        public string attribute_option_name2 { get; set; }

        public string attribute_option_name3 { get; set; }

        public string attribute_option_data1 { get; set; }

        public string attribute_option_data2 { get; set; }

        public string attribute_option_data3 { get; set; }

        public bool? is_combo_product { get; set; }

        public string combo_type { get; set; }

        public string sku { get; set; }

        public string name { get; set; }

        public string group_name { get; set; }

        public string description { get; set; }

        public decimal? item_order { get; set; }

        public decimal? bcy_rate { get; set; }

        public decimal? rate { get; set; }

        public decimal? sales_rate { get; set; }

        public decimal? quantity { get; set; }

        public decimal? quantity_manuallyfulfilled { get; set; }

        public string unit { get; set; }

        public string pricebook_id { get; set; }

        public string header_id { get; set; }

        public string header_name { get; set; }

        public float? discount_amount { get; set; }

        public float? discount { get; set; }

        public object[] discounts { get; set; }

        public string tax_id { get; set; }

        public string tax_name { get; set; }

        public string tax_type { get; set; }

        public float? tax_percentage { get; set; }

        public Line_Item_Taxes[] line_item_taxes { get; set; }

        public float? item_total { get; set; }

        public float? item_sub_total { get; set; }

        public string product_type { get; set; }

        public string line_item_type { get; set; }

        public string item_type { get; set; }

        public bool? is_invoiced { get; set; }

        public bool? is_unconfirmed_product { get; set; }

        public object[] tags { get; set; }

        public string image_name { get; set; }

        public string image_type { get; set; }

        public string image_document_id { get; set; }

        public string document_id { get; set; }

        public object[] item_custom_fields { get; set; }

        public Custom_Field_Hash custom_field_hash { get; set; }

        public float? quantity_invoiced { get; set; }

        public float? quantity_packed { get; set; }

        public float? quantity_shipped { get; set; }

        public float? quantity_picked { get; set; }

        public float? quantity_backordered { get; set; }

        public float? quantity_dropshipped { get; set; }

        public float? quantity_cancelled { get; set; }

        public float? quantity_delivered { get; set; }

        public Package_Details package_details { get; set; }

        public float? quantity_invoiced_cancelled { get; set; }

        public float? quantity_returned { get; set; }

        public int? is_fulfillable { get; set; }

        public string project_id { get; set; }

        public string location_id { get; set; }

        public string location_name { get; set; }

        public List<SalesOrderMappedItem> mapped_items { get; set; }

    }


    public class SalesOrderMappedItem
    {

        public bool? is_component { get; set; }
        
        public decimal? quantity_shipped { get; set; }
        
        public decimal? quantity_returned { get; set; }
        
        public bool? is_combo_product { get; set; }
        
        public string line_item_id { get; set; }
        
        public decimal? quantity_invoiced_cancelled { get; set; }
        
        public string item_type { get; set; }
        
        public decimal? quantity_cancelled { get; set; }
        
        public decimal? quantity_dropshipped { get; set; }
        
        public string location_id { get; set; }
        
        public int? item_order { get; set; }
        
        public string image_name { get; set; }
        
        public decimal? quantity_picked { get; set; }
        
        public bool? track_serial_number { get; set; }
        
        public string parent_line_item_id { get; set; }
        
        public string super_parent_item_name { get; set; }
        
        public decimal? quantity_packed { get; set; }
        
        public bool? is_fulfillable { get; set; }
        
        public bool? is_returnable { get; set; }
        
        public string parent_item_name { get; set; }
        
        public string super_parent_line_item_id { get; set; }
        
        public string image_type { get; set; }
        
        public decimal? quantity { get; set; }
        
        public decimal? quantity_manuallyfulfilled { get; set; }
        
        public string image_document_id { get; set; }
        
        public string item_id { get; set; }
        
        public decimal? quantity_delivered { get; set; }
        
        public decimal? mapped_quantity { get; set; }
        
        public string unit { get; set; }
        
        public string location_name { get; set; }
        
        public string product_type { get; set; }
        
        public decimal? quantity_invoiced { get; set; }
        
        public decimal? quantity_backordered { get; set; }
        
        public string name { get; set; }
        
        public bool? track_batch_number { get; set; }
        
        public string combo_type { get; set; }
    
    }

    public class Package_Details
    {

        public float? length { get; set; }

        public float? width { get; set; }

        public float? height { get; set; }

        public string weight { get; set; }

        public string weight_unit { get; set; }

        public string dimension_unit { get; set; }

    }

    public class Line_Item_Taxes
    {

        public string tax_id { get; set; }

        public string tax_name { get; set; }

        public decimal? tax_amount { get; set; }

        public decimal? tax_percentage { get; set; }

        public string tax_specific_type { get; set; }

    }

    public class SalesOrderLocation
    {

        public string location_id { get; set; }

        public string location_name { get; set; }

        public string status { get; set; }

    }

    public class Tax
    {

        public float? tax_amount { get; set; }

        public string tax_name { get; set; }

        public string tax_amount_formatted { get; set; }

    }

    public class Package
    {

        public string package_id { get; set; }

        public string package_number { get; set; }

        public string date { get; set; }

        public string status { get; set; }

        public string detailed_status { get; set; }

        public string status_message { get; set; }

        public string shipment_id { get; set; }

        public string shipment_number { get; set; }

        public string shipment_status { get; set; }

        public string carrier { get; set; }

        public string service { get; set; }

        public string tracking_number { get; set; }

        public string shipment_date { get; set; }

        public string delivery_days { get; set; }

        public bool? delivery_guarantee { get; set; }

        public string delivery_method { get; set; }

        public float? quantity { get; set; }

        public bool? is_tracking_enabled { get; set; }

        public Shipment_Order shipment_order { get; set; }

    }

    public class Shipment_Order
    {

        public string shipment_id { get; set; }

        public string shipment_number { get; set; }

        public string shipment_date { get; set; }

        public string shipment_date_with_time { get; set; }

        public string tracking_number { get; set; }

        public string delivery_date { get; set; }

        public string delivery_date_with_time { get; set; }

        public string shipment_type { get; set; }

        public int? associated_packages_count { get; set; }

        public string carrier { get; set; }

        public string service { get; set; }

        public string delivery_days { get; set; }

        public bool? delivery_guarantee { get; set; }

        public string tracking_url { get; set; }

        public bool? is_carrier_shipment { get; set; }

    }

}
