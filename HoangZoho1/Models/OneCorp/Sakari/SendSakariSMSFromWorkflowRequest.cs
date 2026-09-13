using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HoangZoho1.Models.OneCorp.Sakari
{

    public class SendSakariSMSFromWorkflowRequest
    {

        public string OwnerId { get; set; }

        public string ContactPhone { get; set; }

        public string MessageContent { get; set; }

    }

}
