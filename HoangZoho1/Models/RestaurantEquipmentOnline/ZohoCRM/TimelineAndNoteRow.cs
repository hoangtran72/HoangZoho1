using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HoangZoho1.Models.RestaurantEquipmentOnline.ZohoCRM
{

    public class TimelineAndNoteRow
    {

        public int Index { get; set; }

        public string Type { get; set; }

        public string Subject { get; set; }

        public string Content { get; set; }

        public string CreatedBy { get; set; }

        public DateTime? CreatedTime { get; set; }

    }

}
