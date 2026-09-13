using HoangZoho1.Models.Common;
using HoangZoho1.Models.OneCorp.ZohoCRM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HoangZoho1.Services.OneCorp
{

    public interface IOneCorpCrmService
    {

        #region Contacts

        Task<ApiResultDto<SearchContactsByPhoneResponse>> SearchContactsByPhone(string phoneNumber);

        Task<ApiResultDto<GetContactByIdResponse>> GetContactById(string contactId);

        Task<ApiResultDto<UpsertResponse<UpsertDetail>>> UpdateContact(string contactId, UpsertRequest<ContactForUpdation> upsertRequest);

        #endregion

        #region Leads

        Task<ApiResultDto<SearchLeadsByPhoneResponse>> SearchLeadsByPhone(string phoneNumber);

        Task<ApiResultDto<GetLeadByIdResponse>> GetLeadById(string leadId);

        Task<ApiResultDto<GetLeadStatusHistoriesResponse>> GetRelatedStatusHistories(string leadId);

        Task<ApiResultDto<UpsertResponse<UpsertDetail>>> UpdateLead(string leadId, UpsertRequest<LeadForUpdation> upsertRequest);

        #endregion

        #region Deals

        Task<ApiResultDto<QueryDealsResponse>> QueryDeals(ZohoCoqlRequest coqlRequest);

        #endregion

        #region ScheduleOnce Booking

        Task<ApiResultDto<SearchBookingResponse>> SearchScheduleOnceBookings(string criteria);

        Task<ApiResultDto<QueryBookingsResponse>> QueryBookings(ZohoCoqlRequest coqlRequest);

        Task<ApiResultDto<UpsertResponse<UpsertDetail>>> CreateBooking
            (UpsertRequest<BookingForCreation> createBookingRequest);

        Task<ApiResultDto<UpsertResponse<UpsertDetail>>> UpdateBooking
            (string bookingId, UpsertRequest<BookingForUpdation> updateBookingRequest);

        #endregion

        #region Calls

        Task<ApiResultDto<GetCallByIdResponse>> GetCallById(string callId);

        #endregion

        #region Tasks

        Task<ApiResultDto<GetTaskByIdResponse>> GetTaskById(string taskId);

        Task<ApiResultDto<UpsertResponse<UpsertDetail>>> UpdateTask(string taskId, UpsertRequest<TaskForUpdation> upsertRequest);

        #endregion

        #region Sakari SMS Logs

        Task<ApiResultDto<UpsertResponse<UpsertDetail>>> CreateSakariSMSLog(UpsertRequest<SMSLogForUpsert> createSmsLogRequest);

        Task<ApiResultDto<UpsertResponse<UpsertDetail>>> UpdateSakariSMSLog(string logId, UpsertRequest<SMSLogForUpsert> createSmsLogRequest);

        Task<ApiResultDto<QuerySakariSMSLogsResponse>> QuerySakariSMSLogs(ZohoCoqlRequest coqlRequest);

        #endregion

        #region Notes

        Task<ApiResultDto<UpsertResponse<UpsertDetail>>> CreateNote(UpsertRequest<NoteForCreation> upsertRequest);

        #endregion

        #region Sakari Users

        Task<ApiResultDto<GetSakariUserByIdResponse>> GetSakariUserById(string sakariUserId);

        Task<ApiResultDto<QuerySakariUsersResponse>> QuerySakariUsers(ZohoCoqlRequest coqlRequest);

        Task<ApiResultDto<UpsertResponse<UpsertDetail>>> CreateSakariUser(UpsertRequest<SakariUserForUpsert> createUserRequest);

        Task<ApiResultDto<UpsertResponse<UpsertDetail>>> UpdateSakariUser(string sakariUserId, UpsertRequest<SakariUserForUpsert> updateUserRequest);

        #endregion

        #region Zoho Projects

        Task<ApiResultDto<QueryZPCommentsResponse>> QueryZPComments(ZohoCoqlRequest coqlRequest);

        Task<ApiResultDto<QueryZPTasksResponse>> QueryZPTasks(ZohoCoqlRequest coqlRequest);

        Task<ApiResultDto<GetZPCommentByIdResponse>> GetZPCommentById(string commentId);

        #endregion

        #region Users

        Task<ApiResultDto<GetUserByIdResponse>> GetUserById(string userId);

        Task<ApiResultDto<QueryUsersResponse>> QueryUsers(ZohoCoqlRequest coqlRequest);

        #endregion

        #region Blueprint

        Task<ApiResultDto<UpdateBlueprintResponse>> UpdateBlueprint
            (string recordModule, string recordId, UpdateBlueprintRequest blueprintRequest);

        #endregion

    }

}
