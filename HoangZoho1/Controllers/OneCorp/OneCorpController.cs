using HoangZoho1.Constants;
using HoangZoho1.Helpers;
using HoangZoho1.Models.Common;
using HoangZoho1.Models.OneCorp.Custom;
using HoangZoho1.Models.OneCorp.Sakari;
using HoangZoho1.Models.OneCorp.ScheduleOnce;
using HoangZoho1.Models.OneCorp.ZohoCRM;
using HoangZoho1.Models.OneCorp.ZohoProjects;
using HoangZoho1.Models.OneCorp.ZohoWorkdrive;
using HoangZoho1.Services.OneBudget;
using HoangZoho1.Services.OneCorp;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HoangZoho1.Controllers.OneCorp
{
    [Route("api/onecorp")]
    [ApiController]
    public class OneCorpController : ControllerBase
    {

        private readonly IOneBudgetCustomService _oneBudgetCustomService;
        private readonly IOneCorpCustomService _oneCorpCustomService;
        private readonly IOneCorpProjectsService _oneCorpProjectsService;
        private readonly IOneCorpWorkdriveService _oneCorpWorkdriveService;
        private readonly IOneCorpSakariService _oneCorpSakariService;

        public OneCorpController(IOneCorpCustomService oneCorpCustomService,
            IOneCorpWorkdriveService oneCorpWorkdriveService,
            IOneCorpProjectsService oneCorpProjectsService,
            IOneCorpSakariService oneCorpSakariService, 
            IOneBudgetCustomService oneBudgetCustomService)
        {
            _oneCorpCustomService = oneCorpCustomService;
            _oneCorpWorkdriveService = oneCorpWorkdriveService;
            _oneCorpProjectsService = oneCorpProjectsService;
            _oneCorpSakariService = oneCorpSakariService;
            _oneBudgetCustomService = oneBudgetCustomService;
        }

        #region LDS: Lead Distribution System

        [HttpPost("sync-booking")]
        public async Task<IActionResult> SyncScheduleOnceBooking([FromBody] 
            BookingPayload bookingPayload)
        {
            var ip = HttpContext.Connection.RemoteIpAddress.ToString();
            var apiResult = new ApiResultDto<string>()
            {
                Code = ResultCode.BadRequest,
                Message = OneCorpConstants.SSB_400
            };
            var emailContent = new EmailContent
            {
                Email = EmailConstants.MyEmail_Username,
                Password = EmailConstants.MyEmail_Password,
                Body = @$"<b>IP Address</b>: {ip}<br><br><b>Request Body</b>:{JsonConvert.SerializeObject(bookingPayload)}",
                Subject = $"[OneCorp] Sync Booking: {bookingPayload.data.id}",
                Clients = "hoangtran7292@gmail.com",
                SmtpPort = EmailConstants.SmtpPort,
                SmtpServer = EmailConstants.Gmail_SmtpServer
            };
            string bookingSubject = string.Empty;

            try
            {

                var bookingDetails = bookingPayload.data;
                bookingSubject = bookingDetails.subject;
                
                apiResult = await _oneCorpCustomService.ScheduleOnce_SyncBooking(bookingPayload);

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
            finally
            {
                emailContent.Subject += $" - {apiResult.Message}";
                emailContent.Body += $"<br><br><b>Response</b>:<br>{JsonConvert.SerializeObject(apiResult)}";
                if (emailContent.Subject.Contains("FAILED", StringComparison.InvariantCultureIgnoreCase))
                {
                    emailContent.Clients += ";systems@onecorpaustralia.com.au";
                    await EmailHelpers.SendEmail(emailContent);
                }
                
            }
        }

        //[HttpPost("sync-booking-by-id/{bookingId}")]
        //public async Task<IActionResult> SyncScheduleonceBookingById(string bookingId)
        //{
        //    var ip = HttpContext.Connection.RemoteIpAddress.ToString();
        //    var apiResult = new ApiResultDto<string>()
        //    {
        //        Code = ResultCode.BadRequest,
        //        Message = OneCorpConstants.SSB_400
        //    };
        //    /*
        //    var emailContent = new EmailContent
        //    {
        //        Email = EmailConstants.MyEmail_Username,
        //        Password = EmailConstants.MyEmail_Password,
        //        Body = @$"<b>IP Address</b>: {ip}<br><br><b>Request Body</b>:{JsonConvert.SerializeObject(bookingPayload)}",
        //        Subject = $"[OneCorp] Sync Booking: {bookingPayload.data.id}",
        //        Clients = "hoangtran7292@gmail.com",
        //        SmtpPort = EmailConstants.SmtpPort,
        //        SmtpServer = EmailConstants.Gmail_SmtpServer
        //    };
        //    */

        //    try
        //    {

        //        apiResult = await _oneCorpCustomService.SyncBookingById(bookingId);
        //        apiResult = await _oneBudgetCustomService.SyncBookingById(bookingId);
        //        switch (apiResult.Code)
        //        {
        //            case ResultCode.OK:
        //                return Ok(apiResult);
        //            default:
        //                return BadRequest(apiResult);
        //        }
        //    }
        //    catch (Exception)
        //    {
        //        return BadRequest(apiResult);
        //    }
        //    /*
        //    finally
        //    {
        //        emailContent.Subject += $" - {apiResult.Message}";
        //        if (emailContent.Subject.Contains("FAILED", StringComparison.InvariantCultureIgnoreCase))
        //        {
        //            emailContent.Clients += ";systems@onecorpaustralia.com.au@onecorpaustralia.com.au";
        //        }
        //        emailContent.Body += $"<br><br><b>Response</b>:<br>{JsonConvert.SerializeObject(apiResult)}";
        //        await EmailHelpers.SendEmail(emailContent);
        //    }
        //    */
        //}

        [HttpPost("sync-call/{callId}")]
        public async Task<IActionResult> SyncZohoCall(string callId)
        {
            var apiResult = new ApiResultDto<string>()
            {
                Code = ResultCode.BadRequest,
                Message = OneCorpConstants.SCMCU_400
            };

            try
            {
                apiResult = await _oneCorpCustomService.SyncCallToDBUponCreationUpdation(callId);
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

        [HttpPost("sync-task/{taskId}")]
        public async Task<IActionResult> SyncZohoTask(string taskId)
        {
            var apiResult = new ApiResultDto<string>()
            {
                Code = ResultCode.BadRequest,
                Message = OneCorpConstants.STMCU_400
            };

            try
            {
                apiResult = await _oneCorpCustomService.SyncTaskToDBUponCreationUpdation(taskId);
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

        [HttpPost("sync-task-upon-deletion/{taskId}")]
        public IActionResult SyncZohoTaskUponDeletion(string taskId)
        {
            var apiResult = new ApiResultDto<string>()
            {
                Code = ResultCode.BadRequest,
                Message = OneCorpConstants.STMD_400
            };

            try
            {
                apiResult = _oneCorpCustomService.SyncTaskToDBUponDeletion(taskId);
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

        [HttpPost("sync-lead-upon-deletion/{leadId}")]
        public IActionResult SyncZohoLeadUponDeletion(string leadId)
        {
            var apiResult = new ApiResultDto<string>()
            {
                Code = ResultCode.BadRequest,
                Message = OneCorpConstants.SLMD_400
            };

            try
            {
                apiResult = _oneCorpCustomService.SyncLeadToDBUponDeletion(leadId);
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

        [HttpPost("sync-lead/{leadId}")]
        public async Task<IActionResult> SyncZohoLead(string leadId)
        {
            var apiResult = new ApiResultDto<string>()
            {
                Code = ResultCode.BadRequest,
                Message = OneCorpConstants.SLMCU_400
            };

            try
            {
                apiResult = await _oneCorpCustomService.SyncLeadToDBUponCreationUpdation(leadId);
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

        [HttpPost("sync-lead-status-history/{leadId}")]
        public async Task<IActionResult> SyncZohoLeadStatusHistory(string leadId)
        {
            var apiResult = new ApiResultDto<string>()
            {
                Code = ResultCode.BadRequest,
                Message = OneCorpConstants.SLMCU_400
            };

            try
            {
                apiResult = await _oneCorpCustomService.SyncLeadStatusHistoryToDB(leadId);
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

        [HttpPost("import-tasks-to-db")]
        public async Task<IActionResult> ImportTasksToDB()
        {
            var apiResult = new ApiResultDto<string>()
            {
                Code = ResultCode.BadRequest,
                Message = OneCorpConstants.SLMCU_400
            };

            try
            {
                apiResult = await _oneCorpCustomService.ImportTasksToDB();
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

        #region Zoho Sign

        [HttpPost("sign/upload-to-workdrive/{requestId}")]
        public async Task<IActionResult> UploadZohoSignDocumentToWorkdrive(string requestId)
        {
            var apiResult = new ApiResultDto<string>()
            {
                Code = ResultCode.BadRequest,
                Message = OneCorpConstants.USDW_400
            };

            var emailContent = new EmailContent
            {
                Email = EmailConstants.MyEmail_Username,
                Password = EmailConstants.MyEmail_Password,
                Subject = $"[OneCorp] Upload Sign Document to Workdrive: {requestId}",
                Body = @$"The outcome of the function is:<br/>$Result$<br/><br/>Thanks & Regards,<br/>OneCorp Automation",
                Clients = "hoangtran7292@gmail.com",
                SmtpPort = EmailConstants.SmtpPort,
                SmtpServer = EmailConstants.Gmail_SmtpServer
            };

            try
            {
                apiResult = await _oneCorpCustomService.UploadZohoSignDocument2Workdrive(requestId);
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
            finally
            {
                emailContent.Subject += $" - {apiResult.Message}";
                if (emailContent.Subject.Contains("FAILED", StringComparison.InvariantCultureIgnoreCase))
                {
                    emailContent.Clients += ";systems@onecorpaustralia.com.au";
                }
                emailContent.Body = emailContent.Body.Replace("$Result$", JsonConvert.SerializeObject(apiResult));
                await EmailHelpers.SendEmail(emailContent);
            }
        }

        #endregion

        #region Zoho Workdrive

        [HttpPost("workdrive/create-sub-folders")]
        public async Task<IActionResult> CreateSubFolders
            (CreateSubFoldersRequest createSubFoldersRequest)
        {
            var apiResult = new ApiResultDto<string>()
            {
                Code = ResultCode.BadRequest,
                Message = OneCorpConstants.USDW_400
            };

            string[] subFolders = createSubFoldersRequest.SubFolders;
            string parentId = createSubFoldersRequest.ParentId;

            try
            {
                apiResult = await _oneCorpCustomService.CreateSubFolders(subFolders, parentId);
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

        [HttpPost("workdrive/search-across-folder")]
        public async Task<IActionResult> SearchAcrossFolder
            ([FromBody] SearchAcrossFolderRequest searchRequest)
        {
            var apiResult = new ApiResultDto<SearchAcrossFolderResponse>()
            {
                Code = ResultCode.BadRequest,
                Message = OneCorpConstants.SAF_400
            };

            try
            {
                apiResult = await _oneCorpWorkdriveService.SearchAcrossFolder(searchRequest);
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

        [HttpPost("workdrive/move-folder")]
        public async Task<IActionResult> MoveFolder
            ([FromBody] MoveFolderRequest moveFolderRequest)
        {
            var apiResult = new ApiResultDto<MoveFolderResponse>()
            {
                Code = ResultCode.BadRequest,
                Message = OneCorpConstants.MF_400
            };

            try
            {
                apiResult = await _oneCorpWorkdriveService.MoveFolder(moveFolderRequest);
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

        #region Zoho CRM

        [HttpPost("crm/query-phone-groups")]
        public async Task<IActionResult> QueryPhoneGroupsByUserEmail
            ([FromBody] QueryPhoneGroupsRequest queryPhoneGroupsRequest)
        {
            var apiResult = new ApiResultDto<GetSakariUserByIdResponse>()
            {
                Code = ResultCode.BadRequest,
                Message = OneCorpConstants.USDW_400
            };

            try
            {

                string userEmail = queryPhoneGroupsRequest.UserEmail;
                apiResult = await _oneCorpCustomService.GetSakariUsersDetailsByEmail(userEmail);
                return Ok(apiResult);

            }
            catch (Exception)
            {
                return BadRequest(apiResult);
            }
        }

        #endregion

        #region Zoho Projects

        [HttpGet("zp-comments/deals/{dealId}")]
        public async Task<IActionResult> GetZPCommentsForDeal(string dealId)
        {
            var apiResult = new ApiResultDto<List<List<string>>>()
            {
                Code = ResultCode.BadRequest,
                Message = OneCorpConstants.USDW_400
            };

            try
            {
                var selectQuery = new ZohoCoqlRequest()
                {
                    select_query = $"select Project_Name, Project_URL, Task_Name, Task_URL, Comment_Id, Added_Person, Related_Deal, Related_Contact, Comment_Created_Time, Comment_Last_Modified_Time, Content, Has_Attachment from ZP_Comments WHERE Related_Deal = '{dealId}' order by Comment_Created_Time DESC LIMIT 2000"
                };
                apiResult = await _oneCorpCustomService.QueryZPComments(selectQuery);
                return Ok(apiResult);
            }
            catch (Exception)
            {
                return BadRequest(apiResult);
            }
        }

        [HttpGet("deals/{dealId}/zp-tasks")]
        public async Task<IActionResult> GetZpTasksForDeal(string dealId)
        {
            var apiResult = new ApiResultDto<List<List<string>>>()
            {
                Code = ResultCode.BadRequest,
                Message = OneCorpConstants.USDW_400
            };

            try
            {
                var selectQuery = new ZohoCoqlRequest()
                {
                    select_query = $"select Task_Name, Task_URL, Task_Status, Task_Status_Color_Code, Project_Name, Project_URL from Deals_x_ZP_Tasks WHERE Related_Deal.id = '{dealId}' order by Project_Name"
                };
                apiResult = await _oneCorpCustomService.QueryZPTasks(selectQuery);
                return Ok(apiResult);
            }
            catch (Exception)
            {
                return BadRequest(apiResult);
            }
        }

        [HttpGet("zp-comments/contacts/{contactId}")]
        public async Task<IActionResult> GetZPCommentsForContact(string contactId)
        {
            var apiResult = new ApiResultDto<List<List<string>>>()
            {
                Code = ResultCode.BadRequest,
                Message = OneCorpConstants.USDW_400
            };

            try
            {
                var selectQuery = new ZohoCoqlRequest()
                {
                    select_query = $"select Project_Name, Project_URL, Task_Name, Task_URL, Comment_Id, Added_Person, Related_Deal, Related_Contact, Comment_Created_Time, Comment_Last_Modified_Time, Content, Has_Attachment from ZP_Comments WHERE Related_Contact = '{contactId}' order by Project_Name ASC, Task_Name ASC, Comment_Created_Time DESC LIMIT 2000"
                };
                apiResult = await _oneCorpCustomService.QueryZPComments(selectQuery);
                return Ok(apiResult);
            }
            catch (Exception)
            {
                return BadRequest(apiResult);
            }
        }

        [HttpPost("associate-tag")]
        public async Task<IActionResult> AssociateTag(AssociateTagRequest associateRequest)
        {
            var apiResult = new ApiResultDto<string>()
            {
                Code = ResultCode.BadRequest,
                Message = OneCorpConstants.USDW_400
            };

            try
            {
                apiResult = await _oneCorpProjectsService.AssociateTag(associateRequest);
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

        #region [LEGACY] Sakari 

        [HttpPost("sakari/send-sms-from-workflow")]
        public async Task<IActionResult> SendSmsFromWorkflow
            (SendSakariSMSFromWorkflowRequest sendSmsRequest)
        {
            var apiResult = new ApiResultDto<string>()
            {
                Code = ResultCode.BadRequest,
                Message = OneCorpConstants.USDW_400
            };

            try
            {
                apiResult = await _oneCorpCustomService.SendSakariSMSFromWorkflow(sendSmsRequest);
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

        [HttpPost("sakari/send-sms-to-contact")]
        public async Task<IActionResult> SendSmsToContact
            (SendSakariSMSToContactRequest sendSmsToContact)
        {
            var apiResult = new ApiResultDto<string>()
            {
                Code = ResultCode.BadRequest,
                Message = OneCorpConstants.USDW_400
            };

            try
            {
                apiResult = await _oneCorpCustomService.SendSakariSMSToZohoContact(sendSmsToContact);
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

        [HttpPost("sakari/send-sms-to-lead")]
        public async Task<IActionResult> SendSmsToLead
            (SendSakariSMSToLeadRequest sendSmsToLead)
        {
            var apiResult = new ApiResultDto<string>()
            {
                Code = ResultCode.BadRequest,
                Message = OneCorpConstants.USDW_400
            };

            try
            {
                apiResult = await _oneCorpCustomService.SendSakariSMSToZohoLead(sendSmsToLead);
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

        [HttpGet("sakari/webhook/message-payload")]
        public async Task<IActionResult> SakariMessagePayloadGet([FromBody] MessagePayload messagePayload)
        {

            var ip = HttpContext.Connection.RemoteIpAddress.ToString();
            var apiResult = new ApiResultDto<string>()
            {
                Code = ResultCode.BadRequest,
                Message = OneCorpConstants.SSM2Z_400
            };

            var emailContent = new EmailContent
            {
                Email = EmailConstants.MyEmail_Username,
                Password = EmailConstants.MyEmail_Password,
                Body = @$"<b>IP Address</b>: {ip}<br><br><b>Request Body</b>:{JsonConvert.SerializeObject(messagePayload)}",
                Subject = $"[OneCorp] Sakari SMS Webhook",
                Clients = "hoangtran7292@gmail.com",
                SmtpPort = EmailConstants.SmtpPort,
                SmtpServer = EmailConstants.Gmail_SmtpServer
            };

            try
            {
                apiResult = await _oneCorpCustomService.SyncSakariMessagePayload(messagePayload);
                return Ok(apiResult);

            }
            catch (Exception)
            {
                return BadRequest(apiResult);
            }
            finally
            {
                emailContent.Subject += $" - {apiResult.Message}";
                if (apiResult.Code != ResultCode.OK)
                {
                    emailContent.Clients += ";systems@onecorpaustralia.com.au";
                }
                emailContent.Body += $"<br><br><b>Response</b>:<br>{JsonConvert.SerializeObject(apiResult)}";
                await EmailHelpers.SendEmail(emailContent);
            }
        }

        [HttpPost("sakari/webhook/message-payload")]
        public async Task<IActionResult> SakariMessagePayload([FromBody] MessagePayload messagePayload)
        {

            var ip = HttpContext.Connection.RemoteIpAddress.ToString();
            var apiResult = new ApiResultDto<string>()
            {
                Code = ResultCode.BadRequest,
                Message = OneCorpConstants.SSM2Z_400
            };

            var emailContent = new EmailContent
            {
                Email = EmailConstants.MyEmail_Username,
                Password = EmailConstants.MyEmail_Password,
                Body = @$"<b>IP Address</b>: {ip}<br><br><b>Request Body</b>:{JsonConvert.SerializeObject(messagePayload)}",
                Subject = $"[OneCorp] Sakari SMS Webhook",
                Clients = "hoangtran7292@gmail.com",
                SmtpPort = EmailConstants.SmtpPort,
                SmtpServer = EmailConstants.Gmail_SmtpServer
            };

            try
            {
                apiResult = await _oneCorpCustomService.SyncSakariMessagePayload(messagePayload);
                return Ok(apiResult);

            }
            catch (Exception)
            {
                return BadRequest(apiResult);
            }
            finally
            {
                emailContent.Subject += $" - {apiResult.Message}";
                if (apiResult.Code != ResultCode.OK)
                {
                    emailContent.Clients += ";systems@onecorpaustralia.com.au@onecorpaustralia.com.au";
                }
                emailContent.Body += $"<br><br><b>Response</b>:<br>{JsonConvert.SerializeObject(apiResult)}";
                await EmailHelpers.SendEmail(emailContent);
            }
        }

        [HttpPost("sakari/webhook/message-payload-test")]
        public async Task<IActionResult> SakariMessagePayloadTest([FromBody] object messagePayload)
        {

            var ip = HttpContext.Connection.RemoteIpAddress.ToString();
            var apiResult = new ApiResultDto<string>()
            {
                Code = ResultCode.BadRequest,
                Message = OneCorpConstants.SSM2Z_400
            };

            var emailContent = new EmailContent
            {
                Email = EmailConstants.MyEmail_Username,
                Password = EmailConstants.MyEmail_Password,
                Body = @$"<b>IP Address</b>: {ip}<br><br><b>Request Body</b>:{JsonConvert.SerializeObject(messagePayload)}",
                Subject = $"[OneCorp] Sakari SMS Webhook Test",
                Clients = "hoangtran7292@gmail.com",
                SmtpPort = EmailConstants.SmtpPort,
                SmtpServer = EmailConstants.Gmail_SmtpServer
            };

            try
            {
                return Ok(apiResult);
            }
            catch (Exception)
            {
                return BadRequest(apiResult);
            }
            finally
            {
                emailContent.Subject += $" - {apiResult.Message}";
                await EmailHelpers.SendEmail(emailContent);
            }
        }

        [HttpPost("sakari/mass-sync-messages")]
        public async Task<IActionResult> MassSyncSakariMessages()
        {
            var apiResult = new ApiResultDto<string>()
            {
                Code = ResultCode.OK,
                Message = OneCorpConstants.MSSM2Z_400 
            };
            try
            {
                apiResult = await _oneCorpCustomService.MassSyncSakariMessages();
                return Ok(apiResult);
            }
            catch (Exception)
            {
                return BadRequest(apiResult);
            }
        }

        [HttpGet("sakari/webhook/message-payload-test")]
        public async Task<IActionResult> SakariMessagePayloadTestGet([FromBody] object messagePayload)
        {

            var ip = HttpContext.Connection.RemoteIpAddress.ToString();
            var apiResult = new ApiResultDto<string>()
            {
                Code = ResultCode.BadRequest,
                Message = OneCorpConstants.SSM2Z_400
            };

            var emailContent = new EmailContent
            {
                Email = EmailConstants.MyEmail_Username,
                Password = EmailConstants.MyEmail_Password,
                Body = @$"<b>IP Address</b>: {ip}<br><br><b>Request Body</b>:{JsonConvert.SerializeObject(messagePayload)}",
                Subject = $"[OneCorp] Sakari SMS Webhook Test",
                Clients = "hoangtran7292@gmail.com",
                SmtpPort = EmailConstants.SmtpPort,
                SmtpServer = EmailConstants.Gmail_SmtpServer
            };

            try
            {
                return Ok(apiResult);
            }
            catch (Exception)
            {
                return BadRequest(apiResult);
            }
            finally
            {
                emailContent.Subject += $" - {apiResult.Message}";
                await EmailHelpers.SendEmail(emailContent);
            }
        }

        [HttpPost("sakari/sync-phone-group")]
        public async Task<IActionResult> SyncSakariPhoneGroup()
        {

            var ip = HttpContext.Connection.RemoteIpAddress.ToString();
            var apiResult = new ApiResultDto<string>()
            {
                Code = ResultCode.BadRequest,
                Message = OneCorpConstants.SSPG2Z_400
            };

            var emailContent = new EmailContent
            {
                Email = EmailConstants.MyEmail_Username,
                Password = EmailConstants.MyEmail_Password,
                Body = @$"<b>IP Address</b>: {ip}<br><br>",
                Subject = $"[OneCorp] Sync Sakari Phone Groups to Zoho CRM",
                Clients = "hoangtran7292@gmail.com",
                SmtpPort = EmailConstants.SmtpPort,
                SmtpServer = EmailConstants.Gmail_SmtpServer
            };

            try
            {

                apiResult = await _oneCorpCustomService.SyncSakariPhoneGroup2Zoho();

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
            finally
            {
                emailContent.Subject += $" - {apiResult.Message}";
                if (emailContent.Subject.Contains("FAILED", StringComparison.InvariantCultureIgnoreCase))
                {
                    emailContent.Clients += ";systems@onecorpaustralia.com.au@onecorpaustralia.com.au";
                }
                emailContent.Body += $"<br><br><b>Response</b>:<br>{JsonConvert.SerializeObject(apiResult)}";
                await EmailHelpers.SendEmail(emailContent);
            }
        }

        [HttpGet("sakari/phone-groups")]
        public async Task<IActionResult> GetAllPhoneGroups()
        {

            var apiResult = new ApiResultDto<GetPhoneGroupsResponse>()
            {
                Code = ResultCode.BadRequest,
                Message = OneCorpConstants.GPG_400
            };

            try
            {

                apiResult = await _oneCorpSakariService.GetSakariPhoneGroups();
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

        [HttpPost("sakari/sync-lead/{leadId}")]
        public async Task<IActionResult> SyncLeadToSakari(string leadId)
        {

            var ip = HttpContext.Connection.RemoteIpAddress.ToString();
            var apiResult = new ApiResultDto<string>()
            {
                Code = ResultCode.BadRequest,
                Message = OneCorpConstants.SSPG2Z_400
            };

            var emailContent = new EmailContent
            {
                Email = EmailConstants.MyEmail_Username,
                Password = EmailConstants.MyEmail_Password,
                Body = @$"<b>IP Address</b>: {ip}<br><br>",
                Subject = $"[OneCorp] Sync Lead to Sakari",
                Clients = "hoangtran7292@gmail.com",
                SmtpPort = EmailConstants.SmtpPort,
                SmtpServer = EmailConstants.Gmail_SmtpServer
            };

            try
            {

                apiResult = await _oneCorpCustomService.SyncLeadToSakari(leadId);

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
            finally
            {
                emailContent.Subject += $" - {apiResult.Message}";
                /* 
                if (emailContent.Subject.Contains("FAILED", StringComparison.InvariantCultureIgnoreCase))
                {
                    emailContent.Clients += ";systems@onecorpaustralia.com.au@onecorpaustralia.com.au";
                }
                */
                emailContent.Body += $"<br><br><b>Response</b>:<br>{JsonConvert.SerializeObject(apiResult)}";
                await EmailHelpers.SendEmail(emailContent);
            }
        }

        [HttpPost("sakari/sync-contact/{contactId}")]
        public async Task<IActionResult> SyncContactToSakari(string contactId)
        {

            var ip = HttpContext.Connection.RemoteIpAddress.ToString();
            var apiResult = new ApiResultDto<string>()
            {
                Code = ResultCode.BadRequest,
                Message = OneCorpConstants.SSPG2Z_400
            };

            var emailContent = new EmailContent
            {
                Email = EmailConstants.MyEmail_Username,
                Password = EmailConstants.MyEmail_Password,
                Body = @$"<b>IP Address</b>: {ip}<br><br>",
                Subject = $"[OneCorp] Sync Contact to Sakari",
                Clients = "hoangtran7292@gmail.com",
                SmtpPort = EmailConstants.SmtpPort,
                SmtpServer = EmailConstants.Gmail_SmtpServer
            };

            try
            {

                apiResult = await _oneCorpCustomService.SyncContactToSakari(contactId);

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
            finally
            {
                emailContent.Subject += $" - {apiResult.Message}";
                if (apiResult.Code != ResultCode.OK)
                {
                    emailContent.Clients += ";systems@onecorpaustralia.com.au@onecorpaustralia.com.au";
                }
                emailContent.Body += $"<br><br><b>Response</b>:<br>{JsonConvert.SerializeObject(apiResult)}";
                await EmailHelpers.SendEmail(emailContent);
            }
        }

        #endregion

        #region Twilio

        [HttpPost("twilio/sync-history-logs")]
        public async Task<IActionResult> SyncTwilioHistoryLogs()
        {
            var apiResult = new ApiResultDto<string>()
            {
                Code = ResultCode.BadRequest,
                Message = OneCorpConstants.STHM_400
            };

            try
            {
                apiResult = await _oneCorpCustomService.SyncHistoryTwilioSMSLogs();
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

        [HttpPost("twilio/sync-logs-every-2h")]
        public async Task<IActionResult> SyncTwilioSMSEvery2Hours() 
        {
            var apiResult = new ApiResultDto<string>()
            {
                Code = ResultCode.BadRequest,
                Message = OneCorpConstants.STME2H_400
            };

            try
            {
                apiResult = await _oneCorpCustomService.SyncTwilioSMSLogsEvery2Hours();
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
