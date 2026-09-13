using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HoangZoho1.Models.WclSolutions
{

    public class GetMasterItemsResponse
    {

        public GetMasterItemsResponse()
        {
            master_items = new List<Master_Item>();
        }

        public List<Master_Item> master_items { get; set; }

    }

    public class Master_Item
    {
        public string item_id { get; set; }
        public string name { get; set; }
        public string item_name { get; set; }
        public string category_id { get; set; }
        public string category_name { get; set; }
        public string unit { get; set; }
        public string status { get; set; }
        public string source { get; set; }
        public bool? is_combo_product { get; set; }
        public bool? is_linked_with_zohocrm { get; set; }
        public string zcrm_product_id { get; set; }
        public string description { get; set; }
        public string brand { get; set; }
        public string manufacturer { get; set; }
        public decimal? rate { get; set; }
        public string tax_id { get; set; }
        public string tax_name { get; set; }
        public decimal? tax_percentage { get; set; }
        public string purchase_account_id { get; set; }
        public string purchase_account_name { get; set; }
        public string account_id { get; set; }
        public string account_name { get; set; }
        public string purchase_description { get; set; }
        public decimal? purchase_rate { get; set; }
        public string item_type { get; set; }
        public string product_type { get; set; }
        public decimal? stock_on_hand { get; set; }
        public bool? has_attachment { get; set; }
        public bool? is_returnable { get; set; }
        public decimal? available_stock { get; set; }
        public decimal? actual_available_stock { get; set; }
        public string sku { get; set; }
        public string upc { get; set; }
        public string ean { get; set; }
        public string isbn { get; set; }
        public string part_number { get; set; }
        public bool? track_batch_number { get; set; }
        public object reorder_level { get; set; }
        public string image_name { get; set; }
        public string image_type { get; set; }
        public string image_document_id { get; set; }
        public DateTime? created_time { get; set; }
        public DateTime? last_modified_time { get; set; }
        public string length { get; set; }
        public string width { get; set; }
        public string height { get; set; }
        public string weight { get; set; }
        public string weight_unit { get; set; }
        public string dimension_unit { get; set; }
        public List<ItemBatch> batches { get; set; }
    }

    public class ItemBatch
    {
        public string batch_in_id { get; set; }
        public string batch_number { get; set; }
        public string internal_batch_number { get; set; }
        public string external_batch_number { get; set; }
        public string manufacturer_date { get; set; }
        public decimal? in_quantity { get; set; }
        public decimal? balance_quantity { get; set; }
        public string warehouse_id { get; set; }
        public string status { get; set; }
        public string warehouse_name { get; set; }
    }

}
