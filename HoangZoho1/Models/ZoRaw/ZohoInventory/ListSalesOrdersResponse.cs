using System;
using System.Collections.Generic;

namespace HoangZoho1.Models.ZoRaw.ZohoInventory
{

    public class ListSalesOrdersResponse
    {

        public int? code { get; set; }
        
        public string message { get; set; }
        
        public List<Salesorder> salesorders { get; set; }
        
        public Page_Context page_context { get; set; }
    
    }

    public class Search_Criteria
    {

        public string column_name { get; set; }
        
        public string search_text { get; set; }
        
        public string search_text_formatted { get; set; }
        
        public string comparator { get; set; }
    
    }

    public class Salesorder
    {
        
        public string salesorder_id { get; set; }
        
        public string zcrm_potential_id { get; set; }
        
        public string zcrm_potential_name { get; set; }
        
        public string customer_name { get; set; }
        
        public string customer_id { get; set; }
        
        public string email { get; set; }
        
        public string delivery_date { get; set; }
        
        public string company_name { get; set; }
        
        public string color_code { get; set; }
        
        public string current_sub_status_id { get; set; }
        
        public string current_sub_status { get; set; }
        
        public string pickup_location_id { get; set; }
        
        public string salesorder_number { get; set; }
        
        public string reference_number { get; set; }
        
        public string date { get; set; }
        
        public string shipment_date { get; set; }
        
        public string shipment_days { get; set; }
        
        public string due_by_days { get; set; }
        
        public string due_in_days { get; set; }
        
        public string currency_id { get; set; }
        
        public string source { get; set; }
        
        public string currency_code { get; set; }
        
        public decimal? total { get; set; }
        
        public decimal? bcy_total { get; set; }
        
        public decimal? total_invoiced_amount { get; set; }
        
        public DateTime? created_time { get; set; }
        
        public DateTime? last_modified_time { get; set; }
        
        public bool? is_emailed { get; set; }
        
        public decimal? quantity { get; set; }
        
        public decimal? quantity_invoiced { get; set; }
        
        public decimal? quantity_packed { get; set; }
        
        public decimal? quantity_shipped { get; set; }
        
        public string order_status { get; set; }
        
        public string invoiced_status { get; set; }
        
        public string paid_status { get; set; }
        
        public string shipped_status { get; set; }
        
        public string status { get; set; }
        
        public string order_fulfillment_type { get; set; }
        
        public bool? is_drop_shipment { get; set; }
        
        public bool? is_backorder { get; set; }
        
        public bool? is_manually_fulfilled { get; set; }
        
        public string sales_channel { get; set; }
        
        public string sales_channel_formatted { get; set; }
        
        public string salesperson_name { get; set; }
        
        public string branch_id { get; set; }
        
        public string location_id { get; set; }
        
        public string location_name { get; set; }
        
        public bool? has_attachment { get; set; }
        
        public object[] tags { get; set; }
        
        public decimal? balance { get; set; }
        
        public string delivery_method { get; set; }
        
        public string delivery_method_id { get; set; }
        
        public bool? is_viewed_in_mail { get; set; }
        
        public string mail_first_viewed_time { get; set; }
        
        public string mail_last_viewed_time { get; set; }
        
        public bool? is_scheduled_for_quick_shipment_create { get; set; }
    
    }

}
