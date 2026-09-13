using System.Collections.Generic;

namespace HoangZoho1.Models.RestaurantEquipmentOnline.ZohoCRM
{

    public class UpdateQuoteRequest
    {

        public UpdateQuoteRequest()
        {
         
            Quoted_Items = new List<UpdateQuotedItem>();

        }

        public List<UpdateQuotedItem> Quoted_Items { get; set; }
    
    }

    public class UpdateQuotedItem
    {

        public string id { get; set; }
        
        // public object _delete { get; set; }

        public int? Sequence_Number { get; set; }

        public double? Quantity { get; set; }

        public double? Cost_Price { get; set; }

        public double? List_Price { get; set; }

        public string Product_Name { get; set; }
    
    }


}
