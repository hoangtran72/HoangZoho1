using HoangZoho1.Models.Common;
using HoangZoho1.Models.Signarama.ZohoBooks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HoangZoho1.Services.Signarama
{

    public interface ISignaramaBooksService
    {

        Task<ApiResultDto<UploadAttachmentResponse>> UploadAttachment
            (string filePath, string moduleName, string moduleId);

    }

}
