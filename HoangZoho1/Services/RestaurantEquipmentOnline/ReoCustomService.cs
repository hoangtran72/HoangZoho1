using HoangZoho1.Constants;
using HoangZoho1.Helpers;
using HoangZoho1.Models.Common;
using HoangZoho1.Models.GoogleAPI;
using HoangZoho1.Models.PinjarraBakery.ZohoCRM;
using HoangZoho1.Models.RestaurantEquipmentOnline.Custom;
using HoangZoho1.Models.RestaurantEquipmentOnline.JustCall;
using HoangZoho1.Models.RestaurantEquipmentOnline.TNZ;
using HoangZoho1.Models.RestaurantEquipmentOnline.ZohoCRM;
using HoangZoho1.Services.GoogleAPI;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Twilio.TwiML.Voice;
using Module = HoangZoho1.Models.RestaurantEquipmentOnline.ZohoCRM.Module;

namespace HoangZoho1.Services.RestaurantEquipmentOnline
{
    public class ReoCustomService : IReoCustomService
    {

        private readonly IGmailService _gmailService;
        private readonly IReoCrmService _crmService;
        private readonly IReoJustCallService _justCallService;
        private readonly IReoTnzService _tnzService;

        public ReoCustomService(IGmailService gmailService, IReoCrmService reoCrmService,
            IReoJustCallService justCallService, IReoTnzService tnzService)
        {
            _gmailService = gmailService;
            _crmService = reoCrmService;
            _justCallService = justCallService;
            _tnzService = tnzService;
        }

        public async Task<ApiResultDto<string>> HandleSalesEmails(string emailDate, string startTime, string endTime)
        {
            var apiResult = new ApiResultDto<string>()
            {
                Code = ResultCode.BadRequest,
                Message = REOConstants.CUSTOM_GSEP_400
            };

            try
            {
                // STEP 1: Get Sales Email today

                // Step 1.1: Handle Query String
                string query = string.Empty;
                if (!string.IsNullOrEmpty(startTime) && !string.IsNullOrEmpty(endTime))
                {
                    query = $"in:inbox after:{startTime} before:{endTime}";
                }
                else
                {
                    var timeRange = GmailHelpers.ConvertDateToTimestamp(emailDate);
                    query = $"in:inbox after:{timeRange.Item1} before:{timeRange.Item2}";
                }

                // Step 1.2: Get the Emails
                var searchGmailsResult = await _gmailService.SearchForEmails(query);
                if (searchGmailsResult.Code != ResultCode.OK)
                {
                    apiResult.Message = REOConstants.CUSTOM_GSEP_SE1;
                    return apiResult;
                }

                var totalEmail = searchGmailsResult.Data.resultSizeEstimate;

                if (totalEmail == 0)
                {
                    apiResult.Message = REOConstants.CUSTOM_GSEP_SE2;
                    return apiResult;
                }

                var searchGmailList = searchGmailsResult.Data.messages;

                // STEP 2: Get All Gmails
                var gmailList = new List<GmailContent>();
                foreach (var searchGmail in searchGmailList)
                {
                    string gmailId = searchGmail.id;
                    var getGmailByIdResult = await _gmailService.GetGmailById(gmailId);
                    var gmailData = getGmailByIdResult.Data;

                    // Step 2.1: Get From, Date, Subject
                    var gmailContent = new GmailContent();
                    gmailContent.MsgId = gmailData.id;
                    gmailContent.FirstName = string.Empty;
                    gmailContent.LastName = "Gmail API";
                    gmailContent.MailDateTime = DateTime.UtcNow;

                    foreach (var messagePart in gmailData.payload.headers)
                    {
                        if (messagePart.name == "From")
                        {
                            // Step 2.2: Handle Extract Email
                            string fromValue = messagePart.value;
                            string fromEmail = GmailHelpers.GetEmailFromText(fromValue);

                            fromValue = fromValue.Replace(fromEmail, "")
                                .Replace("<", "").Replace(">", "")
                                .Replace("\"", "").Trim();

                            if (fromValue.Contains(" "))
                            {
                                var fromSplits = fromValue.Split(new[] { ' ' }, 2);
                                gmailContent.FirstName = fromSplits[0];
                                gmailContent.LastName = fromSplits[1];
                            }

                            gmailContent.From = fromEmail;
                        }
                        else if (messagePart.name == "Subject")
                        {
                            gmailContent.Subject = messagePart.value;
                        }
                    }

                    // Step 2.3: Get Mail Body
                    string mailBody = string.Empty;
                    if (gmailData.payload.parts == null && gmailData.payload.body != null)
                    {
                        mailBody = gmailData.payload.body.data;
                    }
                    else
                    {
                        mailBody = GmailHelpers.MsgNestedParts(gmailData.payload.parts);
                    }

                    if (!string.IsNullOrEmpty(mailBody))
                    {
                        // Step 2.4: Base64 Decode to readable text
                        string readableText = string.Empty;
                        readableText = GmailHelpers.Base64Decode(mailBody);

                        if (!string.IsNullOrEmpty(readableText))
                        {
                            gmailContent.Body = readableText.Replace("\r", "");
                        }
                        gmailList.Add(gmailContent);
                    }
                }

                // STEP 3: Push Data to Zoho CRM
                foreach (var gmail in gmailList)
                {
                    bool isGumtreeEmail = false;

                    string email = gmail.From;
                    string subject = gmail.Subject;
                    string body = gmail.Body;
                    string gumtreeUsername = string.Empty;
                    string productUrl = string.Empty;
                    string replyUrl = string.Empty;

                    if (email.Contains("gumtree", StringComparison.InvariantCultureIgnoreCase))
                    {
                        isGumtreeEmail = true;

                        // Step 3.1.1: Extract Gumtree username from subject
                        var subjectSplits = subject.Split("replied to your ad");
                        gumtreeUsername = subjectSplits[0].Trim();

                        // Step 3.1.2: Extract Gumtree Product URL from message body
                        var productSplits1 = body.Split("<td class=\"mobCenterTop\"", 2);
                        if (productSplits1.Length > 1)
                        {
                            string tempProductURL = productSplits1[1];
                            var productSplits2 = tempProductURL.Split("replied to your ad <a href=\"", 2);
                            if (productSplits2.Length > 1)
                            {
                                tempProductURL = productSplits2[1];
                                var productSplits3 = tempProductURL.Split("\"", 2);
                                productUrl = productSplits3[0];
                            }
                        }

                        // Step 3.1.3: Extract Gumtree Reply URL from message body
                        var replySplits1 = body.Split("<!-- Reply to message button -->", 2);
                        if (replySplits1.Length > 1)
                        {
                            string tempReplyURL = replySplits1[1];
                            var replySplits2 = tempReplyURL.Split("<!-- End Reply to message button -->");
                            tempReplyURL = replySplits2[0];
                            var replySplits3 = tempReplyURL.Split("<a href=\"");
                            if (replySplits3.Length > 1)
                            {
                                tempReplyURL = replySplits3[1];
                                var replySplits4 = tempReplyURL.Split("\"");
                                replyUrl = replySplits4[0];
                            }
                        }
                    }

                    // Step 3.1.4: Search Contact by Email
                    var searchContactByEmailResult = await _crmService.SearchContactsByEmail(email);
                    if (searchContactByEmailResult.Code == ResultCode.OK)
                    {
                        continue;
                    }

                    // Step 3.2: Search Lead by Email
                    var searchLeadByEmailResult = await _crmService.SearchLeadsByEmail(email);
                    if (searchLeadByEmailResult.Code == ResultCode.OK)
                    {
                        continue;
                    }

                    // Step 3.3: Create Lead and add Note to Lead record
                    var trigger = new List<string>
                    {
                        "blueprint",
                        "workflow"
                    };
                    var leadForCreation = new LeadForCreation()
                    {
                        Email = gmail.From,
                        First_Name = gmail.FirstName,
                        Last_Name = gmail.LastName,
                        Lead_Source = "Gmail API",
                        Lead_Status = "Sales First Attempt",
                        trigger = trigger
                    };

                    leadForCreation.Owner = REOConstants.ReoCrmId;

                    /*
                    string ownerId = await GetRandomAccountManagerId();
                    if (!string.IsNullOrEmpty(ownerId))
                    {
                        leadForCreation.Owner = ownerId;
                    }
                    */

                    if (isGumtreeEmail)
                    {
                        leadForCreation.First_Name = gumtreeUsername;
                        leadForCreation.Last_Name = "Gumtree";
                        leadForCreation.Lead_Source = "Gumtree";
                        leadForCreation.Gmail_Msg_Id = gmail.MsgId;
                        if (!string.IsNullOrEmpty(productUrl) && Uri.IsWellFormedUriString(productUrl, UriKind.Absolute))
                        {
                            leadForCreation.Gumtree_Product_URL = productUrl;
                        }
                        if (!string.IsNullOrEmpty(replyUrl) && Uri.IsWellFormedUriString(replyUrl, UriKind.Absolute))
                        {
                            leadForCreation.Gumtree_Reply_URL = replyUrl;
                        }
                    }
                    var createLeadRequest = new UpsertRequest<LeadForCreation>();
                    createLeadRequest.data.Add(leadForCreation);
                    var createLeadResult = await _crmService.CreateLead(createLeadRequest);
                    continue;
                }

                // STEP 4: Handle Create Lead in Zoho CRM
                apiResult.Code = ResultCode.OK;
                apiResult.Message = REOConstants.CUSTOM_GSEP_200;
                return apiResult;

            }
            catch (Exception ex)
            {
                apiResult.Data = $"{ex.Message} - {ex.StackTrace}";
                return apiResult;
            }
        }

