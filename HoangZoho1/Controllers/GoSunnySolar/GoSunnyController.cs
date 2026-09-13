using HoangZoho1.Constants;
using HoangZoho1.Models.Common;
using HoangZoho1.Models.GoSunnySolar.Podium;
using HoangZoho1.Services.GoSunnySolar;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HoangZoho1.Controllers.GoSunnySolar
{

    [Route("api/gosunny")]
    [ApiController]
    public class GoSunnyController : ControllerBase
    {

        private readonly ISolarCustomService _solarCustomService;
        private readonly ISolarPodiumService _solarPodiumService;

        public GoSunnyController(ISolarCustomService solarCustomService, 
            ISolarPodiumService solarPodiumService)
        {
            _solarCustomService = solarCustomService;
            _solarPodiumService = solarPodiumService;
        }

        #region Zoho CRM

        [HttpGet("upsert-lead/{contactId}")]
        public async Task<IActionResult> UpsertLeadGet(string contactId)
        {
            var apiResult = new ApiResultDto<string>()
            {
                Code = ResultCode.BadRequest,
                Message = CommonConstants.MSG_400
            };
            try
            {
                apiResult = await _solarCustomService.UpsertLeadsInCrmFromGhl(contactId);

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
                apiResult.Message = ex.Message + " " + ex.StackTrace;
                return BadRequest(apiResult);
            }
        }

        [HttpPost("upsert-lead/{contactId}")]
        public async Task<IActionResult> UpsertLeadPost(string contactId)
        {
            var apiResult = new ApiResultDto<string>()
            {
                Code = ResultCode.BadRequest,
                Message = CommonConstants.MSG_400
            };
            try
            {
                apiResult = await _solarCustomService.UpsertLeadsInCrmFromGhl(contactId);

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
                apiResult.Message = ex.Message + " " + ex.StackTrace;
                return BadRequest(apiResult);
            }
        }

        #endregion

        #region Podium

        [HttpGet("podium/templates")]
        public async Task<IActionResult> GetPodiumTemplates()
        {
            var apiResult = new ApiResultDto<GetTemplatesResponse>()
            {
                Code = ResultCode.BadRequest,
                Message = CommonConstants.MSG_400
            };
            try
            {
                apiResult = await _solarPodiumService.GetTemplates();

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
                apiResult.Message = ex.Message + " " + ex.StackTrace;
                return BadRequest(apiResult);
            }
        }

        [HttpPost("podium/messages")]
        public async Task<IActionResult> SendPodiumMessages([FromBody] SendMessageRequest messageRequest)
        {
            var apiResult = new ApiResultDto<SendMessageResponse>()
            {
                Code = ResultCode.BadRequest,
                Message = CommonConstants.MSG_400
            };
            try
            {
                apiResult = await _solarPodiumService.SendMessage(messageRequest);

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
                apiResult.Message = ex.Message + " " + ex.StackTrace;
                return BadRequest(apiResult);
            }
        }

        #endregion

    }

}
