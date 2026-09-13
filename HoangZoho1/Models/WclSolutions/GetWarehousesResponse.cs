using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HoangZoho1.Models.WclSolutions
{

    public class GetWarehousesResponse
    {
        


        public int code { get; set; }
        
        public string message { get; set; }
        
        public List<Warehouse> warehouses { get; set; }
    
    }

    public class Warehouse
    {

        public string warehouse_id { get; set; }
        
        public string warehouse_name { get; set; }
        
        public string attention { get; set; }
        
        public string address { get; set; }
        
        public string address1 { get; set; }
        
        public string address2 { get; set; }
        
        public string city { get; set; }
        
        public string state { get; set; }
        
        public string state_code { get; set; }
        
        public string country { get; set; }
        
        public string zip { get; set; }
        
        public string phone { get; set; }
        
        public string email { get; set; }
        
        public bool? is_org_level_primary { get; set; }
        
        public bool? is_primary { get; set; }
        
        public bool? is_permitted_warehouse { get; set; }
        
        public string status { get; set; }
        
        public bool? is_fba_warehouse { get; set; }
        
        public object[] sales_channels { get; set; }
        
        public string branch_id { get; set; }
        
        public string branch_name { get; set; }
        
        public string tax_reg_no { get; set; }
        
        public decimal? total_zones { get; set; }
        
        public decimal? total_storagelocations { get; set; }
    
    }

}
