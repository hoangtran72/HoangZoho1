using HoangZoho1.Models.Common;
using HoangZoho1.Models.ZoRaw.ZohoCRM;
using System.Threading.Tasks;

namespace HoangZoho1.Services.ZoRaw
{

    public interface IZoRawCrmService
    {

        Task<ApiResultDto<QueryBoxWeightsResponse>> QueryBoxWeights(ZohoCoqlRequest coqlRequest);

        Task<ApiResultDto<string>> SyncOrderFulfillmentFromSalesOrder
            (SyncOrderFulfillmentFromSalesOrderRequest request);

    }

}
