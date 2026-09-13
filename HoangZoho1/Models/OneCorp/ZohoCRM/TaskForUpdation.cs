using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HoangZoho1.Models.OneCorp.ZohoCRM
{

    public class TaskForUpdation
    {

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string LDS_Id { get; set; }

    }

}
