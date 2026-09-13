using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HoangZoho1.Models.RestaurantEquipmentOnline.TNZ
{

    public class SendTnzSmsRequest
    {

        public string Token { get; set; }

        public string UserId { get; set; }

        public string UserName { get; set; }

        public string UserPhone { get; set; }

        public string FullName { get; set; }

        public string RelatedLead { get; set; }

        public string RelatedContact { get; set; }

        public SendSMSRequest SmsRequest { get; set; }

    }

}
