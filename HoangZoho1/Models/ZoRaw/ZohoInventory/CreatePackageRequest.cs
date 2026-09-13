using Newtonsoft.Json;
using System.Collections.Generic;

namespace HoangZoho1.Models.ZoRaw.ZohoInventory
{

    public class CreatePackageRequest
    {

        public string date { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public List<Custom_Field> custom_fields { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public List<CreatePackageLineItem> line_items { get; set; }

    }

    public class CreatePackageLineItem
    {

        public CreatePackageLineItem()
        {
            
            batches = new List<CreatePackageBatch>();

        }

        public string so_line_item_id { get; set; }

        public float? quantity { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public List<CreatePackageBatch> batches { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public List<CreatePackageMappedItem> mapped_items { get; set; }

    }

    public class CreatePackageMappedItem
    {

        public string so_line_item_id { get; set; }

        public float? quantity { get; set; }
    
        public List<CreatePackageBatch> batches { get; set; }

    }

    public class CreatePackageBatch
    {

        public string batch_id { get; set; }

        public string out_quantity { get; set; }

        public string batch_in_id { get; set; }

    }

}
