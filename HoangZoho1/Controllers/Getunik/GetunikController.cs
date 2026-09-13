using HoangZoho1.Constants;
using HoangZoho1.Helpers;
using HoangZoho1.Models.Common;
using HoangZoho1.Models.Getunik.Custom;
using HoangZoho1.Models.Getunik.ZohoCRM;
using HoangZoho1.Models.Getunik.ZohoProjects;
using HoangZoho1.Models.GetUnik.ZohoBooks;
using HoangZoho1.Services.GetUnik;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HoangZoho1.Controllers.GetUnik
{

    [Route("api/getunik")]
    [ApiController]
    public class GetunikController : ControllerBase
    {

        private readonly IGetunikCrmService _getunikCrmService;
        private readonly IGetunikCustomService _getunikCustomService;
        private readonly IGetunikBooksService _getunikBooksService;

        public GetunikController(IGetunikCrmService getUnikCrmService,
            IGetunikCustomService getunikCustomService,
            IGetunikBooksService getunikBooksService)
        {
            _getunikCrmService = getUnikCrmService;
            _getunikCustomService = getunikCustomService;
            _getunikBooksService = getunikBooksService;
        }

        [HttpPost("books/create-and-submit/{recordId}")]
        public async Task<IActionResult> CreateAndSubmitInvoice(string recordId)
        {
            var apiResult = new ApiResultDto<string>()
            {
                Code = ResultCode.BadRequest,
                Message = GetunikConstants.HCIAS_400
            };

            try
            {
                apiResult = await _getunikCustomService.HandleCreateAndSubmitInvoice(recordId);
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

        [HttpPost("books/submit/{invoiceId}")]
        public async Task<IActionResult> SubmitInvoice(string invoiceId)
        {
            var apiResult = new ApiResultDto<SubmitInvoiceResponse>()
            {
                Code = ResultCode.BadRequest,
                Message = CommonConstants.MSG_400
            };
            try
            {
                apiResult = await _getunikBooksService.SubmitInvoice(invoiceId);
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

        [HttpPost("crm/quotes")]
        public async Task<IActionResult> CreateQuote([FromBody] object request)
        {
            var ip = HttpContext.Connection.RemoteIpAddress.ToString();
            string requestBody = JsonConvert.SerializeObject(request);
            var apiResult = new ApiResultDto<UpsertResponse<UpsertDetail>>()
            {
                Code = ResultCode.BadRequest,
                Message = CommonConstants.MSG_400
            };
            var emailContent = new EmailContent
            {
                Email = EmailConstants.MyEmail_Username,
                Password = EmailConstants.MyEmail_Password,
                Body = @$"<b>IP Address</b>: {ip}<br><br><b>Request Body</b>:{requestBody}",
                Subject = $"[getUnik] CreateQuote",
                Clients = "hoangtran7292@gmail.com",
                SmtpPort = EmailConstants.SmtpPort,
                SmtpServer = EmailConstants.Gmail_SmtpServer
            };

            try
            {

                apiResult = await _getunikCrmService.CreateQuote(requestBody);
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
            finally
            {
                emailContent.Subject += $" - {apiResult.Message}";
                emailContent.Body += $"<br><br><b>Response</b>:<br>{JsonConvert.SerializeObject(apiResult)}";
                await EmailHelpers.SendEmail(emailContent);
            }
        }

        [HttpPut("crm/quotes/{quoteId}")]
        public async Task<IActionResult> UpdateQuote(string quoteId, [FromBody] object request)
        {
            var ip = HttpContext.Connection.RemoteIpAddress.ToString();
            string requestBody = JsonConvert.SerializeObject(request);
            var apiResult = new ApiResultDto<UpsertResponse<UpsertDetail>>()
            {
                Code = ResultCode.BadRequest,
                Message = CommonConstants.MSG_400
            };
            var emailContent = new EmailContent
            {
                Email = EmailConstants.MyEmail_Username,
                Password = EmailConstants.MyEmail_Password,
                Body = @$"<b>IP Address</b>: {ip}<br><br><b>Request Body</b>:{requestBody}",
                Subject = $"[getUnik] UpdateQuote: {quoteId}",
                Clients = "hoangtran7292@gmail.com",
                SmtpPort = EmailConstants.SmtpPort,
                SmtpServer = EmailConstants.Gmail_SmtpServer
            };

            try
            {

                apiResult = await _getunikCrmService.UpdateQuote(quoteId, requestBody);
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
            finally
            {
                emailContent.Subject += $" - {apiResult.Message}";
                emailContent.Body += $"<br><br><b>Response</b>:<br>{JsonConvert.SerializeObject(apiResult)}";
                await EmailHelpers.SendEmail(emailContent);
            }
        }

        [HttpGet("crm/products/{productId}")]
        public async Task<IActionResult> GetProductById(string productId)
        {
            var ip = HttpContext.Connection.RemoteIpAddress.ToString();
            var apiResult = new ApiResultDto<GetProductByIdResponse>()
            {
                Code = ResultCode.BadRequest,
                Message = CommonConstants.MSG_400
            };

            try
            {

                apiResult = await _getunikCrmService.GetProductById(productId);
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

        [HttpGet("crm/products/{productId}/deliverables")]
        public async Task<IActionResult> GetProductDeliverablesById(string productId)
        {
            var ip = HttpContext.Connection.RemoteIpAddress.ToString();
            var apiResult = new ApiResultDto<GetProductDeliverablesResponse>()
            {
                Code = ResultCode.BadRequest,
                Message = CommonConstants.MSG_400
            };

            try
            {

                apiResult = await _getunikCrmService.GetProductDeliverables(productId);
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

        [HttpPost("projects/get-by-url")]
        public async Task<IActionResult> GetProjectDetailsByUrl(
            [FromBody] GetProjectDetailsByUrlRequest getProjectDetailsByUrlRequest)
        {
            var ip = HttpContext.Connection.RemoteIpAddress.ToString();
            var apiResult = new ApiResultDto<GetProjectByIdResponse>()
            {
                Code = ResultCode.BadRequest,
                Message = GetunikConstants.GPDFU_400
            };

            try
            {

                string projectUrl = getProjectDetailsByUrlRequest.ProjectUrl;

                apiResult = await _getunikCustomService.GetProjectDetailsFromUrl(projectUrl);
                
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

        [HttpPost("widget/create-backlog-tasks")]
        public async Task<IActionResult> CreateBacklogTasks(
            [FromBody] CreateBacklogTasksRequest createBacklogTasksRequest)
        {

            var apiResult = new ApiResultDto<string>()
            {
                Code = ResultCode.BadRequest,
                Message = GetunikConstants.GPDFU_400
            };

            try
            {

                apiResult = await _getunikCustomService.CreateBacklogTasks(createBacklogTasksRequest);

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
