namespace HoangZoho1.Models.RestaurantEquipmentOnline.Custom
{

    public class EasyUpdateQuoteRequest
    {

        public string QuoteId { get; set; }
        
        public EasyProduct[] Products { get; set; }
    
    }

    public class EasyProduct
    {
     
        public string ProductId { get; set; }
        
        public string LineItemId { get; set; }
    
    }

}
