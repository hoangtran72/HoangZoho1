using HoangZoho1.Models.Common;
using HoangZoho1.Models.OneCorp.Custom;
using HoangZoho1.Models.OneCorp.Sakari;
using HoangZoho1.Models.OneCorp.ScheduleOnce;
using HoangZoho1.Models.OneCorp.ZohoCRM;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HoangZoho1.Services.OneCorp
{
    public interface IOneCorpCustomService
    {

        #region ScheduleOnce Booking

        Task<ApiResultDto<string>> ScheduleOnce_SyncBooking(BookingPayload bookingPayload);

        Task<ApiResultDto<string>> SyncBookingById(string bookingId);

        #endregion

        #region Lead Distribution System

        Task<ApiResultDto<string>> SyncCallToDBUponCreationUpdation(string callId);

        Task<ApiResultDto<string>> SyncTaskToDBUponCreationUpdation(string taskId);

        ApiResultDto<string> SyncTaskToDBUponDeletion(string taskId);

        Task<ApiResultDto<string>> SyncLeadToDBUponCreationUpdation(string leadId);

        ApiResultDto<string> SyncLeadToDBUponDeletion(string leadId);

        Task<ApiResultDto<string>> SyncLeadStatusHistoryToDB(string leadId);

        Task<ApiResultDto<string>> ImportTasksToDB();

        #endregion

        #region Zoho Workdrive

        Task<ApiResultDto<string>> UploadZohoSignDocument2Workdrive(string requestId);

        Task<ApiResultDto<string>> CreateSubFolders(string[] subFolders, string parentId);

        #endregion

        #region Sakari 

        Task<ApiResultDto<string>> SendSakariSMSToZohoContact(SendSakariSMSToContactRequest sendSMSRequest);

        Task<ApiResultDto<string>> SendSakariSMSToZohoLead(SendSakariSMSToLeadRequest sendSMSRequest);

        Task<ApiResultDto<string>> SendSakariSMSFromWorkflow(SendSakariSMSFromWorkflowRequest sendSMSRequest);

        Task<ApiResultDto<string>> SyncSakariPhoneGroup2Zoho();

        Task<ApiResultDto<GetSakariUserByIdResponse>> GetSakariUsersDetailsByEmail(string userEmail);

        Task<ApiResultDto<string>> SyncSakariMessagePayload(MessagePayload messagePayload);

        Task<ApiResultDto<string>> MassSyncSakariMessages();

        Task<ApiResultDto<string>> SyncLeadToSakari(string leadId);

        Task<ApiResultDto<string>> SyncContactToSakari(string contactId);


        #endregion

        #region Zoho Projects

        Task<ApiResultDto<List<List<string>>>> QueryZPComments(ZohoCoqlRequest coqlRequest);

        Task<ApiResultDto<List<List<string>>>> QueryZPTasks(ZohoCoqlRequest coqlRequest);

        #endregion

        #region Twilio

        Task<ApiResultDto<string>> SyncHistoryTwilioSMSLogs();

        Task<ApiResultDto<string>> SyncTwilioSMSLogsEvery2Hours();

        #endregion

    }
}
