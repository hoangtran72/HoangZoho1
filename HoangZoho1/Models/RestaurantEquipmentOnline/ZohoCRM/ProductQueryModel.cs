using System.Collections.Generic;

namespace HoangZoho1.Models.RestaurantEquipmentOnline.ZohoCRM
{

    public class ProductQueryModel
    {

        public float? Cost_Price { get; set; }

        public float? Unit_Price { get; set; }

        public float? KE_Price { get; set; }

        public string Product_Code { get; set; }
        
        public string Description { get; set; }
        
        public string Product_Dimensions { get; set; }
        
        public string Product_Name { get; set; }

        public string Image_URL { get; set; }

        public string Final_Category { get; set; }

        public string Categories { get; set; }

        public float? Qty_in_Stock { get; set; }

        public string id { get; set; }

        public float? Width { get; set; }

        public float? Height { get; set; }

        public float? Depth { get; set; }

        public Dictionary<string, string> DSim { get; set; }

        public string Supplier { get; set; }

    }

}
