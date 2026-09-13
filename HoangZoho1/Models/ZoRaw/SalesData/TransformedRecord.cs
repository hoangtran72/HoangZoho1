using System.Collections.Generic;

namespace HoangZoho1.Models.ZoRaw.SalesData
{

    public class TransformedRecord
    {

        public string MonthYear { get; set; }

        public string Qty { get; set; }

        public string Amount { get; set; }

        public Dictionary<string, string> FixedFields { get; set; } = new Dictionary<string, string>();

    }

}
