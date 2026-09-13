using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HoangZoho1.Models.GoogleAPI
{
    public class GmailContent
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string MsgId { get; set; }

        public string From { get; set; }

        public string Subject { get; set; }

        public string Body { get; set; }

        public DateTime? MailDateTime { get; set; }

    }
}
