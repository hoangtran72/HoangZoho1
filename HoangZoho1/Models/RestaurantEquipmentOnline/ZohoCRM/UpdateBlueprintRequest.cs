using System.Collections.Generic;

namespace HoangZoho1.Models.RestaurantEquipmentOnline.ZohoCRM
{

    public class UpdateBlueprintRequest<T>
    {

        public UpdateBlueprintRequest()
        {
            
            blueprint = new List<Blueprint<T>>();

        }

        public List<Blueprint<T>> blueprint { get; set; }
    
    }

    public class Blueprint<T>
    {

        public string transition_id { get; set; }
        
        public T data { get; set; }
    
    }

}
