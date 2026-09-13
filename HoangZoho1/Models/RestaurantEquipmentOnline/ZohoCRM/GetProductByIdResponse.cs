using System;

namespace HoangZoho1.Models.RestaurantEquipmentOnline.ZohoCRM
{

    public class GetProductByIdResponse
    {

        public ProductData[] data { get; set; }
    
    }

    public class ProductData
    {

        public double? Qty_in_Demand { get; set; }

        public Owner Owner { get; set; }
        
        public object[] Products { get; set; }
        
        public string Large_Item_Reasons { get; set; }

        public double? Special_Price_NZ { get; set; }
        
        public DateTime? Sales_Start_Date { get; set; }

        public object[] Tax { get; set; }

        public bool? Large_Item { get; set; }
        
        public bool? Product_Active { get; set; }
        
        public double? B2B_Price { get; set; }
        
        public DateTime? Last_Activity_Time { get; set; }
        
        public double? Special_Price { get; set; }
        
        public string Packed_Dimensions { get; set; }

        public string Packed_Height_mm { get; set; }

        public double? Quantity_in_Stock_NZ { get; set; }

        public string Manufacturer { get; set; }

        public string id { get; set; }

        public string Packed_Weight { get; set; }

        public string Packed_Width_mm { get; set; }
        
        public DateTime? Created_Time { get; set; }
        
        public string Product_Name { get; set; }
        
        //public object Handler { get; set; }
        
        public DateTime? Support_Start_Date { get; set; }
        
        public string Weight { get; set; }
        
        public string Packed_Depth_mm { get; set; }
        
        public string Customer_ID { get; set; }
        
        public Created_By Created_By { get; set; }
        
        public double? KE_Price { get; set; }
        
        public bool? Taxable { get; set; }
        
        public string Product_Category { get; set; }
        
        public double? Cost_Price { get; set; }
        
        public string Description { get; set; }
        
        public string Product_Dimensions { get; set; }

        public string Record_Image { get; set; }

        public Modified_By Modified_By { get; set; }

        public string Product_Code { get; set; }

        public string Image_URL { get; set; }

        public string Product_Height_mm { get; set; }

        public string Meta_Title { get; set; }

        public string Product_Width_mm { get; set; }

        public double? Cost_Price_NZ { get; set; }

        public string Supplier { get; set; }

        public DateTime? Support_Expiry_Date { get; set; }

        public double? Unit_Price_NZ { get; set; }

        public DateTime? Modified_Time { get; set; }

        public string Meta_Description { get; set; }

        public string Product_Depth_mm { get; set; }

        public double? Commission_Rate { get; set; }

        public string Usage_Unit { get; set; }

        public double? Qty_Ordered { get; set; }

        public double? Qty_in_Stock { get; set; }

        public string Final_Category { get; set; }

        public string Categories { get; set; }

        public string[] Tag { get; set; }
         
        public DateTime? Sales_End_Date { get; set; }

        public double? Unit_Price { get; set; }

        public double? Reorder_Level { get; set; }

    }

}
