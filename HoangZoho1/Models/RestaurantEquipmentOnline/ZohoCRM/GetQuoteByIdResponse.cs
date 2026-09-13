using System;

namespace HoangZoho1.Models.RestaurantEquipmentOnline.ZohoCRM
{

    public class GetQuoteByIdResponse
    {

        public QuoteData[] data { get; set; }

    }

    public class QuoteData
    {

        public Owner Owner { get; set; }
        
        public string currency_symbol { get; set; }
        
        public string Prefer_Call_or_Email { get; set; }

        public string Quote_Invoice_URL { get; set; }

        public double? Quantity_in_Stock { get; set; }

        public double? Tax_Amount { get; set; }

        public DateTime? Last_Activity_Time { get; set; }

        public double? RRP_Discount_Percent { get; set; }

        // public string Deal_Name { get; set; }

        public double? Exchange_Rate { get; set; }

        public string Billing_Country { get; set; }

        public string Currency { get; set; }

        public double? Weekly_Rental_Price { get; set; }

        public double? Profit { get; set; }

        public string id { get; set; }

        public string Carrier { get; set; }

        public Quoted_Items[] Quoted_Items { get; set; }

        public double? Cost_Total { get; set; }

        public double? Daily_Rental_Price { get; set; }

        public double? Grand_Total { get; set; }

        public Approval approval { get; set; }

        public double? Sub_Total_after_Discount { get; set; }
        
        public string Billing_Street { get; set; }
        public DateTime? Created_Time { get; set; }

        // public object Handler { get; set; }

        public string Billing_Code { get; set; }

        public string Shipping_City { get; set; }
        
        public string Shipping_Country { get; set; }
        
        public string Shipping_Code { get; set; }
        
        public string Billing_City { get; set; }
        
        public string Quote_Number { get; set; }

        public Created_By Created_By { get; set; }

        public DateTime? Equipment_Required_Date { get; set; }
        
        public string Shipping_Street { get; set; }
        
        public double? RRP_Discount_Value { get; set; }
        
        public string Description { get; set; }
        
        public double? Discount { get; set; }
        
        public string Product_Dimensions { get; set; }
        
        public string Shipping_State { get; set; }
        
        public string Website { get; set; }
        
        public Layout_Id layout_id { get; set; }
        
        public string Quote_No { get; set; }
        
        public Modified_By Modified_By { get; set; }
        
        public double? Shipping_Cost { get; set; }

        public string Valid_Till { get; set; }

        //public string Account_Name { get; set; }

        public string Shopify_Payment_Link { get; set; }

        public double? Quote_Margin { get; set; }

        public string Quote_Stage { get; set; }

        public string Delivery_Type { get; set; }

        public DateTime Modified_Time { get; set; }

        public string Lead_Name { get; set; }

        public string Terms_and_Conditions { get; set; }

        public double? Sub_Total { get; set; }

        public double? Shipping_Handling { get; set; }

        public string Subject { get; set; }

        public Contact_Name Contact_Name { get; set; }

        public string Usage_Unit { get; set; }

        public double? Qty_In_Demand { get; set; }

        public double? Qty_Ordered { get; set; }

        public string Billing_State { get; set; }

        public string[] Tag { get; set; }

        public object Reorder_Level { get; set; }

        public Has_More has_more { get; set; }

    }

    public class Contact_Name
    {
        
        public string name { get; set; }
        
        public string id { get; set; }
    
    }

    public class Has_More
    {
        
        public bool? Quoted_Items { get; set; }
    
    }

    public class Quoted_Items
    {

        public double? Cost_Price { get; set; }
        
        public DateTime? Modified_Time { get; set; }

        public DateTime? Created_Time { get; set; }

        public Parent_Id Parent_Id { get; set; }

        public string Sequence_Number { get; set; }

        public Product_Name Product_Name { get; set; }
        
        public double? Quantity { get; set; }
        
        public Layout_Id layout_id { get; set; }

        public double? Net_Total { get; set; }

        public bool? in_merge { get; set; }

        public double? Total { get; set; }

        public double? Total_Cost { get; set; }

        public object Price_Book_Name { get; set; }

        public double? List_Price { get; set; }
        
        public string id { get; set; }
    
    }

    public class Product_Name
    {

        public string Product_Code { get; set; }

        public double? Qty_Ordered { get; set; }
        
        public Layout Layout { get; set; }
        
        public string name { get; set; }
        
        public double? Qty_in_Stock { get; set; }
        
        public object[] Tax { get; set; }
        
        public string id { get; set; }
        
        public bool? Taxable { get; set; }
        
        public double? Unit_Price { get; set; }
        
        public double? Reorder_Level { get; set; }
    
    }

    public class Layout
    {

        public string id { get; set; }
    
    }

}
