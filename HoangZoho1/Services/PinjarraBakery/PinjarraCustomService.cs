using HoangZoho1.Constants;
using HoangZoho1.Helpers;
using HoangZoho1.Models.Common;
using HoangZoho1.Models.PinjarraBakery.ZohoCRM;
using HoangZoho1.Models.PinjarraBakery.ZohoForm;
using HoangZoho1.Models.PinjarraBakery.ZohoProjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace HoangZoho1.Services.PinjarraBakery
{
    public class PinjarraCustomService : IPinjarraCustomService
    {

        private readonly IPinjarraProjectService _pinjarraProjectService;
        private readonly IPinjarraCrmService _pinjarraCrmService;

        public PinjarraCustomService(IPinjarraProjectService pinjarraProjectService, IPinjarraCrmService pinjarraCrmService)
        {
            _pinjarraProjectService = pinjarraProjectService;
            _pinjarraCrmService = pinjarraCrmService;
        }

        #region Recruitment Process

        public async Task<ApiResultDto<string>> Recruitment_InitializeTasks(string projectId)
        {
            var apiResult = new ApiResultDto<string>()
            {
                Code = ResultCode.BadRequest,
                Message = PinjarraBakeryConstants.IRT_400
            };
            try
            {
                // Step 1: Get Project Details to get Contact Id
                var getProjectDetailsResult = await _pinjarraProjectService.GetProjectDetails(projectId);
                if (getProjectDetailsResult.Code != ResultCode.OK)
                {
                    apiResult.Message = PinjarraBakeryConstants.IRT_GetProjectById_400;
                    return apiResult;
                }
                var project = getProjectDetailsResult.Data.projects[0];
                var customFields = project.custom_fields;

                string contactUrl = string.Empty;
                string firstName = string.Empty;
                foreach (var customField in customFields)
                {
                    if (!string.IsNullOrEmpty(customField.ContactURL))
                    {
                        contactUrl = customField.ContactURL;
                    }
                    else if (!string.IsNullOrEmpty(customField.FirstName))
                    {
                        firstName = customField.FirstName;
                    }
                }
                if (string.IsNullOrEmpty(contactUrl))
                {
                    apiResult.Message = PinjarraBakeryConstants.IRT_ContactUrlEmpty;
                    return apiResult;
                }
                var contactSplits = contactUrl.Split('/');
                string contactId = contactSplits[contactSplits.Length - 1];
                string bookingEmail = PinjarraBakeryConstants.BookingEmailTemplate.Replace("{FirstName}", firstName);
                string refusalEmail = PinjarraBakeryConstants.RefusalEmailTemplate.Replace("{FirstName}", firstName);

                // STEP 2: Get R04 Id
                var getR04Result = await _pinjarraCrmService.GetContactRelatedR04s(contactId);
                string r04Id = string.Empty;
                if (getR04Result.Code == ResultCode.OK)
                {
                    var getR04Response = getR04Result.Data;
                    var r04Details = getR04Response.data.OrderByDescending(r04 => r04.Created_Time).FirstOrDefault();
                    if (r04Details != null)
                    {
                        r04Id = r04Details.id;
                    }
                }

                var getAllTasksResult = await _pinjarraProjectService.GetAllTasksInProject(projectId);
                if (getAllTasksResult.Code != ResultCode.OK)
                {
                    apiResult.Message = PinjarraBakeryConstants.IRT_GetAllTasks_400;
                    return apiResult;
                }
                var allTasks = getAllTasksResult.Data.tasks;

                foreach (var task in allTasks)
                {
                    string taskName = task.name;
                    string tasklistName = task.tasklist?.name;
                    if (tasklistName == "Step 2" && taskName == "Follow up")
                    {
                        string taskId = task.id.ToString();
                        string formR07Url = $"{PinjarraBakeryConstants.FormR07URL}?contactid={contactId}&projectid={projectId}&taskid={taskId}";
                        string description = PinjarraBakeryConstants.Step2_FollowUp
                            .Replace("https://docs.google.com/document/d/1yrkZSjfrowetNjwIfYW9Vjd_GnACKKpbrGhtWfoufuY/edit?usp=sharing", formR07Url);
                        
                        // Update Email Content
                        var updateTaskRequest = new TaskForUpdation()
                        {
                            Description = description,
                            EmailSubject = PinjarraBakeryConstants.BookingEmailSubject,
                            EmailContent = bookingEmail
                        };
                        var updateResult = await _pinjarraProjectService.UpdateTask(projectId, taskId, updateTaskRequest);
                    }
                    else if (tasklistName == "Step 2" && taskName == "FIRST Informal Meeting")
                    {
                        string taskId = task.id.ToString();
                        string formR07AUrl = $"{PinjarraBakeryConstants.FormR07AURL}?contactid={contactId}&projectid={projectId}&taskid={taskId}";
                        string description = PinjarraBakeryConstants.Step2_FirstInformalMeeting
                            .Replace("https://docs.google.com/document/d/1IbVInPPZr3a-cnPn7UXkS5V0izlhhpeOF6VwgP3USc8/edit?usp=sharing", formR07AUrl);
                        if (!string.IsNullOrEmpty(r04Id))
                        {
                            string formR04Url = $"{PinjarraBakeryConstants.PrefixCrmR04Url}/{r04Id}";
                            description = description
                                .Replace("https://docs.google.com/document/d/1kaeuWfTw_CmmMDJqqKWJZOwStq8qAjry5uFCFln0g8Y/edit?usp=sharing", formR04Url);
                        }
                        var updateTaskRequest = new TaskForUpdation()
                        {
                            Description = description
                        };
                        var updateResult = await _pinjarraProjectService.UpdateTask(projectId, taskId, updateTaskRequest);
                    }
                    else if (tasklistName == "Step 3" && taskName == "Initial Assessment of Franchisee Suitability")
                    {
                        string taskId = task.id.ToString();
                        string formR11Url = $"{PinjarraBakeryConstants.FormR11URL}?contactid={contactId}&projectid={projectId}&taskid={taskId}";
                        string description = PinjarraBakeryConstants.Step3_InitialAssessment
                            .Replace("https://docs.google.com/document/d/1VRM6l7b71XAZi1NV4Q15UKgK1O-LnDLkfa3-_3zGRzs/edit?usp=sharing", formR11Url);
                        var updateTaskRequest = new TaskForUpdation()
                        {
                            Description = description
                        };
                        var updateResult = await _pinjarraProjectService.UpdateTask(projectId, taskId, updateTaskRequest);
                    }
                    else if (tasklistName == "Step 3" && taskName == "If Deemed UNSUITABLE: Send Refusal Letter #1")
                    {
                        string taskId = task.id.ToString();
                        var updateTaskRequest = new TaskForUpdation()
                        {
                            EmailSubject = PinjarraBakeryConstants.RefusalEmailSubject,
                            EmailContent = refusalEmail
                        };
                        var updateResult = await _pinjarraProjectService.UpdateTask(projectId, taskId, updateTaskRequest);
                    }
                    else if (tasklistName == "Step 3" && taskName == "If Deemed SUITABLE: SECOND Informal Meeting")
                    {
                        string taskId = task.id.ToString();
                        string formR08Url = $"{PinjarraBakeryConstants.FormR08URL}?contactid={contactId}&projectid={projectId}&taskid={taskId}";
                        string description = PinjarraBakeryConstants.Step3_IfSuitable
                            .Replace("https://docs.google.com/document/d/1y3LmzlE936KTVWlTXzBOlnv6WNWN5li7Uw0YrLX-Rv4/edit?usp=sharing", formR08Url);
                        var updateTaskRequest = new TaskForUpdation()
                        {
                            Description = description
                        };
                        var updateResult = await _pinjarraProjectService.UpdateTask(projectId, taskId, updateTaskRequest);
                    }
                    else if (tasklistName == "Step 4" && taskName == "Assessment of Franchisee Suitability")
                    {
                        string taskId = task.id.ToString();
                        string formR05AUrl = $"{PinjarraBakeryConstants.FormR05AURL}?contactid={contactId}&projectid={projectId}&taskid={taskId}";
                        string formR11Url = $"{PinjarraBakeryConstants.FormR11URL}?contactid={contactId}&projectid={projectId}&taskid={taskId}";
                        string description = PinjarraBakeryConstants.Step4_Assessment
                            .Replace("https://docs.google.com/document/d/1i9UgJ19vAv2wEYQqTe9jKXFgzSJZ-pozo5_DU7vctE4/edit?usp=sharing", formR05AUrl)
                            .Replace("https://docs.google.com/document/d/1VRM6l7b71XAZi1NV4Q15UKgK1O-LnDLkfa3-_3zGRzs/edit?usp=sharing", formR11Url);
                        var updateTaskRequest = new TaskForUpdation()
                        {
                            Description = description
                        };
                        var updateResult = await _pinjarraProjectService.UpdateTask(projectId, taskId, updateTaskRequest);
                    }
                    else if (tasklistName == "Step 4" && taskName == "If Deemed UNSUITABLE: Send Refusal Letter #2")
                    {
                        string taskId = task.id.ToString();
                        var updateTaskRequest = new TaskForUpdation()
                        {
                            EmailSubject = PinjarraBakeryConstants.RefusalEmailSubject,
                            EmailContent = refusalEmail
                        };
                        var updateResult = await _pinjarraProjectService.UpdateTask(projectId, taskId, updateTaskRequest);
                    }
                    else if (tasklistName == "Step 5" && taskName == "If Deemed Suitable: FIRST FORMAL Meeting")
                    {
                        string taskId = task.id.ToString();
                        string formR09Url = $"{PinjarraBakeryConstants.FormR09URL}?contactid={contactId}&projectid={projectId}&taskid={taskId}";
                        string description = PinjarraBakeryConstants.Step5_IfSuitable
                            .Replace("https://docs.google.com/document/d/136qkc_degQKbBGYPLa_NN2QZwCDRbIjcwz5vZtWTXoI/edit?usp=sharing", formR09Url);
                        var updateTaskRequest = new TaskForUpdation()
                        {
                            Description = description
                        };
                        var updateResult = await _pinjarraProjectService.UpdateTask(projectId, taskId, updateTaskRequest);
                    }
                    else if (tasklistName == "Step 6" && taskName == "Follow Up")
                    {
                        string taskId = task.id.ToString();
                        string formR10Url = $"{PinjarraBakeryConstants.FormR10URL}?contactid={contactId}&projectid={projectId}&taskid={taskId}";
                        string description = PinjarraBakeryConstants.Step6_FollowUp
                            .Replace("https://docs.google.com/document/d/1V0IGCjqcCXCJDRcuZUhFNL2bXl4oSrFvkZPQuM9epzo/edit?usp=sharing", formR10Url);
                        var updateTaskRequest = new TaskForUpdation()
                        {
                            Description = description
                        };
                        var updateResult = await _pinjarraProjectService.UpdateTask(projectId, taskId, updateTaskRequest);
                    }
                    else if (tasklistName == "Step 6" && taskName == "Receive Application Form")
                    {
                        string taskId = task.id.ToString();
                        string formR13Url = $"{PinjarraBakeryConstants.FormR13URL}?contactid={contactId}&projectid={projectId}&taskid={taskId}";
                        string formR12Url = $"{PinjarraBakeryConstants.FormR12URL}?contactid={contactId}&projectid={projectId}&taskid={taskId}";
                        string formR05AUrl = $"{PinjarraBakeryConstants.FormR05AURL}?contactid={contactId}&projectid={projectId}&taskid={taskId}";
                        string formR11Url = $"{PinjarraBakeryConstants.FormR11URL}?contactid={contactId}&projectid={projectId}&taskid={taskId}";
                        string description = PinjarraBakeryConstants.Step6_ReceiveApplicationForm
                            .Replace("https://docs.google.com/document/d/1SlJ9VzPj86fVNuoZiFJ5DWvN1uJIxb9M1Bikl2F-YWk/edit?usp=sharing", formR13Url)
                            .Replace("https://docs.google.com/document/d/1wCdOBVM9ySQLjuD_qz8MQDyKBApkr-j5J9Kpi8kYFLw/edit?usp=sharing", formR12Url)
                            .Replace("https://docs.google.com/document/d/1i9UgJ19vAv2wEYQqTe9jKXFgzSJZ-pozo5_DU7vctE4/edit?usp=sharing", formR05AUrl)
                            .Replace("https://docs.google.com/document/d/1VRM6l7b71XAZi1NV4Q15UKgK1O-LnDLkfa3-_3zGRzs/edit?usp=sharing", formR11Url);
                        var updateTaskRequest = new TaskForUpdation()
                        {
                            Description = description
                        };
                        var updateResult = await _pinjarraProjectService.UpdateTask(projectId, taskId, updateTaskRequest);
                    }
                    else if (tasklistName == "Step 7" && taskName == "Applicant Chooses NOT TO PROCEED")
                    {
                        string taskId = task.id.ToString();
                        string formR23Url = $"{PinjarraBakeryConstants.FormR23URL}?contactid={contactId}&projectid={projectId}&taskid={taskId}";
                        string description = PinjarraBakeryConstants.Step7_Not2Proceed
                            .Replace("https://docs.google.com/document/d/1nqXevepwdebkP2cymFpWKIYy3f-aNr1SlMNQGJSxNrk/edit?usp=sharing", formR23Url);
                        var updateTaskRequest = new TaskForUpdation()
                        {
                            Description = description
                        };
                        var updateResult = await _pinjarraProjectService.UpdateTask(projectId, taskId, updateTaskRequest);
                    }
                    else if (tasklistName == "Step 7" && taskName == "Applicant chooses TO PROCEED: Second Formal Meeting")
                    {
                        string taskId = task.id.ToString();
                        string formR09AUrl = $"{PinjarraBakeryConstants.FormR09AURL}?contactid={contactId}&projectid={projectId}&taskid={taskId}";
                        string formR11Url = $"{PinjarraBakeryConstants.FormR11URL}?contactid={contactId}&projectid={projectId}&taskid={taskId}";
                        string formR14AUrl = $"{PinjarraBakeryConstants.FormR14AURL}?contactid={contactId}&projectid={projectId}&taskid={taskId}";
                        string formR21Url = $"{PinjarraBakeryConstants.FormR21URL}?contactid={contactId}&projectid={projectId}&taskid={taskId}";
                        string description = PinjarraBakeryConstants.Step7_Proceed
                            .Replace("https://docs.google.com/document/d/1X7GXmnIM82spjSijNO_UWHYzGdKbimxrQRStqK63Pfc/edit?usp=sharing", formR09AUrl)
                            .Replace("https://docs.google.com/document/d/1VRM6l7b71XAZi1NV4Q15UKgK1O-LnDLkfa3-_3zGRzs/edit?usp=sharing", formR11Url)
                            .Replace("https://docs.google.com/document/d/13hJMG75x3wpdb2WUYyL0bYFKTlB8_LjMvr6lEV9bW9s", formR14AUrl)
                            .Replace("https://docs.google.com/document/d/1f92k5CAqhy87SS_BFR6oxm0Swd7CHnt_XkS-Wz__5wM/edit?usp=sharing", formR21Url);
                        var updateTaskRequest = new TaskForUpdation()
                        {
                            Description = description
                        };
                        var updateResult = await _pinjarraProjectService.UpdateTask(projectId, taskId, updateTaskRequest);
                    }
                    else if (tasklistName == "Step 9" && taskName == "Provide the Final set of Documents to the Applicant")
                    {
                        string taskId = task.id.ToString();
                        string formR15Url = $"{PinjarraBakeryConstants.FormR15URL}?contactid={contactId}&projectid={projectId}&taskid={taskId}";
                        string formR20Url = $"{PinjarraBakeryConstants.FormR20URL}?contactid={contactId}&projectid={projectId}&taskid={taskId}";
                        string formR21Url = $"{PinjarraBakeryConstants.FormR21URL}?contactid={contactId}&projectid={projectId}&taskid={taskId}";

                        string description = PinjarraBakeryConstants.Step9_ProvideFinalSetOfDocuments
                            .Replace("https://docs.google.com/document/d/1O-bekXryUU5XZJFaGaQvrX2zOjwIDY9CqnCit1OdAYY/edit?usp=sharing", formR15Url)
                            .Replace("https://docs.google.com/document/d/1zH-cSBiAG8ruSMUziBWdFlhuOaPSnhNZKeNYO4JYgLM/edit?usp=sharing", formR20Url)
                            .Replace("https://docs.google.com/document/d/1f92k5CAqhy87SS_BFR6oxm0Swd7CHnt_XkS-Wz__5wM/edit?usp=sharing", formR21Url);
                        var updateTaskRequest = new TaskForUpdation()
                        {
                            Description = description
                        };
                        var updateResult = await _pinjarraProjectService.UpdateTask(projectId, taskId, updateTaskRequest);
                    }
                }
                apiResult.Code = ResultCode.OK;
                apiResult.Message = PinjarraBakeryConstants.IRT_200;
                return apiResult;
            }
            catch (Exception ex)
            {
                apiResult.Message = PinjarraBakeryConstants.IRT_400;
                apiResult.Data = ex.Message + " - " + ex.StackTrace;
                return apiResult;
            }
        }

        public async Task<ApiResultDto<string>> Recruitment_SendEmail(string projectId, string taskId)
        {
            var apiResult = new ApiResultDto<string>()
            {
                Code = ResultCode.BadRequest,
                Message = PinjarraBakeryConstants.SE_400
            };

            try
            {
                // Step 1: Get Project Details to get Applicant First Name and Email
                var getProjectDetailsResult = await _pinjarraProjectService.GetProjectDetails(projectId);
                if (getProjectDetailsResult.Code != ResultCode.OK)
                {
                    apiResult.Message = PinjarraBakeryConstants.SE_GetProjectById_400;
                    return apiResult;
                }
                var project = getProjectDetailsResult.Data.projects[0];
                var customFields = project.custom_fields;

                string firstName = string.Empty;
                string email = string.Empty;

                foreach (var customField in customFields)
                {
                    if (!string.IsNullOrEmpty(customField.FirstName))
                    {
                        firstName = customField.FirstName;
                    }
                    else if (!string.IsNullOrEmpty(customField.EmailAddress))
                    {
                        email = customField.EmailAddress;
                    }
                }

                // Step 2: Get Task Details to get Email Content
                var getTaskByIdResult = await _pinjarraProjectService.GetTaskById(projectId, taskId);
                if (getTaskByIdResult.Code != ResultCode.OK)
                {
                    apiResult.Message = PinjarraBakeryConstants.SE_GetTaskById_400;
                    return apiResult;
                }
                var taskDetails = getTaskByIdResult.Data.tasks.FirstOrDefault();
                var taskCustomFields = taskDetails.custom_fields;
                string emailBody = string.Empty;
                string emailSubject = string.Empty;
                foreach (var field in taskCustomFields)
                {
                    string columnName = field.column_name;
                    if (columnName == "UDF_TEXT1")
                    {
                        emailBody = field.value;
                    }
                    else if (columnName == "UDF_CHAR1")
                    {
                        emailSubject = field.value;
                    }
                }
                if (string.IsNullOrEmpty(emailBody))
                {
                    apiResult.Message = PinjarraBakeryConstants.SE_EmailContentEmpty_400;
                    return apiResult;
                }
                emailBody = emailBody.Replace("\n", "<br>");
                string finalEmailBody = PinjarraBakeryConstants.PinjarraEmailTemplate
                    .Replace("{EmailContent}", emailBody);

                // Step 3: Send Email to Applicant
                var emailContent = new EmailContent()
                {
                    SenderName = PinjarraBakeryConstants.PBFranchisingName,
                    Subject = emailSubject,
                    Body = finalEmailBody,
                    Clients = email,
                    Email = PinjarraBakeryConstants.Franchisee_Username,
                    Password = PinjarraBakeryConstants.Franchisee_Password,
                    SmtpPort = CommonConstants.SmtpPort,
                    SmtpServer = CommonConstants.Gmail_SmtpServer,
                };
                await EmailHelpers.SendEmail(emailContent);
                apiResult.Code = ResultCode.OK;
                apiResult.Message = PinjarraBakeryConstants.SE_200;
                return apiResult;
            }
            catch (Exception ex)
            {
                apiResult.Message = PinjarraBakeryConstants.SE_400;
                apiResult.Data = ex.Message + " - " + ex.StackTrace;
                return apiResult;
            }
        }

        public async Task<ApiResultDto<string>> Recruitment_SendR04NotiEmail(R04NotiEmailRequest r04Request)
        {
            var apiResult = new ApiResultDto<string>()
            {
                Code = ResultCode.BadRequest,
                Message = PinjarraBakeryConstants.R04_400
            };

            try
            {
                string emailSubject = PinjarraBakeryConstants.R04NotiEmailSubject
                    .Replace("{FullName}", r04Request.FullName);

                string emailBody = PinjarraBakeryConstants.R04NotiEmailTemplate
                    .Replace("{FullName}", r04Request.FullName)
                    .Replace("{Email}", r04Request.Email)
                    .Replace("{Mobile}", r04Request.Mobile)
                    .Replace("{ContactURL}", $"{PinjarraBakeryConstants.PrefixContactEndpoint}/{r04Request.ContactId}")
                    .Replace("{R04URL}", $"{PinjarraBakeryConstants.PrefixR04Endpoint}/{r04Request.R04Id}");

                string finalEmailBody = PinjarraBakeryConstants.PinjarraEmailTemplate
                    .Replace("{EmailContent}", emailBody);

                var emailContent = new EmailContent()
                {
                    SenderName = PinjarraBakeryConstants.PBFranchisingName,
                    Subject = emailSubject,
                    Body = finalEmailBody,
                    Clients = PinjarraBakeryConstants.DanEmail,
                    Email = PinjarraBakeryConstants.Franchisee_Username,
                    Password = PinjarraBakeryConstants.Franchisee_Password,
                    SmtpPort = CommonConstants.SmtpPort,
                    SmtpServer = CommonConstants.Gmail_SmtpServer,
                };

                await EmailHelpers.SendEmail(emailContent);
                apiResult.Code = ResultCode.OK;
                apiResult.Message = PinjarraBakeryConstants.R04_200;
                return apiResult;
            }
            catch (Exception ex)
            {
                apiResult.Message = PinjarraBakeryConstants.R04_400;
                apiResult.Data = $"{ex.Message} - {ex.StackTrace}";
                return apiResult;
            }
        }

        #endregion

        #region Bakehouse Product Logs

        public async Task<ApiResultDto<string>> HandleBakehouseProductLog(HandleProductLogRequest productLogRequest)
        {

            var apiResult = new ApiResultDto<string>()
            {
                Code = ResultCode.BadRequest,
                Message = PinjarraBakeryConstants.BPL_400
            };

            try
            {
                // STEP 1: Search Product Log Request
                string productDate = productLogRequest.ProductDate;
                string newProductDate = productDate.Replace("-", " ");
                string crmProductDate = DateTimeHelpers.ConvertFormDateToCrmDate(productDate);
                string name = productLogRequest.Name;
                string keyStep1 = productLogRequest.KeyStep1;
                string keyStep2 = productLogRequest.KeyStep2;
                string keyStep3 = productLogRequest.KeyStep3;
                string keyStep4 = productLogRequest.KeyStep4;
                string searchCriteria = $"((Name1:equals:{HttpUtility.UrlEncode(name)})and(Product_Date:equals:{HttpUtility.UrlEncode(crmProductDate)}))";
                var searchProductLogResult = await _pinjarraCrmService.SearchBakeHouseProductLogs(searchCriteria);

                if (searchProductLogResult.Code == ResultCode.BadRequest)
                {
                    apiResult.Message = PinjarraBakeryConstants.BPL_SearchProductLog_400;
                    return apiResult;
                }

                var allKeySteps = new HashSet<string>();
                if (!string.IsNullOrEmpty(keyStep1))
                {
                    allKeySteps.Add(keyStep1);
                }
                if (!string.IsNullOrEmpty(keyStep2))
                {
                    allKeySteps.Add(keyStep2);
                }
                if (!string.IsNullOrEmpty(keyStep3))
                {
                    allKeySteps.Add(keyStep3);
                }
                if (!string.IsNullOrEmpty(keyStep4))
                {
                    allKeySteps.Add(keyStep4);
                }

                string keySteps = string.Empty;
                foreach (var keyStep in allKeySteps)
                {
                    if (string.IsNullOrEmpty(keySteps))
                    {
                        keySteps = $"- {keyStep}";
                    }
                    else
                    {
                        keySteps += $"\n- {keyStep}";
                    }
                }

                // STEP 2: Get Product Log by Id to get Subform Data
                if (searchProductLogResult.Code == ResultCode.NoContent)
                {
                    // STEP 3: Create Product Log Record
                    var productLogs = new List<ProductLog>();

                    var productLog = new ProductLog()
                    {
                        Department = productLogRequest.Department,
                        Comments = productLogRequest.Comment,
                        Key_Steps = keySteps,
                        Product = productLogRequest.Product,
                        Time_minute = Convert.ToDecimal(productLogRequest.Time),
                        Yield = Convert.ToDecimal(productLogRequest.Yield)
                    };
                    productLogs.Add(productLog);

                    var productLogForCreation = new ProductLogForCreation()
                    {
                        Name = $"Log-{name}-{newProductDate}",
                        Name1 = name,
                        Product_Date = crmProductDate,
                        Product_Logs = productLogs
                    };

                    var createProductLogRequest = new UpsertRequest<ProductLogForCreation>();
                    createProductLogRequest.data.Add(productLogForCreation);
                    var createProductLogResponse = await _pinjarraCrmService.CreateProductLog(createProductLogRequest);
                    if (createProductLogResponse.Code != ResultCode.OK)
                    {
                        apiResult.Message = PinjarraBakeryConstants.BPL_CreateProductLog_400;
                        return apiResult;
                    }
                }
                else
                {
                    // STEP 4: Update Product Record
                    // Step 4.1: Get Product Log Id
                    var productLogData = searchProductLogResult.Data;
                    string productLogId = productLogData.data.FirstOrDefault().id;

                    // Step 4.2: Update Product Log
                    var productLogs = new List<ProductLog>();
                    var productLog = new ProductLog()
                    {
                        Department = productLogRequest.Department,
                        Comments = productLogRequest.Comment,
                        Key_Steps = keySteps,
                        Product = productLogRequest.Product,
                        Time_minute = Convert.ToDecimal(productLogRequest.Time),
                        Yield = Convert.ToDecimal(productLogRequest.Yield)
                    };
                    productLogs.Add(productLog);

                    var productLogForUpdation = new ProductLogForUpdation()
                    {
                        Product_Logs = productLogs
                    };

                    var updateProductLogRequest = new UpsertRequest<ProductLogForUpdation>();
                    updateProductLogRequest.data.Add(productLogForUpdation);
                    var updateProductLogResponse = await _pinjarraCrmService.UpdateProductLog(productLogId, updateProductLogRequest);
                    if (updateProductLogResponse.Code != ResultCode.OK)
                    {
                        apiResult.Message = PinjarraBakeryConstants.BPL_UpdateProductLog_400;
                        return apiResult;
                    }
                }

                apiResult.Code = ResultCode.OK;
                apiResult.Message = PinjarraBakeryConstants.BPL_200;
                return apiResult;
            
            }
            catch (Exception ex)
            {
                apiResult.Data = $"{ex.Message + " - " + ex.StackTrace}";
                return apiResult;
            }
        }

        #endregion

    }
}
