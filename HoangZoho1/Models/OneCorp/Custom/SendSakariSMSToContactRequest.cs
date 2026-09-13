using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HoangZoho1.Models.OneCorp.Custom
{

    public class SendSakariSMSToContactRequest
    {

        public string SmsContent { get; set; }

        public string DealId { get; set; }

        public string ContactId { get; set; }

        public string ContactFirstName { get; set; }

        public string ContactLastName { get; set; }

        public string ContactFullName { get; set; }

        public string ContactEmail { get; set; }

        public string ContactPhone { get; set; }

        public string UserId { get; set; }

        public string UserFullName { get; set; }

        public string UserEmail { get; set; }

        public string GroupId { get; set; }

    }

}
