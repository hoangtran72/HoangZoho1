using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HoangZoho1.Models.PinjarraBakery.ZohoCRM
{
    public class ProductLogForUpdation
    {
        public ProductLogForUpdation()
        {

            Product_Logs = new List<ProductLog>();

        }

        public List<ProductLog> Product_Logs { get; set; }

    }

}
