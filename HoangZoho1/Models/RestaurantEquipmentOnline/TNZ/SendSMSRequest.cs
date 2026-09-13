using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HoangZoho1.Models.RestaurantEquipmentOnline.TNZ
{

    public class SendSMSRequest
    {

        public MessageData MessageData { get; set; }

    }

    public class MessageData
    {

        public string Message { get; set; }
        public Destination[] Destinations { get; set; }
    
    }

    public class Destination
    {

        public string Recipient { get; set; }

    }

}
