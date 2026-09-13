using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HoangZoho1.Models.GoSunnySolar.ZohoCRM
{
    public class LeadForUpdation
    {

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string GHL_Link { get; set; }

    }
}
