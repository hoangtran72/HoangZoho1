using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HoangZoho1.Models.OneCorp.ZohoCRM
{

    [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
    public class SakariUserForUpsert
    {

        public SakariUserForUpsert()
        {
            Sakari_Groups = new List<SakariGroupForUpsert>();
        }

        public string Name { get; set; }

        public string Email { get; set; }

        public List<SakariGroupForUpsert> Sakari_Groups { get; set; }

    }

    [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
    public class SakariGroupForUpsert
    {

        public string id { get; set; }

        public string Group_Id { get; set; }

        public string Group_Name { get; set; }

        public string Phone_Number { get; set; }

    }

}
