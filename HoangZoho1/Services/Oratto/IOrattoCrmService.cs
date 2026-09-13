using HoangZoho1.Models.Common;
using HoangZoho1.Models.Oratto.ZohoCRM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HoangZoho1.Services.Oratto
{

    public interface IOrattoCrmService
    {

        #region Timeline

        Task<ApiResultDto<GetModuleTimelineResponse>> GetModuleTimeline(string moduleName,
            string moduleId);

        #endregion

        #region Solicitors

        Task<ApiResultDto<QuerySolicitorsResponse>> QuerySolicitors(ZohoCoqlRequest coqlRequest);

        Task<ApiResultDto<GetSolicitorByIdResponse>> GetSolicitorById(string solicitorId);

        #endregion

        #region Leads

        Task<ApiResultDto<GetLeadRelatedSolicitorsResponse>> GetLeadRelatedSolicitors(string leadId);

        Task<ApiResultDto<UpsertResponse<UpsertDetail>>> CreateLeadSolicitorLinking(UpsertRequest<LeadSolicitorLinkingForCreation> linkingRequest);

        Task<ApiResultDto<ZohoDeleteResponse>> DeleteLeadSolicitorLinking(string linkingId);

        #endregion

        #region Matters

        Task<ApiResultDto<GetMatterByIdResponse>> GetMatterById(string matterId);

        Task<ApiResultDto<UpsertResponse<UpsertDetail>>> UpdateMatter(string matterId, 
            UpsertRequest<MatterForUpdation> updateMatterRequest);

        #endregion

        #region Queued Attachments

        Task<ApiResultDto<UpsertResponse<UpsertDetail>>> UpdateQueuedAttachments
            (string attachmentId, UpsertRequest<AttachmentForUpdation> upsertRequest);

        Task<ApiResultDto<QueryAttachmentsResponse>> QueryAttachments(
            ZohoCoqlRequest coqlRequest);

        #endregion

        #region Queued Emails

        Task<ApiResultDto<UpsertResponse<UpsertDetail>>> UpdatedQueuedEmails
            (string emailId, UpsertRequest<EmailForUpdation> upsertRequest);

        #endregion

    }

}
