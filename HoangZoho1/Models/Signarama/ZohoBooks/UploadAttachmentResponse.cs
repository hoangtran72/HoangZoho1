using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HoangZoho1.Models.Signarama.ZohoBooks
{

    public class UploadAttachmentResponse
    {

        public UploadAttachmentResponse()
        {

            documents = new List<UploadDocument>();

        }

        public int? code { get; set; }

        public string message { get; set; }

        public List<UploadDocument> documents { get; set; }

    }

    public class UploadDocument
    {

        public string document_id { get; set; }

        public string file_name { get; set; }

        public string file_type { get; set; }

        public int? file_size { get; set; }

        public string file_size_formatted { get; set; }

    }

}
