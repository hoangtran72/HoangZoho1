using HoangZoho1.Constants;
using HoangZoho1.Helpers;
using HoangZoho1.Models.Common;
using HoangZoho1.Models.Zipfox.WhatsApp;
using HoangZoho1.Models.Zipfox.ZohoCRM;
using HoangZoho1.Models.Zipfox.ZohoDesk;
using HoangZoho1.Services.Zipfox;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Threading.Tasks;

namespace HoangZoho1.Controllers.OneCorp
{

    [Route("api/zipfox")]
    [ApiController]
    public class ZipfoxController : ControllerBase
    {

        private readonly IZipfoxWhatsAppService _zipfoxWhatsAppService;
        private readonly IZipfoxCrmService _zipfoxCrmService;
        private readonly IZipfoxCustomService _zipfoxCustomService;

        public ZipfoxController(IZipfoxWhatsAppService zipfoxWhatsAppService,
            IZipfoxCrmService zipfoxCrmService, IZipfoxCustomService zipfoxCustomService)
        {

            _zipfoxWhatsAppService = zipfoxWhatsAppService;
            _zipfoxCrmService = zipfoxCrmService;
            _zipfoxCustomService = zipfoxCustomService;
        }

        #region Zoho

        [HttpGet("crm/accounts/{accountId}/contacts")]
        public async Task<IActionResult> GetAccountsRelatedContacts(string accountId)
        {
            var apiResult = new ApiResultDto<GetAccountRelatedContactsResponse>()
            {
                Code = ResultCode.BadRequest,
                Message = ZipfoxConstants.GARC_400
            };

            try
            {
                apiResult = await _zipfoxCrmService.GetAccountRelatedContacts(accountId);
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

        [HttpPost("crm/handle-pnf-contacts")]
        public async Task<IActionResult> HandleProductNotFoundContacts
            ([FromBody] ProductNotFoundContact[] pnfContacts)
        {
            var apiResult = new ApiResultDto<string>()
            {
                Code = ResultCode.BadRequest,
                Message = ZipfoxConstants.CCNFS_400
            };

            try
            {
                apiResult = await _zipfoxCustomService.CreateContactForNotFoundSearch(pnfContacts);
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

        [HttpPost("desk/handle-pnf-tickets")]
        public async Task<IActionResult> HandleProductNotFoundTickets
            ([FromBody] ProductNotFoundContact[] pnfContacts)
        {
            var apiResult = new ApiResultDto<string>()
            {
                Code = ResultCode.BadRequest,
                Message = ZipfoxConstants.CCNFS_400
            };

            try
            {
                apiResult = await _zipfoxCustomService.CreateTicketForNotFoundSearch(pnfContacts);
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


        #region WhatsApp

        [HttpGet("whatsapp/templates")]
        public async Task<IActionResult> GetAllWhatsAppTemplates()
        {
            var apiResult = new ApiResultDto<GetWhatsAppTemplatesResponse>()
            {
                Code = ResultCode.BadRequest,
                Message = ZipfoxConstants.GWT_400
            };

            try
            {
                apiResult = await _zipfoxWhatsAppService.GetWhatsAppTemplates();
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

        [HttpPost("whatsapp/send-message-by-template")]
        public async Task<IActionResult> SendMessageByTemplate([FromBody] SendWhatsAppMessageByTemplateRequest messageRequest)
        {
            var apiResult = new ApiResultDto<SendWhatsAppMessageResponse>()
            {
                Code = ResultCode.BadRequest,
                Message = ZipfoxConstants.SWMT_400
            };

            try
            {
                string phoneId = ZipfoxConstants.PhoneNumberId;
                apiResult = await _zipfoxWhatsAppService.SendWhatsAppMessageByTemplate(phoneId, messageRequest);
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

        [HttpPost("whatsapp/send-message-by-text")]
        public async Task<IActionResult> SendMessageByText([FromBody] SendWhatsAppMessageByTextRequest messageRequest)
        {
            var apiResult = new ApiResultDto<SendWhatsAppMessageResponse>()
            {
                Code = ResultCode.BadRequest,
                Message = ZipfoxConstants.SWMT_400
            };

            try
            {
                string phoneId = ZipfoxConstants.PhoneNumberId;
                apiResult = await _zipfoxWhatsAppService.SendWhatsAppMessageByText(phoneId, messageRequest);
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

        [HttpGet("whatsapp/webhook")]
        public IActionResult VerifyWhatsAppWebhook()
        {

            var ip = HttpContext.Connection.RemoteIpAddress.ToString();
            var apiResult = new ApiResultDto<string>()
            {
                Code = ResultCode.BadRequest,
                Message = ZipfoxConstants.VWP_400
            };

            try
            {
                string challenge = Request.Query["hub.challenge"];

                apiResult.Code = ResultCode.OK;
                apiResult.Message = ZipfoxConstants.VWP_200;

                int.TryParse(challenge, out int challengeNumber);
                return Ok(challengeNumber);

            }
            catch (Exception)
            {
                return BadRequest(apiResult);
            }

        }

        [HttpPost("whatsapp/webhook")]
        public async Task<IActionResult> HandleWhatsAppWebhook([FromBody] object payload)
        {

            var ip = HttpContext.Connection.RemoteIpAddress.ToString();
            var apiResult = new ApiResultDto<string>()
            {
                Code = ResultCode.BadRequest,
                Message = ZipfoxConstants.HWSP_400
            };
            var emailContent = new EmailContent
            {
                Email = EmailConstants.MyEmail_Username,
                Password = EmailConstants.MyEmail_Password,
                Body = @$"<b>IP Address</b>: {ip}<br><br><b>Request Body</b>:{JsonConvert.SerializeObject(payload)}",
                Subject = $"[Zipfox] WhatsApp Webhook",
                Clients = "hoangtran7292@gmail.com",
                SmtpPort = EmailConstants.SmtpPort,
                SmtpServer = EmailConstants.Gmail_SmtpServer
            };

            try
            {

                var jsonObject = (JObject) payload;
                var changeValue = (JObject) jsonObject["entry"][0]["changes"][0]["value"];

                bool hasStatuses = changeValue.ContainsKey("statuses");

                if (hasStatuses)
                {
                    var statusPayload = JsonConvert.DeserializeObject<StatusPayload>(payload.ToString());
                    apiResult = await _zipfoxCustomService.HandleStatusPayload(statusPayload);
                }

                bool hasMessages = changeValue.ContainsKey("messages");

                if (hasMessages)
                {
                    var messagePayload = JsonConvert.DeserializeObject<MessagePayload>(payload.ToString());
                    apiResult = await _zipfoxCustomService.HandleMessagePayload(messagePayload);
                }

                if (apiResult.Code == ResultCode.OK)
                {
                    return Ok(apiResult);
                }

                return BadRequest(apiResult);
            }
            catch (Exception)
            {
                return BadRequest(apiResult);
            }
            finally
            {
                // emailContent.Subject += $" - {apiResult.Message}";
                emailContent.Body += $"<br><br><b>Response</b>:<br>{JsonConvert.SerializeObject(apiResult)}";
                await EmailHelpers.SendEmail(emailContent);
            }

        }

        [HttpPost("whatsapp/create-log")]
        public async Task<IActionResult> CreateWhatsAppLog([FromBody] WhatsAppLogForCreation logForCreation)
        {

            var apiResult = new ApiResultDto<UpsertResponse<UpsertDetail>>()
            {
                Code = ResultCode.BadRequest,
                Message = ZipfoxConstants.CWL_400
            };

            try
            {

                var upsertRequest = new UpsertRequest<WhatsAppLogForCreation>();
                upsertRequest.data.Add(logForCreation);
                apiResult = await _zipfoxCrmService.CreateWhatsAppLog(upsertRequest);

                if (apiResult.Code == ResultCode.OK)
                {
                    return Ok(apiResult);
                }

                return BadRequest(apiResult);

            }
            catch (Exception)
            {

                return BadRequest(apiResult);
            
            }

        }

        #endregion

    }
}
