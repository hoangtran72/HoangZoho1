using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HoangZoho1.Models.RestaurantEquipmentOnline.ZohoCRM
{

    [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
    public class TNZLogForCreation
    {

        public string Owner { get; set; }

        public string Message_ID { get; set; }

        public string Related_Lead { get; set; }

        public string Related_Contact { get; set; }

        public string Direction { get; set; }

        public string Detail { get; set; }

    }

}
