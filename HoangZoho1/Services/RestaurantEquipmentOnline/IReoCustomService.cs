using HoangZoho1.Models.Common;
using HoangZoho1.Models.RestaurantEquipmentOnline.Custom;
using HoangZoho1.Models.RestaurantEquipmentOnline.JustCall;
using HoangZoho1.Models.RestaurantEquipmentOnline.TNZ;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HoangZoho1.Services.RestaurantEquipmentOnline
{
    public interface IReoCustomService
    {

        Task<ApiResultDto<string>> HandleSalesEmails(string emailDate, string startTime, string endTime);

        Task<int> GetLeadNumberDaily();

        Task<string> GetRandomAccountManagerId();

        Task<ApiResultDto<string>> SyncCallToZohoCRM(CallPayload callPayload);

        Task<ApiResultDto<string>> SyncSMSToZohoCRM(SMSPayload smsPayload);

        Task<ApiResultDto<string>> MassSyncSMSToZohoCRM();

        Task<ApiResultDto<string>> MassSyncCallsToZohoCRM();

        Task<ApiResultDto<string>> SendTnzSMSAndCreateLog(SendTnzSmsRequest sendSMSRequest);

        Task<ApiResultDto<List<List<string>>>> GetTimelineAndNote(string recordModule, string recordId);

        Task<ApiResultDto<GetProductDataTableResponse>> GetProductDataTable(string productSkus);

        Task<ApiResultDto<EasyAddQuoteResponse>> EasyAddQuote_Deal(EasyAddQuoteRequest easyAddQuoteRequest);

        Task<ApiResultDto<GetQuoteTableResponse>> GetQuoteDataTable(string quoteId);

        Task<ApiResultDto<GetQuoteTableResponse>> GetSuggestedDataTable(GetSuggestedProductRequest request);

        Task<ApiResultDto<string>> EasyUpdateQuote(EasyUpdateQuoteRequest request);

        Task<ApiResultDto<string>> BlueprintConnectWithLead(string leadId, ConnectWithLeadRequest request);

    }
}
