using HoangZoho1.Models.Common;
using HoangZoho1.Models.Oratto.Custom;
using HoangZoho1.Models.Oratto.OpenAI;
using HoangZoho1.Models.Oratto.Gemini;
using HoangZoho1.Models.Oratto.ZohoMail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HoangZoho1.Services.Oratto
{

    public interface IOrattoCustomService
    {

        Task<ApiResultDto<string>> SendEmailForReassign(string matterId);

        Task<ApiResultDto<string>> SendEmailForLastUpdated(string matterId);

        Task<ApiResultDto<ZohoApiResponse>> SendEmailFromContact(SendEmailFromContact sendEmail);

        Task<ApiResultDto<QuerySolicitorsDto>> QuerySolicitors(string leadId);

        Task<ApiResultDto<string>> AssignSolicitorsToLead(string leadId, string solicitorIds);

        Task<ApiResultDto<Models.Oratto.OpenAI.UploadFileResponse>> 
            UploadMailAttachmentsToOpenAi(DownloadAttachmentRequest attachmentRequest);

        Task<ApiResultDto<string>> GenerateAttachmentContentByGemini(DownloadAttachmentRequest attachmentRequest);
        
        Task<ApiResultDto<ChatCompletionResponse>> ChatCompletionsForQueuedEmail(string emailId, ChatCompletionRequest completionRequest);

    }

}
