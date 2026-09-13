using HoangZoho1.Constants;
using HoangZoho1.Helpers;
using HoangZoho1.Models.Common;
using HoangZoho1.Models.LocalingTours;
using HoangZoho1.Models.OneCorp.ScheduleOnce;
using HoangZoho1.Services.GetUnik;
using HoangZoho1.Services.LocalingTours;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HoangZoho1.Controllers.LocalingTours
{

    [Route("api/localingtours")]
    [ApiController]
    public class LocalingToursController : ControllerBase
    {

        private readonly ILocalingToursCustomService _localingToursCustomService;

        public LocalingToursController(ILocalingToursCustomService 
            localingToursCustomService)
        {
            _localingToursCustomService = localingToursCustomService;
        }

        [HttpPost("xero-webhook")]
        public async Task<IActionResult> ReceiveWebhook()
        {

            string rawBody = string.Empty;

            var apiResult = new ApiResultDto<string>()
            {
                Code = ResultCode.BadRequest,
                Message = "Xero Webhook Processing FAILED"
            };

            try
            {
                // Allow the request body to be read multiple times
                Request.EnableBuffering();
                
                using (var reader = new StreamReader(Request.Body, Encoding.UTF8, detectEncodingFromByteOrderMarks: false, leaveOpen: true))
                {
                    rawBody = await reader.ReadToEndAsync();
                    Request.Body.Position = 0; // Rewind for later use (if needed)
                }

                string emailSubject = "[Xero Webhook] Localing Tours Notification";
                string emailBody = "Payload:<br/>" + rawBody;

                var emailContent = new EmailContent()
                {
                    Email = EmailConstants.MyEmail_Username,
                    Password = EmailConstants.MyEmail_Password,
                    Body = emailBody,
                    Subject = emailSubject,
                    Clients = "hoangtran7292@gmail.com",
                    SmtpPort = EmailConstants.SmtpPort,
                    SmtpServer = EmailConstants.Gmail_SmtpServer
                };

                await EmailHelpers.SendEmail(emailContent);

                // Step 2: Get the Xero signature from the header
                if (!Request.Headers.TryGetValue("x-xero-signature", out var xeroSignature))
                {
                    return Unauthorized();
                }

                // Step 3: Validate the signature
                if (!XeroHelpers.IsXeroWebhookValid(rawBody, xeroSignature,
                    LocalingToursConstants.Xero_WebhookKey))
                {
                    return Unauthorized();
                }

                var xeroWebhook = JsonConvert.DeserializeObject<XeroInvoicePayload>(rawBody);
                var xeroEvents = xeroWebhook.events;

                if (xeroEvents == null || xeroEvents.Length == 0)
                {
                    return Ok("No events found in the webhook payload.");
                }

                foreach (var xeroEvent in xeroEvents)
                {
                    string resourceId = xeroEvent.resourceId;
                    string tenantId = xeroEvent.tenantId;
                    string eventCategory = xeroEvent.eventCategory;
                    string eventType = xeroEvent.eventType;

                    if (tenantId == LocalingToursConstants.Xero_TenantId && 
                        eventCategory == "INVOICE" && eventType == "UPDATE")
                    {
                        apiResult = await _localingToursCustomService.XeroSync2ZohoStandalone(resourceId);
                    }

                    await Task.Delay(2000); // Small delay to avoid overwhelming the system

                }

                if (apiResult.Code != ResultCode.OK)
                {

                    emailSubject = "[Xero Webhook] Sync to Zoho FAILED";
                    emailBody = "Payload:<br/>" + rawBody;
                    emailBody += $"<br/>Error:{apiResult.Message}<br/>";


                    emailContent = new EmailContent()
                    {
                        Email = EmailConstants.MyEmail_Username,
                        Password = EmailConstants.MyEmail_Password,
                        Body = emailBody,
                        Subject = emailSubject,
                        Clients = "hoangtran7292@gmail.com",
                        SmtpPort = EmailConstants.SmtpPort,
                        SmtpServer = EmailConstants.Gmail_SmtpServer
                    };

                    await EmailHelpers.SendEmail(emailContent);

                }

                return Ok();

            }
            catch (Exception ex)
            {

                string emailSubject = "[Xero Webhook] Sync to Zoho FAILED";
                string emailBody = "Payload:<br/>" + rawBody;
                emailBody += $"<br/>Error:{ex.Message} - {ex.StackTrace}<br/>";

                var emailContent = new EmailContent()
                {
                    Email = EmailConstants.MyEmail_Username,
                    Password = EmailConstants.MyEmail_Password,
                    Body = emailBody,
                    Subject = emailSubject,
                    Clients = "hoangtran7292@gmail.com",
                    SmtpPort = EmailConstants.SmtpPort,
                    SmtpServer = EmailConstants.Gmail_SmtpServer
                };

                await EmailHelpers.SendEmail(emailContent);
                return Ok();

            }

        }

        [HttpPost("amazon-s3/upload")]
        public async Task<IActionResult> Upload()
        {
            // Grab the first file in the request, ignoring the key name
            var file = Request.Form.Files.FirstOrDefault();

            if (file == null)
                return BadRequest("No file.");

            var url = await _localingToursCustomService.UploadS3Async(file);

            return Ok(new
            {
                success = true,
                url
            });
        }

    }

}
