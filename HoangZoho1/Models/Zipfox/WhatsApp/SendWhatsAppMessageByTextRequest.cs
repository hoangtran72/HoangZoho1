using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HoangZoho1.Models.Zipfox.WhatsApp
{

    public class SendWhatsAppMessageByTextRequest
    {

        public SendWhatsAppMessageByTextRequest()
        {

            messaging_product = "whatsapp";
            recipient_type = "individual";
            type = "text";

        }

        public string messaging_product { get; set; }
        
        public string recipient_type { get; set; }

        public string to { get; set; }
        
        public string type { get; set; }
        
        public WhatsAppText text { get; set; }

    }

    public class WhatsAppText
    {

        public string body { get; set; }
    
    }

}
