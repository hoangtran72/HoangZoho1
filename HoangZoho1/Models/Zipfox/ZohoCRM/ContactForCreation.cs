using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HoangZoho1.Models.Zipfox.ZohoCRM
{

    [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
    public class ContactForCreation
    {

        public string First_Name { get; set; }

        public string Last_Name { get; set; }

        public string Email { get; set; }

        public string Lead_Source { get; set; }

        public string Product_Not_Found_Description { get; set; }

        public string Search_Value { get; set; }

        public string Estimated_Quantity { get; set; }

    }

}
