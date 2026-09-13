using System.Collections.Generic;

namespace HoangZoho1.Models.ZoRaw.ZohoInventory
{

    public class SearchBatchesResponse
    {

        public SearchBatchesResponse()
        {
                
            batches = new List<SearchBatchResult>();

        }

        public int? code { get; set; }
        
        public string message { get; set; }
        
        public List<SearchBatchResult> batches { get; set; }
        
        public Page_Context page_context { get; set; }
    
    }

    public class Page_Context
    {

        public int? page { get; set; }
        
        public int? per_page { get; set; }
        
        public bool? has_more_page { get; set; }
        
        public string sort_column { get; set; }
        
        public string sort_order { get; set; }
    
    }

    public class SearchBatchResult
    {
        
        public string batch_id { get; set; }
        
        public string batch_in_id { get; set; }
        
        public string batch_number { get; set; }
        
        public string internal_batch_number { get; set; }
        
        public string external_batch_number { get; set; }
        
        public string manufacturer_date { get; set; }
        
        public string manufacturer_batch_number { get; set; }
        
        public string manufactured_date { get; set; }
        
        public string expiry_date { get; set; }
        
        public float? in_quantity { get; set; }
        
        public float? balance_quantity { get; set; }
        
        public string location_id { get; set; }
    
    }

}