        public async Task<int> GetLeadNumberDaily()
        {
            int leadNumber = 0;

            try
            {
                // Step 1: Prepare query to get number of Leads
                var tz = TimeZoneInfo.FindSystemTimeZoneById("AUS Eastern Standard Time");
                int gmtOffset = tz.GetUtcOffset(DateTime.UtcNow).Hours;
                string currentDate = TimeZoneInfo.ConvertTimeFromUtc
                    (DateTime.UtcNow, tz).ToString("yyyy-MM-dd");

                string startTime = $"{currentDate}T00:00:00+{gmtOffset}:00";
                string endTime = $"{currentDate}T23:59:59+{gmtOffset}:00";

                string leadQuery = @$"SELECT First_Name, Last_Name, id FROM Leads 
                                    WHERE Created_Time >= '{startTime}' AND Created_Time <= '{endTime}'";

                // Step 2: Call Query to get Leads
                var queryLeadResponse = await _crmService.QueryLeads(leadQuery);
                if (queryLeadResponse.Code != ResultCode.OK)
                {
                    return leadNumber;
                }

                var queryLeadDetails = queryLeadResponse.Data;
                var queryInfo = queryLeadDetails.info;
                bool moreRecord = queryInfo.more_records;
                leadNumber += queryInfo.count;
                int searchNumber = 1;

                while (moreRecord)
                {
                    int skip = 200 * searchNumber;
                    leadQuery = @$"SELECT First_Name, Last_Name, id FROM Leads 
                                    WHERE Created_Time >= '{startTime}' AND Created_Time <= '{endTime}'
                                    LIMIT 200 OFFSET {skip};";

                    queryLeadResponse = await _crmService.QueryLeads(leadQuery);
                    if (queryLeadResponse.Code != ResultCode.OK)
                    {
                        return leadNumber;
                    }

                    queryLeadDetails = queryLeadResponse.Data;
                    queryInfo = queryLeadDetails.info;

                    moreRecord = queryInfo.more_records;
                    leadNumber += queryInfo.count;
                    searchNumber++;
                }

                return leadNumber;
            }
            catch (Exception)
            {
                return leadNumber;
            }
        }

        public async Task<string> GetRandomAccountManagerId()
        {
            string accountId = string.Empty;
            try
            {
                var getActiveUsersResult = await _crmService.GetUserByTypes("ActiveUsers");
                if (getActiveUsersResult.Code != ResultCode.OK)
                {
                    return accountId;
                }

                var getUsersDetails = getActiveUsersResult.Data;
                var userList = getUsersDetails.users;

                var accountManagerIds = new List<string>();
                foreach (var user in userList)
                {
                    string userId = user.id;
                    string roleName = user.role.name;
                    if (roleName == "Account Manager")
                    {
                        accountManagerIds.Add(userId);
                    }
                }
                int numberOfManagers = accountManagerIds.Count();
                if (numberOfManagers == 0)
                {
                    return accountId;
                }

                var random = new Random();
                int randomNumber = random.Next(numberOfManagers);

                accountId = accountManagerIds[randomNumber];

                return accountId;
            }
            catch (Exception)
            {
                return accountId;
            }
        }

        public async Task<ApiResultDto<string>> SyncSMSToZohoCRM(SMSPayload smsPayload)
        {
            var apiResult = new ApiResultDto<string>()
            {
                Code = ResultCode.BadRequest,
                Message = REOConstants.CUSTOM_SSZ_400
            };

            try
            {

                var smsDetails = smsPayload.data;

                // STEP 1: Search JustCall Logs in CRM to see if it exists
                string criteria = $"SMS_Id:equals:{smsDetails.messageid}";
                var searchJustCallLogsResult = await _crmService.SearchJustCallLogs(criteria);
                bool logExist = false;
                if (searchJustCallLogsResult.Code == ResultCode.OK)
                {
                    logExist = true;
                }

                if (logExist)
                {
                    apiResult.Code = ResultCode.OK;
                    apiResult.Message = REOConstants.CUSTOM_SSZ_E01;
                    return apiResult;
                }

                // STEP 2: Create JustCall Logs in CRM

                // STEP 2.1: Handle Direction
                var direction = smsDetails.direction;
                string directionStr = string.Empty;

                // STEP 2.2: Handle Subject and Description
                string contactName = smsDetails.contact_name;
                string contactNumber = smsDetails.contact_number;
                string subject = string.Empty;
                string description = string.Empty;
                string smsContent = smsDetails.content;
                string justCallNumber = smsDetails.justcall_number;
                if (direction == "0")
                {
                    subject = $"SMS sent to {contactName} {contactNumber}";
                    description = $"Sent via: {justCallNumber}\n\n{smsContent}";
                    directionStr = "Outgoing";
                }
                else
                {
                    subject = $"New SMS from {contactName} {contactNumber}";
                    description = $"Received on: {justCallNumber}\n\n{smsContent}";
                    directionStr = "Incoming";
                }

                // STEP 2.3: Handle Log Time
                var dateTimeStr = smsDetails.datetime;
                var datetime = DateTime.Parse(dateTimeStr);
                var unspecifiedDateTime = DateTime.SpecifyKind(datetime, DateTimeKind.Unspecified);
                var tz = TimeZoneInfo.FindSystemTimeZoneById("AUS Eastern Standard Time");

                var auLogTime = TimeZoneInfo.ConvertTimeFromUtc(unspecifiedDateTime, tz);
                var auLogTimeStr = $"{auLogTime.ToString("yyyy-MM-ddTHH:mm:ss")}+11:00";

                // STEP 2.4: Handle MMS
                string isMMSStr = smsDetails.is_mms;
                bool isMMS = isMMSStr == "1" ? true : false;

                string mmsContent = string.Empty;
                if (smsDetails.mms != null && smsDetails.mms.Count() > 0)
                {
                    foreach (var mms in smsDetails.mms)
                    {
                        string mediaUrl = mms.media_url;
                        string content = mms.content_type;

                        mmsContent = $"{mediaUrl} ({content})";
                    }
                }

                // STEP 2.5: Handle Owner
                string ownerId = REOConstants.MattCrmId;
                int agentId = smsDetails.agent_id.Value;
                var getAgentByIdResult = await _justCallService.GetUserById(agentId);
                var agentDetails = getAgentByIdResult.Data.data;

                var agentEmail = agentDetails.email;

                var searchUserByEmail = await _crmService.SearchUserByEmail(agentEmail);

                if (searchUserByEmail.Code == ResultCode.OK)
                {
                    var userDetails = searchUserByEmail.Data.users[0];
                    ownerId = userDetails.id;
                }

                var justCallLogForCreation = new JustCallLogForCreation()
                {
                    SMS_Id = smsDetails.messageid.ToString(),
                    Subject = subject,
                    Contact_Name = contactName,
                    Contact_Number = contactNumber,
                    Log_Type = "SMS",
                    Email = smsDetails.contact_email,
                    Delivery_Status = smsDetails.delivery_status,
                    SMS_Content = smsDetails.content,
                    Direction = directionStr,
                    Log_Time = auLogTimeStr,
                    Description = description,
                    Is_MMS = isMMS,
                    MMS_Content = mmsContent,
                    Owner = ownerId,
                    JustCall_Number = justCallNumber
                };

                // STEP 2.6: Create JustCall Log
                var upsertRequest = new UpsertRequest<JustCallLogForCreation>();
                upsertRequest.data.Add(justCallLogForCreation);

                var createJustCallLogResponse = await _crmService.CreateJustCallLog(upsertRequest);

                apiResult.Code = ResultCode.OK;
                apiResult.Message = REOConstants.SSZ_200;
                return apiResult;

            }
            catch (Exception ex)
            {
                apiResult.Message = $"{ex.Message} - {ex.StackTrace}";
                return apiResult;
            }

        }

