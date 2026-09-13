using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HoangZoho1.Models.OneCorp.Sakari
{

    [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
    public class UpsertContactRequest
    {

        public string email { get; set; }

        public string firstName { get; set; }

        public string lastName { get; set; }

        public ContactMobile mobile { get; set; }

        public List<ContactTag> tags { get; set; }

    }

}
