using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HoangZoho1.Models.Oratto.ZohoCRM
{

    public class QueryAttachmentsResponse
    {

        public AttachmentData[] data { get; set; }

        public Info info { get; set; }

    }

    public class AttachmentData
    {

        public string Name { get; set; }

        public string id { get; set; }

        public string Status { get; set; }

    }

}
