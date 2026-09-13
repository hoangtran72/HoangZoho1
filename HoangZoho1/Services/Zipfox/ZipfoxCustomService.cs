using HoangZoho1.Constants;
using HoangZoho1.Helpers;
using HoangZoho1.Models.Common;
using HoangZoho1.Models.Zipfox;
using HoangZoho1.Models.Zipfox.WhatsApp;
using HoangZoho1.Models.Zipfox.ZohoCRM;
using HoangZoho1.Models.Zipfox.ZohoDesk;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace HoangZoho1.Services.Zipfox
{

    public class ZipfoxCustomService : IZipfoxCustomService
    {

        private readonly IZipfoxCrmService _zipfoxCrmService;
        private readonly IZipfoxDeskService _zipfoxDeskService;

        public ZipfoxCustomService(IZipfoxCrmService zipfoxCrmService, 
            IZipfoxDeskService zipfoxDeskService)
        {
            _zipfoxCrmService = zipfoxCrmService;
            _zipfoxDeskService = zipfoxDeskService;
        }

        #region WhatsApp Functions

        public async Task<ApiResultDto<string>> HandleMessagePayload(MessagePayload payload)
        {

            var apiResult = new ApiResultDto<string>()
            {

                Code = ResultCode.BadRequest,
                Message = ZipfoxConstants.HWMP_400

            };

            try
            {

                // STEP 0: Pause 5 - 10 seconds
                var random = new Random();
                int number = random.Next(5, 11);
                Thread.Sleep(number * 1000);

                // STEP 1: Extract Message Payload
                var value = payload.entry[0].changes[0].value;
                var messages = value.messages;
                var messageDetails = messages[0];

                string fromNumber = messageDetails.from;
                string messageId = messageDetails.id;
                string timestamp = messageDetails.timestamp;

                var metadata = value.metadata;
                string zipfoxPhoneNumber = metadata.display_phone_number;

                // Step 1.1: Handle Sent Time and Read Time
                var convertRequest = new ConvertTimeStampRequest()
                {
                    ReturnedFormat = "yyyy-MM-ddTHH:mm:ss",
                    TimeZone = "Pacific Standard Time",
                    Type = 1,
                    TimeStamp = double.Parse(timestamp)
                };
                string timeStr = DateTimeHelpers.UnixTimeStampToDateTime(convertRequest);

                string messageType = messageDetails.type;

                bool shouldUpdateContact = false;

                // Step 3.1: Search Contacts by Phone
                string phone = fromNumber.Substring(3);
                var searchContactsResponse = await _zipfoxCrmService.SearchContactsByPhone(phone);
                string contactId = "";
                if (searchContactsResponse.Code == ResultCode.OK)
                {
                    var contactDetails = searchContactsResponse.Data.data[0];
                    contactId = contactDetails.id;
                }

                if (messageType == "text")
                {
                    shouldUpdateContact = true;
                    var textDetails = messageDetails.text;
                    string textContent = textDetails.body;

                    // STEP 2: Query Logs by Message Id
                    var coqlRequest = new ZohoCoqlRequest()
                    {
                        select_query = $"SELECT Name FROM WhatsApp_Logs WHERE Message_Id = '{messageId}'"
                    };
                    var queryLogsResponse = await _zipfoxCrmService.QueryWhatsAppLogs(coqlRequest);
                    if (queryLogsResponse.Code != ResultCode.OK)
                    {
                        // STEP 3: Create Contact Log

                        // Step 3.2: Create Log
                        var logForCreation = new WhatsAppLogForCreation()
                        {
                            From_Phone_Number = fromNumber,
                            To_Phone_Number = zipfoxPhoneNumber,
                            Related_Contact = contactId,
                            Message_Id = messageId,
                            Status = "Received",
                            Direction = "Incoming",
                            Received_Time = timeStr,
                            Message_Content = textContent
                        };

                        var upsertRequest = new UpsertRequest<WhatsAppLogForCreation>();
                        upsertRequest.data.Add(logForCreation);

                        var insertLogResponse = await _zipfoxCrmService.CreateWhatsAppLog(upsertRequest);
                        if (insertLogResponse.Code != ResultCode.OK)
                        {
                            apiResult.Message = ZipfoxConstants.HWMP_E01;
                            return apiResult;
                        }

                    }

                }
                else if (messageType == "button")
                {
                    var buttonDetails = messageDetails.button;
                    string buttonPayload = buttonDetails.payload;
                    string buttonText = buttonDetails.text;
                    shouldUpdateContact = true;

                    // Search RFQ using Message Id
                    var rfqCoqlRequest = new ZohoCoqlRequest()
                    {
                        select_query = $"SELECT Name FROM RFQs WHERE Message_Ids like '%{messageId}%'"
                    };
                    var queryRfqsResponse = await _zipfoxCrmService.QueryRFQs(rfqCoqlRequest);
                    if (queryRfqsResponse.Code == ResultCode.OK)
                    {
                        var rfqList = queryRfqsResponse.Data.data;
                        foreach (var rfq in rfqList)
                        {
                            string rfqId = rfq.id;
                            var updateRfqRequest = new UpsertRequest<RFQForUpdation>();
                            var rfqForUpdation = new RFQForUpdation()
                            {
                                RFQ_Replied = true
                            };
                            updateRfqRequest.data.Add(rfqForUpdation);
                            var updateRfqResponse = await _zipfoxCrmService.UpdateRFQ(rfqId, updateRfqRequest);
                        }
                    }
                }

                if (shouldUpdateContact && !string.IsNullOrEmpty(contactId))
                {

                    var upsertRequest = new UpsertRequest<ContactForUpdation>();
                    var contactForUpdation = new ContactForUpdation()
                    {
                        WhatsApp_OK = "Accepted"
                    };
                    upsertRequest.data.Add(contactForUpdation);

                    var updateContactResponse = await _zipfoxCrmService.UpdateContact(contactId, upsertRequest);
                    if (updateContactResponse.Code != ResultCode.OK)
                    {
                        apiResult.Message = ZipfoxConstants.HWMP_E02;
                    }

                }

                apiResult.Code = ResultCode.OK;
                apiResult.Message = ZipfoxConstants.HWMP_200;

                return apiResult;

            }
            catch (Exception ex)
            {
                apiResult.Message = $"{ex.Message} - {ex.StackTrace}";
                return apiResult;
            }
        }

        public async Task<ApiResultDto<string>> HandleStatusPayload(StatusPayload payload)
        {

            var apiResult = new ApiResultDto<string>()
            {

                Code = ResultCode.BadRequest,
                Message = ZipfoxConstants.HWSP_400

            };

            var textInfo = new CultureInfo("en-US", false).TextInfo;

            try
            {

                // STEP 0: Pause 5 - 10 seconds
                var random = new Random();
                int number = random.Next(5, 11);
                Thread.Sleep(number * 1000);

                // STEP 1: Extract Status Payload
                var value = payload.entry[0].changes[0].value;
                var statuses = value.statuses;
                var statusDetails = statuses[0];

                string messageId = statusDetails.id;
                string status = statusDetails.status;
                string timestamp = statusDetails.timestamp;

                // Step 1.1: Handle Sent Time and Read Time
                var convertRequest = new ConvertTimeStampRequest()
                {
                    ReturnedFormat = "yyyy-MM-ddTHH:mm:ss",
                    TimeZone = "Pacific Standard Time",
                    Type = 1,
                    TimeStamp = double.Parse(timestamp)
                };
                string timeStr = DateTimeHelpers.UnixTimeStampToDateTime(convertRequest);

                string recipientId = statusDetails.recipient_id;
                var conversationDetails = statusDetails.conversation;

                string conversationId = "";
                if (conversationDetails != null)
                {
                    conversationId = conversationDetails.id;
                }
                
                var metadata = value.metadata;
                string zipfoxPhoneNumber = metadata.display_phone_number;

                // STEP 2: Query Logs by Message Id
                var coqlRequest = new ZohoCoqlRequest()
                {
                    select_query = $"SELECT Name FROM WhatsApp_Logs WHERE Message_Id = '{messageId}'"
                };
                var queryLogsResponse = await _zipfoxCrmService.QueryWhatsAppLogs(coqlRequest);
                string logId = "";

                if (queryLogsResponse.Code != ResultCode.OK)
                {
                    // STEP 3: Create Contact Log
                    // Step 3.1: Search Contacts by Phone
                    string phone = recipientId.Substring(3);
                    var searchContactsResponse = await _zipfoxCrmService.SearchContactsByPhone(phone);
                    string contactId = "";
                    if (searchContactsResponse.Code == ResultCode.OK)
                    {
                        var contactDetails = searchContactsResponse.Data.data[0];
                        contactId = contactDetails.id;
                    }

                    if (status.Equals("Failed", StringComparison.InvariantCultureIgnoreCase))
                    {
                        var contactForUpdation = new ContactForUpdation()
                        {
                            WhatsApp_OK = "Declined"
                        };
                        var upsertContactRequest = new UpsertRequest<ContactForUpdation>();
                        upsertContactRequest.data.Add(contactForUpdation);

                        var updateContactResponse = await _zipfoxCrmService.UpdateContact(contactId, upsertContactRequest);
                    }

                    // Step 3.2: Create Log
                    var logForCreation = new WhatsAppLogForCreation()
                    {
                        From_Phone_Number = zipfoxPhoneNumber,
                        To_Phone_Number = recipientId,
                        Conversation_Id = conversationId,
                        Related_Contact = contactId,
                        Message_Id = messageId,
                        Status = textInfo.ToTitleCase(status),
                        Direction = "Outgoing"
                    };
                    if (status == "sent" || status == "delivered")
                    {
                        logForCreation.Sent_Time = timeStr;
                    }
                    else if (status == "read")
                    {
                        logForCreation.Read_Time = timeStr;

                        // Search RFQ using Message Id
                        var rfqCoqlRequest = new ZohoCoqlRequest()
                        {
                            select_query = $"SELECT Name FROM RFQs WHERE Message_Ids like '%{messageId}%'"
                        };
                        var queryRfqsResponse = await _zipfoxCrmService.QueryRFQs(rfqCoqlRequest);
                        if (queryRfqsResponse.Code == ResultCode.OK)
                        {
                            var rfqList = queryRfqsResponse.Data.data;
                            foreach (var rfq in rfqList)
                            {
                                string rfqId = rfq.id;
                                var updateRfqRequest = new UpsertRequest<RFQForUpdation>();
                                var rfqForUpdation = new RFQForUpdation()
                                {
                                    RFQ_Replied = true
                                };
                                updateRfqRequest.data.Add(rfqForUpdation);
                                var updateRfqResponse = await _zipfoxCrmService.UpdateRFQ(rfqId, updateRfqRequest);
                            }
                        }
                    }

                    var upsertRequest = new UpsertRequest<WhatsAppLogForCreation>();
                    upsertRequest.data.Add(logForCreation);

                    var insertLogResponse = await _zipfoxCrmService.CreateWhatsAppLog(upsertRequest);
                    if (insertLogResponse.Code != ResultCode.OK)
                    {
                        apiResult.Message = ZipfoxConstants.HWSP_E01;
                        return apiResult;
                    }

                }
                else
                {
                    // STEP 4: Update Contact Log
                    var logDetails = queryLogsResponse.Data.data[0];
                    logId = logDetails.id;

                    var logForUpdation = new WhatsAppLogForUpdation()
                    {
                        Status = textInfo.ToTitleCase(status),
                        From_Phone_Number = zipfoxPhoneNumber,
                        To_Phone_Number = recipientId
                    };
                    if (status == "sent" || status == "delivered")
                    {
                        logForUpdation.Sent_Time = timeStr;
                    }
                    else if (status == "read")
                    {
                        logForUpdation.Read_Time = timeStr;

                        // Search RFQ using Message Id
                        var rfqCoqlRequest = new ZohoCoqlRequest()
                        {
                            select_query = $"SELECT Name FROM RFQs WHERE Message_Ids like '%{messageId}%'"
                        };
                        var queryRfqsResponse = await _zipfoxCrmService.QueryRFQs(rfqCoqlRequest);
                        if (queryRfqsResponse.Code == ResultCode.OK)
                        {
                            var rfqList = queryRfqsResponse.Data.data;
                            foreach (var rfq in rfqList)
                            {
                                string rfqId = rfq.id;
                                var updateRfqRequest = new UpsertRequest<RFQForUpdation>();
                                var rfqForUpdation = new RFQForUpdation()
                                {
                                    RFQ_Replied = true
                                };
                                updateRfqRequest.data.Add(rfqForUpdation);
                                var updateRfqResponse = await _zipfoxCrmService.UpdateRFQ(rfqId, updateRfqRequest);
                            }
                        }
                    }

                    var upsertRequest = new UpsertRequest<WhatsAppLogForUpdation>();
                    upsertRequest.data.Add(logForUpdation);

                    var updateLogResponse = await _zipfoxCrmService.UpdateWhatsAppLog(logId, upsertRequest);
                    if (updateLogResponse.Code != ResultCode.OK)
                    {
                        apiResult.Message = ZipfoxConstants.HWSP_E02;
                        return apiResult;
                    }

                }

                apiResult.Code = ResultCode.OK;
                apiResult.Message = ZipfoxConstants.HWSP_200;

                return apiResult;

            }
            catch (Exception ex)
            {
                apiResult.Message = $"{ex.Message} - {ex.StackTrace}";
                return apiResult;
            }
        }

        #endregion

        #region Zoho Functions

        public async Task<ApiResultDto<string>> CreateContactForNotFoundSearch
            (ProductNotFoundContact[] contacts)
        {
            var apiResult = new ApiResultDto<string>
            {
                Code = ResultCode.BadRequest,
                Message = ZipfoxConstants.CCNFS_400
            };

            try
            {
                // Loop through the Contact List
                foreach (var contact in contacts)
                {
                    string contactFirstName = contact.FirstName;
                    string contactLastName = contact.LastName;
                    string contactEmail = contact.Email;
                    string source = contact.Source;
                    string productDesc = contact.ProductDescription;
                    string searchValue = contact.SearchValue;
                    string estimatedQuantity = contact.EstimatedQuantity.HasValue 
                        ? contact.EstimatedQuantity.ToString() : "";

                    // STEP 1: Query Contacts using Contact Email
                    var coqlRequest = new ZohoCoqlRequest()
                    {
                        select_query = $"SELECT First_Name, Last_Name FROM Contacts WHERE Email = '{contactEmail}'"
                    };
                    var queryContactsResponse = await _zipfoxCrmService.QueryContacts(coqlRequest);
                    if (queryContactsResponse.Code == ResultCode.OK)
                    {
                        // Contact already exists in Zoho CRM, no need to create
                        continue;
                    }

                    // STEP 2: Prepare Create Contact Request
                    var createContactRequest = new UpsertRequest<ContactForCreation>();
                    var contactForCreation = new ContactForCreation()
                    {
                        First_Name = contactFirstName,
                        Last_Name = contactLastName,
                        Email = contactEmail,
                        Lead_Source = source,
                        Product_Not_Found_Description = productDesc,
                        Search_Value = searchValue,
                        Estimated_Quantity = estimatedQuantity
                    };
                    createContactRequest.data.Add(contactForCreation);
                    createContactRequest.trigger.Add("workflow");

                    var createContactResponse = await _zipfoxCrmService.CreateContact(createContactRequest);
                    Thread.Sleep(500);
                    // break;
                }

                apiResult.Code = ResultCode.OK;
                apiResult.Message = ZipfoxConstants.CCNFS_200;
                return apiResult;

            }
            catch (Exception ex)
            {
                apiResult.Message = $"{ex.Message} - {ex.StackTrace}";
                return apiResult;
            }

        }

        public async Task<ApiResultDto<string>> CreateTicketForNotFoundSearch(ProductNotFoundContact[] contacts)
        {

            var apiResult = new ApiResultDto<string>
            {
                Code = ResultCode.BadRequest,
                Message = ZipfoxConstants.CTNFS_400
            };

            try
            {
                // Loop through the Contact List
                foreach (var contact in contacts)
                {
                    string contactFirstName = contact.FirstName;
                    string contactLastName = contact.LastName;
                    string contactEmail = contact.Email;
                    string source = contact.Source;
                    string addedTime = contact.AddedTime;
                    string productDescription = contact.ProductDescription;
                    decimal estimatedQuantity = 0;
                    if (contact.EstimatedQuantity.HasValue)
                    {
                        estimatedQuantity = Math.Round(contact.EstimatedQuantity.Value, 0);
                    }
                    string searchValue = contact.SearchValue;

                    // STEP 1: Prepare Create Desk Request Body
                    var createTicketRequest = new CreateTicketRequest();
                    createTicketRequest.subject = $"[LEGACY] [{addedTime}] A product was not found in the Zipfox search";
                    var deskContact = new DeskContact()
                    {
                        firstName = contactFirstName,
                        lastName = contactLastName,
                        email = contactEmail
                    };
                    createTicketRequest.contact = deskContact;
                    createTicketRequest.email = contactEmail;
                    createTicketRequest.description = productDescription;
                    var customField = new Cf()
                    {
                        cf_estimated_quantity = estimatedQuantity.ToString(),
                        cf_product_not_found = productDescription
                    };
                    createTicketRequest.cf = customField;

                    // STEP 2: Create Desk Ticket
                    var createTicketResponse = await _zipfoxDeskService.CreateTicket(createTicketRequest);

                    Thread.Sleep(1000);

                }

                apiResult.Code = ResultCode.OK;
                apiResult.Message = ZipfoxConstants.CTNFS_200;
                return apiResult;

            }
            catch (Exception ex)
            {
                apiResult.Message = $"{ex.Message} - {ex.StackTrace}";
                return apiResult;
            }

        }

        #endregion

    }

}
