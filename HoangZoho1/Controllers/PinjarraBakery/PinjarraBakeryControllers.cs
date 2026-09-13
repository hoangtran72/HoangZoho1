using HoangZoho1.Constants;
using HoangZoho1.Helpers;
using HoangZoho1.Models.Common;
using HoangZoho1.Models.PinjarraBakery;
using HoangZoho1.Models.PinjarraBakery.ZohoForm;
using HoangZoho1.Models.PinjarraBakery.ZohoProjects;
using HoangZoho1.Models.Twilio;
using HoangZoho1.Services.PinjarraBakery;
using HoangZoho1.Services.Twilio;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Twilio.Rest.Api.V2010.Account;

namespace HoangZoho1.Controllers.PinjarraBakery
{
    [Route("api/pinjarra")]
    [ApiController]
    public class PinjarraBakeryControllers : ControllerBase
    {
        private readonly ITwilioService _twillioService;
        private readonly IPinjarraCustomService _pinjarraCustomService;

        public PinjarraBakeryControllers(ITwilioService twillioService, IPinjarraCustomService pinjarraCustomService)
        {
            _twillioService = twillioService;
            _pinjarraCustomService = pinjarraCustomService;
        }

        #region Twilio API

        [HttpPost("send-booking-sms")]
        public IActionResult SendBookingSMS([FromBody] BookingSmsRequest smsRequest)
        {
            var apiResult = new ApiResultDto<MessageResource>()
            {
                Code = ResultCode.BadRequest,
                Message = PinjarraBakeryConstants.SendBookingSMS_400
            };
            try
            {
                string smsBody = PinjarraBakeryConstants.BookingSmsBody;
                smsBody = smsBody.Replace("{FirstName}", smsRequest.FirstName)
                                 .Replace("{BookingUrl}", PinjarraBakeryConstants.BookingUrl);

                var sendSms = new SendSMSRequest()
                {
                    AccountSID = PinjarraBakeryConstants.AccountSID,
                    AuthToken = PinjarraBakeryConstants.AuthToken,
                    Body = smsBody,
                    From = PinjarraBakeryConstants.From,
                    To = smsRequest.Mobile
                };
                apiResult = _twillioService.SendTwilioSMS(sendSms);

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

        #endregion

        #region Zoho API

        [HttpGet("initialize-tasks/{projectId}")]
        public async Task<IActionResult> InitializeTasks(string projectId)
        {
            var apiResult = new ApiResultDto<string>()
            {
                Code = ResultCode.BadRequest,
                Message = CommonConstants.MSG_400
            };

            try
            {
                apiResult = await _pinjarraCustomService.Recruitment_InitializeTasks(projectId);
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

        [HttpGet("send-email")]
        public async Task<IActionResult> SendEmail(string projectId, string taskId)
        {
            var apiResult = new ApiResultDto<string>()
            {
                Code = ResultCode.BadRequest,
                Message = CommonConstants.MSG_400
            };

            try
            {
                apiResult = await _pinjarraCustomService.Recruitment_SendEmail(projectId, taskId);
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

        [HttpPost("send-r04-email")]
        public async Task<IActionResult> SendR04NotificationEmail([FromBody] R04NotiEmailRequest r04Request)
        {
            var apiResult = new ApiResultDto<string>()
            {
                Code = ResultCode.BadRequest,
                Message = CommonConstants.MSG_400
            };

            try
            {
                apiResult = await _pinjarraCustomService.Recruitment_SendR04NotiEmail(r04Request);
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

        #endregion

        #region

        [HttpPost("product-log")]
        public async Task<IActionResult> HandleProductLog([FromBody] HandleProductLogRequest createProductLog)
        {
            var apiResult = new ApiResultDto<string>()
            {
                Code = ResultCode.BadRequest,
                Message = CommonConstants.MSG_400
            };

            try
            {
                /*
                EmailContent emailContent = null;
                emailContent = new EmailContent()
                {
                    Subject = "Test Webhook from ZohoForms",
                    Body = $"Dear Hoang,<br><br>Request Body:<br>{JsonConvert.SerializeObject(createProductLog)}",
                    Clients = "hoangtran7292@gmail.com",
                    Email = CommonConstants.HoangTestGmail,
                    Password = CommonConstants.HoangTestPassword,
                    SmtpPort = CommonConstants.SmtpPort,
                    SmtpServer = CommonConstants.Gmail_SmtpServer
                };
                await EmailHelpers.SendEmail(emailContent);
                */

                apiResult = await _pinjarraCustomService.HandleBakehouseProductLog(createProductLog);

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

        #endregion

    }
}
