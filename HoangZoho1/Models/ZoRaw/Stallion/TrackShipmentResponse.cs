using System;

namespace HoangZoho1.Models.ZoRaw.Stallion
{


    public class TrackShipmentResponse
    {
        
        public bool success { get; set; }
        
        public TrackEvent[] events { get; set; }
        
        public string status { get; set; }
        
        public TrackDetails details { get; set; }
    
    }

    public class TrackDetails
    {
        
        public string ship_code { get; set; }
        
        public string destination { get; set; }
        
        public string service { get; set; }
        
        public string tracking { get; set; }
        
        public string url { get; set; }
        
        public string carrier_phone { get; set; }
    
    }

    public class TrackEvent
    {
        
        public string carrier { get; set; }
        
        public string location { get; set; }
        
        public string status { get; set; }
        
        public DateTime? datetime { get; set; }
        
        public string tracking { get; set; }
    
    }


}
