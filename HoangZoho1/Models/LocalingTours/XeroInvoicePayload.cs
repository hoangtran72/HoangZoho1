using System;

namespace HoangZoho1.Models.LocalingTours
{

    public class XeroInvoicePayload
    {

        public XeroEvent[] events { get; set; }
        
        public int? firstEventSequence { get; set; }
        
        public int? lastEventSequence { get; set; }
        
        public string entropy { get; set; }
    
    }

    public class XeroEvent
    {

        public string resourceUrl { get; set; }
        
        public string resourceId { get; set; }
        
        public string tenantId { get; set; }
        
        public string tenantType { get; set; }
        
        public string eventCategory { get; set; }
        
        public string eventType { get; set; }
        
        public DateTime? eventDateUtc { get; set; }
    
    }

}
