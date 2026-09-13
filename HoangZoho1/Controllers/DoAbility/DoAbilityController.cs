using HoangZoho1.Constants;
using HoangZoho1.Models.Common;
using HoangZoho1.Services.DoAbility;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System;
using HoangZoho1.Models.DoAbility;
using Google.Ads.GoogleAds.V18.Services;
using static Google.Ads.GoogleAds.V18.Enums.ConsentStatusEnum.Types;

namespace HoangZoho1.Controllers.DoAbility
{

    [Route("api/doability")]
    [ApiController]
    public class DoAbilityController : ControllerBase
    {

        private readonly IGoogleAdsService _googleAdsService;

        public DoAbilityController(IGoogleAdsService googleAdsService)
        {
            _googleAdsService = googleAdsService;
        }

        [HttpPost("google-ads/create-user-list")]
        public IActionResult CreateUserList(
            [FromBody] CreateUserListRequest createUserListRequest)
        {
            var apiResult = new ApiResultDto<MutateUserListsResponse>()
            {
                Code = ResultCode.BadRequest,
                Message = CommonConstants.MSG_400
            };

            try
            {

                apiResult = _googleAdsService.CreateCustomerMatchUserList(createUserListRequest);
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

        [HttpPost("google-ads/add-user-data-to-customer-match-list")]
        public IActionResult AddUserData2CustomerMatchList(
            [FromBody] AddUserToCustomerMatchUserListRequest addUserRequest)
        {
            var apiResult = new ApiResultDto<UploadUserDataResponse>()
            {
                Code = ResultCode.BadRequest,
                Message = DoAbilityConstants.AU2CMUL_400
            };

            try
            {

                apiResult = _googleAdsService
                    .AddUserToCustomerMatchUserList(addUserRequest);
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

        [HttpDelete("google-ads/remove-user-data-from-customer-match-list")]
        public IActionResult RemoveUserDataFromCustomerMatchList(
            [FromBody] RemoveUserFromCustomerMatchUserListRequest removeRequest)
        {

            var apiResult = new ApiResultDto<string>()
            {
                Code = ResultCode.BadRequest,
                Message = DoAbilityConstants.RUFCMUL_400
            };

            try
            {
                apiResult = _googleAdsService
                    .RemoveUserFromCustomerMatchUserList(removeRequest);

                if (apiResult.Code == ResultCode.OK) return Ok(apiResult);

                return BadRequest(apiResult);

            }
            catch (Exception ex)
            {
                apiResult.Message = ex.Message + " - " + ex.StackTrace;
                return BadRequest(apiResult);
            }

        }

    }

}
