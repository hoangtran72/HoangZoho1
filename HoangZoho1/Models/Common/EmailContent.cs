using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HoangZoho1.Models.Common
{
    public class EmailContent
    {

        public string SenderName { get; set; }

        public string Clients { get; set; }

        public string Subject { get; set; }

        public string Body { get; set; }

        public string SmtpServer { get; set; }

        public int SmtpPort { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

    }
}
