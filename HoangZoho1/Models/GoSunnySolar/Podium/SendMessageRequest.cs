using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HoangZoho1.Models.GoSunnySolar.Podium
{

    public class SendMessageRequest
    {

        public string body { get; set; }

        public Channel channel { get; set; }

        public string contactName { get; set; }

        public string locationUid { get; set; }

        public string senderName { get; set; }

    }

    public class Channel
    {

        public string identifier { get; set; }

        public string type { get; set; }

    }

}
