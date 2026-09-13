using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HoangZoho1.Models.GetUnik.ZohoBooks
{

    public class CreateInvoiceResponse
    {
        public int code { get; set; }
        public string message { get; set; }
        public Invoice invoice { get; set; }
    }

    public class Invoice
    {

        public string invoice_id { get; set; }
        
        public string invoice_number { get; set; }
        
        public string date { get; set; }
        
    //    public string date_formatted { get; set; }
        
    //    public string due_date { get; set; }
        
    //    public string due_date_formatted { get; set; }
        
    //    public string offline_created_date_with_time { get; set; }
        
    //    public string customer_id { get; set; }
        
    //    public string customer_name { get; set; }
        
    //    public Customer_Custom_Fields[] customer_custom_fields { get; set; }
        
    //    public string cf_account_owner { get; set; }
        
    //    public string cf_account_owner_unformatted { get; set; }
        
    //    public Customer_Custom_Field_Hash customer_custom_field_hash { get; set; }
        
    //    public string email { get; set; }
        
    //    public string currency_id { get; set; }
        
    //    public string invoice_source { get; set; }
        
    //    public string invoice_source_formatted { get; set; }
        
    //    public string currency_code { get; set; }
        
    //    public string currency_symbol { get; set; }
        
    //    public string currency_name_formatted { get; set; }
        
    //    public string status { get; set; }
        
    //    public string status_formatted { get; set; }
        
    //    public decimal? unprocessed_payment_amount { get; set; }
        
    //    public string unprocessed_payment_amount_formatted { get; set; }
        
    //    public Custom_Fields[] custom_fields { get; set; }
        
    //    public Custom_Field_Hash custom_field_hash { get; set; }
        
    //    public string recurring_invoice_id { get; set; }

    //    public bool? is_last_child_invoice { get; set; }

    //    public int? payment_terms { get; set; }

    //    public string payment_terms_label { get; set; }
        
    //    public decimal? early_payment_discount_percentage { get; set; }
        
    //    public string early_payment_discount_due_days { get; set; }

    //    public decimal? early_payment_discount_amount { get; set; }

    //    public string early_payment_discount_amount_formatted { get; set; }

    //    public bool? payment_reminder_enabled { get; set; }
        
    //    public decimal? payment_made { get; set; }
        
    //    public string payment_made_formatted { get; set; }
        
    //    public string zcrm_potential_id { get; set; }
        
    //    public string zcrm_potential_name { get; set; }
        
    //    public string reference_number { get; set; }
        
    //    public bool? is_inventory_valuation_pending { get; set; }
        
    //    public bool? is_early_payment_discount_applicable { get; set; }
        
    //    public Lock_Details lock_details { get; set; }
        
    //    public bool? is_progress_invoice { get; set; }
        
    //    public Line_Items[] line_items { get; set; }
        
    //    public decimal? total_retention_amount { get; set; }
        
    //    public string total_retention_amount_formatted { get; set; }
        
    //    public object[] retention_items { get; set; }
        
    //    public string retention_override_preference { get; set; }
        
    //    public decimal? exchange_rate { get; set; }
        
    //    public bool? is_autobill_enabled { get; set; }
        
    //    public bool? inprocess_transaction_present { get; set; }
        
    //    public bool? allow_partial_payments { get; set; }
        
    //    public int? price_precision { get; set; }
        
    //    public decimal? sub_total { get; set; }
        
    //    public string sub_total_formatted { get; set; }
        
    //    public decimal? tax_total { get; set; }
        
    //    public string tax_total_formatted { get; set; }
        
    //    public decimal? discount_total { get; set; }
        
    //    public string discount_total_formatted { get; set; }
        
    //    public decimal? discount_percent { get; set; }
        
    //    public decimal? discount { get; set; }
        
    //    public decimal? discount_applied_on_amount { get; set; }
        
    //    public string discount_type { get; set; }
        
    //    public string discount_account_id { get; set; }
        
    //    public string discount_account_name { get; set; }
        
    //    public string tax_override_preference { get; set; }
        
    //    public string tds_override_preference { get; set; }
        
    //    public bool? is_discount_before_tax { get; set; }
        
    //    public decimal? adjustment { get; set; }
        
    //    public string adjustment_formatted { get; set; }
        
    //    public string adjustment_description { get; set; }
        
    //    public string shipping_charge_tax_id { get; set; }

    //    public string shipping_charge_tax_name { get; set; }

    //    public string shipping_charge_tax_type { get; set; }

    //    public string shipping_charge_tax_percentage { get; set; }

    //    public string shipping_charge_tax { get; set; }

    //    public string bcy_shipping_charge_tax { get; set; }

    //    public decimal? shipping_charge_exclusive_of_tax { get; set; }

    //    public decimal? shipping_charge_inclusive_of_tax { get; set; }

    //    public string shipping_charge_tax_formatted { get; set; }

    //    public string shipping_charge_exclusive_of_tax_formatted { get; set; }

    //    public string shipping_charge_inclusive_of_tax_formatted { get; set; }

    //    public string shipping_charge_account_id { get; set; }

    //    public string shipping_charge_account_name { get; set; }

    //    public decimal? shipping_charge { get; set; }

    //    public string shipping_charge_formatted { get; set; }

    //    public decimal? bcy_shipping_charge { get; set; }

    //    public decimal? bcy_adjustment { get; set; }

    //    public decimal? bcy_sub_total { get; set; }

    //    public decimal? bcy_discount_total { get; set; }

    //    public decimal? bcy_tax_total { get; set; }

    //    public decimal? bcy_total { get; set; }

    //    public decimal? total { get; set; }

    //    public string total_formatted { get; set; }

    //    public decimal? balance { get; set; }

    //    public string balance_formatted { get; set; }

    //    public decimal? write_off_amount { get; set; }

    //    public string write_off_amount_formatted { get; set; }

    //    public decimal? roundoff_value { get; set; }

    //    public string roundoff_value_formatted { get; set; }

    //    public string transaction_rounding_type { get; set; }

    //    public bool? is_inclusive_tax { get; set; }

    //    public decimal? sub_total_inclusive_of_tax { get; set; }

    //    public string sub_total_inclusive_of_tax_formatted { get; set; }

    //    public string tax_reg_no { get; set; }

    //    public string contact_category { get; set; }

    //    public string tax_treatment { get; set; }

    //    public string tax_treatment_formatted { get; set; }

    //    public string tax_rounding { get; set; }

    //    public Tax[] taxes { get; set; }

    //    public string tds_calculation_type { get; set; }

    //    public bool? can_send_invoice_sms { get; set; }

    //    public string payment_expected_date { get; set; }

    //    public string payment_expected_date_formatted { get; set; }

    //    public decimal? payment_discount { get; set; }

    //    public string payment_discount_formatted { get; set; }

    //    public bool? stop_reminder_until_payment_expected_date { get; set; }

    //    public string last_payment_date { get; set; }

    //    public string last_payment_date_formatted { get; set; }

    //    public bool? ach_supported { get; set; }

    //    public bool? ach_payment_initiated { get; set; }

    //    public Payment_Options payment_options { get; set; }

    //    public bool? reader_offline_payment_initiated { get; set; }

    //    public string[] contact_persons { get; set; }

    //    public Contact_Persons_Associated[] contact_persons_associated { get; set; }

    //    public string attachment_name { get; set; }

    //    public object[] documents { get; set; }

    //    public string computation_type { get; set; }

    //    public object[] deliverychallans { get; set; }

    //    public string branch_id { get; set; }

    //    public string branch_name { get; set; }

    //    public string location_id { get; set; }

    //    public string location_name { get; set; }

    //    public string merchant_id { get; set; }

    //    public string merchant_name { get; set; }

    //    public string ecomm_operator_id { get; set; }

    //    public string ecomm_operator_name { get; set; }

    //    public string salesorder_id { get; set; }

    //    public string salesorder_number { get; set; }

    //    public object[] salesorders { get; set; }

    //    public object[] shipping_bills { get; set; }

    //    public Contact_Persons_Details[] contact_persons_details { get; set; }

    //    public Contact contact { get; set; }

    //    public string salesperson_id { get; set; }

    //    public string salesperson_name { get; set; }
        
    //    public bool? is_emailed { get; set; }
        
    //    public int? reminders_sent { get; set; }
        
    //    public string last_reminder_sent_date { get; set; }
        
    //    public string last_reminder_sent_date_formatted { get; set; }
        
    //    public string next_reminder_date_formatted { get; set; }
        
    //    public bool? is_viewed_by_client { get; set; }
        
    //    public string client_viewed_time { get; set; }
        
    //    public string client_viewed_time_formatted { get; set; }
        
    //    public string submitter_id { get; set; }
        
    //    public string approver_id { get; set; }
        
    //    public string submitted_date { get; set; }
        
    //    public string submitted_date_formatted { get; set; }
        
    //    public string submitted_by { get; set; }
        
    //    public string submitted_by_name { get; set; }
        
    //    public string submitted_by_email { get; set; }
        
    //    public string submitted_by_photo_url { get; set; }
        
    //    public string template_id { get; set; }
        
    //    public string template_name { get; set; }
        
    //    public string template_type { get; set; }
        
    //    public string template_type_formatted { get; set; }
        
    //    public string notes { get; set; }
        
    //    public string terms { get; set; }
        
    //    public Billing_Address billing_address { get; set; }
        
    //    public Shipping_Address shipping_address { get; set; }
        
    //    public string invoice_url { get; set; }
        
    //    public string subject_content { get; set; }
        
    //    public bool? can_send_in_mail { get; set; }
        
    //    public DateTime? created_time { get; set; }
        
    //    public DateTime? last_modified_time { get; set; }
        
    //    public string created_date { get; set; }
        
    //    public string created_date_formatted { get; set; }
        
    //    public string created_by_id { get; set; }
        
    //    public string created_by_name { get; set; }
        
    //    public string last_modified_by_id { get; set; }
        
    //    public string page_width { get; set; }
        
    //    public string page_height { get; set; }
        
    //    public string orientation { get; set; }
        
    //    public string is_backorder { get; set; }
        
    //    public string sales_channel { get; set; }
        
    //    public string color_code { get; set; }
        
    //    public string current_sub_status_id { get; set; }
        
    //    public string current_sub_status { get; set; }
        
    //    public string current_sub_status_formatted { get; set; }
        
    //    public object[] sub_statuses { get; set; }
        
    //    public string estimate_id { get; set; }
        
    //    public bool? is_client_review_settings_enabled { get; set; }
        
    //    public decimal? unused_retainer_payments { get; set; }
        
    //    public string unused_retainer_payments_formatted { get; set; }
        
    //    public decimal? credits_applied { get; set; }
        
    //    public string credits_applied_formatted { get; set; }
        
    //    public decimal? tax_amount_withheld { get; set; }
        
    //    public string tax_amount_withheld_formatted { get; set; }
        
    //    public string schedule_time { get; set; }
        
    //    public string schedule_time_formatted { get; set; }
        
    //    public int? no_of_copies { get; set; }
        
    //    public bool? show_no_of_copies { get; set; }
        
    //    public Customer_Default_Billing_Address customer_default_billing_address { get; set; }
        
    //    public bool? includes_package_tracking_info { get; set; }
        
    //    public object[] approvers_list { get; set; }
        
    //    public Qr_Code qr_code { get; set; }
    
    //}

    //public class Customer_Custom_Field_Hash
    //{

    //    public string cf_account_owner { get; set; }
        
    //    public string cf_account_owner_unformatted { get; set; }
    
    //}

    //public class Custom_Field_Hash
    //{
        
    //    public string cf_start { get; set; }
        
    //    public string cf_start_unformatted { get; set; }
        
    //    public string cf_end { get; set; }
        
    //    public string cf_end_unformatted { get; set; }
        
    //    public string cf_acc_nr { get; set; }
        
    //    public string cf_acc_nr_unformatted { get; set; }
        
    //    public string cf_rechnung_versendet { get; set; }
        
    //    public string cf_rechnung_versendet_unformatted { get; set; }
        
    //    public string cf_wiederholung { get; set; }
        
    //    public string cf_wiederholung_unformatted { get; set; }
        
    //    public string cf_falls_periodische_rechnung_ { get; set; }
        
    //    public string cf_falls_periodische_rechnung__unformatted { get; set; }
        
    //    public string cf_fixed_price_invoice_created { get; set; }
        
    //    public bool? cf_fixed_price_invoice_created_unformatted { get; set; }
    
    //}

    //public class Lock_Details
    //{
    //}

    //public class Payment_Options
    //{
        
    //    public object[] payment_gateways { get; set; }
    
    //}

    //public class Contact
    //{
        
    //    public decimal? customer_balance { get; set; }
        
    //    public string customer_balance_formatted { get; set; }
        
    //    public decimal? credit_limit { get; set; }
        
    //    public string credit_limit_formatted { get; set; }
        
    //    public decimal? unused_customer_credits { get; set; }
        
    //    public string unused_customer_credits_formatted { get; set; }
        
    //    public bool? is_credit_limit_migration_completed { get; set; }
    
    //}

    //public class Billing_Address
    //{

    //    public string street { get; set; }
        
    //    public string address { get; set; }
        
    //    public string street2 { get; set; }
        
    //    public string city { get; set; }
        
    //    public string state { get; set; }
        
    //    public string zip { get; set; }
        
    //    public string country { get; set; }
        
    //    public string fax { get; set; }
        
    //    public string phone { get; set; }
        
    //    public string attention { get; set; }
    
    //}

    //public class Shipping_Address
    //{
        
    //    public string street { get; set; }
        
    //    public string address { get; set; }
        
    //    public string street2 { get; set; }
        
    //    public string city { get; set; }
        
    //    public string state { get; set; }
        
    //    public string zip { get; set; }
        
    //    public string country { get; set; }
        
    //    public string fax { get; set; }
        
    //    public string phone { get; set; }
        
    //    public string attention { get; set; }
    
    //}

    //public class Customer_Default_Billing_Address
    //{
        
    //    public string zip { get; set; }
        
    //    public string country { get; set; }
        
    //    public string address { get; set; }
        
    //    public string city { get; set; }
        
    //    public string phone { get; set; }
        
    //    public string street2 { get; set; }
        
    //    public string state { get; set; }
        
    //    public string fax { get; set; }
        
    //    public string state_code { get; set; }
    
    //}

    //public class Qr_Code
    //{
        
    //    public string qr_source { get; set; }
        
    //    public bool? is_qr_enabled { get; set; }
        
    //    public string qr_value { get; set; }
        
    //    public string qr_description { get; set; }
    
    //}

    //public class Customer_Custom_Fields
    //{
        
    //    public string field_id { get; set; }
        
    //    public string customfield_id { get; set; }
        
    //    public bool? show_in_store { get; set; }
        
    //    public bool? show_in_portal { get; set; }
        
    //    public bool? is_active { get; set; }
        
    //    public int? index { get; set; }
        
    //    public string label { get; set; }

    //    public bool? show_on_pdf { get; set; }

    //    public bool? edit_on_portal { get; set; }

    //    public bool? edit_on_store { get; set; }

    //    public string api_name { get; set; }

    //    public bool? show_in_all_pdf { get; set; }
        
    //    public string value_formatted { get; set; }
        
    //    public string search_entity { get; set; }
        
    //    public string data_type { get; set; }
        
    //    public string placeholder { get; set; }
        
    //    public string value { get; set; }
        
    //    public bool? is_dependent_field { get; set; }
    
    //}

    //public class Custom_Fields
    //{
        
    //    public string field_id { get; set; }
        
    //    public string customfield_id { get; set; }
        
    //    public bool? show_in_store { get; set; }
        
    //    public bool? show_in_portal { get; set; }
        
    //    public bool? is_active { get; set; }

    //    public int? index { get; set; }
        
    //    public string label { get; set; }
        
    //    public bool? show_on_pdf { get; set; }
        
    //    public bool? edit_on_portal { get; set; }
        
    //    public bool? edit_on_store { get; set; }
        
    //    public string api_name { get; set; }
        
    //    public bool? show_in_all_pdf { get; set; }
        
    //    public string value_formatted { get; set; }

    //    public string search_entity { get; set; }
        
    //    public string data_type { get; set; }
        
    //    public string placeholder { get; set; }
        
    //    public object value { get; set; }
        
    //    public bool? is_dependent_field { get; set; }
        
    //    public string selected_option_id { get; set; }
    
    //}

    //public class Line_Items
    //{

    //    public string line_item_id { get; set; }
        
    //    public string item_id { get; set; }
        
    //    public string sku { get; set; }
        
    //    public int? item_order { get; set; }
        
    //    public string name { get; set; }
        
    //    public string internal_name { get; set; }
        
    //    public string description { get; set; }

    //    public string discount_account_id { get; set; }

    //    public string discount_account_name { get; set; }

    //    public string unit { get; set; }

    //    public decimal? quantity { get; set; }

    //    public decimal? discount_amount { get; set; }

    //    public string discount_amount_formatted { get; set; }

    //    public decimal? discount { get; set; }

    //    public object[] discounts { get; set; }

    //    public decimal? bcy_rate { get; set; }

    //    public string bcy_rate_formatted { get; set; }

    //    public decimal? rate { get; set; }

    //    public string rate_formatted { get; set; }

    //    public string account_id { get; set; }

    //    public string account_name { get; set; }

    //    public string header_id { get; set; }
        
    //    public string header_name { get; set; }
        
    //    public string pricebook_id { get; set; }
        
    //    public string tax_id { get; set; }
        
    //    public string tax_name { get; set; }
        
    //    public string tax_type { get; set; }
        
    //    public decimal? tax_percentage { get; set; }
        
    //    public decimal? item_total { get; set; }
        
    //    public string item_total_formatted { get; set; }
        
    //    public object[] item_custom_fields { get; set; }
        
    //    public string pricing_scheme { get; set; }
        
    //    public object[] tags { get; set; }
        
    //    public object[] documents { get; set; }
        
    //    public Line_Item_Taxes[] line_item_taxes { get; set; }
        
    //    public object[] line_item_tds { get; set; }
        
    //    public string bill_id { get; set; }
        
    //    public string bill_item_id { get; set; }
        
    //    public string project_id { get; set; }
        
    //    public string project_name { get; set; }
        
    //    public string[] time_entry_ids { get; set; }
        
    //    public string expense_id { get; set; }
        
    //    public string item_type { get; set; }
        
    //    public string item_type_formatted { get; set; }
        
    //    public string expense_receipt_name { get; set; }
        
    //    public string sales_rate { get; set; }
        
    //    public string sales_rate_formatted { get; set; }
        
    //    public string purchase_rate { get; set; }
        
    //    public string purchase_rate_formatted { get; set; }
        
    //    public string salesorder_item_id { get; set; }
        
    //    public decimal? cost_amount { get; set; }
        
    //    public string cost_amount_formatted { get; set; }
        
    //    public decimal? markup_percent { get; set; }
        
    //    public string markup_percent_formatted { get; set; }
    
    //}

    //public class Line_Item_Taxes
    //{

    //    public string tax_id { get; set; }
        
    //    public string tax_name { get; set; }
        
    //    public decimal? tax_amount { get; set; }
        
    //    public string tax_amount_formatted { get; set; }
    
    //}

    //public class Tax
    //{
        
    //    public decimal? tax_amount { get; set; }
        
    //    public string tax_name { get; set; }
        
    //    public string tax_amount_formatted { get; set; }
    
    //}

    //public class Contact_Persons_Associated
    //{
        
    //    public string contact_person_id { get; set; }
        
    //    public string contact_person_name { get; set; }
        
    //    public string first_name { get; set; }
        
    //    public string last_name { get; set; }
        
    //    public string contact_person_email { get; set; }
        
    //    public string phone { get; set; }
        
    //    public string mobile { get; set; }
        
    //    public string zcrm_contact_id { get; set; }
        
    //    public Communication_Preference communication_preference { get; set; }
    
    }

    //public class Communication_Preference
    //{
        
    //    public bool? is_email_enabled { get; set; }
    
    //}

    //public class Contact_Persons_Details
    //{
        
    //    public string contact_person_id { get; set; }
        
    //    public string first_name { get; set; }
        
    //    public string last_name { get; set; }
        
    //    public string email { get; set; }
        
    //    public string phone { get; set; }
        
    //    public string mobile { get; set; }
        
    //    public bool? is_primary_contact { get; set; }
        
    //    public string photo_url { get; set; }
    
    //}

}
