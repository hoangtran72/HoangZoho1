using HoangZoho1.Models.Common;
using HoangZoho1.Models.PinjarraBakery.ZohoCRM;
using System.Threading.Tasks;

namespace HoangZoho1.Services.PinjarraBakery
{
    public interface IPinjarraCrmService
    {

        #region R04 Form

        public Task<ApiResultDto<GetContactR04Response>> GetContactRelatedR04s(string contactId);

        #endregion

        #region BakeHouse Product Log

        public Task<ApiResultDto<GetProductLogByIdResponse>> GetProductLogById(string productLogId);

        public Task<ApiResultDto<SearchProductLogsResponse>> SearchBakeHouseProductLogs(string criteria);

        Task<ApiResultDto<UpsertResponse<UpsertDetail>>> CreateProductLog(UpsertRequest<ProductLogForCreation> productLogRequest);

        Task<ApiResultDto<UpsertResponse<UpsertDetail>>> UpdateProductLog(string bookingId, 
            UpsertRequest<ProductLogForUpdation> productLogRequest);

        #endregion
    }
}
