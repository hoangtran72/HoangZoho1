using HoangZoho1.Constants;
using HoangZoho1.Models.Common;
using HoangZoho1.Models.DiamondMinds;
using HoangZoho1.Services.XeroAuth;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HoangZoho1.Controllers.DiamondMinds
{
    [Route("api/diamond")]
    [ApiController]
    public class DiamondMindControllers : ControllerBase
    {
        private readonly IXeroAuthService _xeroAuthService;

        public DiamondMindControllers(IXeroAuthService xeroAuthService)
        {
            _xeroAuthService = xeroAuthService;
        }

        #region Xero API
        [HttpPost("tokens")]
        public async Task<IActionResult> GetAccessToken([FromBody] GetTokenRequest tokenRequest)
        {
            var apiResult = new ApiResultDto<string>()
            {
                Code = ResultCode.BadRequest,
                Message = CommonConstants.MSG_400
            };

            try
            {
                string accessToken = await _xeroAuthService.GetAccessToken(tokenRequest.ClientName, 
                    tokenRequest.CompanyName);
                if (string.IsNullOrEmpty(accessToken))
                {
                    return BadRequest(apiResult);
                }

                apiResult.Code = ResultCode.OK;
                apiResult.Message = CommonConstants.MSG_200;
                apiResult.Data = accessToken;

                return Ok(apiResult);
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
