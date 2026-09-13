using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HoangZoho1.Models.Oratto.ZohoMail
{

    public class DownloadAttachmentRequest
    {

        public string AccountId { get; set; }

        public string FolderId { get; set; }

        public string MessageId { get; set; }

        public string AttachmentId { get; set; }

        public string AttachmentName { get; set; }

    }

}
