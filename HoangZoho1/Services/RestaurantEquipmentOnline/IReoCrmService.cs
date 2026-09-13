using HoangZoho1.Models.Common;
using HoangZoho1.Models.RestaurantEquipmentOnline.ZohoCRM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HoangZoho1.Services.RestaurantEquipmentOnline
{
    public interface IReoCrmService
    {

        #region Leads

        Task<ApiResultDto<SearchLeadsResponse>> SearchLeadsByEmail(string email);

        Task<ApiResultDto<QueryResponse<LeadQueryModel>>> QueryLeads(string query);

        Task<ApiResultDto<UpsertResponse<UpsertDetail>>> CreateLead(UpsertRequest<LeadForCreation> leadRequest);

        Task<ApiResultDto<UpsertResponse<UpsertDetail>>> UpdateLead(string leadId, UpsertRequest<LeadForUpdation> leadRequest);

        #endregion

        #region Contacts

        Task<ApiResultDto<GetContactByIdResponse>> GetContactById(string contactId);

        Task<ApiResultDto<SearchContactsResponse>> SearchContactsByEmail(string email);

        #endregion

        #region Products

        Task<ApiResultDto<GetProductByIdResponse>> GetProductById(string productId);

        Task<ApiResultDto<QueryResponse<ProductQueryModel>>> QueryProducts(string query);

        #endregion

        #region Quotes

        Task<ApiResultDto<QueryResponse<QuoteQueryModel>>> QueryQuotes(string query);

        Task<ApiResultDto<GetQuoteByIdResponse>> GetQuoteById(string quoteId);

        Task<ApiResultDto<UpsertResponse<UpsertDetail>>> CreateQuote
            (UpsertRequest<QuoteForCreation> quoteRequest);

        Task<ApiResultDto<UpsertResponse<UpsertDetail>>> UpdateQuote
            (string quoteId, UpsertRequest<UpdateQuoteRequest> quoteRequest);

        #endregion

        #region Deals

        Task<ApiResultDto<GetDealByIdResponse>> GetDealById(string dealId);

        #endregion

        #region Users

        Task<ApiResultDto<GetUsersResponse>> GetUserByTypes(string userType);

        Task<ApiResultDto<GetUsersResponse>> SearchUserByEmail(string email);

        #endregion

        #region JustCall Logs

        Task<ApiResultDto<SearchJustCallLogsResponse>> SearchJustCallLogs(string criteria);

        Task<ApiResultDto<UpsertResponse<UpsertDetail>>> CreateJustCallLog(UpsertRequest<JustCallLogForCreation> justcallRequest);

        #endregion

        #region TNZ Logs

        Task<ApiResultDto<UpsertResponse<UpsertDetail>>> CreateTNZLog(UpsertRequest<TNZLogForCreation> tnzRequest);

        #endregion

        #region Notes

        Task<ApiResultDto<UpsertResponse<UpsertDetail>>> CreateNote(UpsertRequest<NoteForCreation> noteRequest);

        Task<ApiResultDto<GetNotesResponse>> GetNotes(string recordModule, string recordId);

        #endregion

        #region Timeline

        Task<ApiResultDto<GetTimelineResponse>> GetTimeline(string recordModule, string recordId);

        #endregion

        #region Blueprint

        Task<ApiResultDto<UpdateBlueprintResponse>> 
            UpdateLeadBlueprint(string recordModule, string recordId, UpdateBlueprintRequest<LeadConnectedData> request);

        #endregion

    }
}
