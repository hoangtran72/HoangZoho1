using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HoangZoho1.Models.OneCorp.ZohoWorkdrive
{

    public class UploadFileRequest
    {

        public string FileName { get; set; }

        public string ParentId { get; set; }

        public bool OverrideNameExist { get; set; }

        public byte[] Content { get; set; }

    }

}
