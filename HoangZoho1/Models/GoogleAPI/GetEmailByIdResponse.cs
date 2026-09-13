using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HoangZoho1.Models.GoogleAPI
{

    public class GetEmailByIdResponse
    {

        public string id { get; set; }

        public string threadId { get; set; }

        public List<string> labelIds { get; set; }

        public string snippet { get; set; }

        public MessagePart payload { get; set; }

        public int? sizeEstimate { get; set; }

        public string historyId { get; set; }

        public string internalDate { get; set; }

    }

    public class MessagePart
    {

        public string partId { get; set; }

        public string mimeType { get; set; }

        public string filename { get; set; }

        public List<MessagePartHeader> headers { get; set; }

        public MessagePartBody body { get; set; }

        public List<MessagePart> parts { get; set; }

    }

    public class Body
    {
        public int size { get; set; }
    }

    public class Header
    {
        public string name { get; set; }
        public string value { get; set; }
    }

    public class MessagePartHeader
    {
        public string name { get; set; }
        public string value { get; set; }
    }

    public class MessagePartBody
    {
        public int? size { get; set; }
        
        public string attachmentId { get; set; }

        public string data { get; set; }

    }

}
