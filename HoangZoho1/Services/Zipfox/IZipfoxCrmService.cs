using HoangZoho1.Models.Common;
using HoangZoho1.Models.Zipfox.ZohoCRM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HoangZoho1.Services.Zipfox
{

    public interface IZipfoxCrmService
    {

        #region Accounts

        Task<ApiResultDto<GetAccountRelatedContactsResponse>> GetAccountRelatedContacts(string accountId);

        #endregion

        #region Contacts

        Task<ApiResultDto<SearchContactsByPhoneResponse>> SearchContactsByPhone(string phone);

        Task<ApiResultDto<QueryContactsResponse>> QueryContacts(ZohoCoqlRequest coqlRequest);

        Task<ApiResultDto<UpsertResponse<UpsertDetail>>> UpdateContact(string contactId, UpsertRequest<ContactForUpdation> upsertRequest);

        Task<ApiResultDto<UpsertResponse<UpsertDetail>>> CreateContact(UpsertRequest<ContactForCreation> upsertRequest);

        #endregion

        #region WhatsApp Logs

        Task<ApiResultDto<QueryWhatsAppLogsResponse>> QueryWhatsAppLogs(ZohoCoqlRequest coqlRequest);

        Task<ApiResultDto<UpsertResponse<UpsertDetail>>> CreateWhatsAppLog(UpsertRequest<WhatsAppLogForCreation> upsertRequest);

        Task<ApiResultDto<UpsertResponse<UpsertDetail>>> UpdateWhatsAppLog(string logId, UpsertRequest<WhatsAppLogForUpdation> upsertRequest);

        #endregion

        #region RFQ

        Task<ApiResultDto<UpsertResponse<UpsertDetail>>> UpdateRFQ(string rfqId, UpsertRequest<RFQForUpdation> upsertRequest);

        Task<ApiResultDto<QueryRFQsResponse>> QueryRFQs(ZohoCoqlRequest coqlRequest);

        #endregion

    }

}