        public async Task<ApiResultDto<string>> MassSyncSMSToZohoCRM()
        {

            var apiResult = new ApiResultDto<string>()
            {
                Code = ResultCode.BadRequest,
                Message = REOConstants.CUSTOM_SSZ_400
            };

            try
            {

                var agentDictionary = new Dictionary<int, string>();
                agentDictionary.Add(120325, "4221896000000238013");
                agentDictionary.Add(120330, "4221896000000310001");
                agentDictionary.Add(120331, "4221896000000312001");
                agentDictionary.Add(120332, "4221896000001171001");
                agentDictionary.Add(120333, "4221896000014031001");
                agentDictionary.Add(120334, "4221896000014665001");
                agentDictionary.Add(120335, "4221896000018404001");
                agentDictionary.Add(129852, "4221896000026037001");
                agentDictionary.Add(150459, "4221896000055109001");
                agentDictionary.Add(156915, "4221896000062172001");

                for (int i = 401; i <= 443; i++)
                {
                    var getListSMSResult = await _justCallService.GetListOfSMS(i.ToString());

                    if (getListSMSResult.Code == ResultCode.OK)
                    {
                        Thread.Sleep(5000);
                        var smsList = getListSMSResult.Data.data;

                        foreach (var smsPayload in smsList)
                        {
                            var smsDetails = smsPayload;

                            // STEP 1: Search JustCall Logs in CRM to see if it exists
                            string criteria = $"SMS_Id:equals:{smsDetails.id}";
                            var searchJustCallLogsResult = await _crmService.SearchJustCallLogs(criteria);
                            bool logExist = false;
                            if (searchJustCallLogsResult.Code == ResultCode.OK)
                            {
                                logExist = true;
                            }

                            if (logExist)
                            {
                                apiResult.Code = ResultCode.OK;
                                apiResult.Message = REOConstants.CUSTOM_SSZ_E01;
                                continue;
                            }

                            // STEP 2: Create JustCall Logs in CRM

                            // STEP 2.1: Handle Direction
                            var direction = smsDetails.direction;
                            string directionStr = string.Empty;

                            // STEP 2.2: Handle Subject and Description
                            string contactName = smsDetails.contact_name;
                            string contactNumber = smsDetails.client_number;
                            string subject = string.Empty;
                            string description = string.Empty;
                            string smsContent = smsDetails.body;
                            string justCallNumber = smsDetails.justcall_number;
                            if (direction == "0")
                            {
                                subject = $"SMS sent to {contactName} {contactNumber}";
                                description = $"Sent via: {justCallNumber}\n\n{smsContent}";
                                directionStr = "Outgoing";
                            }
                            else
                            {
                                subject = $"New SMS from {contactName} {contactNumber}";
                                description = $"Received on: {justCallNumber}\n\n{smsContent}";
                                directionStr = "Incoming";
                            }

                            // STEP 2.3: Handle Log Time
                            var dateTimeStr = smsDetails.datetime;
                            var datetime = DateTime.Parse(dateTimeStr);
                            var tz = TimeZoneInfo.FindSystemTimeZoneById("AUS Eastern Standard Time");

                            var auLogTime = TimeZoneInfo.ConvertTimeFromUtc(datetime, tz);
                            var auLogTimeStr = $"{auLogTime.ToString("yyyy-MM-ddTHH:mm:ss")}+11:00";

                            // STEP 2.4: Handle MMS
                            string isMMSStr = smsDetails.is_mms;
                            bool isMMS = isMMSStr == "1" ? true : false;

                            string mmsContent = string.Empty;
                            if (smsDetails.mms != null && smsDetails.mms.Count() > 0)
                            {
                                foreach (var mms in smsDetails.mms)
                                {
                                    string mediaUrl = mms.media_url;
                                    string content = mms.content_type;

                                    mmsContent = $"{mediaUrl} ({content})";
                                }
                            }

                            // STEP 2.5: Handle Owner
                            string ownerId = REOConstants.MattCrmId;
                            int agentId = smsDetails.agent_id.Value;

                            if (agentDictionary.ContainsKey(agentId))
                            {
                                ownerId = agentDictionary[agentId];
                            }

                            var justCallLogForCreation = new JustCallLogForCreation()
                            {
                                SMS_Id = smsDetails.id.ToString(),
                                Subject = subject,
                                Contact_Name = contactName,
                                Contact_Number = contactNumber,
                                Log_Type = "SMS",
                                // Email = smsDetails.contact_email,
                                Delivery_Status = smsDetails.delivery_status,
                                SMS_Content = smsDetails.body,
                                Direction = directionStr,
                                Log_Time = auLogTimeStr,
                                Description = description,
                                Is_MMS = isMMS,
                                MMS_Content = mmsContent,
                                Owner = ownerId,
                                JustCall_Number = justCallNumber
                            };

                            // STEP 2.6: Create JustCall Log
                            var upsertRequest = new UpsertRequest<JustCallLogForCreation>();
                            upsertRequest.data.Add(justCallLogForCreation);

                            var createJustCallLogResponse = await _crmService.CreateJustCallLog(upsertRequest);
                        }
                    }
                }

                apiResult.Code = ResultCode.OK;
                apiResult.Message = REOConstants.CUSTOM_MSZ_200;
                return apiResult;
            }
            catch (Exception ex)
            {
                apiResult.Message = $"{ex.Message} - {ex.StackTrace}";
                return apiResult;
            }
        }

        public async Task<ApiResultDto<string>> SyncCallToZohoCRM(CallPayload callPayload)
        {
            var apiResult = new ApiResultDto<string>()
            {
                Code = ResultCode.BadRequest,
                Message = REOConstants.CUSTOM_SCZ_400
            };

            try
            {
                var callDetails = callPayload.data;

                // STEP 1: Search JustCall Logs in CRM to see if it exists
                string criteria = $"Call_Id:equals:{callDetails.callid}";
                var searchJustCallLogsResult = await _crmService.SearchJustCallLogs(criteria);
                bool logExist = false;
                if (searchJustCallLogsResult.Code == ResultCode.OK)
                {
                    logExist = true;
                }

                if (logExist)
                {
                    apiResult.Code = ResultCode.OK;
                    apiResult.Message = REOConstants.CUSTOM_SCZ_E01;
                    return apiResult;
                }

                // STEP 2: Create JustCall Logs in CRM

                // STEP 2.1: Handle Direction
                var direction = callDetails.direction;
                string directionStr = string.Empty;
                if (direction == "0")
                {
                    directionStr = "Outgoing";
                }
                else if (direction == "1")
                {
                    directionStr = "Incoming";
                }
                else if (direction == "2")
                {
                    directionStr = "Voicemail";
                }

                // STEP 2.2: Handle Subject and Description
                string contactName = callDetails.contact_name;
                string contactNumber = callDetails.contact_number;
                string subject = callDetails.subject;
                string description = callDetails.description;
                string justCallNumber = callDetails.called_via;
                string recordingUrl = callDetails.recording_url;
                string durationStr = callDetails.call_duration;
                int duration = callDetails.call_duration_sec.Value;
                string callStatus = callDetails.call_status;
                string callNotes = callDetails.notes;
                string callType = callDetails.type;
                string missedCallType = callDetails.missed_call_type;

                // STEP 2.3: Handle Log Time
                var dateTimeStr = callDetails.datetime;
                var datetime = DateTime.Parse(dateTimeStr);
                var tz = TimeZoneInfo.FindSystemTimeZoneById("AUS Eastern Standard Time");

                var auLogTime = TimeZoneInfo.ConvertTimeFromUtc(datetime, tz);
                var auLogTimeStr = $"{auLogTime.ToString("yyyy-MM-ddTHH:mm:ss")}+11:00";

                // STEP 2.4: Handle Forwarding
                var forwarding = callDetails.forwarded_number;
                var forwardNumber = forwarding.number;
                var forwardReason = forwarding.reason;
                var forwardReasonCode = forwarding.reason_code;

                // STEP 2.5: Handle Owner
                string ownerId = REOConstants.MattCrmId;
                int agentId = callDetails.agent_id.Value;
                var getAgentByIdResult = await _justCallService.GetUserById(agentId);
                var agentDetails = getAgentByIdResult.Data.data;

                var agentEmail = agentDetails.email;

                var searchUserByEmail = await _crmService.SearchUserByEmail(agentEmail);

                if (searchUserByEmail.Code == ResultCode.OK)
                {
                    var userDetails = searchUserByEmail.Data.users[0];
                    ownerId = userDetails.id;
                }

                if (agentId == 156915)
                {
                    ownerId = "4221896000062172001";
                }


                // STEP 2.6: Handle Call Type
                string callTypeStr = null;
                if (subject.Contains("Outbound Call", StringComparison.InvariantCultureIgnoreCase))
                {
                    callTypeStr = "Outbound Call";
                }
                else if (subject.Contains("Incoming Call", StringComparison.InvariantCultureIgnoreCase))
                {
                    callTypeStr = "Incoming Call";
                }
                else if (subject.Contains("Missed Call", StringComparison.InvariantCultureIgnoreCase))
                {
                    callTypeStr = "Missed Call";
                }
                else if (subject.Contains("Voicemail", StringComparison.InvariantCultureIgnoreCase))
                {
                    callTypeStr = "Voicemaill";
                }

                // STEP 2.7: Handle IVR
                var ivr = callDetails.ivr;
                string ivrDigit = null;
                string ivrDigitDesc = null;
                if (ivr != null)
                {
                    ivrDigit = ivr.digit;
                    ivrDigitDesc = ivr.digit_description;
                }

                // STEP 2.8: Missed Call Type:
                // 1 - Call arrived after your working hours
                // 2 - Call was not picked by any agent
                // 3 - Call never rang any of the available agents. Generally happens when an incoming caller hangs up while the welcome message is being played or IVR options are being presented
                string missedCallTypeStr = string.Empty;
                switch (missedCallType)
                {
                    case "1":
                        missedCallTypeStr = "Call arrived after working hours";
                        break;
                    case "2":
                        missedCallTypeStr = "Call not picked by any agent";
                        break;
                    case "3":
                        missedCallTypeStr = "Call never rang any of the available agents";
                        break;
                    default:
                        break;
                }

                var justCallLogForCreation = new JustCallLogForCreation()
                {
                    Call_Id = callDetails.callid.ToString(),
                    Subject = subject,
                    Log_Type = "Call",
                    Contact_Name = contactName,
                    Contact_Number = contactNumber,
                    Email = callDetails.contact_email,
                    Direction = directionStr,
                    Log_Time = auLogTimeStr,
                    Description = description,
                    Recording_URL = recordingUrl,
                    Owner = ownerId,
                    JustCall_Number = justCallNumber,
                    Call_Duration = durationStr,
                    Call_Duration_in_seconds = duration,
                    Forward_Number = forwardNumber,
                    Forward_Reason = forwardReason,
                    Call_Status = callStatus,
                    Call_Type = callTypeStr,
                    Call_Notes = callNotes,
                    IVR_Digit = ivrDigit,
                    IVR_Digit_Description = ivrDigitDesc,
                    Missed_Call_Type = missedCallTypeStr
                };

                // STEP 2.6: Create JustCall Log
                var upsertRequest = new UpsertRequest<JustCallLogForCreation>();
                upsertRequest.data.Add(justCallLogForCreation);

                var createJustCallLogResponse = await _crmService.CreateJustCallLog(upsertRequest);

                apiResult.Code = ResultCode.OK;
                apiResult.Message = REOConstants.SCZ_200;
                return apiResult;

            }
            catch (Exception ex)
            {
                apiResult.Message = $"{ex.Message} - {ex.StackTrace}";
                return apiResult;
            }
        }

