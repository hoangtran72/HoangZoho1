using HoangZoho1.Models.Common;
using HoangZoho1.Models.GetUnik.ZohoBooks;
using HoangZoho1.Models.ZoRaw.ZohoInventory;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HoangZoho1.Services.ZoRaw
{

    public interface IZoRawInventoryService
    {

        Task<ApiResultDto<GetSalesOrderByIdResponse>> GetSalesOrderById(string salesOrderId);

        Task<ApiResultDto<ListSalesOrdersResponse>> GetSalesOrderByNumber(string salesOrderNumber);

        Task<ApiResultDto<GetPackageByIdResponse>> GetPackageById(string packageId);

        Task<ApiResultDto<CreatePackageResponse>> CreatePackage(string salesOrderId,
            CreatePackageRequest createPackageRequest);

        Task<ApiResultDto<GetItemByIdResponse>> GetItemById(string itemId);

        Task<ApiResultDto<SearchBatchesResponse>> SearchBatches(SearchBatchesRequest searchBatchesRequest);

        Task<ApiResultDto<ListAllLocationsResponse>> ListAllLocations();

        Task<ApiResultDto<GetLocationByIdResponse>> GetLocationById(string locationId);

        Task<ApiResultDto<CreateShipmentResponse>> CreateShipment
            (string salesOrderId, string packageId, CreateShipmentRequest createShipmentRequest);

        Task<ApiResultDto<UploadFileToRecordResponse>> UploadFileToRecord
            (string filePath, string recordModule, string recordId);

        Task<ApiResultDto<GetRecordAttachmentsResponse>> GetRecordAttachments
            (string recordModule, string recordId);

        Task<ApiResultDto<SearchDeliveryOrdersResponse>> SearchOrderFulfillments
            (string salesOrderId);

        Task<ApiResultDto<UpdateOrderFulfillmentResponse>> UpdateOrderFulfillment
            (string fulfillmentId, UpdateOrderFulfillmentRequest updateDeliveryOrderRequest);

    }

}
