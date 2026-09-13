using HoangZoho1.Constants;
using HoangZoho1.Helpers;
using HoangZoho1.Models.Common;
using HoangZoho1.Models.Oratto.Custom;
using HoangZoho1.Models.Oratto.OpenAI;
using HoangZoho1.Models.Oratto.ZohoMail;
using HoangZoho1.Services.Oratto;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HoangZoho1.Controllers.Oratto
{

    [Route("api/oratto")]
    [ApiController]
    public class OrattoController : ControllerBase
    {

        private readonly IOrattoCustomService _orattoCustomService;

        public OrattoController(IOrattoCustomService orattoCustomService, 
            IOrattoDocumentService orattoDocumentService)
        {
            _orattoCustomService = orattoCustomService;
        }

        [HttpPost("reassign-solicitor")]
        public async Task<IActionResult> ReassignSolicitor([FromBody] ReassignSolicitorRequest request)
        {
            var apiResult = new ApiResultDto<string>()
            {
                Code = ResultCode.BadRequest,
                Message = OrattoConstants.SERS_400
            };

            try
            {

                string matterId = request.MatterId;
                apiResult = await _orattoCustomService.SendEmailForReassign(matterId);
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

        [HttpPost("handle-last-update")]
        public async Task<IActionResult> SendLastUpdated([FromBody] SendLastUpdatedRequest request)
        {

            var apiResult = new ApiResultDto<string>()
            {
                Code = ResultCode.BadRequest,
                Message = OrattoConstants.SELU_400
            };

            try
            {

                string matterId = request.MatterId;
                apiResult = await _orattoCustomService.SendEmailForLastUpdated(matterId);
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



        [HttpGet("query-solicitors/{leadId}")]
        public async Task<IActionResult> QuerySolicitors(string leadId)
        {

            var apiResult = new ApiResultDto<QuerySolicitorsDto>()
            {
                Code = ResultCode.BadRequest,
                Message = OrattoConstants.SELU_400
            };

            try
            {

                apiResult = await _orattoCustomService.QuerySolicitors(leadId);
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

        [HttpPost("assign-solicitors")]
        public async Task<IActionResult> AssignSolicitorsToLead([FromBody] AssignSolicitorsToLeadRequest assignRequest)
        {

            var apiResult = new ApiResultDto<string>()
            {
                Code = ResultCode.BadRequest,
                Message = OrattoConstants.AS2L_400
            };

            try
            {

                string leadId = assignRequest.LeadId;
                string solicitorIds = assignRequest.SolicitorIds;

                apiResult = await _orattoCustomService.AssignSolicitorsToLead(leadId, solicitorIds);
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

        [HttpPost("upload-attachment-to-openai")]
        public async Task<IActionResult> UploadMailAttachmentsToOpenAI([FromBody] DownloadAttachmentRequest attachmentRequest)
        {

            var apiResult = new ApiResultDto<UploadFileResponse>()
            {
                Code = ResultCode.BadRequest,
                Message = OrattoConstants.UMA2O_400
            };

            try
            {

                apiResult = await _orattoCustomService.UploadMailAttachmentsToOpenAi(attachmentRequest);

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

        
        [HttpPost("generate-attachment-content")]
        public async Task<IActionResult> GenerateAttachmentContent([FromBody] DownloadAttachmentRequest attachmentRequest)
        {

            var apiResult = new ApiResultDto<string>()
            {
                Code = ResultCode.BadRequest,
                Message = OrattoConstants.GACBG_400
            };

            try
            {

                apiResult = await _orattoCustomService.GenerateAttachmentContentByGemini(attachmentRequest);

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

        [HttpPost("chat-completions/{emailId}")]
        public async Task<IActionResult> ChatCompletions(string emailId, [FromBody] ChatCompletionRequest completionRequest)
        {

            var apiResult = new ApiResultDto<ChatCompletionResponse>()
            {
                Code = ResultCode.BadRequest,
                Message = OrattoConstants.CC4QE_400
            };
            var emailContent = new EmailContent
            {
                Email = EmailConstants.MyEmail_Username,
                Password = EmailConstants.MyEmail_Password,
                Body = @$"<b>Request Body</b>:{JsonConvert.SerializeObject(completionRequest)}",
                Subject = $"[Simply.Law] Chat Completions: {emailId}",
                Clients = "hoangtran7292@gmail.com",
                SmtpPort = EmailConstants.SmtpPort,
                SmtpServer = EmailConstants.Gmail_SmtpServer
            };

            try
            {
                
                apiResult = await _orattoCustomService.ChatCompletionsForQueuedEmail
                    (emailId, completionRequest);
                if (apiResult.Code != ResultCode.OK)
                {
                    // Try 1 more time
                    apiResult = await _orattoCustomService.ChatCompletionsForQueuedEmail
                            (emailId, completionRequest);
                }

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
                emailContent.Body += $"<br><br><b>Response</b>:<br>{JsonConvert.SerializeObject(apiResult)}";
                await EmailHelpers.SendEmail(emailContent);
                return BadRequest(apiResult);
            }

        }



    }
}
