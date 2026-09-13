using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HoangZoho1.Models.PinjarraBakery.ZohoCRM
{
    public class ProductLogForCreation
    {
        public ProductLogForCreation()
        {

            Product_Logs = new List<ProductLog>();

        }

        public string Name { get; set; }

        public string Name1 { get; set; }

        public string Product_Date { get; set; }

        public List<ProductLog> Product_Logs { get; set; }

    }

    public class ProductLog
    {

        public string Department { get; set; }

        public string Key_Steps { get; set; }

        public string Product { get; set; }

        public decimal? Time_minute { get; set; }

        public decimal? Yield { get; set; }

        public string Comments { get; set; }

    }
}
