namespace HoangZoho1.Models.ZoRaw.Custom
{

    public class BookShipmentFromWidgetRequest
    {

        public string ServiceId { get; set; }

        public string SalesOrderId { get; set; }

        public string PackageId { get; set; }

        public string DeliveryOrderId { get; set; }

        public float? Length { get; set; }
        
        public float? Width { get; set; }
        
        public float? Height { get; set; }
        
        public float? Weight { get; set; }
       
        public string Comment { get; set; }

        public string GetRatesDetails { get; set; }

    }


}