        public async Task<ApiResultDto<string>> MassSyncCallsToZohoCRM()
        {
            var apiResult = new ApiResultDto<string>()
            {
                Code = ResultCode.BadRequest,
                Message = REOConstants.CUSTOM_SSZ_400
            };

            try
            {

                var agentDictionary = new Dictionary<int, string>();
                agentDictionary.Add(120325, "4221896000000238013");
                agentDictionary.Add(120330, "4221896000000310001");
                agentDictionary.Add(120331, "4221896000000312001");
                agentDictionary.Add(120332, "4221896000001171001");
                agentDictionary.Add(120333, "4221896000014031001");
                agentDictionary.Add(120334, "4221896000014665001");
                agentDictionary.Add(120335, "4221896000018404001");
                agentDictionary.Add(129852, "4221896000026037001");
                agentDictionary.Add(150459, "4221896000055109001");
                agentDictionary.Add(156915, "4221896000062172001");

                for (int i = 651; i <= 750; i++)
                {
                    var getCallListResult = await _justCallService.GetListOfCalls(i.ToString());

                    if (getCallListResult.Code == ResultCode.OK)
                    {   
                        var callList = getCallListResult.Data.data;

                        foreach (var callPayload in callList)
                        {
                            var callDetails = callPayload;

                            // STEP 1: Search JustCall Logs in CRM to see if it exists
                            string criteria = $"Call_Id:equals:{callDetails.id}";
                            var searchJustCallLogsResult = await _crmService.SearchJustCallLogs(criteria);
                            bool logExist = false;
                            if (searchJustCallLogsResult.Code == ResultCode.OK)
                            {
                                logExist = true;
                            }

                            if (logExist)
                            {
                                apiResult.Code = ResultCode.OK;
                                apiResult.Message = REOConstants.CUSTOM_SSZ_E01;
                                continue;
                            }

                            // STEP 2: Create JustCall Logs in CRM

                            // STEP 2.1: Get all fields from JustCall Logs
                            int callId = callDetails.id;
                            string contactNumber = callDetails.contact_number;
                            string contactName = callDetails.contact_name;
                            if (string.IsNullOrEmpty(contactName))
                            {
                                contactName = "New JustCall";
                            }
                            string justCallNumber = callDetails.justcall_number;
                            string type = callDetails.type;
                            string status = callDetails.status;
                            string logTime = callDetails.time_utc;
                            string duration = callDetails.duration;
                            string friendlyDuration = callDetails.friendly_duration;
                            string notes = callDetails.notes;
                            string rating = callDetails.rating;
                            string disposition_Code = callDetails.disposition_code;
                            string missedCallType = callDetails.missed_call_type;
                            string recording = callDetails.recording;
                            int agentId = callDetails.agent_id;
                            string callUrl = callDetails.call_info_url;
                            string direction = callDetails.direction;

                            // STEP 2.2: Handle Direction: 0 - Outgoing, 1 - Incoming
                            string directionStr = string.Empty;

                            if (direction == "0")
                            {
                                directionStr = "Outgoing";
                            }
                            else if (direction == "1")
                            {
                                directionStr = "Incoming";
                            }

                            // STEP 2.3: Call Type: 2 - Outgoing calls, 3 - Answered incoming calls, 4 - Missed calls
                            string callType = string.Empty;
                            string subject = string.Empty;
                            switch (type)
                            {
                                case "2":
                                    callType = "Outbound Call";
                                    subject = $"Outbound Call to {contactName} ({friendlyDuration})";
                                    break;
                                case "3":
                                    callType = "Incoming Call";
                                    subject = $"Incoming call from {contactName} ({friendlyDuration})";
                                    break;
                                case "4":
                                    callType = "Missed Call";
                                    subject = $"Missed call from {contactName}";
                                    break;
                                default:
                                    break;
                            }

                            switch (direction)
                            {
                                case "0":
                                    directionStr = "Outgoing";
                                    break;
                                case "1":
                                    directionStr = "Incoming";
                                    break;
                            }

                            // STEP 2.4: Missed Call Type:
                            // 1 - Call arrived after your working hours
                            // 2 - Call was not picked by any agent
                            // 3 - Call never rang any of the available agents. Generally happens when an incoming caller hangs up while the welcome message is being played or IVR options are being presented
                            string missedCallTypeStr = string.Empty;
                            switch (missedCallType)
                            {
                                case "1":
                                    missedCallTypeStr = "Call arrived after working hours";
                                    break;
                                case "2":
                                    missedCallTypeStr = "Call not picked by any agent";
                                    break;
                                case "3":
                                    missedCallTypeStr = "Call never rang any of the available agents";
                                    break;
                                default:
                                    break;
                            }

                            // STEP 2.6: Handle Log Time
                            var dateTimeStr = callDetails.time;
                            var datetime = DateTime.Parse(dateTimeStr);
                            var tz = TimeZoneInfo.FindSystemTimeZoneById("AUS Eastern Standard Time");
                            var auLogTime = TimeZoneInfo.ConvertTimeFromUtc(datetime, tz);
                            var auLogTimeStr = $"{auLogTime.ToString("yyyy-MM-ddTHH:mm:ss")}+11:00";

                            // STEP 2.7: Handle Owner
                            string ownerId = REOConstants.MattCrmId;

                            if (agentDictionary.ContainsKey(agentId))
                            {
                                ownerId = agentDictionary[agentId];
                            }

                            var justCallLogForCreation = new JustCallLogForCreation()
                            {
                                Call_Id = callDetails.id.ToString(),
                                Subject = subject,
                                Log_Type = "Call",
                                Contact_Name = contactName,
                                Contact_Number = contactNumber,
                                // Email = callDetails.contact_email,
                                Direction = directionStr,
                                Log_Time = auLogTimeStr,
                                Call_Type = callType,
                                Owner = ownerId,
                                JustCall_Number = justCallNumber,
                                Call_Duration = friendlyDuration,
                                Call_Duration_in_seconds = int.Parse(duration),
                                Recording_URL = callUrl,
                                Missed_Call_Type = missedCallTypeStr,
                            };

                            // STEP 2.6: Create JustCall Log
                            var upsertRequest = new UpsertRequest<JustCallLogForCreation>();
                            upsertRequest.data.Add(justCallLogForCreation);

                            var createJustCallLogResponse = await _crmService.CreateJustCallLog(upsertRequest);
                        }
                    }
                }

                apiResult.Code = ResultCode.OK;
                apiResult.Message = REOConstants.CUSTOM_MSZ_200;
                return apiResult;
            }
            catch (Exception ex)
            {
                apiResult.Message = $"{ex.Message} - {ex.StackTrace}";
                return apiResult;
            }

        }

        public async Task<ApiResultDto<string>> SendTnzSMSAndCreateLog(SendTnzSmsRequest tnzRequest)
        {
            var apiResult = new ApiResultDto<string>()
            {
                Code = ResultCode.BadRequest,
                Message = REOConstants.STS_400
            };

            try
            {

                // STEP 1: Send SMS using TNZ API
                string token = tnzRequest.Token;
                var smsRequest = tnzRequest.SmsRequest;
                var messageData = smsRequest.MessageData;
                var destination = messageData.Destinations[0];
                string phone = destination.Recipient;

                string message = messageData.Message;

                var sendTnzSMSResponse = await _tnzService.SendSMS(token, smsRequest);
                if (sendTnzSMSResponse.Code != ResultCode.OK)
                {
                    return apiResult;
                }

                // STEP 2: Create TNZ Log in Zoho CRM
                string fullName = tnzRequest.FullName;
                string userId = tnzRequest.UserId;
                string userName = tnzRequest.UserName;
                string userPhone = tnzRequest.UserPhone;
                string messageId = sendTnzSMSResponse.Data.MessageID;
                string relatedLead = tnzRequest.RelatedLead;
                string relatedContact = tnzRequest.RelatedContact;

                var tnzLogForCreation = new TNZLogForCreation()
                {
                    Message_ID = messageId,
                    Owner = userId,
                    Direction = "Outgoing",
                    Detail = message,
                };

                string noteModule = "";
                string parentId = "";
                if (!string.IsNullOrEmpty(relatedLead))
                {
                    tnzLogForCreation.Related_Lead = relatedLead;
                    noteModule = "Leads";
                    parentId = relatedLead;
                }

                if (!string.IsNullOrEmpty(relatedContact))
                {
                    tnzLogForCreation.Related_Contact = relatedContact;
                    noteModule = "Contacts";
                    parentId = relatedContact;
                }
                var upsertRequest = new UpsertRequest<TNZLogForCreation>();
                upsertRequest.data.Add(tnzLogForCreation);

                var upsertResponse = await _crmService.CreateTNZLog(upsertRequest);
                if (upsertResponse.Code != ResultCode.OK)
                {
                    apiResult.Message = REOConstants.CTZ_400;
                    return apiResult;
                }

                // STEP 3: Create Note for Contact and Lead
                if (!string.IsNullOrEmpty(noteModule))
                {
                    string noteTitle = $"TNZ SMS sent to {fullName} {phone}";
                    string noteContent = $"Sent via: {userName} {userPhone}\n\n{message}";


                    var noteForCreation = new NoteForCreation()
                    {
                        se_module = noteModule,
                        Note_Title = noteTitle,
                        Note_Content = noteContent,
                        Parent_Id = parentId
                    };

                    var createNoteRequest = new UpsertRequest<NoteForCreation>();
                    createNoteRequest.data.Add(noteForCreation);
                    var createNoteResponse = _crmService.CreateNote(createNoteRequest);

                }

                apiResult.Code = ResultCode.OK;
                apiResult.Message = REOConstants.STS_200;
                return apiResult;

            }
            catch (Exception ex)
            {
                apiResult.Message = $"{ex.Message} - {ex.StackTrace}";
                return apiResult;
            }
        }

