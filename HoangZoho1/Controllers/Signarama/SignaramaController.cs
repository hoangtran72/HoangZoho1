using HoangZoho1.Constants;
using HoangZoho1.Models.Common;
using HoangZoho1.Models.Signarama.Custom;
using HoangZoho1.Models.Signarama.ZohoBooks;
using HoangZoho1.Services.Signarama;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HoangZoho1.Controllers.Signarama
{

    [Route("api/signarama")]
    [ApiController]
    public class SignaramaController : ControllerBase
    {

        private readonly ISignaramaCustomService _signaramaCustomService;

        public SignaramaController(ISignaramaCustomService signaramaCustomService)
        {
            _signaramaCustomService = signaramaCustomService;
        }

        [HttpPost("handle-uploaded-estimate")]
        public async Task<IActionResult> ReassignSolicitor(
            [FromBody] HandleEstimateRequest request)
        {
            var apiResult = new ApiResultDto<HandleEstimateResponse>()
            {
                Code = ResultCode.BadRequest,
                Message = SignaramaConstants.HUE_400
            };

            try
            {

                apiResult = await _signaramaCustomService.HandleUploadedEstimate(request);
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

        [HttpPost("upload-file-to-estimate")]
        public async Task<IActionResult> UploadFile2Estimate(
            [FromBody] UploadAttachmentRequest request)
        {

            var apiResult = new ApiResultDto<UploadAttachmentResponse>()
            {
                Code = ResultCode.BadRequest,
                Message = SignaramaConstants.UAB_400
            };

            try
            {

                apiResult = await _signaramaCustomService.UploadAttachment(request);
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
