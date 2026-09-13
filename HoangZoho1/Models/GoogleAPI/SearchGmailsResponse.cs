using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HoangZoho1.Models.GoogleAPI
{

    public class SearchGmailsResponse
    {

        public Message[] messages { get; set; }

        public string nextPageToken { get; set; }

        public int resultSizeEstimate { get; set; }

    }

    public class Message
    {

        public string id { get; set; }

        public string threadId { get; set; }

    }

}
