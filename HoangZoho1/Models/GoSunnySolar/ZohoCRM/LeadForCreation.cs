using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HoangZoho1.Models.GoSunnySolar.ZohoCRM
{
    public class LeadForCreation
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string First_Name { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Last_Name { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Lead_Source { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string GHL_Link { get; set; }
    }
}
