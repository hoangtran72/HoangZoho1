using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HoangZoho1.Models.RestaurantEquipmentOnline.Shopify
{

    public class GetQuoteDetailsResponse
    {

        public bool? success { get; set; }

        public decimal? discount { get; set; }

        public decimal? shipping_rate { get; set; }

        public string quote_from { get; set; }

    }

}
