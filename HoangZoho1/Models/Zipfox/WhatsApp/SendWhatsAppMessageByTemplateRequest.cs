using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HoangZoho1.Models.Zipfox.WhatsApp
{

    public class SendWhatsAppMessageByTemplateRequest
    {

        public SendWhatsAppMessageByTemplateRequest()
        {

            messaging_product = "whatsapp";

            recipient_type = "individual";

            type = "template";

        }

        public string messaging_product { get; set; }

        public string recipient_type { get; set; }

        public string to { get; set; }

        public string type { get; set; }
        
        public WhatsAppTemplate template { get; set; }

    }

    public class WhatsAppTemplate
    {

        public string name { get; set; }

        public WhatsAppLanguage language { get; set; }

    }

    public class WhatsAppLanguage
    {

        public string code { get; set; }

    }

}
