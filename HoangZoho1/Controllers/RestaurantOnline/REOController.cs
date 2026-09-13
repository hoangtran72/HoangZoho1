using HoangZoho1.Constants;
using HoangZoho1.Helpers;
using HoangZoho1.Models.Common;
using HoangZoho1.Models.RestaurantEquipmentOnline.Custom;
using HoangZoho1.Models.RestaurantEquipmentOnline.JustCall;
using HoangZoho1.Models.RestaurantEquipmentOnline.Shopify;
using HoangZoho1.Models.RestaurantEquipmentOnline.TNZ;
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
    [Route("api/reo")]
    [ApiController]
    public class REOController : ControllerBase
    {

        private readonly IReoCustomService _reoCustomService;
        private readonly IReoShopifyService _reoShopifyService;
        private readonly IReoTnzService _reoTnzService;

        public REOController(IReoCustomService reoCustomService, 
            IReoShopifyService reoShopifyService, IReoTnzService reoTnzService)
        {
            _reoCustomService = reoCustomService;
            _reoShopifyService = reoShopifyService;
            _reoTnzService = reoTnzService;
        }

        [HttpPost("handle-sales-emails")]
        public async Task<IActionResult> HandleSalesEmails(string emailDate, string startTime, string endTime)
        {
            var apiResult = new ApiResultDto<string>()
            {
                Code = ResultCode.BadRequest,
                Message = REOConstants.CUSTOM_GSEP_400
            };
            try
            {
                apiResult = await _reoCustomService.HandleSalesEmails(emailDate, startTime, endTime);

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

        [HttpGet("get-lead-number-daily")]
        public async Task<IActionResult> GetLeadNumber()
        {
            try
            {
                int leadNumber = await _reoCustomService.GetLeadNumberDaily();
                return Ok(leadNumber);
            }
            catch (Exception ex)
            {
                return BadRequest($"{ex.Message} = {ex.StackTrace}");
            }
        }

        [HttpPost("sync-call")]
        public async Task<IActionResult> SyncCallToZohoCRM([FromBody] CallPayload callPayload)
        {
            var ip = HttpContext.Connection.RemoteIpAddress.ToString();
            var apiResult = new ApiResultDto<string>()
            {
                Code = ResultCode.OK,
                Message = REOConstants.SSZ_200
            };
            var emailContent = new EmailContent
            {
                Email = EmailConstants.MyEmail_Username,
                Password = EmailConstants.MyEmail_Password,
                Body = @$"<b>IP Address</b>: {ip}<br><br><b>Request Body</b>:<br/>{JsonConvert.SerializeObject(callPayload)}",
                Subject = $"[REO] Sync JustCall Call: {callPayload.data.callid}",
                Clients = "hoangtran7292@gmail.com",
                SmtpPort = EmailConstants.SmtpPort,
                SmtpServer = EmailConstants.Gmail_SmtpServer
            };

            try
            {
                apiResult = await _reoCustomService.SyncCallToZohoCRM(callPayload);

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
                apiResult.Message = $"{ex.Message} - {ex.StackTrace}";
                return BadRequest(apiResult);
            }
            finally
            {
                if (apiResult.Code != ResultCode.OK)
                {
                    emailContent.Subject += " FAIL";
                    emailContent.Body += $"<br><br><b>API Result</b>:<br/>{JsonConvert.SerializeObject(apiResult)}";
                    await EmailHelpers.SendEmail(emailContent);
                }
                else
                {
                    emailContent.Subject += " SUCCESS";
                }
            }
        }

        [HttpPost("mass-sync-call")]
        public async Task<IActionResult> MassSyncCallToZohoCRM()
        {
            var apiResult = new ApiResultDto<string>()
            {
                Code = ResultCode.OK,
                Message = REOConstants.MSCZ_400
            };

            try
            {
                apiResult = await _reoCustomService.MassSyncCallsToZohoCRM();

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
                apiResult.Message = $"{ex.Message} - {ex.StackTrace}";
                return BadRequest(apiResult);
            }
        }

        [HttpPost("sync-sms")]
        public async Task<IActionResult> SyncSMSToZohoCRM([FromBody] SMSPayload smsPayload)
        {
            var ip = HttpContext.Connection.RemoteIpAddress.ToString();
            var apiResult = new ApiResultDto<string>()
            {
                Code = ResultCode.OK,
                Message = REOConstants.SSZ_200
            };
            var emailContent = new EmailContent
            {
                Email = EmailConstants.MyEmail_Username,
                Password = EmailConstants.MyEmail_Password,
                Body = @$"<b>IP Address</b>: {ip}<br><br><b>Request Body</b>:<br/>{JsonConvert.SerializeObject(smsPayload)}",
                Subject = $"[REO] Sync JustCall SMS: {smsPayload.data.messageid}",
                Clients = "hoangtran7292@gmail.com",
                SmtpPort = EmailConstants.SmtpPort,
                SmtpServer = EmailConstants.Gmail_SmtpServer
            };

            try
            {
                apiResult = await _reoCustomService.SyncSMSToZohoCRM(smsPayload);

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
                apiResult.Code = ResultCode.BadRequest;
                apiResult.Message = REOConstants.SSZ_400;
                apiResult.Data = $"{ex.Message} - {ex.StackTrace}";
                return BadRequest(apiResult);
            }
            finally
            {
                if (apiResult.Code != ResultCode.OK)
                {
                    emailContent.Subject += " FAIL";
                    emailContent.Body += $"<br><br><b>API Result</b>:<br/>{JsonConvert.SerializeObject(apiResult)}";
                    await EmailHelpers.SendEmail(emailContent);
                }
                else
                {
                    emailContent.Subject += " SUCCESS";
                }
            }
        }

        [HttpPost("mass-sync-sms")]
        public async Task<IActionResult> MassSyncSMSToZohoCRM()
        {
            var apiResult = new ApiResultDto<string>()
            {
                Code = ResultCode.OK,
                Message = REOConstants.SSZ_200
            };
            try
            {
                apiResult = await _reoCustomService.MassSyncSMSToZohoCRM();

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
                apiResult.Code = ResultCode.BadRequest;
                apiResult.Message = REOConstants.SSZ_400;
                apiResult.Data = $"{ex.Message} - {ex.StackTrace}";
                return BadRequest(apiResult);
            }
        }

        [HttpPost("tnz-webhook")]
        public async Task<IActionResult> HandleTnzWebhook([FromBody] object tnzRequest)
        {
            var apiResult = new ApiResultDto<string>()
            {
                Code = ResultCode.OK,
                Message = REOConstants.CUSTOM_HTW_200
            };
            var emailContent = new EmailContent
            {
                Email = EmailConstants.MyEmail_Username,
                Password = EmailConstants.MyEmail_Password,
                Body = @$"<b>Request Body</b>:<br/>{JsonConvert.SerializeObject(tnzRequest)}",
                Subject = $"[REO] Handle TNZ Webhook",
                Clients = "hoangtran7292@gmail.com",
                SmtpPort = EmailConstants.SmtpPort,
                SmtpServer = EmailConstants.Gmail_SmtpServer
            };
            try
            {
                return Ok(apiResult);
            }
            catch (Exception ex)
            {
                apiResult.Code = ResultCode.BadRequest;
                apiResult.Message = REOConstants.CUSTOM_HTW_400;
                apiResult.Data = $"{ex.Message} - {ex.StackTrace}";
                return BadRequest(apiResult);
            }
            finally
            {
                if (apiResult.Code != ResultCode.OK)
                {
                    emailContent.Subject += " FAIL";
                }
                else
                {
                    emailContent.Subject += " SUCCESS";
                }
                // emailContent.Body += $"<br><br><b>API Result<b/>:<br/>{JsonConvert.SerializeObject(apiResult)}";
                await EmailHelpers.SendEmail(emailContent);
            }
        }

        [HttpPost("tnz/send-sms")]
        public async Task<IActionResult> SendTnzSMS([FromBody] SendTnzSmsRequest tnzSmsRequest)
        {
            var apiResult = new ApiResultDto<string>()
            {
                Code = ResultCode.BadRequest,
                Message = REOConstants.STS_400
            };
            try
            {

                string token = tnzSmsRequest.Token;
                var sendSmsRequest = tnzSmsRequest.SmsRequest;

                apiResult = await _reoCustomService.SendTnzSMSAndCreateLog(tnzSmsRequest);

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
                apiResult.Message = $"{ex.Message} - {ex.StackTrace}";
                return BadRequest(apiResult);
            }
        }

        [HttpGet("quotes/{quoteId}")]
        public async Task<IActionResult> GetQuoteDetails(string quoteId)
        {
            var apiResult = new ApiResultDto<GetQuoteDetailsResponse>()
            {
                Code = ResultCode.OK,
                Message = REOConstants.CUSTOM_GSQ_400
            };
            try
            {
                var getQuoteRequest = new GetQuoteDetailsRequest
                {
                    quote = quoteId
                };
                apiResult = await _reoShopifyService.GetQuoteDetails(getQuoteRequest);
                return Ok(apiResult);
            }
            catch (Exception ex)
            {
                apiResult.Code = ResultCode.BadRequest;
                apiResult.Message = $"{ex.Message} - {ex.StackTrace}";
                return BadRequest(apiResult);
            }
        }

        [HttpPost("timeline-and-note")]
        public async Task<IActionResult> GetRecordTimelineAndNotes
            (GetTimelineAndNotesRequest timelineAndNotesRequest)
        {
            var apiResult = new ApiResultDto<List<List<string>>>
            {
                Code = ResultCode.OK,
                Message = REOConstants.GTAN_400
            };
            try
            {

                string recordModule = timelineAndNotesRequest.RecordModule;
                string recordId = timelineAndNotesRequest.RecordId;
               
                apiResult = await _reoCustomService.GetTimelineAndNote(recordModule, recordId);
                
                return Ok(apiResult);
            }
            catch (Exception ex)
            {
                apiResult.Code = ResultCode.BadRequest;
                apiResult.Message = $"{ex.Message} - {ex.StackTrace}";
                return BadRequest(apiResult);
            }
        }

        [HttpPost("get-product-data-table")]
        public async Task<IActionResult> GetProductDataTable
            (GetProductDataTableRequest request)
        {
            var apiResult = new ApiResultDto<GetProductDataTableResponse>()
            {
                Code = ResultCode.OK,
                Message = REOConstants.GPDT_400
            };
            try
            {

                string productSKUs = request.ProductSKUS;

                apiResult = await _reoCustomService.GetProductDataTable(productSKUs);

                return Ok(apiResult);

            }
            catch (Exception ex)
            {
                apiResult.Code = ResultCode.BadRequest;
                apiResult.Message = $"{ex.Message} - {ex.StackTrace}";
                return BadRequest(apiResult);
            }
        }

        [HttpPost("easy-add-quote")]
        public async Task<IActionResult> EasyAddQuote
            (EasyAddQuoteRequest easyAddQuoteRequest)
        {
            var apiResult = new ApiResultDto<EasyAddQuoteResponse>()
            {
                Code = ResultCode.OK,
                Message = REOConstants.EAQ_400
            };
            try
            {
                apiResult = await _reoCustomService.EasyAddQuote_Deal(easyAddQuoteRequest);
                if (apiResult.Code == ResultCode.OK)
                {
                    return Ok(apiResult);
                }
                return BadRequest(apiResult);
            }
            catch (Exception ex)
            {
                apiResult.Code = ResultCode.BadRequest;
                apiResult.Message = $"{ex.Message} - {ex.StackTrace}";
                return BadRequest(apiResult);
            }
        }

        [HttpGet("crm/quote-table/{quoteId}")]
        public async Task<IActionResult> GetQuoteTable
            (string quoteId)
        {
            var apiResult = new ApiResultDto<GetQuoteTableResponse>()
            {
                Code = ResultCode.OK,
                Message = REOConstants.EAQ_400
            };
            try
            {
                apiResult = await _reoCustomService.GetQuoteDataTable(quoteId);
                if (apiResult.Code == ResultCode.OK)
                {
                    return Ok(apiResult);
                }
                return BadRequest(apiResult);
            }
            catch (Exception ex)
            {
                apiResult.Code = ResultCode.BadRequest;
                apiResult.Message = $"{ex.Message} - {ex.StackTrace}";
                return BadRequest(apiResult);
            }
        }

        [HttpPost("crm/suggested-table")]
        public async Task<IActionResult> GetSuggestedTable
            (GetSuggestedProductRequest request)
        {
            var apiResult = new ApiResultDto<GetQuoteTableResponse>()
            {
                Code = ResultCode.OK,
                Message = REOConstants.EAQ_400
            };
            try
            {
                apiResult = await _reoCustomService.GetSuggestedDataTable(request);
                if (apiResult.Code == ResultCode.OK)
                {
                    return Ok(apiResult);
                }
                return BadRequest(apiResult);
            }
            catch (Exception ex)
            {
                apiResult.Code = ResultCode.BadRequest;
                apiResult.Message = $"{ex.Message} - {ex.StackTrace}";
                return BadRequest(apiResult);
            }
        }

        [HttpPut("crm/quotes")]
        public async Task<IActionResult> EasyUpdateQuote
            (EasyUpdateQuoteRequest request)
        {
            var apiResult = new ApiResultDto<string>()
            {
                Code = ResultCode.OK,
                Message = REOConstants.EAQ_400
            };
            try
            {
                apiResult = await _reoCustomService.EasyUpdateQuote(request);
                if (apiResult.Code == ResultCode.OK)
                {
                    return Ok(apiResult);
                }
                return BadRequest(apiResult);
            }
            catch (Exception ex)
            {
                apiResult.Code = ResultCode.BadRequest;
                apiResult.Message = $"{ex.Message} - {ex.StackTrace}";
                return BadRequest(apiResult);
            }
        }

        [HttpPut("crm/connect-with-lead/{leadId}")]
        public async Task<IActionResult> Blueprint_ConnectWithLead
            (string leadId, ConnectWithLeadRequest request)
        {
            var apiResult = new ApiResultDto<string>()
            {
                Code = ResultCode.OK,
                Message = REOConstants.EAQ_400
            };

            try
            {

                apiResult = await _reoCustomService
                    .BlueprintConnectWithLead(leadId, request);
                if (apiResult.Code == ResultCode.OK)
                {
                    return Ok(apiResult);
                }
                return BadRequest(apiResult);

            }
            catch (Exception ex)
            {
                apiResult.Code = ResultCode.BadRequest;
                apiResult.Message = $"{ex.Message} - {ex.StackTrace}";
                return BadRequest(apiResult);
            }
        }

    }
}
