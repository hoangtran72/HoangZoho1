using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HoangZoho1.Models.WclSolutions
{

    public class GetItemBatchesResponse
    {

        public GetItemBatchesResponse()
        {

            batches = new List<Batch>();

        }

        public int? code { get; set; }
        
        public string message { get; set; }
        
        public List<Batch> batches { get; set; }
        
        public Page_Context page_context { get; set; }
    
    }

    public class Batch
    {

        public string batch_in_id { get; set; }

        public string batch_number { get; set; }

        public string internal_batch_number { get; set; }

        public string external_batch_number { get; set; }

        public string manufacturer_date { get; set; }

        public decimal? in_quantity { get; set; }

        public decimal? balance_quantity { get; set; }

        public string warehouse_id { get; set; }

        public string warehouse_name { get; set; }

        public string status { get; set; }

    }

}
