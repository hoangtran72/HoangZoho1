using System.Collections.Generic;

namespace HoangZoho1.Models.ZoRaw.SalesData
{
    public class ExcelMapping
    {

        // Fixed columns mapping: internal name -> Excel header
        public Dictionary<string, string> FixedColumns { get; set; }

        // Row numbers (1-based)
        public int FixedHeaderRow { get; set; }      // e.g., row 2 has fixed columns
        public int DynamicHeaderRow { get; set; }   // e.g., row 1 has month-year headers
        public int StartRow { get; set; }          // data starts here

        // Ignore row check
        public int IgnoreColumnIndex { get; set; } = 4;
        public List<string> IgnoreValues { get; set; } = new List<string> { "Total", "Subtotal", "Grand Total" };

    }
}
