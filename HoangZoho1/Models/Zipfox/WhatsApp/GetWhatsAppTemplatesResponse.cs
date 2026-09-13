using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HoangZoho1.Models.Zipfox.WhatsApp
{

    public class GetWhatsAppTemplatesResponse
    {

        public WhatsAppTemplateData[] data { get; set; }

        public WhatsAppPaging paging { get; set; }

    }

    public class WhatsAppPaging
    {
        public WhatsAppCursors cursors { get; set; }
    }

    public class WhatsAppCursors
    {

        public string before { get; set; }

        public string after { get; set; }

    }

    public class WhatsAppTemplateData
    {

        public string name { get; set; }

        public WhatsAppComponent[] components { get; set; }

        public string language { get; set; }

        public string status { get; set; }

        public string category { get; set; }

        public string id { get; set; }

    }

    public class WhatsAppComponent
    {

        public string type { get; set; }

        public string format { get; set; }

        public string text { get; set; }

    }

}
