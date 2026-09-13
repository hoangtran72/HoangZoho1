using HoangZoho1.Constants;
using HoangZoho1.Models.Common;
using HoangZoho1.Models.DaviesImagingGroup;
using HoangZoho1.Models.Getunik.Custom;
using HoangZoho1.Services.DaviesImagingGroup;
using HoangZoho1.Services.GetUnik;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace HoangZoho1.Controllers.DaviesImagingGroup
{

    [Route("api/dig")]
    [ApiController]
    public class DIGController : ControllerBase
    {

        private readonly IDigCustomService _digCustomService;

        public DIGController(IDigCustomService digCustomService)
        {
            _digCustomService = digCustomService;
        }

        [HttpPost("send-spec-plus-email")]
        public async Task<IActionResult> SendSpecPlusEmail(
            [FromBody] SendSpecPlusEmailRequest sendSpecPlusEmailRequest)
        {

            var apiResult = new ApiResultDto<string>()
            {
                Code = ResultCode.BadRequest,
                Message = DaviesImagingGroupConstants.SendSpecPlusEmail_400
            };

            try
            {

                apiResult = await _digCustomService.SendSpecPlusEmail(sendSpecPlusEmailRequest);

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

    }
}
