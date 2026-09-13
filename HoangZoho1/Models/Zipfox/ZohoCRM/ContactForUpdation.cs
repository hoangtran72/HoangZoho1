using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HoangZoho1.Models.Zipfox.ZohoCRM
{

    [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
    public class ContactForUpdation
    {

        public string WhatsApp_OK { get; set; }

        public bool? Notification_sent { get; set; }

    }

}