        public async Task<ApiResultDto<List<List<string>>>> 
            GetTimelineAndNote(string recordModule, string recordId)
        {

            var apiResult = new ApiResultDto<List<List<string>>>()
            {
                Code = ResultCode.BadRequest,
                Message = REOConstants.GTAN_400
            };
            var tableData = new List<List<string>>();

            try
            {
                var newZpComments = new List<List<string>>();
                var tableRow = new List<TimelineAndNoteRow>();
                int tableIndex = 0;

                //// STEP 1: Get Record Notes
                //var getRecordNotesResponse = await _crmService.GetNotes(recordModule, recordId);
                //if (getRecordNotesResponse.Code == ResultCode.OK)
                //{
                //    var recordNotes = getRecordNotesResponse.Data.data;
                //    foreach (var note in recordNotes)
                //    {
                //        var row = new TimelineAndNoteRow()
                //        {
                //            Type = "Note",
                //        };
                //        tableIndex++;
                //        row.Index = tableIndex;
                //        // Handle Note Title
                //        string noteTitle = note.Note_Title;
                //        row.Subject = noteTitle;
                //        // Handle Note Content
                //        string noteContent = note.Note_Content;
                //        row.Content = noteContent;
                //        // Handle Created By
                //        var createdBy = note.Created_By;
                //        string createdByName = createdBy.name;
                //        string createdByEmail = createdBy.email;
                //        row.CreatedBy = createdByName;
                //        // Handle Created Time
                //        var createdTime = note.Created_Time;
                //        row.CreatedTime = createdTime;
                //        tableRow.Add(row);
                //    }
                //}

                // STEP 2: Get Record Timeline
                var getRecordTimelineResponse = await 
                    _crmService.GetTimeline(recordModule, recordId);
                string getTimelineStr = JsonConvert.SerializeObject(getRecordTimelineResponse);

                var timelines = getRecordTimelineResponse.Data.__timeline;
                foreach (var timeline in timelines)
                {
                    var row = new TimelineAndNoteRow()
                    {
                        Type = "Timeline",
                    };

                    string subject = string.Empty;
                    string content = string.Empty;

                    // Step 2.1: Handle Subject and Content
                    var automationDetails = timeline.automation_details;
                    var record = timeline.record;
                    var module = new Module();
                    string moduleApiName = string.Empty;
                    var recordName = string.Empty;
                    if (record != null)
                    {
                        module = record.module;
                        moduleApiName = module.api_name;
                        recordModule = record.name;
                    }

                    string automationType = string.Empty;
                    if (automationDetails != null)
                    {
                        automationType = automationDetails.type;
                    }    

                    if (automationType == "workflow_rule")
                    {
                        var rule = automationDetails.rule;
                        string ruleName = rule.name;
                        subject = $"Workflow Rule: <b>{ruleName}</b>";
                        if (moduleApiName == "Deluge")
                        {
                            content = $"Trigger Function: <b>{recordName}</b>";
                        }
                    }    
                    else if (automationType == "path_finder")
                    {
                        var rule = automationDetails.rule;
                        string ruleName = rule.name;
                        subject = ruleName;
                        var pathFinder = automationDetails.path_finder;
                        var state = pathFinder.state;
                        string stateName = state.name;
                        content = $"{stateName}: {recordName}";
                    }    
                    else if (string.IsNullOrEmpty(automationType))
                    {
                        if (moduleApiName == "Notes")
                        {
                            // No need to handle if API Name is Note
                            continue;
                        }
                    }    

                    var doneBy = timeline.done_by;
                    string doneByName = doneBy.name;
                    row.CreatedBy = doneByName;


                }


                return apiResult;

            }
            catch (Exception ex)
            {
                apiResult.Message = $"{ex.Message} - {ex.StackTrace}";
                return apiResult;
            }

            

        }

        public async Task<ApiResultDto<GetProductDataTableResponse>> GetProductDataTable(string productSkus)
        {

            var apiResult = new ApiResultDto<GetProductDataTableResponse>()
            {
                Code = ResultCode.BadRequest,
                Message = REOConstants.GPDT_400
            };

            try
            {

                // STEP 1: Get Product Codes
                var productDict = new Dictionary<string, ProductQueryModel>();
                var productCodes = productSkus.Split(',').ToHashSet();
                var productCodesArr = new List<string>();
                var finalCategories = new HashSet<string>();
                var finalProductData = new List<ProductQueryModel>();

                List<int> targetWidths = new List<int>();
                List<int> targetHeights = new List<int>();
                List<int> targetDepths = new List<int>();

                foreach (var productCode in productCodes)
                {

                    string productSku = productCode.Trim().ToUpper();
                    productCodesArr.Add(productSku);

                    string productQuery = $"Select Product_Name, Product_Code, Image_URL, Product_Dimensions, Cost_Price, Unit_Price, " +
                        $"Qty_in_Stock, KE_Price, Description, Final_Category, Supplier from Products where Product_Code = '{productSku}' and Product_Active = true";

                    var queryProductsResponse = await _crmService.QueryProducts(productQuery);

                    if (queryProductsResponse.Code == ResultCode.OK) {

                        var productData = queryProductsResponse.Data.data[0];
                        string productId = productData.id;

                        string productDimensions = productData.Product_Dimensions;

                        var targetDimensions = StringHelpers.ExtractDimensions(productDimensions);

                        int targetWidth = targetDimensions.Width;
                        int targetHeight = targetDimensions.Height;
                        int targetDepth = targetDimensions.Depth;

                        targetWidths.Add(targetWidth);
                        targetHeights.Add(targetHeight);
                        targetDepths.Add(targetDepth);

                        string finalCategory = productData.Final_Category;
                        if (!string.IsNullOrEmpty(finalCategory))
                        {
                            finalCategories.Add(finalCategory);
                        }
                        productDict.Add(productId, productData);
                    }
                }

                foreach (var category in finalCategories)
                {
                    string productQuery = $"Select Product_Name, Product_Code, Image_URL, Product_Dimensions, Cost_Price, Unit_Price, " +
                        $"Qty_in_Stock, KE_Price, Description, Final_Category, Supplier from Products where Final_Category = '{category}'  and Product_Active = true";

                    var queryProductsResponse = await _crmService.QueryProducts(productQuery);
                    if (queryProductsResponse.Code == ResultCode.OK)
                    {
                        var queryProducts = queryProductsResponse.Data.data;

                        foreach (var product in queryProducts)
                        {
                            string productId = product.id;
                            if (!productDict.ContainsKey(productId))
                            {
                                productDict.Add(productId, product);
                            }
                        }
                    }
                }

                var tableData = new List<List<string>>();
                int index = 0;
                foreach (var key in productDict.Keys)
                {
                    index++;
                    var productData = productDict[key];
                    string productId = productData.id;
                    string productUrl = REOConstants.ProductPrefixURL + "/" + productId;
                    var productRow = new List<string>();

                    // Handle No
                    string no = index.ToString();
                    string addItemButton = $"<button type=\"button\" id=\"btn-product-{productId}\" class=\"btn btn-primary btn-sm\">Add Item</button>";
                    no = $"{no}";
                    productRow.Add(no);

                    // Handle Product Image
                    string imageColumn = "";
                    string productName = productData.Product_Name;
                    string productImage = productData.Image_URL;
                    string dimension = productData.Product_Dimensions;
                    var qtyInStock = productData.Qty_in_Stock;

                    if (!string.IsNullOrEmpty(productImage))
                    {
                        imageColumn = $"<img src='{productImage}' alt='{productName}' style='max-width: 150px;'/>";
                    }
                    imageColumn = $"<a href='{productUrl}' target='_blank'>{imageColumn}</a>";
                    imageColumn = $"{imageColumn}<br/>{addItemButton}";
                    imageColumn = $"<div style=\"display: flex; flex-direction: column; align-items: center; justify-content: center;\">{imageColumn}</div>";
                    productRow.Add(imageColumn);

                    // Handle Item Name
                    string productCode = productData.Product_Code;
                    string itemColumn = $"<a href='{productUrl}' target='_blank'>" +
                        $"<div style=\"font-size: 1.2rem; font-weight: bold; margin-bottom: 0.5rem;\">" +
                        $"{productName}</div></a>";
                    string productInfo = "";
                    if (!string.IsNullOrEmpty(productCode))
                    {
                        productInfo += "<b>SKU</b>: " + productCode + "";
                    }
                    if (!string.IsNullOrEmpty(dimension))
                    {
                        if (string.IsNullOrEmpty(productInfo))
                        {
                            productInfo = "<b>Dimension</b>: " + dimension;
                        }
                        else
                        {
                            productInfo += "<br/><b>Dimension</b>: " + dimension;
                        }
                    }

                    if (qtyInStock != null)
                    {
                        string qtyInfo = "";
                        if (qtyInStock <= 0)
                        {
                            qtyInfo = $"<span class=\"text-danger\"><b>Out of Stock, {qtyInStock} units</b></span>";
                        }
                        else
                        {
                            qtyInfo = $"<span class=\"text-success\"><b>In Stock, {qtyInStock} units</b></span>";
                        }
                        if (string.IsNullOrEmpty(productInfo))
                        {
                            productInfo = qtyInfo;
                        }
                        else
                        {
                            productInfo += $"<br/>{qtyInfo}";
                        }
                    }
                    if (!string.IsNullOrEmpty(productInfo))
                    {
                        productInfo = $"<p>{productInfo}</p>";
                        itemColumn += $"<p>{productInfo}</p>";
                    }
                    itemColumn = $"{itemColumn}";

                    productRow.Add(itemColumn);

                    // Handle Supplier
                    string supplierColumn = "";
                    string supplier = productData.Supplier;
                    if (!string.IsNullOrEmpty(supplier))
                    {
                        supplierColumn = supplier;
                    }
                    productRow.Add(supplierColumn);
                    productData.Supplier = supplier;

                    // Handle Cost Price
                    string priceColumn = "";
                    var costPrice = productData.Cost_Price;
                    var unitPrice = productData.Unit_Price;
                    string costPriceStr = "";
                    if (costPrice != null)
                    {
                        costPriceStr = $"<b>{costPrice.Value.ToString("#,##0.##")} AUD</b>";
                    }
                    string unitPriceStr = "";
                    if (unitPrice != null)
                    {
                        unitPriceStr = $"<b>{unitPrice.Value.ToString("#,##0.##")} AUD</b>";
                    }
                    priceColumn = $"<p>Cost Price: {costPriceStr}</p>" +
                        $"<p>Unit Price: {unitPriceStr}</p>";
                    productRow.Add(priceColumn);

                    //// Handle Description
                    //string productDescription = productData.Description;
                    //string descColumn = $"<div id=\"description{index}\" class=\"truncated-description\"></div>";
                    //if (!string.IsNullOrEmpty(productDescription))
                    //{
                    //    descColumn = $"<div id=\"description{index}\" class=\"truncated-description\">{productDescription}</div>" +
                    //        $"<button class=\"show-more-button btn btn-link\" data-target=\"description{index}\">Show More</button>";
                    //}
                    //productRow.Add(descColumn);

                    // Handle Dimension Match
                    string productDimensions = productData.Product_Dimensions;
                    var currentDimensions = StringHelpers.ExtractDimensions(productDimensions);

                    int productWidth = currentDimensions.Width;
                    int productHeight = currentDimensions.Height;
                    int productDepth = currentDimensions.Depth;

                    productData.Width = productWidth;
                    productData.Height = productHeight;
                    productData.Depth = productDepth;

                    int diIndex = 0;

                    var productSimilarities = new Dictionary<string, string>();
                    foreach (var targetWidth in targetWidths)
                    {
                        int targetHeight = targetHeights[diIndex];
                        int targetDepth = targetDepths[diIndex];
                        double dimensionMatch = StringHelpers.CalculateDimensionSimilarity(targetWidth, targetDepth, targetHeight,
                            productWidth, productDepth, productHeight);
                        productSimilarities.Add(productCodesArr[diIndex], dimensionMatch.ToString("#,##0.##"));
                        diIndex++;
                    }
                    productData.DSim = productSimilarities;

                    productRow.Add(string.Join("<br/>", productSimilarities.Select(kv => $"<b>{kv.Value}</b> ({kv.Key})")));

                    tableData.Add(productRow);
                    finalProductData.Add(productData);
                }
                                
                var tableDataResponse = new GetProductDataTableResponse();
                tableDataResponse.TableData = tableData;
                tableDataResponse.ProductData = finalProductData;

                apiResult.Code = ResultCode.OK;
                apiResult.Message = REOConstants.GPDT_400;
                apiResult.Data = tableDataResponse;

                return apiResult;
            }
            catch (Exception ex)
            {
                apiResult.Message = $"{ex.Message} - {ex.StackTrace}";
                return apiResult;
            }

        }

