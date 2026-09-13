using HoangZoho1.Constants;
using HoangZoho1.Models.Common;
using HoangZoho1.Models.GGInsurance;
using HoangZoho1.Services.GetUnik;
using HoangZoho1.Services.GGInsurance;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace HoangZoho1.Controllers.GGInsurance
{

    [Route("api/gg-insurance")]
    [ApiController]
    public class GGInsuranceController : ControllerBase
    {

        private readonly IRingCentralService _ringCentralService;
        private readonly IGGInsuranceCustomService _ggCustomService;
        private readonly ILogger<GGInsuranceController> _logger;

        public GGInsuranceController(IGGInsuranceCustomService ggCustomService, 
            IRingCentralService ringCentralService)
        {
            _ggCustomService = ggCustomService;
            _ringCentralService = ringCentralService;
        }

        [HttpGet("redirect")]
        public async Task<IActionResult> ZohoCrmRedirectUrl4Phone([FromQuery] string phone)
        {

            var apiResult = new ApiResultDto<string>()
            {
                Code = ResultCode.BadRequest,
                Message = GetunikConstants.HCIAS_400
            };

            try
            {

                apiResult = await _ggCustomService.GetZohoCrmRedirectUrl4Phone(phone);
                switch (apiResult.Code)
                {
                    case ResultCode.OK:
                        string finalUrl = apiResult.Data;
                        return Redirect(finalUrl);
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

        
        [HttpPost("send-mms")]
        public async Task<IActionResult> HandleMms([FromBody] RingCentralMmsRequest body)
        {

            // 3. Call the service
            var result = await _ringCentralService.SendMmsAsync(body);

            return Ok(result);
        }

    }

}
