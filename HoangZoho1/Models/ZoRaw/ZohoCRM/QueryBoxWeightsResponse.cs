namespace HoangZoho1.Models.ZoRaw.ZohoCRM
{


    public class QueryBoxWeightsResponse
    {
        public BoxWeightData[] data { get; set; }
        public Info info { get; set; }
    }

    public class Info
    {
        public int count { get; set; }
        public bool more_records { get; set; }
    }

    public class BoxWeightData
    {

        public decimal? Width_in { get; set; }
        
        public decimal? Length_in { get; set; }
        
        public decimal? Height_in { get; set; }
        
        public decimal? Weight_kg { get; set; }
        
        public string id { get; set; }
        
        public string Name { get; set; }
    
    }

}
