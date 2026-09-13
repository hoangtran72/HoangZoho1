using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HoangZoho1.Models.RestaurantEquipmentOnline.ZohoCRM
{

    public class QueryResponse<T>
    {
        public List<T> data { get; set; }
        public Info info { get; set; }
    }

    public class Info
    {
        public int count { get; set; }
        public bool more_records { get; set; }
    }

}
