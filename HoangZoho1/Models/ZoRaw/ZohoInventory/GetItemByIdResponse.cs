using System;
using System.Collections.Generic;

namespace HoangZoho1.Models.ZoRaw.ZohoInventory
{

    public class GetItemByIdResponse
    {

        public int? code { get; set; }

        public string message { get; set; }

        public InventoryItem item { get; set; }

    }

    public class InventoryItem
    {

        public List<MappedItem> mapped_items { get; set; }

        public string item_id { get; set; }

        public string name { get; set; }

        public string sku { get; set; }

        public string brand { get; set; }

        public string manufacturer { get; set; }

        public string category_id { get; set; }

        public string category_name { get; set; }

        public string image_name { get; set; }

        public string image_type { get; set; }

        public string status { get; set; }

        public string source { get; set; }

        public bool? is_linked_with_zohocrm { get; set; }

        public string zcrm_product_id { get; set; }

        public string crm_owner_id { get; set; }

        public string unit { get; set; }

        public string unit_id { get; set; }

        public string description { get; set; }

        public float? rate { get; set; }

        public string account_id { get; set; }

        public string account_name { get; set; }

        public string tax_id { get; set; }

        public string tax_name { get; set; }

        public int tax_percentage { get; set; }

        public string tax_type { get; set; }

        public string tax_status { get; set; }

        public string tax_groups_details { get; set; }

        public string tax_country_code { get; set; }

        public string tax_information { get; set; }

        public string purchase_tax_id { get; set; }

        public string purchase_tax_name { get; set; }

        public string purchase_tax_exemption_id { get; set; }

        public string purchase_tax_exemption_code { get; set; }

        public Purchase_Tax_Information purchase_tax_information { get; set; }

        public bool? is_default_tax_applied { get; set; }

        public int? purchase_tax_percentage { get; set; }

        public string purchase_tax_type { get; set; }

        public bool? is_taxable { get; set; }

        public string tax_exemption_id { get; set; }

        public string tax_exemption_code { get; set; }

        public string associated_template_id { get; set; }

        public Document[] documents { get; set; }

        public string purchase_description { get; set; }

        public string sales_tax_rule_id { get; set; }

        public string sales_tax_rule_name { get; set; }

        public string purchase_tax_rule_id { get; set; }

        public string purchase_tax_rule_name { get; set; }

        public float? pricebook_rate { get; set; }

        public string pricing_scheme { get; set; }

        public object[] price_brackets { get; set; }

        public Default_Price_Brackets[] default_price_brackets { get; set; }

        public float? sales_rate { get; set; }

        public float? purchase_rate { get; set; }

        public string sales_margin { get; set; }

        public string purchase_account_id { get; set; }

        public string purchase_account_name { get; set; }

        public string inventory_account_id { get; set; }

        public string inventory_account_name { get; set; }

        public DateTime? created_time { get; set; }

        public string offline_created_date_with_time { get; set; }

        public DateTime? last_modified_time { get; set; }

        public object[] tags { get; set; }

        public bool? can_be_sold { get; set; }

        public bool? can_be_purchased { get; set; }

        public bool? track_inventory { get; set; }

        public string item_type { get; set; }

        public string product_type { get; set; }

        public string inventory_valuation_method { get; set; }

        public bool? is_returnable { get; set; }

        public string reorder_level { get; set; }

        public string minimum_order_quantity { get; set; }

        public string maximum_order_quantity { get; set; }

        public float? initial_stock { get; set; }

        public float? initial_stock_rate { get; set; }

        public float total_initial_stock { get; set; }

        public string vendor_id { get; set; }

        public string vendor_name { get; set; }

        public float? stock_on_hand { get; set; }

        public string asset_value { get; set; }

        public float available_stock { get; set; }

        public float actual_available_stock { get; set; }

        public float? committed_stock { get; set; }

        public float? actual_committed_stock { get; set; }

        public float? available_for_sale_stock { get; set; }

        public float? actual_available_for_sale_stock { get; set; }

        public object[] custom_fields { get; set; }

        public Custom_Field_Hash custom_field_hash { get; set; }

        public bool? track_batch_number { get; set; }

        public bool? is_storage_location_enabled { get; set; }

        public bool is_fulfillable { get; set; }

        public string upc { get; set; }

        public string ean { get; set; }

        public string isbn { get; set; }

        public string part_number { get; set; }

        public bool? is_combo_product { get; set; }

        public string combo_type { get; set; }

        public bool? image_sync_in_progress { get; set; }

        public object[] sales_channels { get; set; }

        public Location[] locations { get; set; }

        public object[] preferred_vendors { get; set; }

        public Package_Details package_details { get; set; }

        public object[] integration_references { get; set; }

    }

    public class Purchase_Tax_Information
    {

        public string tax_specification { get; set; }

        public string tax_specific_type { get; set; }

        public string end_date { get; set; }

        public bool? is_non_advol_tax { get; set; }

        public object[] tax_groups_details { get; set; }

        public string type { get; set; }

        public string type_formatted { get; set; }

        public string country_code { get; set; }

        public float? percentage { get; set; }

        public string id { get; set; }

        public string text { get; set; }

        public string status { get; set; }

        public string start_date { get; set; }

    }

    public class Document
    {

        public bool? can_send_in_mail { get; set; }

        public string file_name { get; set; }

        public int? attachment_order { get; set; }

        public string source { get; set; }

        public string document_id { get; set; }

        public string file_size { get; set; }

        public string source_formatted { get; set; }

        public string uploaded_by { get; set; }

        public string file_type { get; set; }

        public string file_size_formatted { get; set; }

        public string uploaded_on { get; set; }

        public string uploaded_by_id { get; set; }

        public string alter_text { get; set; }

        public string uploaded_on_date_formatted { get; set; }

    }

    public class Default_Price_Brackets
    {

        public float? start_quantity { get; set; }

        public float? end_quantity { get; set; }

        public float? pricebook_rate { get; set; }

    }


    public class MappedItem
    {
        public bool? is_component { get; set; }
        public bool? is_combo_product { get; set; }
        public string image_document_id { get; set; }
        public string item_id { get; set; }
        public string item_type { get; set; }
        public decimal? mapped_quantity { get; set; }
        public string image_name { get; set; }
        public string unit { get; set; }
        public string product_type { get; set; }
        public bool? track_serial_number { get; set; }
        public string name { get; set; }
        public string super_parent_item_name { get; set; }
        public bool? track_batch_number { get; set; }
        public bool? is_fulfillable { get; set; }
        public Location[] locations { get; set; }
        public string combo_type { get; set; }
        public string parent_item_name { get; set; }
        public bool? is_returnable { get; set; }
        public bool? is_storage_location_enabled { get; set; }
        public string image_type { get; set; }
        public string status { get; set; }
    }

}
