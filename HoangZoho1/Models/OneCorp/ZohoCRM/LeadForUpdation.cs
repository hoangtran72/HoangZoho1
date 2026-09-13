using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HoangZoho1.Models.OneCorp.ZohoCRM
{

    [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
    public class LeadForUpdation
    {

        public string LDS_Id { get; set; }

        public string Sakari_Id { get; set; }

    }

}
