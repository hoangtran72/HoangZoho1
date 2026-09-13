using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HoangZoho1.Models.Signarama.Custom
{

    public class UploadAttachmentRequest
    {

        public string ResourceId { get; set; }

        public string FileName { get; set; }

        public string ModuleName { get; set; }

        public string ModuleId { get; set; }

    }

}
