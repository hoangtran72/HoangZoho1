using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HoangZoho1.Models.Twilio
{
    public class SendSMSRequest
    {
        public string AccountSID { get; set; }

        public string AuthToken { get; set; }

        public string Body { get; set; }

        public string From { get; set; }

        public string To { get; set; }
    }
}
