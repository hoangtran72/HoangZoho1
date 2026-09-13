using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HoangZoho1.Models.OneCorp.Sakari
{

    public class SendSakariSMSRequest
    {

        public SendSakariSMSRequest()
        {

            contacts = new List<SakariContact>();

            type = "SMS";

        }

        public List<SakariContact> contacts { get; set; }

        public SakariPhoneNumberFilter phoneNumberFilter { get; set; }

        public string template { get; set; }

        public string type { get; set; }

    }

    public class SakariPhoneNumberFilter
    {

        public SakariGroup group { get; set; }

    }

    public class SakariContact
    {

        public string email { get; set; }

        public string firstName { get; set; }

        public string lastName { get; set; }

        public SakariMobile mobile { get; set; }
    
    }

    public class SakariMobile
    {

        public string number { get; set; }

        public string country { get; set; }
    
    }

    public class SakariGroup
    {

        public string id { get; set; }

    }

}
