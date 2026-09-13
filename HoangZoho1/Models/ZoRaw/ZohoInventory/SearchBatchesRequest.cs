namespace HoangZoho1.Models.ZoRaw.ZohoInventory
{
    
    public class SearchBatchesRequest
    {

        public string location_id { get; set; }

        public string item_id { get; set; }

        public bool? include_empty_batches { get; set; }

    }

}