        public async Task<ApiResultDto<EasyAddQuoteResponse>> EasyAddQuote_Deal(EasyAddQuoteRequest easyAddQuoteRequest)
        {
            var apiResult = new ApiResultDto<EasyAddQuoteResponse>()
            {
                Code = ResultCode.BadRequest,
                Message = REOConstants.EAQ_400
            };

            try
            {

                string dealId = easyAddQuoteRequest.DealId;
                decimal discount = easyAddQuoteRequest.Discount.Value;
                var quoteItems = easyAddQuoteRequest.QuoteItems;

                // STEP 1: Get Deal by Id
                var getDealByIdResponse = await _crmService.GetDealById(easyAddQuoteRequest.DealId);
                var dealDetails = getDealByIdResponse.Data.data[0];

                // STEP 2: Extract Deal Details and Handle Quote Request Body
                string dealName = dealDetails.Deal_Name;
                var quoteForCreation = new QuoteForCreation();

                // Handle Quote Name
                string quoteSubject = "";
                var getCurrentTimeRequest = new GetCurrentTimeRequest()
                {
                    TimeZone = "AUS Eastern Standard Time",
                    Format = "dd-MM-yyyy"
                };

                string currentDate = DateTimeHelpers.GetCurrentTime(getCurrentTimeRequest);
                string quotePrefix = $"Quote for Deal {dealName} {currentDate}";

                // Get all Quote with prefix
                string quoteQuery = $"select Subject from Quotes where Subject like '{quotePrefix}%'";
                var queryQuotesResponse = await _crmService.QueryQuotes(quoteQuery);

                if (queryQuotesResponse.Code != ResultCode.OK)
                {
                    quoteSubject = $"{quotePrefix} #1";
                }
                else
                {
                    var quoteData = queryQuotesResponse.Data.data;
                    for (int i = 0; i < 100; i++)
                    {
                        quoteSubject = $"{quotePrefix} #{i + 1}";
                        foreach (var quote in quoteData)
                        {
                            string quoteSubjectStr = quote.Subject;
                            if (quoteSubjectStr == quoteSubject)
                            {
                                continue;
                            }
                        }
                        break;
                    }
                }    
                

                // Handle Contact Name
                string dealContactId = "";
                var dealContact = dealDetails.Contact_Name;
                if (dealContact != null)
                {
                    dealContactId = dealContact.id;
                }

                // Handle Account Name
                string dealAccountId = "";
                var dealAccount = dealDetails.Account_Name;
                if (dealAccount!= null)
                {
                    dealAccountId = dealAccount.id;
                }

                // Handle Currency
                string currency = dealDetails.Currency;

                // Handle Quote Items
                var quotedItems = new List<CreateQuoteItem>();
                foreach (var quoteItem in quoteItems)
                {
                    string productName = quoteItem.Product_Name;
                    decimal quantity = quoteItem.Quantity.Value;
                    decimal listPrice = quoteItem.List_Price.Value;
                    decimal costPrice = quoteItem.Cost_Price.Value;

                    var quotedItem = new CreateQuoteItem()
                    {
                        Product_Name = productName,
                        Quantity = quantity,
                        List_Price = listPrice,
                        Cost_Price = costPrice,
                    };
                    quotedItems.Add(quotedItem);
                }

                // Handle Address
                var getContactByIdResponse = await _crmService.GetContactById(dealContactId);
                var contactData = getContactByIdResponse.Data.data;
                GetContactByIdData contactDetails = null;
                if (contactData != null)
                {
                    contactDetails = contactData[0];
                }
                if (contactDetails != null)
                {
                    string billingStreet = contactDetails.Billing_Street;
                    quoteForCreation.Billing_Street = billingStreet;
                    string billingCity = contactDetails.Billing_City;
                    quoteForCreation.Billing_City = billingCity;
                    string billingState = contactDetails.Billing_State;
                    quoteForCreation.Billing_State = billingState;
                    string billingCode = contactDetails.Billing_Code;
                    quoteForCreation.Billing_Code = billingCode;
                    string billingCountry = contactDetails.Billing_Country;
                    quoteForCreation.Billing_Country = billingCountry;
                    string shippingStreet = contactDetails.Shipping_Street;
                    quoteForCreation.Shipping_Street = shippingStreet;
                    string shippingCity = contactDetails.Shipping_City;
                    quoteForCreation.Shipping_City = shippingCity;
                    string shippingState = contactDetails.Shipping_State;
                    quoteForCreation.Shipping_State = shippingState;
                    string shippingcode = contactDetails.Shipping_Code;
                    quoteForCreation.Shipping_Code = shippingcode;
                    string shippingCountry = contactDetails.Shipping_Country;
                    quoteForCreation.Shipping_Country = shippingCountry;
                }    

                quoteForCreation.Subject = quoteSubject;
                quoteForCreation.Account_Name = dealAccountId;
                quoteForCreation.Contact_Name = dealContactId;
                quoteForCreation.Quote_Stage = "Negotiating";
                quoteForCreation.Currency = currency;
                quoteForCreation.Deal_Name = dealId;
                quoteForCreation.Quoted_Items = quotedItems;
                quoteForCreation.Discount = discount;

                // STEP 3: Create Quote
                var upsertData = new UpsertRequest<QuoteForCreation>();
                upsertData.data.Add(quoteForCreation);
                var createQuoteResponse = await _crmService.CreateQuote(upsertData);

                if (createQuoteResponse.Code == ResultCode.OK)
                {
                    var quoteId = createQuoteResponse.Data.data[0].details.id;
                    var easyAddQuoteResponse = new EasyAddQuoteResponse()
                    {
                        QuoteId = quoteId
                    };
                    apiResult.Code = ResultCode.OK;
                    apiResult.Message = REOConstants.EAQ_200;
                    apiResult.Data = easyAddQuoteResponse;
                    return apiResult;
                }

                return apiResult;

            }
            catch (Exception ex)
            {
                apiResult.Message = $"{ex.Message} - {ex.StackTrace}";
                return apiResult;
            }
        }

