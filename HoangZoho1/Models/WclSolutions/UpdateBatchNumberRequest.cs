using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HoangZoho1.Models.WclSolutions
{

    public class UpdateBatchNumberRequest
    {

        public UpdateBatchNumberRequest()
        {

            line_items = new List<Line_Items>();

        }

        public List<Line_Items> line_items { get; set; }

    }

    public class Line_Items
    {

        public Line_Items()
        {

            item_custom_fields = new List<Item_Custom_Fields>();

        }

        public string line_item_id { get; set; }
        
        public string variant_id { get; set; }
        
        public bool? track_batch_number { get; set; }
        
        public string item_id { get; set; }
        
        public bool? is_returnable { get; set; }
        
        public string product_id { get; set; }
        
        public bool? is_combo_product { get; set; }
        
        public string warehouse_id { get; set; }
        
        public string warehouse_name { get; set; }
        
        public string sku { get; set; }
        
        public string name { get; set; }
        
        public string group_name { get; set; }
        
        public string description { get; set; }
        
        public int? item_order { get; set; }
        
        public decimal? bcy_rate { get; set; }
        
        public string bcy_rate_formatted { get; set; }
        
        public decimal? rate { get; set; }
        
        public string rate_formatted { get; set; }
        
        public decimal? sales_rate { get; set; }
        
        public string sales_rate_formatted { get; set; }
        
        public decimal? quantity { get; set; }

        public string unit { get; set; }
        
        public string pricebook_id { get; set; }
        
        public string header_id { get; set; }
        
        public string header_name { get; set; }
        
        public decimal? discount { get; set; }
        
        public object[] discounts { get; set; }
        
        public string tax_id { get; set; }
        
        public string tax_name { get; set; }
        
        public string tax_type { get; set; }
        
        public decimal? tax_percentage { get; set; }
        
        public object[] line_item_taxes { get; set; }
        
        public decimal? item_total { get; set; }
        
        public string item_total_formatted { get; set; }
        
        public int item_sub_total { get; set; }
        
        public string item_sub_total_formatted { get; set; }
        
        public decimal? item_total_inclusive_of_tax { get; set; }
        
        public string item_total_inclusive_of_tax_formatted { get; set; }
        
        public string line_item_type { get; set; }
        
        public string item_type { get; set; }
        
        public string item_type_formatted { get; set; }
        
        public bool? is_invoiced { get; set; }
        
        public bool? is_unconfirmed_product { get; set; }
        
        public object[] tags { get; set; }
        
        public string image_name { get; set; }
        
        public string image_type { get; set; }
        
        public string image_document_id { get; set; }
        
        public string document_id { get; set; }

        public List<Item_Custom_Fields> item_custom_fields { get; set; }

        public decimal? quantity_invoiced { get; set; }

        public decimal? quantity_packed { get; set; }

        public decimal? quantity_shipped { get; set; }

        public decimal? quantity_backordered { get; set; }

        public decimal? quantity_dropshipped { get; set; }

        public decimal? quantity_cancelled { get; set; }

        public decimal? quantity_delivered { get; set; }

        public decimal? quantity_invoiced_cancelled { get; set; }

        public decimal? quantity_returned { get; set; }

        public int? is_fulfillable { get; set; }

        public string project_id { get; set; }

    }

    public class Item_Custom_Fields
    {

        public string customfield_id { get; set; }

        public string value { get; set; }

    }

}
