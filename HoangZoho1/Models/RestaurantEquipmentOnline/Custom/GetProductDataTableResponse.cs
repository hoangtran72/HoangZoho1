using HoangZoho1.Models.RestaurantEquipmentOnline.ZohoCRM;
using System.Collections.Generic;

namespace HoangZoho1.Models.RestaurantEquipmentOnline.Custom
{

    public class GetProductDataTableResponse
    {

        public List<List<string>> TableData { get; set; }

        public List<ProductQueryModel> ProductData { get; set; }

    }

}
