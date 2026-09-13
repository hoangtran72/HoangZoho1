using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HoangZoho1.Models.OneCorp.Custom
{

    public class SendSakariSMSToLeadRequest
    {

        public string SmsContent { get; set; }

        public string LeadId { get; set; }

        public string LeadFirstName { get; set; }

        public string LeadLastName { get; set; }

        public string LeadFullName { get; set; }

        public string LeadEmail { get; set; }

        public string LeadPhone { get; set; }

        public string UserId { get; set; }

        public string UserFullName { get; set; }

        public string UserEmail { get; set; }

        public string GroupId { get; set; }

    }

}