        public async Task<ApiResultDto<GetQuoteTableResponse>> GetQuoteDataTable(string quoteId)
        {

            var apiResult = new ApiResultDto<GetQuoteTableResponse>()
            {
                Code = ResultCode.BadRequest,
                Message = REOConstants.GPDT_400
            };

            var quoteTableResponse = new GetQuoteTableResponse();

            try
            {

                var getQuoteByIdResponse = await _crmService.GetQuoteById(quoteId);

                var quoteDetails = getQuoteByIdResponse.Data.data[0];

                var quoteItems = quoteDetails.Quoted_Items;

                var tableData = new List<List<object>>();

                var productDatas = new List<ProductDataModel>();

                // STEP 3: Handle Quoted Items
                int index = 0;
                foreach (var item in quoteItems)
                {
                    index++;

                    var productData = new ProductDataModel();
                    var productRow = new List<object>();

                    var itemProduct = item.Product_Name;
                    string productId = itemProduct.id;
                    productData.ProductId = productId;

                    var getProductByIdResponse = await _crmService.GetProductById(productId);
                    var productDetails = getProductByIdResponse.Data.data[0];

                    string productUrl = REOConstants.ProductPrefixURL + "/" + productId;
                    string productName = productDetails.Product_Name;

                    string finalCategory = productDetails.Final_Category;

                    // Step 3.1: Handle No
                    string no = index.ToString();
                    string lineItemId = item.id;
                    no = $"{no}";
                    productData.LineItemId = lineItemId;
                    productRow.Add(no);

                    string replaceItemButton = string.Empty;
                    if (!string.IsNullOrEmpty(finalCategory))
                    {
                        replaceItemButton = $"<button type=\"button\" id=\"btn-product-{productId}\" data-line-item-id=\"{lineItemId}\" class=\"btn btn-success btn-sm\">Replace</button>";
                    }
                    else
                    {
                        replaceItemButton = $"<button type=\"button\" id=\"btn-product-{productId}\" data-line-item-id=\"{lineItemId}\" class=\"btn btn-success btn-sm\" disabled>Replace</button>";
                    }    

                    // Step 3.2: Handle Product Image
                    string imageColumn = "";
                    string productImage = productDetails.Image_URL;
                    string dimension = productDetails.Product_Dimensions;
                    var qtyInStock = productDetails.Qty_in_Stock;

                    if (!string.IsNullOrEmpty(productImage))
                    {
                        imageColumn = $"<img src='{productImage}' alt='{productName}' style='max-height: 80px;'/>";
                    }
                    imageColumn = $"<a href='{productUrl}' target='_blank'>{imageColumn}</a>";
                    imageColumn = $"{imageColumn}<br/>{replaceItemButton}";
                    imageColumn = $"<div style=\"display: flex; flex-direction: column; align-items: center; justify-content: center;\">{imageColumn}</div>";
                    productRow.Add(imageColumn);

                    // Step 3.3: Handle Item Name
                    string productCode = productDetails.Product_Code;
                    string itemColumn = $"<a href='{productUrl}' target='_blank'>" +
                        $"<div style=\"font-weight: bold; margin-bottom: 0.5rem;\">" +
                        $"{productName}</div></a>";
                    string productInfo = "";
                    if (!string.IsNullOrEmpty(productCode))
                    {
                        productInfo += "<b>SKU</b>: " + productCode + "";
                    }

                    //if (!string.IsNullOrEmpty(dimension))
                    //{
                    //    if (string.IsNullOrEmpty(productInfo))
                    //    {
                    //        productInfo = "<b>Dimension</b>: " + dimension;
                    //    }
                    //    else
                    //    {
                    //        productInfo += "<br/><b>Dimension</b>: " + dimension;
                    //    }
                    //}

                    if (qtyInStock != null)
                    {
                        string qtyInfo = "";
                        if (qtyInStock <= 0)
                        {
                            qtyInfo = $"<span class=\"text-danger\"><b>Out of Stock, {qtyInStock} units</b></span>";
                        }
                        else
                        {
                            qtyInfo = $"<span class=\"text-success\"><b>In Stock, {qtyInStock} units</b></span>";
                        }
                        if (string.IsNullOrEmpty(productInfo))
                        {
                            productInfo = qtyInfo;
                        }
                        else
                        {
                            productInfo += $"<br/>{qtyInfo}";
                        }
                    }
                    if (!string.IsNullOrEmpty(productInfo))
                    {
                        productInfo = $"<p>{productInfo}</p>";
                        itemColumn += $"<p>{productInfo}</p>";
                    }
                    itemColumn = $"{itemColumn}";

                    productRow.Add(itemColumn);

                    // Handle Supplier Column
                    string supplierColumn = "";
                    string supplier = productDetails.Supplier;
                    if (!string.IsNullOrEmpty(supplier))
                    {
                        supplierColumn = supplier;
                    }
                    productRow.Add(supplierColumn);

                    // Step 3.4: Handle Price
                    // Step 3.4.1: Handle Cost Price Column
                    string priceColumn = "";
                    var costPrice = item.Cost_Price;
                    
                    string costPriceStr = "";
                    if (costPrice == null)
                    {
                        costPrice = 0;
                    }
                    productRow.Add("$" + costPrice.Value.ToString("N0"));

                    var unitPrice = item.List_Price;
                    if (unitPrice == null)
                    {
                        unitPrice = 0;
                    }
                    productRow.Add("$" + unitPrice.Value.ToString("N0"));

                    // Step 3.5: Handle Dimension
                    var currentDimensions = StringHelpers.ExtractDimensions(dimension);
                    int productWidth = currentDimensions.Width;
                    int productHeight = currentDimensions.Height;
                    int productDepth = currentDimensions.Depth;

                    // Step 3.5.1: Handle Height
                    productRow.Add(productHeight);

                    // Step 3.5.2: Handle Width
                    productRow.Add(productWidth);

                    // Step 3.5.3: Handle Depth
                    productRow.Add(productDepth);
                    tableData.Add(productRow);
                    productDatas.Add(productData);

                }

                quoteTableResponse.TableData = tableData;
                quoteTableResponse.ProductData = productDatas;

                apiResult.Code = ResultCode.OK;
                apiResult.Message = REOConstants.GQDT_200;
                apiResult.Data = quoteTableResponse;

                return apiResult;

            }
            catch (Exception ex)
            {

                apiResult.Message = $"{ex.Message} - {ex.StackTrace}";
                return apiResult;
            
            }

        }

        public async Task<ApiResultDto<GetQuoteTableResponse>> GetSuggestedDataTable
            (GetSuggestedProductRequest request)
        {

            var apiResult = new ApiResultDto<GetQuoteTableResponse>()
            {
                Code = ResultCode.BadRequest,
                Message = REOConstants.GSDT_400
            };

            var quoteTableResponse = new GetQuoteTableResponse();

            try
            {

                var tableData = new List<List<object>>();
                var productDatas = new List<ProductDataModel>();
                var allQueryProducts = new List<ProductQueryModel>();

                // STEP 1: Get Product Details
                string lineItemId = request.LineItemId;
                string productId = request.ProductId;
                var getProductByIdResponse = await _crmService.GetProductById(productId);
                var productDetails = getProductByIdResponse.Data.data[0];

                // STEP 2: Query all products
                // Step 2.1: Query all products with same Final Category
                string finalCategory = productDetails.Final_Category;
                string finalCategoryQuery = @$"Select Product_Name, Product_Code, Product_Dimensions, Image_URL, 
                    Cost_Price, Unit_Price, KE_Price, Description, Final_Category, Categories, Qty_in_Stock, Supplier
                    from Products where (Final_Category = '{finalCategory}' and Product_Active = true) 
                    and id != '{productId}'";

                var queryProductsByFinalCategoryResponse = await _crmService.QueryProducts(finalCategoryQuery);

                var queryProductsByFinalCategory = queryProductsByFinalCategoryResponse.Data.data;

                foreach (var queryProduct in queryProductsByFinalCategory)
                {
                    string queryProductId = queryProduct.id;
                    var existProduct = allQueryProducts.Where(p => p.id == queryProductId)
                        .FirstOrDefault();

                    if (existProduct == null)
                    {
                        allQueryProducts.Add(queryProduct);
                    }
                }

                // Step 2.2: Query all products with same Categories
                string categories = productDetails.Categories;
                if (!string.IsNullOrEmpty(categories))
                {
                    string[] categoryArr = categories.Split(',');
                    foreach (var category in categoryArr)
                    {
                        string categoryQuery = @$"Select Product_Name, Product_Code, Product_Dimensions, Image_URL, 
                            Cost_Price, Unit_Price, KE_Price, Description, Final_Category, Categories, Qty_in_Stock, Supplier
                            from Products where (Categories like '%{category}%' and Product_Active = true) 
                            and id != '{productId}'";
                        var queryProductsByCategoryResponse = await _crmService.QueryProducts(categoryQuery);
                        var queryProductsByCategory = queryProductsByCategoryResponse.Data.data;
                        foreach (var queryProduct in queryProductsByCategory)
                        {
                            string queryProductId = queryProduct.id;
                            var existProduct = allQueryProducts.Where(p => p.id == queryProductId)
                                .FirstOrDefault();
                            if (existProduct == null)
                            {
                                allQueryProducts.Add(queryProduct);
                            }
                        }
                    }
                }

                // STEP 3: Handle Suggested Items
                int index = 0;

                float minPrice = 100000000;
                float maxPrice = 0;
                float minHeight = 100000000;
                float maxHeight = 0;
                float minWidth = 100000000;
                float maxWidth = 0;
                float minDepth = 100000000;
                float maxDepth = 0;

                foreach (var queryProduct in allQueryProducts)
                {

                    index ++;

                    var productData = new ProductDataModel();
                    var productRow = new List<object>();

                    string queryProductId = queryProduct.id;

                    string productUrl = REOConstants.ProductPrefixURL + "/" + queryProductId;
                    string productName = queryProduct.Product_Name;

                    productData.ProductName = productName;
                    productData.ProductUrl = productUrl;
                    productData.ProductId = queryProductId;

                    // Step 3.1: Handle No
                    string no = index.ToString();
                    no = $"{no}";
                    productData.LineItemId = lineItemId;
                    productRow.Add(no);

                    string selectItemButton = string.Empty;
                    selectItemButton = $"<button type=\"button\" id=\"replace-product-{queryProductId}\" data-line-item-id=\"{lineItemId}\" class=\"btn btn-primary btn-sm\">Select</button>";

                    // Step 3.2: Handle Product Image
                    string imageColumn = "";
                    string productImage = queryProduct.Image_URL;
                    productData.ImageUrl = productImage;
                    string dimension = queryProduct.Product_Dimensions;
                    var qtyInStock = queryProduct.Qty_in_Stock;

                    productData.QtyInStock = qtyInStock;

                    if (!string.IsNullOrEmpty(productImage))
                    {
                        imageColumn = $"<img src='{productImage}' alt='{productName}' style='max-height: 80px;'/>";
                    }
                    imageColumn = $"<a href='{productUrl}' target='_blank'>{imageColumn}</a>";
                    imageColumn = $"{imageColumn}<br/>{selectItemButton}";
                    imageColumn = $"<div style=\"display: flex; flex-direction: column; align-items: center; justify-content: center;\">{imageColumn}</div>";
                    productRow.Add(imageColumn);

                    // Step 3.3: Handle Item Name
                    string productCode = queryProduct.Product_Code;
                    string itemColumn = $"<a href='{productUrl}' target='_blank'>" +
                        $"<div style=\"font-weight: bold; margin-bottom: 0.5rem;\">" +
                        $"{productName}</div></a>";
                    string productInfo = "";
                    if (!string.IsNullOrEmpty(productCode))
                    {
                        productInfo += "<b>SKU</b>: " + productCode + "";
                        productData.ProductCode = productCode;
                    }

                    if (qtyInStock != null)
                    {
                        string qtyInfo = "";
                        if (qtyInStock <= 0)
                        {
                            qtyInfo = $"<span class=\"text-danger\"><b>Out of Stock, {qtyInStock} units</b></span>";
                        }
                        else
                        {
                            qtyInfo = $"<span class=\"text-success\"><b>In Stock, {qtyInStock} units</b></span>";
                        }
                        if (string.IsNullOrEmpty(productInfo))
                        {
                            productInfo = qtyInfo;
                        }
                        else
                        {
                            productInfo += $"<br/>{qtyInfo}";
                        }
                    }
                    if (!string.IsNullOrEmpty(productInfo))
                    {
                        productInfo = $"<p>{productInfo}</p>";
                        itemColumn += $"<p>{productInfo}</p>";
                    }
                    itemColumn = $"{itemColumn}";

                    productRow.Add(itemColumn);

                    // Handle Supplier
                    string supplierColumn = "";
                    string supplier = queryProduct.Supplier;
                    if (!string.IsNullOrEmpty(supplier))
                    {
                        supplierColumn = supplier;
                    }
                    productData.Supplier = supplier;
                    productRow.Add(supplierColumn);

                    // Step 3.4: Handle Price
                    // Step 3.4.1: Handle Cost Price Column
                    var costPrice = queryProduct.Cost_Price;
                    if (costPrice == null)
                    {
                        costPrice = 0;
                    }
                    
                    productData.CostPrice = costPrice;
                    productRow.Add("$" + costPrice.Value.ToString("N0"));

                    var unitPrice = queryProduct.Unit_Price;
                    if (unitPrice == null)
                    {
                        unitPrice = 0;
                    }
                    productData.UnitPrice = unitPrice;
                    if (unitPrice < minPrice)
                    {
                        minPrice = unitPrice.Value;
                    }
                    if (unitPrice > maxPrice)
                    {
                        maxPrice = unitPrice.Value;
                    }

                    productRow.Add("$" + unitPrice.Value.ToString("N0"));

                    // Step 3.5: Handle Dimension
                    var currentDimensions = StringHelpers.ExtractDimensions(dimension);

                    int productWidth = currentDimensions.Width;
                    if (productWidth < minWidth)
                    {
                        minWidth = productWidth;
                    }
                    if (productWidth > maxWidth)
                    {
                        maxWidth = productWidth;
                    }

                    int productHeight = currentDimensions.Height;
                    if (productHeight < minHeight)
                    {
                        minHeight = productHeight;
                    }
                    if (productHeight > maxHeight)
                    {
                        maxHeight = productHeight;
                    }

                    int productDepth = currentDimensions.Depth;
                    if (productDepth < minDepth)
                    {
                        minDepth = productDepth;
                    }
                    if (productDepth > maxDepth)
                    {
                        maxDepth = productDepth;
                    }

                    // Step 3.5.1: Handle Height
                    productRow.Add(productHeight);
                    productData.Height = productHeight;

                    // Step 3.5.2: Handle Width
                    productRow.Add(productWidth);
                    productData.Width = productWidth;

                    // Step 3.5.3: Handle Depth
                    productRow.Add(productDepth);
                    productData.Depth = productDepth;

                    tableData.Add(productRow);
                    productDatas.Add(productData);

                }

                quoteTableResponse.TableData = tableData;
                quoteTableResponse.ProductData = productDatas;
                
                quoteTableResponse.MinPrice = minPrice;
                quoteTableResponse.MaxPrice = maxPrice;
                quoteTableResponse.MinHeight = minHeight;
                quoteTableResponse.MaxHeight = maxHeight;
                quoteTableResponse.MinWidth = minWidth;
                quoteTableResponse.MaxWidth = maxWidth;
                quoteTableResponse.MinDepth = minDepth;
                quoteTableResponse.MaxDepth = maxDepth;

                apiResult.Code = ResultCode.OK;
                apiResult.Message = REOConstants.GSDT_200;
                apiResult.Data = quoteTableResponse;

                return apiResult;

            }
            catch (Exception ex)
            {

                apiResult.Message = $"{ex.Message} - {ex.StackTrace}";
                return apiResult;

            }

        }

