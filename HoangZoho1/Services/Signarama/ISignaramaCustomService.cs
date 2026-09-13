using HoangZoho1.Models.Common;
using HoangZoho1.Models.Signarama.Custom;
using HoangZoho1.Models.Signarama.ZohoBooks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HoangZoho1.Services.Signarama
{

    public interface ISignaramaCustomService
    {

        Task<ApiResultDto<HandleEstimateResponse>> HandleUploadedEstimate
            (HandleEstimateRequest estimateRequest);

        Task<ApiResultDto<UploadAttachmentResponse>> UploadAttachment
            (UploadAttachmentRequest attachmentRequest);

    }

}
