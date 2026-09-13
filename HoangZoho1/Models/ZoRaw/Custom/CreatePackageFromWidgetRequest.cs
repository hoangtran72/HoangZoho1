using System.Collections.Generic;

namespace HoangZoho1.Models.ZoRaw.Custom
{

    public class CreatePackageFromWidgetRequest
    {

        public List<ScannedItem> ScannedItems { get; set; }

        public string SalesOrderId { get; set; }
    
    }

    public class ScannedItem
    {

        public string item_id { get; set; }
        
        public string line_item_id { get; set; }
        
        public string sku { get; set; }
        
        public string item_name { get; set; }
        
        public string upcEan { get; set; }
        
        public float? quantity { get; set; }
        
        public string unit { get; set; }
        
        public string batchInfo { get; set; }

        public List<MappedItem> mapped_items { get; set; }

    }

    public class MappedItem
    {

        public string line_item_id { get; set; }

        public float? quantity { get; set; }

        public string batchInfo { get; set; }

    }

}