        public async Task<ApiResultDto<string>> EasyUpdateQuote
            (EasyUpdateQuoteRequest request)
        {

            var apiResult = new ApiResultDto<string>()
            {
                Code = ResultCode.BadRequest,
                Message = REOConstants.EUQ_400
            };

            try
            {

                string quoteId = request.QuoteId;
                var updatedItems = request.Products;

                // STEP 1: Get Quote by Id
                var getQuoteByIdResponse = await _crmService.GetQuoteById(quoteId);

                var quoteDetails = getQuoteByIdResponse.Data.data[0];
                var quotedItems = quoteDetails.Quoted_Items;

                var updateQuoteRequest = new UpsertRequest<UpdateQuoteRequest>();
 
                var updateQuotedItems = new List<UpdateQuotedItem>();

                int sequence = 0;

                foreach (var currentItem in quotedItems)
                {
                    sequence++;
                    var updatedItem = new UpdateQuotedItem();

                    string currentLineItemId = currentItem.id;
                    string currentProductId = currentItem.Product_Name.id;
                    double quantity = currentItem.Quantity.Value;

                    var foundItem = updatedItems
                        .FirstOrDefault(x => x.LineItemId == currentLineItemId);

                    string foundProductId = foundItem.ProductId;

                    if (currentProductId == foundProductId)
                    {
                        // No Update on the row, go to next row
                        continue;
                    }    

                    var getUpdatedProductByIdResponse = await 
                        _crmService.GetProductById(foundProductId);

                    var updatedProduct = getUpdatedProductByIdResponse.Data.data[0];

                    updatedItem.id = currentLineItemId;
                    updatedItem.Sequence_Number = sequence;
                    updatedItem.Product_Name = foundProductId;
                    updatedItem.List_Price = updatedProduct.Unit_Price;
                    updatedItem.Cost_Price = updatedProduct.Cost_Price;
                    updatedItem.Quantity = quantity;
                    updateQuotedItems.Add(updatedItem);

                }

                if (updateQuotedItems.Count == 0)
                {
                    apiResult.Code = ResultCode.OK;
                    apiResult.Message = REOConstants.EUQ_S01;
                    return apiResult;
                }

                var quoteData = new UpdateQuoteRequest()
                {
                    Quoted_Items = updateQuotedItems
                };

                updateQuoteRequest.data.Add(quoteData);

                var updateQuoteResponse = await 
                    _crmService.UpdateQuote(quoteId, updateQuoteRequest);

                if (updateQuoteResponse.Code != ResultCode.OK)
                {
                    return apiResult;
                }

                apiResult.Code = ResultCode.OK;
                apiResult.Message = REOConstants.EAQ_200;

                return apiResult;

            }
            catch (Exception ex)
            {
                apiResult.Message = $"{ex.Message} - {ex.StackTrace}";
                return apiResult;
            }

        }

        public async Task<ApiResultDto<string>> BlueprintConnectWithLead(
            string leadId, ConnectWithLeadRequest request)
        {
            
            var apiResult = new ApiResultDto<string>()
            {
                Code = ResultCode.BadRequest,
                Message = REOConstants.BCWL_400
            };

            try
            {

                // STEP 1: Update Lead Blueprint

                var leadData = new LeadConnectedData()
                {
                    Notes = request.notes,
                    First_Name = request.firstName,
                    Last_Name = request.lastName,
                    Company = request.company,
                    Business_Type = request.businessType,
                    Delivery_Date = request.equipmentDate,
                    Email = request.email,
                    Mobile = request.mobile,
                    Shipping_City = request.shippingCity,
                    Shipping_Zip_Postal_Code = request.shippingCode
                };

                var updateBlueprintRequest = new UpdateBlueprintRequest<LeadConnectedData>();

                var blueprints = new List<Blueprint<LeadConnectedData>>();
                var blueprint = new Blueprint<LeadConnectedData>()
                {
                    transition_id = REOConstants.Blueprint_ConnectWithLead_TransitionId,
                    data = leadData
                };

                blueprints.Add(blueprint);
                updateBlueprintRequest.blueprint = blueprints;

                string recordModule = "Leads";

                var updateBluePrintResponse = await _crmService
                    .UpdateLeadBlueprint(recordModule, leadId, updateBlueprintRequest);

                if (updateBluePrintResponse.Code != ResultCode.OK)
                {
                    return apiResult;
                }

                // STEP 2: Update Lead Record
                string businessDuration = request.businessDuration;
                string intendedUse = request.intendedUse;
                string brandPreference = request.brandPreference;
                string priority = request.priority;
                string delivery = request.delivery;
                bool shouldUpdateLead = false;

                var leadForUpdation = new LeadForUpdation();
                if (!string.IsNullOrEmpty(businessDuration))
                {
                    shouldUpdateLead = true;
                    leadForUpdation.Business_Duration_Setup_Stage = businessDuration;
                }
                if (!string.IsNullOrEmpty(intendedUse))
                {
                    shouldUpdateLead = true;
                    leadForUpdation.Intended_Use_Dishes_or_Services = intendedUse;
                }
                if (!string.IsNullOrEmpty(brandPreference))
                {
                    shouldUpdateLead = true;
                    leadForUpdation.Brand_Preference = brandPreference;
                }
                if (!string.IsNullOrEmpty(priority))
                {
                    shouldUpdateLead = true;
                    leadForUpdation.Priority_Delivery_or_Price = priority;
                }
                if (!string.IsNullOrEmpty(delivery))
                {
                    shouldUpdateLead = true;
                    leadForUpdation.Urgent_Delivery_Option = delivery;
                }

                if (shouldUpdateLead)
                {
                    var upsertDetails = new UpsertRequest<LeadForUpdation>();
                    upsertDetails.data.Add(leadForUpdation);
                    var updateLeadResponse = await _crmService
                        .UpdateLead(leadId, upsertDetails);
                    if (updateLeadResponse.Code != ResultCode.OK)
                    {
                        return apiResult;
                    }
                }

                apiResult.Message = REOConstants.BCWL_200;
                apiResult.Code = ResultCode.OK;

                return apiResult;

            }
            catch(Exception ex)
            {

                apiResult.Message = $"{ex.Message} - {ex.StackTrace}";
                return apiResult;

            }

        }
    }

}
