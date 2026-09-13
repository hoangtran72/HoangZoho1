using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HoangZoho1.Models.RestaurantEquipmentOnline.ZohoCRM
{
    [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
    public class LeadForCreation
    {

        public LeadForCreation()
        {
            trigger = new List<string>();
        }

        public string First_Name { get; set; }

        public string Last_Name { get; set; }

        public string Email { get; set; }

        public string Lead_Source { get; set; }

        public string Lead_Status { get; set; }

        public List<string> trigger { get; set; }

        public string Gmail_Msg_Id { get; set; }

        public string Gumtree_Product_URL { get; set; }

        public string Gumtree_Reply_URL { get; set; }

        public string Owner { get; set; }

    }
}
