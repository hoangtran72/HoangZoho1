using System.Collections.Generic;

namespace HoangZoho1.Models.RestaurantEquipmentOnline.ZohoCRM
{

    public class QuoteForCreation
    {

        public string Subject { get; set; }

        public string Deal_Name { get; set; }

        public string Quote_Stage { get; set; }

        public string Currency { get; set; }

        public string Contact_Name { get; set; }

        public string Account_Name { get; set; }

        public string Billing_Street { get; set; }

        public string Billing_City { get; set; }

        public string Billing_State { get; set; }

        public string Billing_Code { get; set; }

        public string Billing_Country { get; set; }

        public string Shipping_Street { get; set; }

        public string Shipping_City { get; set; }

        public string Shipping_State { get; set; }

        public string Shipping_Code { get; set; }

        public string Shipping_Country { get; set; }

        public List<CreateQuoteItem> Quoted_Items { get; set; }

        public decimal? Discount { get; set; }


    }

    public class CreateQuoteItem
    {

        public string Product_Name { get; set; }

        public decimal? List_Price { get; set; }

        public decimal? Cost_Price { get; set; }

        public decimal? Quantity { get; set; }

    }

}