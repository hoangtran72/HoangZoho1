using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HoangZoho1.Models.PinjarraBakery.ZohoCRM
{

    public class SearchProductLogsResponse
    {

        public List<ProductLogSearchData> data { get; set; }

        public Info info { get; set; }

    }

    public class ProductLogSearchData
    {

        public Owner Owner { get; set; }

        public int Total_Amount { get; set; }

        public string Name { get; set; }

        public string Product_Date { get; set; }

        public Modified_By Modified_By { get; set; }

        public string id { get; set; }

        public string Name1 { get; set; }

        public DateTime? Modified_Time { get; set; }

        public DateTime? Created_Time { get; set; }

        public Created_By Created_By { get; set; }

        public List<string> Tag { get; set; }

        public int? Total_Time_minute { get; set; }

    }

}
