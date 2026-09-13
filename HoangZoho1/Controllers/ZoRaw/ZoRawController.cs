using HoangZoho1.Constants;
using HoangZoho1.Models.Common;
using HoangZoho1.Models.ZoRaw.Custom;
using HoangZoho1.Models.ZoRaw.SalesData;
using HoangZoho1.Models.ZoRaw.Stallion;
using HoangZoho1.Models.ZoRaw.ZohoCRM;
using HoangZoho1.Models.ZoRaw.ZohoInventory;
using HoangZoho1.Services.ZoRaw;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HoangZoho1.Controllers.ZoRaw
{

    [Route("api/zoraw")]
    [ApiController]
    public class ZoRawController : ControllerBase
    {

        private readonly IZoRawStallionService _stallionService;
        private readonly IZoRawInventoryService _zohoInventoryService;
        private readonly IZoRawCustomService _zoRawCustomService;
        private readonly IZoRawCrmService _zoRawCrmService;

        public ZoRawController(IZoRawStallionService stallionService,
            IZoRawInventoryService zohoInventoryService,
            IZoRawCustomService zoRawCustomService,
            IZoRawCrmService zoRawCrmService)
        {
            _stallionService = stallionService;
            _zohoInventoryService = zohoInventoryService;
            _zoRawCustomService = zoRawCustomService;
            _zoRawCrmService = zoRawCrmService;
        }

        #region Stallion

        [HttpPost("stallion/get-rates")]
        public async Task<IActionResult> GetRates
            ([FromBody] GetStallionRatesRequest getRatesRequest)
        {
            var apiResult = new ApiResultDto<GetStallionRatesResponse>()
            {
                Code = ResultCode.BadRequest,
                Message = ZoRawConstants.SEGR_400
            };
            try
            {
                apiResult = await _stallionService.GetRates(getRatesRequest);
                switch (apiResult.Code)
                {
                    case ResultCode.OK:
                        return Ok(apiResult);
                    default:
                        return BadRequest(apiResult);
                }
            }
            catch (Exception ex)
            {
                apiResult.Message = ex.Message + " - " + ex.StackTrace;
                return BadRequest(apiResult);
            }
        }

        #endregion

        #region Zoho CRM

        [HttpGet("crm/query/box-weights")]
        public async Task<IActionResult> QueryBoxWeights()
        {

            var apiResult = new ApiResultDto<QueryBoxWeightsResponse>()
            {
                Code = ResultCode.BadRequest,
                Message = ZoRawConstants.GLBI_400
            };
            try
            {

                var coqlRequest = new ZohoCoqlRequest()
                {
                    select_query = $"Select Name, Length_in, Width_in, Height_in, Weight_kg from Box_Weights " +
                    $"where Is_Active = true ORDER BY Created_Time ASC"
                };

                apiResult = await _zoRawCrmService.QueryBoxWeights(coqlRequest);
                switch (apiResult.Code)
                {
                    case ResultCode.OK:
                        return Ok(apiResult);
                    default:
                        return BadRequest(apiResult);
                }
            }
            catch (Exception ex)
            {
                apiResult.Message = ex.Message + " - " + ex.StackTrace;
                return BadRequest(apiResult);
            }

        }

        #endregion

        #region Zoho Inventory

        [HttpGet("inventory/items/{itemId}")]
        public async Task<IActionResult> GetItemById(string itemId)
        {

            var apiResult = new ApiResultDto<GetItemByIdResponse>()
            {
                Code = ResultCode.BadRequest,
                Message = ZoRawConstants.GLBI_400
            };
            try
            {
                apiResult = await _zohoInventoryService.GetItemById(itemId);
                switch (apiResult.Code)
                {
                    case ResultCode.OK:
                        return Ok(apiResult);
                    default:
                        return BadRequest(apiResult);
                }
            }
            catch (Exception ex)
            {
                apiResult.Message = ex.Message + " - " + ex.StackTrace;
                return BadRequest(apiResult);
            }

        }

        [HttpPost("inventory/items/search-batches")]
        public async Task<IActionResult> SearchBatches(
            [FromBody] SearchBatchesRequest searchBatchesRequest)
        {

            var apiResult = new ApiResultDto<SearchBatchesResponse>()
            {
                Code = ResultCode.BadRequest,
                Message = ZoRawConstants.SBR_400
            };

            try
            {
                apiResult = await _zohoInventoryService.SearchBatches(searchBatchesRequest);
                switch (apiResult.Code)
                {
                    case ResultCode.OK:
                        return Ok(apiResult);
                    default:
                        return BadRequest(apiResult);
                }
            }
            catch (Exception ex)
            {
                apiResult.Message = ex.Message + " - " + ex.StackTrace;
                return BadRequest(apiResult);
            }

        }

        [HttpGet("inventory/locations/{locationId}")]
        public async Task<IActionResult> GetLocationById(string locationId)
        {

            var apiResult = new ApiResultDto<GetLocationByIdResponse>()
            {
                Code = ResultCode.BadRequest,
                Message = ZoRawConstants.GLBI_400
            };
            try
            {
                apiResult = await _zohoInventoryService.GetLocationById(locationId);
                switch (apiResult.Code)
                {
                    case ResultCode.OK:
                        return Ok(apiResult);
                    default:
                        return BadRequest(apiResult);
                }
            }
            catch (Exception ex)
            {
                apiResult.Message = ex.Message + " - " + ex.StackTrace;
                return BadRequest(apiResult);
            }

        }

        [HttpGet("inventory/locations")]
        public async Task<IActionResult> ListAllLocations()
        {

            var apiResult = new ApiResultDto<ListAllLocationsResponse>()
            {
                Code = ResultCode.BadRequest,
                Message = ZoRawConstants.LAL_400
            };
            try
            {
                apiResult = await _zohoInventoryService.ListAllLocations();
                switch (apiResult.Code)
                {
                    case ResultCode.OK:
                        return Ok(apiResult);
                    default:
                        return BadRequest(apiResult);
                }
            }
            catch (Exception ex)
            {
                apiResult.Message = ex.Message + " - " + ex.StackTrace;
                return BadRequest(apiResult);
            }

        }

        [HttpGet("inventory/salesorders/{salesOrderId}")]
        public async Task<IActionResult> GetSalesOrderById(string salesOrderId)
        {

            var apiResult = new ApiResultDto<GetSalesOrderByIdResponse>()
            {
                Code = ResultCode.BadRequest,
                Message = ZoRawConstants.GSBI_400
            };
            try
            {
                apiResult = await _zohoInventoryService.GetSalesOrderById(salesOrderId);
                switch (apiResult.Code)
                {
                    case ResultCode.OK:
                        return Ok(apiResult);
                    default:
                        return BadRequest(apiResult);
                }
            }
            catch (Exception ex)
            {
                apiResult.Message = ex.Message + " - " + ex.StackTrace;
                return BadRequest(apiResult);
            }

        }

        [HttpPost("inventory/salesorders-number")]
        public async Task<IActionResult> GetSalesOrderByNumber(string salesOrderId)
        {

            var apiResult = new ApiResultDto<GetSalesOrderByIdResponse>()
            {
                Code = ResultCode.BadRequest,
                Message = ZoRawConstants.GSBI_400
            };
            try
            {
                apiResult = await _zohoInventoryService.GetSalesOrderById(salesOrderId);
                switch (apiResult.Code)
                {
                    case ResultCode.OK:
                        return Ok(apiResult);
                    default:
                        return BadRequest(apiResult);
                }
            }
            catch (Exception ex)
            {
                apiResult.Message = ex.Message + " - " + ex.StackTrace;
                return BadRequest(apiResult);
            }

        }

        [HttpPost("inventory/shipment/attachment")]
        public async Task<IActionResult> UploadFile2Shipment
            ([FromBody] UploadFileToShipmentRequest uploadFileToShipmentRequest)
        {

            var apiResult = new ApiResultDto<UploadFileToRecordResponse>()
            {
                Code = ResultCode.BadRequest,
                Message = ZoRawConstants.UF2S_400
            };

            try
            {
                
                string shipmentId = uploadFileToShipmentRequest.ShipmentId;
                string filePath = uploadFileToShipmentRequest.FilePath;

                apiResult = await _zohoInventoryService
                    .UploadFileToRecord(filePath, "shipmentorders", shipmentId);
                switch (apiResult.Code)
                {
                    case ResultCode.OK:
                        return Ok(apiResult);
                    default:
                        return BadRequest(apiResult);
                }

            }
            catch (Exception ex)
            {

                apiResult.Message = ex.Message + " - " + ex.StackTrace;
                return BadRequest(apiResult);
            
            }

        }

        #endregion

        #region Custom Function

        [HttpPost("custom/get-rates-from-widget")]
        public async Task<IActionResult> GetRatesFromWidget
            (GetRatesFromWidgetRequest request)
        {

            var apiResult = new ApiResultDto<List<CommonShipmentRate>>()
            {
                Code = ResultCode.BadRequest,
                Message = ZoRawConstants.LAL_400
            };
            try
            {
                apiResult = await _zoRawCustomService.GetRatesFromWidget(request);
                switch (apiResult.Code)
                {
                    case ResultCode.OK:
                        return Ok(apiResult);
                    default:
                        return BadRequest(apiResult);
                }
            }
            catch (Exception ex)
            {
                apiResult.Message = ex.Message + " - " + ex.StackTrace;
                return BadRequest(apiResult);
            }

        }

        [HttpPost("custom/book-shipment-from-widget")]
        public async Task<IActionResult> BookShipmentFromWidget
            (BookShipmentFromWidgetRequest bookShipmentRequest)
        {
            var apiResult = new ApiResultDto<string>()
            {
                Code = ResultCode.BadRequest,
                Message = ZoRawConstants.CSFW_400
            };
            try
            {
                apiResult = await _zoRawCustomService
                    .BookShipmentFromWidget(bookShipmentRequest);
                switch (apiResult.Code)
                {
                    case ResultCode.OK:
                        return Ok(apiResult);
                    default:
                        return BadRequest(apiResult);
                }
            }
            catch (Exception ex)
            {
                apiResult.Message = ex.Message + " - " + ex.StackTrace;
                return BadRequest(apiResult);
            }
        }

        [HttpPost("custom/create-package-from-widget")]
        public async Task<IActionResult> CreatePackageFromWidget
            (CreatePackageFromWidgetRequest createPackageFromWidgetRequest)
        {

            var apiResult = new ApiResultDto<string>()
            {
                Code = ResultCode.BadRequest,
                Message = ZoRawConstants.CSFW_400
            };
            try
            {
                apiResult = await _zoRawCustomService
                    .CreatePackageFromWidget(createPackageFromWidgetRequest);
                switch (apiResult.Code)
                {
                    case ResultCode.OK:
                        return Ok(apiResult);
                    default:
                        return BadRequest(apiResult);
                }
            }
            catch (Exception ex)
            {
                apiResult.Message = ex.Message + " - " + ex.StackTrace;
                return BadRequest(apiResult);
            }

        }

        [HttpPost("custom/handle-sales-excel")]
        public IActionResult HandleSalesExcel()
        {

            var apiResult = new ApiResultDto<string>()
            {
                Code = ResultCode.BadRequest,
                Message = ZoRawConstants.HEFP_400
            };
            try
            {

                var mapping = new ExcelMapping
                {
                    FixedColumns = new Dictionary<string, string>
                    {
                        { "CustomerProvince", "CUST_PROVINCE" },
                        { "CustomerNo", "CUSTOMER_NO" },
                        { "ShipToNo", "SHIP_TO_NO" },
                        { "ShipToName", "SHIP_TO_NAME" },
                        { "Address", "SHIP_TO_ADDRESS1" },
                        //{ "City", "SHIP_TO_CITY" },
                        { "Category", "CATEGORY_DESCRIPTION" },
                        { "ItemDescription", "ITEM_NO_DESCRIPTION" }
                    },
                    FixedHeaderRow = 2,
                    DynamicHeaderRow = 1,
                    StartRow = 3,
                    IgnoreColumnIndex = 4,
                    IgnoreValues = new List<string> { "Total", "Subtotal" }
                };

                string inputFile = @"E:\Upwork\ZoRaw\Account Management Work\Purity - Sales - 2025.10.xlsx";
                string outputFile = @"E:\Upwork\ZoRaw\Account Management Work\Purity - Sales - 2025.10 - TRANSFORMED.xlsx";

                // 1. Transform Excel
                var transformedRecords = _zoRawCustomService.ReadExcelToTransformedRecords
                    (inputFile, mapping, true, outputFile);

                //// 2. Convert to Zoho-ready format
                //var zohoFieldMapping = new Dictionary<string, string>
                //{
                //    { "CustomerProvince", "Customer_Province" },
                //    { "CustomerNo", "Customer_No" },
                //    { "ShipToNo", "Ship_To_No" },
                //    { "ShipToName", "Ship_To_Name" },
                //    { "Address", "Ship_To_Address" },
                //    { "Category", "Category_Description" },
                //    { "ItemDescription", "Item_Description" },
                //    { "MonthYear", "Month" },
                //    { "Qty", "Quantity" },
                //    { "Amount", "Amount" }
                //};

                switch (apiResult.Code)
                {
                    case ResultCode.OK:
                        apiResult.Code = ResultCode.OK;
                        apiResult.Message = ZoRawConstants.HEFP_200;
                        return Ok(apiResult);
                    default:
                        return BadRequest(apiResult);
                }
            }
            catch (Exception ex)
            {
                apiResult.Message = ex.Message + " - " + ex.StackTrace;
                return BadRequest(apiResult);
            }

        }

        #endregion

    }

}
