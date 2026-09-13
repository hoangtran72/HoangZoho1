using System.Collections.Generic;

namespace HoangZoho1.Models.RestaurantEquipmentOnline.Custom
{

    public class EasyAddQuoteRequest
    {

        public string DealId { get; set; }

        public decimal? Discount { get; set; }

        public List<QuoteItem> QuoteItems { get; set; }

    }

    public class QuoteItem
    {

        public decimal? Cost_Price { get; set; }

        public decimal? List_Price { get; set; }

        public string Product_Name { get; set; }

        public decimal? Quantity { get; set; }

    }

}
