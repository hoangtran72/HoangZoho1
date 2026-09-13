using HoangZoho1.Constants;
using HoangZoho1.Helpers;
using HoangZoho1.Models.Common;
using HoangZoho1.Models.Lumicare.ZohoCRM;
using HoangZoho1.Models.Lumicare.ZohoForm;
using HoangZoho1.Services.Lumicare;
using HoangZoho1.Services.ZohoAuth;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HoangZoho1.Controllers.Lumicare
{

    [Route("api/lumicare")]
    [ApiController]
    public class LumicareController : ControllerBase
    {

        private readonly ILumicareCustomService _lumicareCustomService;
        private readonly ILumicareCrmService _lumicareCrmService;
        private readonly IZohoAuthService _zohoAuthService;

        public LumicareController(ILumicareCustomService lumicareCustomService, ILumicareCrmService lumicareCrmService, IZohoAuthService zohoAuthService)
        {
            _lumicareCustomService = lumicareCustomService;
            _lumicareCrmService = lumicareCrmService;
            _zohoAuthService = zohoAuthService;
        }

        [HttpGet("workdrive/token")]
        public async Task<IActionResult> GetAccessToken()
        {
            var ip = HttpContext.Connection.RemoteIpAddress.ToString();
            var apiResult = new ApiResultDto<string>()
            {
                Code = ResultCode.BadRequest,
                Message = LumicareConstants.CUSTOM_GWT_400
            };

            try
            {
                string token = await _zohoAuthService.GetAccessToken(LumicareConstants.Lumicare, CommonConstants.ZohoWorkDrive);
                if (!string.IsNullOrEmpty(token))
                {
                    apiResult.Code = ResultCode.OK;
                    apiResult.Message = LumicareConstants.CUSTOM_GWT_200;
                    apiResult.Data = token;
                    return Ok(apiResult);
                }

                return BadRequest(apiResult);
            }
            catch (Exception ex)
            {
                apiResult.Message = ex.Message + " " + ex.StackTrace;
                return BadRequest(apiResult);
            }
        }

        [HttpPost("handle-training-form")]
        public async Task<IActionResult> HandleTrainingFormPost(
            [FromBody] TrainingFormRequest trainingFormRequest)
        {
            var ip = HttpContext.Connection.RemoteIpAddress.ToString();
            var apiResult = new ApiResultDto<string>()
            {
                Code = ResultCode.BadRequest,
                Message = LumicareConstants.CUSTOM_HTF_400
            };
            var emailContent = new EmailContent
            {
                Email = EmailConstants.MyEmail_Username,
                Password = EmailConstants.MyEmail_Password,
                Body = @$"<b>IP Address</b>: {ip}<br><br><b>Request Body</b>:<br/>{JsonConvert.SerializeObject(trainingFormRequest)}",
                Subject = $"[Lumicare] Training Request",
                Clients = "hoangtran7292@gmail.com",
                SmtpPort = EmailConstants.SmtpPort,
                SmtpServer = EmailConstants.Gmail_SmtpServer
            };

            try
            {
                apiResult = await _lumicareCustomService.HandleTrainingForm(trainingFormRequest);

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
            finally
            {
                emailContent.Body += $"<br><br><b>API Result</b>:<br/>{JsonConvert.SerializeObject(apiResult)}";
                if (apiResult.Message.Contains("FAIL", StringComparison.InvariantCultureIgnoreCase))
                {
                    emailContent.Subject += " FAIL";
                    await EmailHelpers.SendEmail(emailContent);
                }
                else
                {
                    emailContent.Subject += " SUCCESS";
                }
            }
        }

        [HttpGet("handle-training-form")]
        public async Task<IActionResult> HandleTrainingFormGet(
            [FromBody] TrainingFormRequest trainingFormRequest)
        {
            var ip = HttpContext.Connection.RemoteIpAddress.ToString();
            var apiResult = new ApiResultDto<string>()
            {
                Code = ResultCode.BadRequest,
                Message = LumicareConstants.CUSTOM_HTF_400
            };
            var emailContent = new EmailContent
            {
                Email = EmailConstants.MyEmail_Username,
                Password = EmailConstants.MyEmail_Password,
                Body = @$"<b>IP Address</b>: {ip}<br><br><b>Request Body</b>:<br/>{JsonConvert.SerializeObject(trainingFormRequest)}",
                Subject = $"[Lumicare] Training Request",
                Clients = "hoangtran7292@gmail.com",
                SmtpPort = EmailConstants.SmtpPort,
                SmtpServer = EmailConstants.Gmail_SmtpServer
            };

            try
            {
                apiResult = await _lumicareCustomService.HandleTrainingForm(trainingFormRequest);

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
            finally
            {
                if (apiResult.Message.Contains("FAIL", StringComparison.InvariantCultureIgnoreCase))
                {
                    emailContent.Subject += " FAIL";
                }
                else
                {
                    emailContent.Subject += " SUCCESS";
                }
                emailContent.Body += $"<br><br><b>API Result</b>:<br/>{JsonConvert.SerializeObject(apiResult)}";
                await EmailHelpers.SendEmail(emailContent);
            }
        }

        [HttpPost("approve-client-onboarding/{contactId}")]
        public async Task<IActionResult> ApproveClientToEnroll(string contactId)
        {
            var apiResult = new ApiResultDto<string>()
            {
                Code = ResultCode.BadRequest,
                Message = LumicareConstants.CUSTOM_ACE_400
            };
            try
            {
                apiResult = await _lumicareCustomService.ApproveClientToEnroll(contactId);

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

        [HttpPost("check-distributor")]
        public async Task<IActionResult> CheckDistributor(CheckDistributorRequest checkDistributorRequest)
        {
            var apiResult = new ApiResultDto<string>()
            {
                Code = ResultCode.BadRequest,
                Message = CommonConstants.MSG_400
            };

            try
            {
                apiResult = await _lumicareCustomService.CheckDistributorCredential(checkDistributorRequest);
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

        [HttpGet("latest-news")]
        public async Task<IActionResult> GetNewsList()
        {
            var apiResult = new ApiResultDto<QueryLastestNewsResponse>()
            {
                Code = ResultCode.BadRequest,
                Message = CommonConstants.MSG_400
            };

            try
            {
                var coqlRequest = new ZohoCoqlRequest()
                {
                    select_query = @"select Name, News_Headline, News_Date, Content_Banner_Image_URL, Headline_Image_URL, Image_URL_1, 
                        Image_URL_2, Image_URL_3, Image_URL_4, Image_URL_5, Image_URL_6, Image_URL_7, 
                        Image_URL_8, Image_URL_9, Image_URL_10 from Latest_News where Name is not null AND Is_Published = 'true' order by Created_Time DESC"
                };

                apiResult = await _lumicareCrmService.QueryLatestNews(coqlRequest);
                return Ok(apiResult);
            }
            catch (Exception ex)
            {
                apiResult.Message = ex.Message + " " + ex.StackTrace;
                return BadRequest(apiResult);
            }
        }

        [HttpGet("all-latest-news")]
        public async Task<IActionResult> GetAllLatestNews()
        {
            var apiResult = new ApiResultDto<QueryLastestNewsResponse>()
            {
                Code = ResultCode.BadRequest,
                Message = CommonConstants.MSG_400
            };

            try
            {
                var coqlRequest = new ZohoCoqlRequest()
                {
                    select_query = @"select Name, News_Headline, News_Date, Content_Banner_Image_URL, Headline_Image_URL, Image_URL_1, 
                        Image_URL_2, Image_URL_3, Image_URL_4, Image_URL_5, Image_URL_6, Image_URL_7, 
                        Image_URL_8, Image_URL_9, Image_URL_10 from Latest_News where Name is not null order by Created_Time DESC"
                };

                apiResult = await _lumicareCrmService.QueryLatestNews(coqlRequest);
                return Ok(apiResult);
            }
            catch (Exception ex)
            {
                apiResult.Message = ex.Message + " " + ex.StackTrace;
                return BadRequest(apiResult);
            }
        }

        [HttpGet("latest-news/{newsId}")]
        public async Task<IActionResult> GetNewsDetails(string newsId)
        {
            var apiResult = new ApiResultDto<QueryLastestNewsResponse>()
            {
                Code = ResultCode.BadRequest,
                Message = CommonConstants.MSG_400
            };

            try
            {
                var coqlRequest = new ZohoCoqlRequest()
                {
                    select_query = $@"select Name, News_Content, News_Date, Content_Banner_Image_URL, Headline_Image_URL, Image_URL_1, 
                        Image_URL_2, Image_URL_3, Image_URL_4, Image_URL_5, Image_URL_6, Image_URL_7, 
                        Image_URL_8, Image_URL_9, Image_URL_10 from Latest_News where id = '{newsId}' order by Created_Time DESC"
                };

                apiResult = await _lumicareCrmService.QueryLatestNews(coqlRequest);
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

        [HttpGet("headline-news")]
        public async Task<IActionResult> GetHeadlineNews()
        {
            var apiResult = new ApiResultDto<QueryLastestNewsResponse>()
            {
                Code = ResultCode.BadRequest,
                Message = CommonConstants.MSG_400
            };

            try
            {
                var coqlRequest = new ZohoCoqlRequest()
                {
                    select_query = @"select Name, News_Headline, News_Date, Headline_Image_URL from Latest_News where Is_Headline ='true' order by Created_Time DESC LIMIT 1"
                };

                apiResult = await _lumicareCrmService.QueryLatestNews(coqlRequest);
                return Ok(apiResult);
            }
            catch (Exception ex)
            {
                apiResult.Message = ex.Message + " " + ex.StackTrace;
                return BadRequest(apiResult);
            }
        }

    }
}
