namespace HoangZoho1.Models.ZoRaw.Freightcom
{

    public class CalculateFreightClassRequest
    {

        public FreightcomBox box { get; set; }
    
    }

    public class FreightcomBox
    {

        public FreightcomWeight weight { get; set; }

        public FreightcomCuboid cuboid { get; set; }
    
    }

}
