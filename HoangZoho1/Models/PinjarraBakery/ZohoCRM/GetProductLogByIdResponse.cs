using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HoangZoho1.Models.PinjarraBakery.ZohoCRM
{

    public class GetProductLogByIdResponse
    {
        public List<ProductLogData> data { get; set; }
    }

    public class ProductLogData
    {

        public Owner Owner { get; set; }

        public decimal? Total_Amount { get; set; }

        public List<Product_Logs> Product_Logs { get; set; }

        public string Name { get; set; }

        public string Product_Date { get; set; }

        public Modified_By Modified_By { get; set; }

        public string id { get; set; }

        public string Name1 { get; set; }

        public Approval approval { get; set; }

        public DateTime? Modified_Time { get; set; }

        public DateTime? Created_Time { get; set; }

        public Created_By Created_By { get; set; }

        public List<string> Tag { get; set; }

        public decimal? Total_Time_minute { get; set; }

    }

    public class Product_Logs
    {

        public string Department { get; set; }

        public DateTime? Created_Time { get; set; }

        public string Key_Steps { get; set; }

        public string Comments { get; set; }

        public decimal? Time_minute { get; set; }

        public string Product { get; set; }

        public decimal? Yield { get; set; }

        public string id { get; set; }

        public string LinkingModule5_Serial_Number { get; set; }

    }

    public class Parent_Id
    {
        public string name { get; set; }
        public string id { get; set; }
    }

}
