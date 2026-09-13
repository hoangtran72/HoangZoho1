namespace HoangZoho1.Models.RestaurantEquipmentOnline.ZohoCRM
{

    public class UpdateBlueprintResponse
    {

        public UpdateBlueprint blueprint { get; set; }
    
    }

    public class UpdateBlueprint
    {

        public string code { get; set; }
        
        public UpdateBlueprintDetails details { get; set; }
        
        public string message { get; set; }
        
        public string status { get; set; }
    
    }

    public class UpdateBlueprintDetails
    {
    }

}
