using HoangZoho1.Models.RestaurantEquipmentOnline.ZohoCRM;
using System.Collections.Generic;

namespace HoangZoho1.Models.RestaurantEquipmentOnline.Custom
{
    
    public class GetQuoteTableResponse
    {

        public List<List<object>> TableData { get; set; }

        public List<ProductDataModel> ProductData { get; set; }

        public float? MinPrice { get; set; }

        public float? MaxPrice { get; set; }

        public float? MinHeight { get; set; }

        public float? MaxHeight { get; set; }

        public float? MinWidth { get; set; }

        public float? MaxWidth { get; set; }

        public float? MinDepth { get; set; }

        public float? MaxDepth { get; set; }

    }

}
