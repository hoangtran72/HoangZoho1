using HoangZoho1.Constants;
using HoangZoho1.Helpers;
using HoangZoho1.Models.Common;
using HoangZoho1.Models.RestaurantEquipmentOnline.JustCall;
using HoangZoho1.Models.RunPhp;
using HoangZoho1.Services.GoogleAPI;
using HoangZoho1.Services.RestaurantEquipmentOnline;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HoangZoho1.Controllers.RestaurantOnline
{
    [Route("api/runphp")]
    [ApiController]
    public class RunPHPController : ControllerBase
    {

        [HttpPost("convert-military-time")]
        public IActionResult ConvertMilitaryTime([FromBody] ConvertTimeRequest convertTimeRequest)
        {
            var apiResult = new ApiResultDto<string>()
            {
                Code = ResultCode.BadRequest,
                Message = REOConstants.CUSTOM_HTW_200
            };
            try
            {

                string inputStr = convertTimeRequest.TimeStr;

                var outputStr = DateTime.Parse(inputStr).ToString("HH:mm");

                apiResult.Code = ResultCode.OK;
                apiResult.Data = outputStr;

                return Ok(apiResult);

            }
            catch (Exception ex)
            {

                apiResult.Code = ResultCode.BadRequest;
                apiResult.Message = REOConstants.CUSTOM_HTW_400;
                apiResult.Data = $"{ex.Message} - {ex.StackTrace}";
                return BadRequest(apiResult);

            }
        }

    }
}
