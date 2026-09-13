using HoangZoho1.Constants;
using HoangZoho1.Helpers;
using HoangZoho1.Models.Common;
using HoangZoho1.Models.Oratto.Custom;
using HoangZoho1.Models.Oratto.Gemini;
using HoangZoho1.Models.Oratto.OpenAI;
using HoangZoho1.Models.Oratto.ZohoCRM;
using HoangZoho1.Models.Oratto.ZohoMail;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace HoangZoho1.Services.Oratto
{
    public class OrattoCustomService : IOrattoCustomService
    {

        private readonly IOrattoCrmService _crmService;
        private readonly IOrattoMailService _mailService;
        private readonly IOrattoOpenAIService _aiService;
        private readonly IOrattoGeminiService _geminiService;
        private readonly IOrattoDocumentService _documentService;

        public OrattoCustomService(IOrattoCrmService crmService, IOrattoMailService mailService, IOrattoOpenAIService aiService, IOrattoGeminiService geminiService, IOrattoDocumentService documentService)
        {
            _crmService = crmService;
            _mailService = mailService;
            _aiService = aiService;
            _geminiService = geminiService;
            _documentService = documentService;
        }

        public async Task<ApiResultDto<string>> AssignSolicitorsToLead(string leadId, string solicitorIds)
        {
            var apiResult = new ApiResultDto<string>
            {
                Code = ResultCode.BadRequest,
                Message = OrattoConstants.AS2L_400
            };

            try
            {
                // STEP 1: Get all Linkings from Lead
                var getLeadSolicitorsResponse = await _crmService.GetLeadRelatedSolicitors(leadId);

                var leadSolicitors = new List<LeadSolicitorData>();
                if (getLeadSolicitorsResponse.Code == ResultCode.OK)
                {
                    leadSolicitors = getLeadSolicitorsResponse.Data.data;
                }

                // STEP 2: Create linking for new Solicitors
                var assignedSolicitorIds = solicitorIds.Split(",");

                var currentSolicitorIds = leadSolicitors
                    .Select(l => l.Referred_Solicitors.id).ToList();

                var newSolicitorIds = new List<string>();
                var redundantSolicitorIds = new List<string>();

                foreach (var solicitorId in assignedSolicitorIds)
                {
                    if (!currentSolicitorIds.Contains(solicitorId))
                    {
                        newSolicitorIds.Add(solicitorId);
                    }
                }

                foreach (var solicitorId in currentSolicitorIds)
                {
                    if (!assignedSolicitorIds.Contains(solicitorId))
                    {
                        redundantSolicitorIds.Add(solicitorId);
                    }
                }

                // STEP 3: Create Linking for NEW Solicitor Ids
                foreach (var solicitorId in newSolicitorIds)
                {
                    var upsertRequest = new UpsertRequest<LeadSolicitorLinkingForCreation>();
                    var linkingForCreation = new LeadSolicitorLinkingForCreation()
                    {
                        Leads_Referred = leadId,
                        Referred_Solicitors = solicitorId
                    };
                    upsertRequest.data.Add(linkingForCreation);
                    upsertRequest.trigger.Add(CommonConstants.ZohoWorkflow);
                    var createLinkingResponse = await _crmService.CreateLeadSolicitorLinking(upsertRequest);
                }

                // STEP 4: Delete Linking for REDUNDANT Solicitor Ids
                foreach (var solicitorId in redundantSolicitorIds)
                {
                    var leadSolicitor = leadSolicitors
                        .Where(l => l.Referred_Solicitors.id == solicitorId)
                        .FirstOrDefault();
                    string linkingId = leadSolicitor.id;
                    var deleteLinkingResponse = await _crmService.DeleteLeadSolicitorLinking(linkingId);
                }

                apiResult.Code = ResultCode.OK;
                apiResult.Message = OrattoConstants.AS2L_200;
                return apiResult;
            }
            catch (Exception ex)
            {
                apiResult.Message = $"{ex.Message} - {ex.StackTrace}";
                return apiResult;
            }

        }

        public async Task<ApiResultDto<QuerySolicitorsDto>> QuerySolicitors(string leadId)
        {

            var apiResult = new ApiResultDto<QuerySolicitorsDto>()
            {
                Code = ResultCode.BadRequest,
                Message = OrattoConstants.QS_400
            };

            try
            {

                // STEP 1: Query all Solicitors
                var coqlRequest = new ZohoCoqlRequest()
                {
                    select_query = "SELECT First_Name, Last_Name, Account_Name.Account_Name as Law_Firm_Name, Title, Oratto_Lawyer_Shortlist, Matter_Expertise, Sub_Categories " +
                    "from Contacts WHERE Email is not null AND Oratto_Lawyer = 'Yes' ORDER BY First_Name ASC"
                };
                var querySolicitorsResponse = await _crmService.QuerySolicitors(coqlRequest);
                if (querySolicitorsResponse.Code != ResultCode.OK)
                {
                    return apiResult;
                }
                var allSolicitors = querySolicitorsResponse.Data.data;

                // STEP 2: Select Lead Solicitors
                var getLeadSolicitorsResponse = await _crmService.GetLeadRelatedSolicitors(leadId);
                List<string> leadSolicitorIds = new List<string>();
                if (getLeadSolicitorsResponse.Code == ResultCode.OK)
                {
                    var leadSolicitors = getLeadSolicitorsResponse.Data.data;
                    foreach (var solicitor in leadSolicitors)
                    {
                        var referredSolicitor = solicitor.Referred_Solicitors;
                        string solicitorId = referredSolicitor.id;
                        leadSolicitorIds.Add(solicitorId);
                    }
                }

                // STEP 3: Convert Solicitor to Solicitor Dto
                var solicitorsDto = new List<SolicitorDto>();
                foreach (var solicitor in allSolicitors)
                {
                    string solicitorId = solicitor.id;
                    string title = solicitor.Title;
                    string firstName = solicitor.First_Name;
                    string lastName = solicitor.Last_Name;
                    string fullName = string.Empty;
                    if (!string.IsNullOrEmpty(firstName))
                    {
                        fullName = firstName;
                    }
                    string lawFirmName = solicitor.Law_Firm_Name;
                    if (!string.IsNullOrEmpty(lastName))
                    {
                        if (string.IsNullOrEmpty(fullName))
                        {
                            fullName = lastName;
                        }
                        if (!string.IsNullOrEmpty(fullName))
                        {
                            fullName += $" {lastName}";
                        }
                    }
                    fullName = $"<a target='_blank' href='https://crm.zoho.eu/crm/org20086343963/tab/Contacts/{solicitorId}'>{fullName}</a>";

                    string matterExpertise = solicitor.Matter_Expertise;
                    bool? orattoLawyerShortlist = solicitor.Oratto_Lawyer_Shortlist;
                    string subCategories = solicitor.Sub_Categories;

                    bool select = false;
                    if (leadSolicitorIds.Contains(solicitorId))
                    {
                        select = true;
                    }

                    if (!string.IsNullOrEmpty(subCategories))
                    {
                        subCategories = subCategories.Replace("\n", "<br/>");
                    }

                    bool shortList = false;
                    if (orattoLawyerShortlist.HasValue && orattoLawyerShortlist.Value)
                    {
                        shortList = true;
                    }

                    var solicitorDto = new SolicitorDto()
                    {
                        id = solicitorId,
                        FullName = fullName,
                        LawFirmName = lawFirmName,
                        MatterExpertise = matterExpertise,
                        OrattoLawyerShortlist = shortList ? "Yes" : "No",
                        SubCategories = subCategories,
                        Title = title,
                        Select = select
                    };
                    solicitorsDto.Add(solicitorDto);
                }

                var sortedSolicitorsDto = solicitorsDto.OrderBy(s => s.Select.Value ? 0 : 1)
                    .ThenBy(s => s.FullName).ToList();

                var querySolicitorsDto = new QuerySolicitorsDto()
                {
                    solicitors = sortedSolicitorsDto
                };

                apiResult.Code = ResultCode.OK;
                apiResult.Message = OrattoConstants.QS_200;
                apiResult.Data = querySolicitorsDto;

                return apiResult;

            }
            catch (Exception ex)
            {
                apiResult.Message = $"{ex.Message} - {ex.StackTrace}";
                return apiResult;
            }
        }

        public async Task<ApiResultDto<string>> SendEmailForLastUpdated(string matterId)
        {
            var apiResult = new ApiResultDto<string>()
            {
                Code = ResultCode.BadRequest,
                Message = OrattoConstants.SELU_400
            };

            try
            {

                // STEP 1: Get Matter by Id
                var getMatterByIdResponse = await _crmService.GetMatterById(matterId);
                if (getMatterByIdResponse.Code != ResultCode.OK)
                {
                    apiResult.Message = OrattoConstants.SELU_E02;
                    return apiResult;
                }
                var matterDetails = getMatterByIdResponse.Data.data[0];
                string matterName = matterDetails.Name;
                var solicitor = matterDetails.Referred_Solicitors;
                if (solicitor == null)
                {
                    apiResult.Message = OrattoConstants.SELU_E03;
                    return apiResult;
                }
                string solicitorId = solicitor.id;
                string solicitorName = solicitor.name;
                var relatedLead = matterDetails.Leads_Referred;
                string leadName = string.Empty;
                string leadId = string.Empty;
                if (relatedLead != null)
                {
                    leadName = relatedLead.name;
                    leadId = relatedLead.id;
                }

                var solicitorDetails = await _crmService.GetSolicitorById(solicitorId);

                // STEP 2: Get Matter Timeline
                var getMatterTimelineResponse = await _crmService.GetModuleTimeline("Matters1", matterId);
                if (getMatterTimelineResponse.Code != ResultCode.OK)
                {
                    apiResult.Message = OrattoConstants.SELU_E01;
                    return apiResult;
                }

                var timelines = getMatterTimelineResponse.Data.__timeline;

                foreach (var timeline in timelines)
                {
                    var doneBy = timeline.done_by;
                    string doneByName = doneBy.name;
                    if (solicitorName == doneByName)
                    {
                        // Send Email to Mr. Mark
                        string emailSubject = "Solicitor just updated Matter: $MatterName$";
                        var fieldHistory = timeline.field_history;
                        var record = timeline.record;

                        string historyHtml = string.Empty;
                        if (fieldHistory != null && fieldHistory.Length > 0)
                        {
                            var history = fieldHistory[0];
                            string apiName = history.api_name;
                            var historyValue = history._value;
                            string oldValue = historyValue.old;
                            string newValue = historyValue._new;
                            historyHtml = $"{apiName} Old: {oldValue} => New: {newValue}";
                        }
                        else if (record != null)
                        {
                            var recordModule = record.module;
                            string apiName = recordModule.api_name;
                            string recordName = record.name;
                            historyHtml = $"New {apiName}: {recordName}";
                        }

                        emailSubject = emailSubject.Replace("$MatterName$", matterName);
                        string emailBody = OrattoConstants.LastUpdatedTemplate;
                        emailBody = emailBody.Replace("$SolicitorName$", solicitorName)
                            .Replace("$SolicitorId$", solicitorId)
                            .Replace("$MatterId$", matterId)
                            .Replace("$MatterName$", matterName)
                            .Replace("$History$", historyHtml)
                            .Replace("$LeadId$", leadId)
                            .Replace("$LeadName$", leadName);

                        var sendEmailRequest = new SendEmailFromContact
                        {
                            // ToEmails = "mark@oratto.co.uk,hoangtran7292@gmail.com",
                            ToEmails = "mark@oratto.co.uk,hoangtran7292@gmail.com",
                            EmailBody = emailBody,
                            EmailSubject = emailSubject
                        };
                        var sendEmailResponse = await SendEmailFromContact(sendEmailRequest);
                        if (sendEmailResponse.Code != 0)
                        {
                            apiResult.Message = OrattoConstants.SERS_E03;
                            return apiResult;
                        }
                    }
                    break;

                }

                apiResult.Code = ResultCode.OK;
                apiResult.Message = OrattoConstants.SELU_200;
                return apiResult;

            }
            catch (Exception ex)
            {
                apiResult.Message = $"{ex.Message} - {ex.StackTrace}";
                return apiResult;
            }


        }

        public async Task<ApiResultDto<string>> SendEmailForReassign(string matterId)
        {

            var apiResult = new ApiResultDto<string>()
            {
                Code = ResultCode.BadRequest,
                Message = OrattoConstants.SERS_200
            };

            try
            {

                // STEP 1: Get Matter by Id
                var getMatterByIdResponse = await _crmService.GetMatterById(matterId);
                if (getMatterByIdResponse.Code != ResultCode.OK)
                {
                    apiResult.Message = OrattoConstants.SERS_E01;
                    return apiResult;
                }
                var matterDetails = getMatterByIdResponse.Data.data[0];
                string matterName = matterDetails.Name;

                // STEP 2: Query Contacts by Email
                var currentSolicitor = matterDetails.Referred_Solicitors;

                string currentSolicitorId = currentSolicitor.id;
                string currentSolicitorName = currentSolicitor.name;
                string newSolicitorEmail = matterDetails.New_Solicitor_Email;
                string query = $"SELECT First_Name, Last_Name FROM Contacts WHERE Email = '{newSolicitorEmail}'";

                var coqlRequest = new ZohoCoqlRequest()
                {
                    select_query = query
                };
                var queryContactsResponse = await _crmService.QuerySolicitors(coqlRequest);
                if (queryContactsResponse.Code == ResultCode.OK)
                {

                    var solicitorList = queryContactsResponse.Data.data;
                    var solicitorDetails = solicitorList[0];
                    string newSolicitorId = solicitorDetails.id;
                    string newSolicitorName = $"{solicitorDetails.First_Name} {solicitorDetails.Last_Name}".Trim(); 

                    // STEP 3: Update Matter with new Solicitor
                    var matterForUpdation = new MatterForUpdation()
                    {
                        Referred_Solicitors = newSolicitorId
                    };

                    var upsertRequest = new UpsertRequest<MatterForUpdation>();
                    upsertRequest.data.Add(matterForUpdation);

                    var upsertResponse = await _crmService.UpdateMatter(matterId, upsertRequest);
                    if (upsertResponse.Code != ResultCode.OK)
                    {
                        apiResult.Message = OrattoConstants.SERS_E02;
                        return apiResult;
                    }

                    string emailSubject = "Reassign Solicitor for Matter: $MatterName$";
                    emailSubject = emailSubject.Replace("$MatterName$", matterName);

                    string emailBody = OrattoConstants.AssignEmailTemplate;

                    emailBody = emailBody
                        .Replace("$MatterId$", matterId)
                        .Replace("$MatterName$", matterName)
                        .Replace("$SolicitorId$", currentSolicitorId)
                        .Replace("$SolicitorName$", currentSolicitorName)
                        .Replace("$NewSolicitorId$", newSolicitorId)
                        .Replace("$NewSolicitorName$", newSolicitorName);

                    var sendEmailRequest = new SendEmailFromContact
                    {
                        ToEmails = "mark@oratto.co.uk,hoangtran7292@gmail.com",
                        EmailBody = emailBody,
                        EmailSubject = emailSubject
                    };
                    var sendEmailResponse = await SendEmailFromContact(sendEmailRequest);
                    if (sendEmailResponse.Code != 0)
                    {
                        apiResult.Message = OrattoConstants.SERS_E03;
                        return apiResult;
                    }

                }
                else
                {

                    string emailSubject = "Reassign Solicitor for Matter: $MatterName$";
                    emailSubject = emailSubject.Replace("$MatterName$", matterName);

                    string emailBody = OrattoConstants.CannotAssignEmailTemplate;

                    string currentSolicitorHtml = string.Empty;
                    if (currentSolicitor != null)
                    {
                        currentSolicitorHtml = "<a target = '_blank' href = 'https://crm.zoho.eu/crm/org20086343963/tab/Contacts/$SolicitorId$'>$SolicitorName$</a>";
                        currentSolicitorHtml = currentSolicitorHtml.Replace("$SolicitorId$", currentSolicitorId)
                            .Replace("$SolicitorName$", currentSolicitorName);
                    }

                    emailBody = emailBody.Replace("$MatterId$", matterId).Replace("$MatterName$", matterName)
                        .Replace("$PreviousSolicitor$", currentSolicitorHtml).Replace("$SolicitorEmail$", newSolicitorEmail);

                    var sendEmailRequest = new SendEmailFromContact
                    {
                        ToEmails = "mark@oratto.co.uk,hoangtran7292@gmail.com",
                        EmailBody = emailBody,
                        EmailSubject = emailSubject
                    };
                    var sendEmailResponse = await SendEmailFromContact(sendEmailRequest);
                    if (sendEmailResponse.Code != 0)
                    {
                        apiResult.Message = OrattoConstants.SERS_E03;
                        return apiResult;
                    }

                }

                apiResult.Code = ResultCode.OK;
                apiResult.Message = OrattoConstants.SERS_200;

                return apiResult;
            }
            catch (Exception ex)
            {
                apiResult.Message = $"{ex.Message} - {ex.StackTrace}";
                return apiResult;
            }

        }

        public async Task<ApiResultDto<ZohoApiResponse>> SendEmailFromContact(SendEmailFromContact sendEmail)
        {
            var apiResult = new ApiResultDto<ZohoApiResponse>()
            {
                Code = ResultCode.BadRequest,
                Message = OrattoConstants.SEFC_400
            };

            try
            {

                string endpoint = $"{OrattoConstants.SendEmailEndpoint}";
                string requestBody = JsonConvert.SerializeObject(sendEmail);
                var request = new HttpRequestMessage(
                           HttpMethod.Post,
                           endpoint)
                {
                    Content = new StringContent(requestBody, Encoding.UTF8, "application/json")
                };
                using var httpClient = new HttpClient();
                using var response = await httpClient.SendAsync(request,
                           HttpCompletionOption.ResponseHeadersRead);
                var stream = await response.Content.ReadAsStreamAsync();

                // Convert stream to string
                StreamReader reader = new StreamReader(stream);
                string responseData = reader.ReadToEnd();

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    var responseObj = JsonConvert.DeserializeObject<ZohoApiResponse>(responseData);
                    apiResult.Code = ResultCode.OK;
                    apiResult.Message = OrattoConstants.SEFC_400;
                    apiResult.Data = responseObj;
                }

                return apiResult;
            }
            catch (Exception)
            {
                return apiResult;
            }
        }

        public async Task<ApiResultDto<Models.Oratto.OpenAI.UploadFileResponse>> UploadMailAttachmentsToOpenAi(DownloadAttachmentRequest attachmentRequest)
        {
            var apiResult = new ApiResultDto<Models.Oratto.OpenAI.UploadFileResponse>()
            {
                Code = ResultCode.BadRequest,
                Message = OrattoConstants.UMA2O_400
            };

            try
            {

                // STEP 1: Download Email Attachment

                string fileName = attachmentRequest.AttachmentName;
                
                var downloadAttachmentResult = await _mailService.DownloadEmailAttachment(attachmentRequest);

                if (downloadAttachmentResult.Code != ResultCode.OK)
                {
                    return apiResult;
                }

                // STEP 2: Upload Attachment to OpenAI
                string filePath = downloadAttachmentResult.Data;

                string purpose = "assistants";

                apiResult = await _aiService.UploadFile2OpenAI(filePath, fileName, purpose);

                return apiResult;

            }
            catch (Exception ex)
            {

                apiResult.Message = $"{ex.Message} - {ex.StackTrace}";
                return apiResult;

            }
        }

        public async Task<ApiResultDto<string>> GenerateAttachmentContentByGemini(
            DownloadAttachmentRequest attachmentRequest)
        {

            var apiResult = new ApiResultDto<string>()
            {
                Code = ResultCode.BadRequest,
                Message = OrattoConstants.GACBG_400
            };

            try
            {

                string attachmentId = attachmentRequest.AttachmentId;

                // STEP 3: Get Queued Attachment by Id
                var coqlRequest = new ZohoCoqlRequest()
                {
                    select_query = $"Select Name, Status from Queued_Attachments where Name = '{attachmentId}'"
                };
                var queryAttachmentsResponse = await _crmService.QueryAttachments(coqlRequest);
                string attachmentZohoId = "";
                if (queryAttachmentsResponse.Code == ResultCode.OK)
                {
                    var attachmentList = queryAttachmentsResponse.Data.data;
                    var fistAttachment = attachmentList[0];
                    attachmentZohoId = fistAttachment.id;
                    string attachmentStatus = fistAttachment.Status;
                    if (attachmentStatus == "Completed")
                    {
                        apiResult.Code = ResultCode.OK;
                        apiResult.Message = OrattoConstants.GACBG_E03;
                        return apiResult;
                    }
                }
                if (string.IsNullOrEmpty(attachmentZohoId))
                {
                    apiResult.Message = OrattoConstants.GACBG_E03;
                    return apiResult;
                }

                // STEP 1: Download Email Attachment

                string fileName = attachmentRequest.AttachmentName;

                var downloadAttachmentResult = await _mailService.DownloadEmailAttachment(attachmentRequest);

                if (downloadAttachmentResult.Code != ResultCode.OK)
                {
                    return apiResult;
                }

                var filePath = downloadAttachmentResult.Data;

                var attachmentContents = new List<string>();

                if (fileName.EndsWith(".docx", StringComparison.InvariantCultureIgnoreCase)
                    || fileName.EndsWith(".doc", StringComparison.InvariantCultureIgnoreCase))
                {
                    var convertFileResponse = _documentService.ConvertDocxToPdf(filePath);
                    filePath = convertFileResponse.Data;
                    fileName = Path.GetFileName(filePath);
                }
                else if (fileName.EndsWith(".pptx", StringComparison.InvariantCultureIgnoreCase)
                    || fileName.EndsWith(".ppt", StringComparison.InvariantCultureIgnoreCase))
                {
                    var convertFileResponse = _documentService.ConvertPptxToPdf(filePath);
                    filePath = convertFileResponse.Data;
                    fileName = Path.GetFileName(filePath);
                }
                if (fileName.EndsWith(".pdf", StringComparison.InvariantCultureIgnoreCase))
                {

                    var splitFilesResponse = _documentService.SplitPdfFile(filePath);

                    var splitFiles = splitFilesResponse.Data;

                    foreach (var splitFilePath in splitFiles)
                    {

                        string splitFileName = Path.GetFileName(splitFilePath);

                        // STEP 1: Upload Attachment
                        var uploadAttachmentResponse = await _geminiService.UploadFile(splitFilePath, splitFileName);

                        if (uploadAttachmentResponse.Code != ResultCode.OK)
                        {
                            apiResult.Message = OrattoConstants.GACBG_E01;
                            return apiResult;
                        }

                        // STEP 2: Get File Data
                        var upload = uploadAttachmentResponse.Data.file;
                        string fileUri = upload.uri;
                        string fileType = upload.mimeType;

                        // STEP 3: Generate File Content by Gemini
                        var generateContentRequest = new GenerateContentRequest();
                        var contents = new List<Content>();
                        var content = new Content();
                        var parts = new List<Part>();
                        var textPart = new Part();
                        textPart.text = OrattoConstants.GACBG_ExtractFileContent;
                        parts.Add(textPart);

                        var filePart = new Part();
                        var fileData = new File_Data();
                        fileData.mime_type = fileType;
                        fileData.file_uri = fileUri;
                        filePart.file_data = fileData;
                        parts.Add(filePart);

                        content.parts = parts;
                        contents.Add(content);
                        generateContentRequest.contents = contents;

                        var generateContentResponse = await
                            _geminiService.GenerateContent(generateContentRequest);
                        if (generateContentResponse.Code != ResultCode.OK)
                        {
                            apiResult.Message = OrattoConstants.GACBG_E02;
                            return apiResult;
                        }
                        var generateContentDetails = generateContentResponse.Data;
                        var firstCandidate = generateContentDetails.candidates[0];
                        var firstContent = firstCandidate.content;
                        var firstPart = firstContent.parts[0];
                        var attachmentContent = firstPart.text;
                        attachmentContents.Add(attachmentContent);

                    }
                    
                }
                else
                {
                    // STEP 1: Upload Attachment
                    var uploadAttachmentResponse = await _geminiService.UploadFile(filePath, fileName);

                    if (uploadAttachmentResponse.Code != ResultCode.OK)
                    {
                        apiResult.Message = OrattoConstants.GACBG_E01;
                        return apiResult;
                    }

                    // STEP 2: Get File Data
                    var upload = uploadAttachmentResponse.Data.file;
                    string fileUri = upload.uri;
                    string fileType = upload.mimeType;

                    // STEP 3: Generate File Content by Gemini
                    var generateContentRequest = new GenerateContentRequest();
                    var contents = new List<Content>();
                    var content = new Content();
                    var parts = new List<Part>();
                    var textPart = new Part();
                    textPart.text = OrattoConstants.GACBG_ExtractFileContent;
                    parts.Add(textPart);

                    var filePart = new Part();
                    var fileData = new File_Data();
                    fileData.mime_type = fileType;
                    fileData.file_uri = fileUri;
                    filePart.file_data = fileData;
                    parts.Add(filePart);

                    content.parts = parts;
                    contents.Add(content);
                    generateContentRequest.contents = contents;

                    var generateContentResponse = await
                        _geminiService.GenerateContent(generateContentRequest);
                    if (generateContentResponse.Code != ResultCode.OK)
                    {
                        apiResult.Message = OrattoConstants.GACBG_E02;
                        return apiResult;
                    }
                    var generateContentDetails = generateContentResponse.Data;
                    var firstCandidate = generateContentDetails.candidates[0];
                    var firstContent = firstCandidate.content;
                    var firstPart = firstContent.parts[0];
                    var attachmentContent = firstPart.text;
                    attachmentContents.Add(attachmentContent);
                }

                // STEP 4: Update Queued Attachment
                var updateAttachmentRequest = new UpsertRequest<AttachmentForUpdation>();

                var attachmentData = new AttachmentForUpdation()
                {
                    Status = "Completed"
                };

                int contentIndex = 0;
                foreach (var content in attachmentContents)
                {
                    contentIndex++;
                    switch (contentIndex)
                    {
                        case 1:
                            attachmentData.Content = content;
                            break;
                        case 2:
                            attachmentData.Content_2 = content;
                            break;
                        case 3:
                            attachmentData.Content_3 = content;
                            break;
                        case 4:
                            attachmentData.Content_4 = content;
                            break;
                        case 5:
                            attachmentData.Content_5 = content;
                            break;
                        default:
                            break;
                    }

                }

                var data = new List<AttachmentForUpdation>();
                data.Add(attachmentData);
                updateAttachmentRequest.data = data;
                var workflow = new List<string>();
                workflow.Add("workflow");
                updateAttachmentRequest.trigger = workflow;

                var updateAttachmentResponse = await _crmService.UpdateQueuedAttachments(
                    attachmentZohoId, updateAttachmentRequest);
                if (updateAttachmentResponse.Code != ResultCode.OK)
                {
                    apiResult.Message = OrattoConstants.GACBG_E04;
                    return apiResult;
                }

                apiResult.Code = ResultCode.OK;
                apiResult.Message = OrattoConstants.GACBG_200;
                return apiResult;

            }
            catch (Exception ex)
            {
                apiResult.Message = $"{ex.Message} - {ex.StackTrace}";
                return apiResult;
            }

        }

        public async Task<ApiResultDto<ChatCompletionResponse>> ChatCompletionsForQueuedEmail(string emailId, ChatCompletionRequest completionRequest)
        {
            var apiResult = new ApiResultDto<ChatCompletionResponse>()
            {
                Code = ResultCode.BadRequest,
                Message = OrattoConstants.CC4QE_400
            };

            try
            {

                // STEP 1: Get Chat Completions Response
                var completionResponse = await _aiService.ChatCompletions(completionRequest);

                if (completionResponse.Code != ResultCode.OK)
                {
                    apiResult.Message = OrattoConstants.CC4QE_E01;
                    return apiResult;
                }    

                var completionDetails = completionResponse.Data;
                apiResult.Data = completionDetails;

                var choices = completionDetails.choices;
                var firstChoice = choices[0];
                var message = firstChoice.message;
                var content = message.content;
                content = content.Replace("```html", "").Replace("```", "");

                var usage = completionDetails.usage;
                int promptTokens = (int)usage.prompt_tokens;
                int completionTokens = (int)usage.completion_tokens;

                var emailForUpdation = new EmailForUpdation
                {
                    Status = "AI Response Received",
                    Prompt_Tokens = promptTokens,
                    Completion_Tokens = completionTokens
                };

                int index = 0;
                do
                {
                    index++;
                    int length = (content.Length > CommonConstants.MultilineLength)
                        ? CommonConstants.MultilineLength : content.Length;
                    string tempContent = content.Substring(0, length);
                    content = content.Replace(tempContent, "");

                    switch (index)
                    {
                        case 1:
                            emailForUpdation.Content_1 = tempContent;
                            break;
                        case 2:
                            emailForUpdation.Content_2 = tempContent;
                            break;
                        case 3:
                            emailForUpdation.Content_3 = tempContent;
                            break;
                        case 4:
                            emailForUpdation.Content_4 = tempContent;
                            break;
                        case 5:
                            emailForUpdation.Content_5 = tempContent;
                            break;
                        case 6:
                            emailForUpdation.Content_6 = tempContent;
                            break;
                        case 7:
                            emailForUpdation.Content_7 = tempContent;
                            break;
                        case 8:
                            emailForUpdation.Content_8 = tempContent;
                            break;
                        case 9:
                            emailForUpdation.Content_9 = tempContent;
                            break;
                        case 10:
                            emailForUpdation.Content_10 = tempContent;
                            break;
                        default:
                            break;
                    }

                }
                while (content.Length > CommonConstants.MultilineLength);

                var upsertRequest = new UpsertRequest<EmailForUpdation>();
                upsertRequest.data.Add(emailForUpdation);
                upsertRequest.trigger.Add("workflow");

                var updateEmailResponse = await _crmService
                    .UpdatedQueuedEmails(emailId, upsertRequest);

                if (updateEmailResponse.Code != ResultCode.OK)
                {
                    apiResult.Message = OrattoConstants.CC4QE_E02;
                    return apiResult;
                }

                apiResult.Code = ResultCode.OK;
                apiResult.Message = OrattoConstants.CC4QE_200;

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
