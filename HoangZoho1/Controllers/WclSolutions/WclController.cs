using HoangZoho1.Models.Common;
using HoangZoho1.Models.WclSolutions;
using HoangZoho1.Models.ZoRaw.ZohoInventory;
using HoangZoho1.Services.WclSolutions;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HoangZoho1.Controllers.WclSolutions
{

    [Route("wcl")]
    [ApiController]
    public class WclController : Controller
    {

        private readonly IWclCustomService _wclSolutionService;
        private readonly IWclInventoryService _wclInventoryService;

        public WclController(IWclCustomService wclSolutionService, IWclInventoryService wclInventoryService)
        {
            _wclSolutionService = wclSolutionService;
            _wclInventoryService = wclInventoryService;
        }

        [HttpGet("master-items")]
        public async Task<IActionResult> GetMasterItems()
        {

            var apiResult = new GetMasterItemsResponse();

            try
            {
                apiResult = await _wclSolutionService.GetMasterItems();
                return Ok(apiResult);
            }
            catch (Exception)
            {
                return BadRequest(apiResult);
            }
        }

        [HttpPut("salesorder/{salesorderId}")]
        public async Task<IActionResult> GetMasterItems(string salesorderId, [FromBody] 
            UpdateBatchNumberRequest updateBatchNumberRequest)
        {

            var apiResult = new ApiResultDto<UpdateSalesOrderResponse>();

            try
            {
                apiResult = await _wclInventoryService.UpdateSalesOrder(salesorderId, updateBatchNumberRequest);

                switch (apiResult.Code)
                {
                    case ResultCode.OK:
                        return Ok(apiResult);
                    default:
                        return BadRequest(apiResult);
                }
                
            }
            catch (Exception)
            {
                return BadRequest(apiResult);
            }
        }

        [HttpPost("woo/sync-orders-from-widget")]
        public async Task<IActionResult> SyncWooOrders2Inventory(
            [FromBody] SyncWooOrdersFromWidgetRequest syncWooOrdersFromWidgetRequest)
        {

            var apiResult = new ApiResultDto<string>()
            {
                Code = ResultCode.OK,
                Message = "Sync WooCommerce Orders from Widget SUCCESSFULLY"
            };

            try
            {
                string syncResult = await _wclSolutionService.SyncWooOrder2ZohoResponse(syncWooOrdersFromWidgetRequest);

                apiResult.Data = syncResult;

                switch (apiResult.Code)
                {
                    case ResultCode.OK:
                        return Ok(apiResult);
                    default:
                        return BadRequest(apiResult);
                }

            }
            catch (Exception)
            {
                return BadRequest(apiResult);
            }

        }

    }

}
