using HoangZoho1.Constants;
using HoangZoho1.Helpers;
using HoangZoho1.Models.Common;
using HoangZoho1.Models.Getunik.Custom;
using HoangZoho1.Models.Getunik.ZohoProjects;
using HoangZoho1.Services.Getunik;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace HoangZoho1.Services.GetUnik
{

    public class GetunikCustomService : IGetunikCustomService
    {

        private readonly IGetunikCrmService _getUnikCrmService;
        private readonly IGetunikBooksService _getUnikBooksService;
        private readonly IGetunikProjectsService _getunikProjectsService;

        public GetunikCustomService(IGetunikCrmService getUnikCrmService,
            IGetunikBooksService getUnikBooksService,
            IGetunikProjectsService getunikProjectsService)
        {
            _getUnikCrmService = getUnikCrmService;
            _getUnikBooksService = getUnikBooksService;
            _getunikProjectsService = getunikProjectsService;
        }

        public async Task<ApiResultDto<string>> HandleCreateAndSubmitInvoice(string recordId)
        {

            var apiResult = new ApiResultDto<string>()
            {
                Code = ResultCode.BadRequest,
                Message = GetunikConstants.HCIAS_400
            };

            try
            {
                // STEP 1: Get Temporary Record by Id
                var getTemporaryRecordByIdResponse = 
                    await _getUnikCrmService.GetTemporaryRecordById(recordId);
                if (getTemporaryRecordByIdResponse.Code != ResultCode.OK)
                {
                    apiResult.Message = GetunikConstants.HCIAS_E01 + " - " + getTemporaryRecordByIdResponse.Message;
                    return apiResult;
                }    
                var temporaryRecordDetails = getTemporaryRecordByIdResponse.Data.data[0];
                string payload = temporaryRecordDetails.Payload;
                string payload2 = temporaryRecordDetails.Payload_2;

                string finalPayload = payload;
                if (!string.IsNullOrEmpty(payload2))
                {
                    finalPayload += payload2;
                }

                // STEP 2: Create Invoice using record Payload
                var createInvoiceResponse = 
                    await _getUnikBooksService.CreateInvoice(finalPayload);
                if (createInvoiceResponse.Code != ResultCode.OK)
                {
                    apiResult.Message = GetunikConstants.HCIAS_E02 + " - " + createInvoiceResponse.Message;
                    return apiResult;
                }
                var invoiceDetails = createInvoiceResponse.Data.invoice;
                string invoiceId = invoiceDetails.invoice_id;

                // STEP 3: Submit Invoice for Approval
                var submitInvoiceResponse = 
                    await _getUnikBooksService.SubmitInvoice(invoiceId);
                if (submitInvoiceResponse.Code != ResultCode.OK)
                {

                    // Pause 5s and retry
                    Thread.Sleep(3000);
                    submitInvoiceResponse =
                        await _getUnikBooksService.SubmitInvoice(invoiceId);

                    if (submitInvoiceResponse.Code != ResultCode.OK)
                    {
                        Thread.Sleep(3000);
                        submitInvoiceResponse =
                        await _getUnikBooksService.SubmitInvoice(invoiceId);

                        if (submitInvoiceResponse.Code != ResultCode.OK)
                        {
                            apiResult.Message = GetunikConstants.HCIAS_E03 + " - " + submitInvoiceResponse.Message;
                            return apiResult;
                        }
                        
                    }
                    
                }

                // STEP 4: Update Temporary Record Status
                var updateRecordRequest = new UpsertRequest<object>();
                updateRecordRequest.data.Add(
                    new
                    {
                        Temporary_Record_Status = "Completed"
                    });
                string updateRecordRequestStr = JsonConvert.SerializeObject(updateRecordRequest);
                var updateTemporaryRecordResponse =
                    await _getUnikCrmService.UpdateTemporaryRecord(recordId, updateRecordRequestStr);
                if (updateTemporaryRecordResponse.Code != ResultCode.OK)
                {
                    apiResult.Message = GetunikConstants.HCIAS_E03 + " - " + updateTemporaryRecordResponse.Message;
                    return apiResult;
                }

                apiResult.Code = ResultCode.OK;
                apiResult.Message = GetunikConstants.HCIAS_200;

                return apiResult;
            }
            catch (Exception ex)
            {
                var updateRecordRequest = new UpsertRequest<object>();
                updateRecordRequest.data.Add(
                    new
                    {
                        Temporary_Record_Status = "Failed"
                    });
                string updateRecordRequestStr = JsonConvert.SerializeObject(updateRecordRequest);
                var updateTemporaryRecordResponse =
                    await _getUnikCrmService.UpdateTemporaryRecord(recordId, updateRecordRequestStr);
                if (updateTemporaryRecordResponse.Code != ResultCode.OK)
                {
                    apiResult.Message = GetunikConstants.HCIAS_E03;
                    return apiResult;
                }
                apiResult.Message = $"{ex.Message} - {ex.StackTrace}";
                return apiResult;
            }


        }

        public async Task<ApiResultDto<GetProjectByIdResponse>> GetProjectDetailsFromUrl(string projectUrl)
        {

            var apiResult = new ApiResultDto<GetProjectByIdResponse>()
            {
                Code = ResultCode.BadRequest,
                Message = GetunikConstants.GPDFU_400
            };

            try
            {

                // STEP 1: Extract Project ID from URL
                string projectId = StringHelpers.ExtractProjectId(projectUrl);
                if (string.IsNullOrEmpty(projectId))
                {
                    apiResult.Message = GetunikConstants.GPDFU_E01;
                    return apiResult;
                }    

                // STEP 2: Get Project Details by ID
                var getProjectByIdResponse =
                    await _getunikProjectsService.GetProjectById(projectId);

                if (getProjectByIdResponse.Code != ResultCode.OK)
                {
                    apiResult.Message = GetunikConstants.GPDFU_E02 + " - " + getProjectByIdResponse.Message;
                    return apiResult;
                }

                apiResult.Code = ResultCode.OK;
                apiResult.Message = GetunikConstants.GPDFU_200;
                apiResult.Data = getProjectByIdResponse.Data;

                return apiResult;

            }
            catch (Exception ex)
            {

                apiResult.Message = $"{ex.Message} - {ex.StackTrace}";
                return apiResult;

            }

        }

        public async Task<ApiResultDto<string>> CreateBacklogTasks(CreateBacklogTasksRequest request)
        {

            var apiResult = new ApiResultDto<string>()
            {
                Code = ResultCode.BadRequest,
                Message = GetunikConstants.CBT_400
            };

            try
            {

                // STEP 1: Extract Request Body
                string projectId = request.projectId;
                string currentUserEmail = request.userEmail;
                var taskDetails = request.taskDetails;
                string productId = taskDetails.parentId;
                string productName = taskDetails.parent;
                var subtasks = taskDetails.subtasks;

                // STEP 2: Find Tasklist Backlog in Project
                var getProjectTasklistsResponse =
                    await _getunikProjectsService.GetProjectTasklists(projectId);

                if (getProjectTasklistsResponse.Code != ResultCode.OK)
                {
                    apiResult.Message = GetunikConstants.CBT_400 + " - " + getProjectTasklistsResponse.Message;
                    return apiResult;
                }

                var tasklists = getProjectTasklistsResponse.Data.tasklists;

                var backlogTasklist = tasklists.FirstOrDefault(t => t.name.ToLower().Trim() == "backlog");

                if (backlogTasklist == null)
                {
                    apiResult.Message = GetunikConstants.CBT_E02;
                    return apiResult;
                }
                string backlogTasklistId = backlogTasklist.id;

                // STEP 3: Get Project Details
                var getProjectbyIdResponse = await _getunikProjectsService.GetProjectById(projectId);
                if (getProjectbyIdResponse.Code != ResultCode.OK)
                {
                    apiResult.Message = GetunikConstants.CBT_E01 + " - " + getProjectbyIdResponse.Message;
                    return apiResult;
                }
                var projectDetails = getProjectbyIdResponse.Data;
                var projectBudgetInfo = projectDetails.budget_info;
                string projectCurrency = projectBudgetInfo.currency;

                // STEP 4: Get Project Users
                var getProjectUsersResponse = await _getunikProjectsService.GetProjectUsers();
                var projectUsers = getProjectUsersResponse.Data.users;
                string currentUserZuid = "";
                string currentUserZpuid = "";
                foreach (var user in projectUsers)
                {
                    string userZuid = user.zuid;
                    string userZpuid = user.id;
                    string userEmail = user.email;
                    if (currentUserEmail == userEmail)
                    {
                        currentUserZuid = userZuid;
                        currentUserZpuid = userZpuid;
                        break;
                    }
                }

                // STEP 5: Get Product Details from CRM
                var getProductByIdResponse = await _getUnikCrmService.GetProductById(productId);
                if (getProductByIdResponse.Code != ResultCode.OK)
                {
                    apiResult.Message = GetunikConstants.CBT_E03 + " - " + getProductByIdResponse.Message;
                    return apiResult;
                }

                var productDetails = getProductByIdResponse.Data.data[0];
                string description = productDetails.Description;
                string taskDescription = productDetails.Task_Description;
                if (!string.IsNullOrEmpty(taskDescription))
                {
                    description = taskDescription;
                }

                string accountNumber = string.Empty;
                string finalAccountNumber = string.Empty;
                if (projectCurrency == "CHF")
                {
                    accountNumber = productDetails.CH_Accounting_Number;
                }
                else
                {
                    accountNumber = productDetails.DE_Accounting_Number;
                }
                if (!string.IsNullOrEmpty(accountNumber))
                {
                    finalAccountNumber = StringHelpers.ExtractAccountNumber(accountNumber);
                }
                string circle = productDetails.Circle;

                // STEP 6: Create Task in Zoho Projects
                var createMainTaskRequest = new CreateTaskRequest();
                createMainTaskRequest.name = productName;
                var tasklist = new TasklistForCreation()
                {
                    id = long.Parse(backlogTasklistId)
                };
                createMainTaskRequest.tasklist = tasklist;
                createMainTaskRequest.description = description;
                createMainTaskRequest.priority = "medium";

                // Step 6.1: Handle Status
                var status = new Models.Getunik.ZohoProjects.TaskStatus()
                {
                    id = 1186031000000016068
                };
                createMainTaskRequest.status = status;

                // Step 6.2: Handle Current Date
                var getCurrentTimeRequest = new GetCurrentTimeRequest()
                {
                    Format = "yyyy-MM-dd'T'HH:mm:ss.fff'Z'",
                    TimeZone = "Central European Standard Time"
                };
                string currentTime = DateTimeHelpers.GetCurrentTime
                    (getCurrentTimeRequest);
                createMainTaskRequest.start_date = currentTime;
                createMainTaskRequest.billing_type = "billable";

                // Step 6.3: Handle Owners and Work
                var ownersAndWork = new OwnersAndWork()
                {
                    work_type = "standard",
                    unit = "hours_per_day",
                    copy_task_duration = false
                };

                var taskOwners = new List<TaskOwner>();
                var taskOwner = new TaskOwner()
                {
                    zpuid = currentUserZpuid,
                    work_values = "00:00"
                };
                taskOwners.Add(taskOwner);
                ownersAndWork.owners = taskOwners;

                createMainTaskRequest.owners_and_work = ownersAndWork;
                createMainTaskRequest.account_number = finalAccountNumber;
                createMainTaskRequest.billing_method = "Time and Materials";
                createMainTaskRequest.autocreated_from_crm = true;
                createMainTaskRequest.zcrm_product_id = productId;

                string createTaskRequestStr = JsonConvert.SerializeObject(createMainTaskRequest);

                var createMainTaskResponse = await _getunikProjectsService.CreateTask
                    (projectId, createMainTaskRequest);
                Thread.Sleep(1000);
                if (createMainTaskResponse.Code != ResultCode.OK)
                {
                    string errorMessage = GetunikConstants.CBT_E04.Replace("$TaskName$", productName);
                    apiResult.Message = errorMessage + " - " + createMainTaskResponse.Message;
                    return apiResult;
                }
                string mainTaskId = createMainTaskResponse.Data.id;

                // STEP 7: Create Subtasks
                var parentalInfo = new ParentalInfo()
                {
                    parent_task_id = long.Parse(mainTaskId)
                };
                foreach (string subtask in subtasks)
                {
                    var createSubtaskRequest = new CreateTaskRequest()
                    {
                        name = subtask,
                        tasklist = tasklist,
                        priority = "medium",
                        status = status,
                        start_date = currentTime,
                        billing_type = "billable",
                        owners_and_work = ownersAndWork,
                        account_number = finalAccountNumber,
                        billing_method = "Time and Materials",
                        autocreated_from_crm = true,
                        zcrm_product_id = productId
                    };
                    createSubtaskRequest.parental_info = parentalInfo;

                    var createSubtaskResponse = await _getunikProjectsService.CreateTask
                        (projectId, createSubtaskRequest);
                    Thread.Sleep(1000);

                    if (createSubtaskResponse.Code != ResultCode.OK)
                    {
                        string errorMessage = GetunikConstants.CBT_E05.Replace("{subtaskName}", subtask)
                            .Replace("$TaskName$", productName);

                        apiResult.Message = errorMessage + " - " + createSubtaskResponse.Message;
                        return apiResult;
                    }

                }

                apiResult.Code = ResultCode.OK;
                apiResult.Message = GetunikConstants.CBT_200;

                return apiResult;

            }
            catch (Exception ex)
            {

                apiResult.Message = $"{ex.Message} - {ex.StackTrace}";
                return apiResult;

            }
        }

    }

}
