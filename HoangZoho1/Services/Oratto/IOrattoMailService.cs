using HoangZoho1.Models.Common;
using HoangZoho1.Models.Oratto.ZohoMail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HoangZoho1.Services.Oratto
{

    public interface IOrattoMailService
    {

        public Task<ApiResultDto<string>> DownloadEmailAttachment(
            DownloadAttachmentRequest attachmentRequest);

    }

}
