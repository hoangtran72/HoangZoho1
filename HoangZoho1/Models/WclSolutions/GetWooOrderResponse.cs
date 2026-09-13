using System;
using System.Collections.Generic;

namespace HoangZoho1.Models.WclSolutions
{

    public class GetWooOrderResponse
    {

        public int? id { get; set; }
        
        public int? parent_id { get; set; }
        
        public string status { get; set; }
        
        public string currency { get; set; }
        
        public string version { get; set; }
        
        public bool? prices_include_tax { get; set; }
        
        public DateTime? date_created { get; set; }
        
        public DateTime? date_modified { get; set; }
        
        public string discount_total { get; set; }
        
        public string discount_tax { get; set; }
        
        public string shipping_total { get; set; }
        
        public string shipping_tax { get; set; }
        
        public string cart_tax { get; set; }
        
        public string total { get; set; }
        
        public string total_tax { get; set; }
        
        public int? customer_id { get; set; }
        
        public string order_key { get; set; }

        public Billing billing { get; set; }
        
        public Shipping shipping { get; set; }
        
        public string payment_method { get; set; }
        
        public string payment_method_title { get; set; }
        
        public string transaction_id { get; set; }
        
        public string customer_ip_address { get; set; }
        
        public string customer_user_agent { get; set; }
        
        public string created_via { get; set; }
        
        public string customer_note { get; set; }
        
        public DateTime? date_completed { get; set; }
        
        public object date_paid { get; set; }
        
        public string cart_hash { get; set; }
        
        public string number { get; set; }
        
        public List<Meta_Data> meta_data { get; set; }
        
        public List<Woo_Line_Items> line_items { get; set; }
        
        public List<Tax_Lines> tax_lines { get; set; }
        
        public List<Shipping_Lines> shipping_lines { get; set; }
        
        public List<Fee_Lines> fee_lines { get; set; }
        
        public object[] coupon_lines { get; set; }
        
        public object[] refunds { get; set; }
        
        public string payment_url { get; set; }
        
        public bool? is_editable { get; set; }
        
        public bool? needs_payment { get; set; }
        
        public bool? needs_processing { get; set; }
        
        public DateTime? date_created_gmt { get; set; }
        
        public DateTime? date_modified_gmt { get; set; }
        
        public DateTime? date_completed_gmt { get; set; }
        
        public object date_paid_gmt { get; set; }
        
        public string currency_symbol { get; set; }
        
        public _Links _links { get; set; }
    
    }

    public class Billing
    {
        
        public string first_name { get; set; }
        
        public string last_name { get; set; }
        
        public string company { get; set; }
        
        public string address_1 { get; set; }
        
        public string address_2 { get; set; }
        
        public string city { get; set; }
        
        public string state { get; set; }
        
        public string postcode { get; set; }
        
        public string country { get; set; }
        
        public string email { get; set; }
        
        public string phone { get; set; }
    
    }

    public class Shipping
    {
        
        public string first_name { get; set; }
        
        public string last_name { get; set; }
        
        public string company { get; set; }
        
        public string address_1 { get; set; }
        
        public string address_2 { get; set; }
        
        public string city { get; set; }
        
        public string state { get; set; }
        
        public string postcode { get; set; }
        
        public string country { get; set; }
        
        public string phone { get; set; }
    
    }

    public class _Links
    {
        
        public List<Self> self { get; set; }
        
        public List<Collection> collection { get; set; }
        
        public Email_Templates[] email_templates { get; set; }
    
    }

    public class Self
    {
        
        public string href { get; set; }
        
        public TargetHints targetHints { get; set; }
    
    }

    public class TargetHints
    {

        public List<string> allow { get; set; }
    
    }

    public class Collection
    {
        
        public string href { get; set; }
    
    }

    public class Email_Templates
    {
        
        public bool? embeddable { get; set; }
        
        public string href { get; set; }
    
    }

    public class Meta_Data
    {
        
        public int id { get; set; }
        
        public string key { get; set; }
        
        public object value { get; set; }
    
    }

    public class Woo_Line_Items
    {
        
        public int? id { get; set; }
        
        public string name { get; set; }
        
        public int? product_id { get; set; }
        
        public int? variation_id { get; set; }
        
        public int? quantity { get; set; }
        
        public string tax_class { get; set; }
        
        public string subtotal { get; set; }

        public string subtotal_tax { get; set; }
        
        public string total { get; set; }
        
        public string total_tax { get; set; }
        
        public List<Tax> taxes { get; set; }
        
        public List<Meta_Data> meta_data { get; set; }

        public string sku { get; set; }
        
        public decimal? price { get; set; }
        
        public Image image { get; set; }
        
        public string parent_name { get; set; }
    
    }

    public class Image
    {

        public int? id { get; set; }
        
        public string src { get; set; }
    
    }

    public class Tax
    {

        public int? id { get; set; }
        
        public string total { get; set; }
        
        public string subtotal { get; set; }
    
    }

    public class Tax_Lines
    {

        public int? id { get; set; }
        
        public string rate_code { get; set; }
        
        public int? rate_id { get; set; }
        
        public string label { get; set; }
        
        public bool? compound { get; set; }
        
        public string tax_total { get; set; }
        
        public string shipping_tax_total { get; set; }
        
        public decimal? rate_percent { get; set; }
        
        public List<Meta_Data> meta_data { get; set; }
    
    }

    public class Shipping_Lines
    {
        
        public int? id { get; set; }
        
        public string method_title { get; set; }
        
        public string method_id { get; set; }
        
        public string instance_id { get; set; }
        
        public string total { get; set; }
        
        public string total_tax { get; set; }
        
        public List<Tax> taxes { get; set; }
        
        public string tax_status { get; set; }
        
        public List<Meta_Data> meta_data { get; set; }
    
    }

    public class Fee_Lines
    {

        public int? id { get; set; }
        
        public string name { get; set; }
        
        public string tax_class { get; set; }
        
        public string tax_status { get; set; }
        
        public string amount { get; set; }
        
        public string total { get; set; }
        
        public string total_tax { get; set; }
        
        public List<Tax> taxes { get; set; }
        
        public List<Meta_Data> meta_data { get; set; }
    
    }

}
