using HoangZoho1.Constants;
using HoangZoho1.Helpers;
using HoangZoho1.Models.Common;
using HoangZoho1.Models.OneCorp.Custom;
using HoangZoho1.Models.OneCorp.Sakari;
using HoangZoho1.Models.OneCorp.ScheduleOnce;
using HoangZoho1.Models.OneCorp.ZohoCRM;
using HoangZoho1.Models.OneCorp.ZohoWorkdrive;
using HoangZoho1.Services.SakariAuth;
using MySqlConnector;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace HoangZoho1.Services.OneCorp
{
    public class OneCorpCustomService : IOneCorpCustomService
    {

        private readonly IOneCorpCrmService _oneCorpCrmService;
        private readonly IOneCorpSignService _oneCorpSignService;
        private readonly IOneCorpWorkdriveService _oneCorpWorkdriveService;
        private readonly IOneCorpOnceHubService _oneCorpOnceHubService;
        private readonly IOneCorpSakariService _oneCorpSakariService;
        private readonly IOneCorpTwilioService _oneCorpTwilioService;
        private readonly string DocumentPath = Path.GetDirectoryName(AppDomain.CurrentDomain.BaseDirectory) + @"\Documents";

        public OneCorpCustomService(IOneCorpCrmService oneCorpCrmService, IOneCorpSignService oneCorpSignService,
            IOneCorpWorkdriveService oneCorpWorkdriveService, IOneCorpOnceHubService onceHubService, IOneCorpSakariService sakariService, IOneCorpTwilioService oneCorpTwilioService)
        {
            _oneCorpCrmService = oneCorpCrmService;
            _oneCorpSignService = oneCorpSignService;
            _oneCorpWorkdriveService = oneCorpWorkdriveService;
            _oneCorpOnceHubService = onceHubService;
            _oneCorpSakariService = sakariService;
            _oneCorpTwilioService = oneCorpTwilioService;
        }

        #region ScheduleOnce Bookings

        public async Task<ApiResultDto<string>> ScheduleOnce_SyncBooking(BookingPayload bookingPayload)
        {

            var apiResult = new ApiResultDto<string>()
            {
                Code = ResultCode.BadRequest,
                Message = OneCorpConstants.SSB_400
            };

            try
            {
                // STEP 1: Extract data from booking payload
                var bookingDetails = bookingPayload.data;

                // Step 1.1: Extract booking data
                string bookingId = bookingDetails.id;
                string trackingId = bookingDetails.tracking_id;
                string bookingSubject = bookingDetails.subject;
                string bookingStatus = bookingDetails.status;
                string bookingStatusText = bookingStatus.Replace('_', ' ').ToUpper();
                string customerTimezoneText = bookingDetails.customer_timezone;

                if (bookingStatusText == "COMPLETED")
                {
                    apiResult.Code = ResultCode.OK;
                    apiResult.Message = OneCorpConstants.SSB_StatusCompleted;
                    return apiResult;
                }

                var tz = TimeZoneInfo.FindSystemTimeZoneById("E. Australia Standard Time");
                var timezoneMap = DateTimeHelpers.LoadFromJson($"{DocumentPath}/window_timezones.json");

                string bookingCreationTimeText = TimeZoneInfo.ConvertTimeFromUtc
                    (bookingDetails.creation_time.Value, tz).ToString("yyyy-MM-ddTHH:mm:ss");
                string bookingStartTimeText = TimeZoneInfo.ConvertTimeFromUtc
                    (bookingDetails.starting_time.Value, tz).ToString("yyyy-MM-ddTHH:mm:ss");
                string bookingLastUpdatedTimeText = TimeZoneInfo.ConvertTimeFromUtc
                    (bookingDetails.last_updated_time.Value, tz).ToString("yyyy-MM-ddTHH:mm:ss");

                var customerTimeZone = DateTimeHelpers.ConvertToTimeZoneInfo(customerTimezoneText, timezoneMap);
                string customerBookingStartTime = TimeZoneInfo.ConvertTimeFromUtc(bookingDetails.starting_time.Value, customerTimeZone).ToString("yyyy-MM-ddTHH:mm:ss");

                // Step 1.2: Handle Booking Owner
                string bookingOwner = bookingDetails.owner;
                var getOnceHubUserByIdResponse = await _oneCorpOnceHubService.GetUserById(bookingOwner);

                var onceHubUserDetails = getOnceHubUserByIdResponse.Data;
                string bookingUserFirstName = onceHubUserDetails.first_name;
                string bookingUserLastName = onceHubUserDetails.last_name;
                string bookingUserFullName = "";

                if (!string.IsNullOrEmpty(bookingUserFirstName))
                {
                    bookingUserFullName = bookingUserFirstName;
                }

                if (!string.IsNullOrEmpty(bookingUserLastName))
                {
                    if (string.IsNullOrEmpty(bookingUserFullName))
                    {
                        bookingUserFullName = bookingUserLastName;
                    }
                    else
                    {
                        bookingUserFullName = $"{bookingUserFirstName} {bookingUserLastName}";
                    }
                }

                string userEmail = onceHubUserDetails.email;
                string userQuery = $"select time_zone, status from users WHERE email = '{userEmail}'";

                var selectQuery = new ZohoCoqlRequest()
                {
                    select_query = userQuery
                };
                string userId = string.Empty;
                string userStatus = string.Empty;
                string userTimeZoneText = string.Empty;
                string userBookingStartTime = string.Empty;
                var queryUsersResponse = await _oneCorpCrmService.QueryUsers(selectQuery);
                if (queryUsersResponse.Code == ResultCode.OK)
                {
                    var userDetails = queryUsersResponse.Data.data[0];
                    userId = userDetails.id;
                    userStatus = userDetails.status;
                    if (!userStatus.Equals("active"))
                    {
                        userId = OneCorpConstants.ZohoCRM_JoshUserId;
                    }    
                    var getCrmUserByIdResponse = await _oneCorpCrmService.GetUserById(userId);
                    var crmUserDetails = getCrmUserByIdResponse.Data.users[0];
                    userTimeZoneText = crmUserDetails.time_zone;
                    var userTimeZone = DateTimeHelpers.ConvertToTimeZoneInfo(userTimeZoneText, timezoneMap);
                    userBookingStartTime = TimeZoneInfo.ConvertTimeFromUtc(bookingDetails.starting_time.Value, userTimeZone).ToString("yyyy-MM-ddTHH:mm:ss");
                }
                else
                {
                    userId = OneCorpConstants.ZohoCRM_JoshUserId;
                    userTimeZoneText = "Australia/Sydney";
                    var userTimeZone = DateTimeHelpers.OlsonTimeZoneToTimeZoneInfo(userTimeZoneText);
                    userBookingStartTime = TimeZoneInfo.ConvertTimeFromUtc(bookingDetails.starting_time.Value, userTimeZone).ToString("yyyy-MM-ddTHH:mm:ss");
                }    

                decimal? bookingDuration = bookingDetails.duration_minutes;
                var virtualConferencing = bookingDetails.virtual_conferencing;
                string bookingJoinUrl = string.Empty;
                if (virtualConferencing != null)
                {
                    bookingJoinUrl = virtualConferencing.join_url;
                }
                string bookingLocation = bookingDetails.location_description;

                // Step 1.3: Extract reschedule data
                string bookingRescheduleId = bookingDetails.rescheduled_booking_id;
                string cancelRescheduleURL = bookingDetails.cancel_reschedule_url;
                var cancelReschedule = bookingDetails.cancel_reschedule_information;
                string cancelRescheduleText = string.Empty;
                if (cancelReschedule != null)
                {
                    cancelRescheduleText = $"- Reason: {cancelReschedule.reason}\n- Actioned By: {cancelReschedule.actioned_by}\n" +
                        $"- User Id: {cancelReschedule.user_id}";
                }

                // Step 1.4: Extract Form Submission
                var formSubmissionData = bookingDetails.form_submission;
                string customerName = formSubmissionData.name;
                string customerFirstName = customerName;
                string customerLastName = string.Empty;
                string customerEmail = formSubmissionData.email;
                string customerPhone = formSubmissionData.phone;
                string customerMobile = formSubmissionData.mobile_phone;
                if (!string.IsNullOrEmpty(customerMobile))
                {
                    customerMobile = customerMobile.Replace("-", "");
                    if (!customerMobile.StartsWith("+"))
                    {
                        customerMobile = "+" + customerMobile;
                    }
                }
                string customerNote = formSubmissionData.note;
                string customerCompany = formSubmissionData.company;
                var customerGuests = formSubmissionData.guests;
                string customerGuestsText = string.Empty;
                if (customerGuests != null && customerGuests.Length > 0)
                {
                    int guestCount = 1;
                    foreach (var guest in customerGuests)
                    {
                        if (string.IsNullOrEmpty(customerGuestsText))
                        {
                            customerGuestsText = $"{guestCount}. {guest}";
                        }
                        else
                        {
                            customerGuestsText += $"\n{guestCount}. {guest}";
                        }
                        guestCount++;
                    }
                }
                
                var customerAdditionalDetails = formSubmissionData.custom_fields;
                string customerAdditionalText = string.Empty;
                string setter = string.Empty;
                if (customerAdditionalDetails != null && customerAdditionalDetails.Length > 0)
                {
                    foreach (var additional in customerAdditionalDetails)
                    {

                        string cfName = additional.name;
                        string cfValue = additional.value;

                        if (cfName.Equals("lname", StringComparison.InvariantCultureIgnoreCase))
                        {
                            customerName = $"{customerName} {cfValue}";
                            customerLastName = cfValue;
                        }
                        else if (cfName.Equals("setter", StringComparison.InvariantCultureIgnoreCase))
                        {
                            setter = cfValue;
                        }

                        if (string.IsNullOrEmpty(customerAdditionalText))
                        {
                            customerAdditionalText = $"{cfName}: {cfValue}";
                        }
                        else
                        {
                            customerAdditionalText += $"\n{cfName}: {cfValue}";
                        }
                    }
                }

                // Step 1.5: Other Information
                string bookingPage = bookingDetails.booking_page;
                string masterPage = bookingDetails.master_page;
                string eventType = bookingDetails.event_type;
                string conversation = bookingDetails.conversation;
                var externalCalendar = bookingDetails.external_calendar;
                string externalCalendarText = string.Empty;
                if (externalCalendar != null)
                {
                    externalCalendarText = $"- Type: {externalCalendar.type}\n- Name: {externalCalendar.name}\n" +
                        $"- Id: {externalCalendar.id}\n- Event Id: {externalCalendar.event_id}";
                }

                // STEP 2: Search ScheduleOne Booking on Zoho CRM
                string criteria = $"(Booking_Id:equals:{HttpUtility.UrlEncode(bookingId)})";
                var searchBookingsResponse = await _oneCorpCrmService.SearchScheduleOnceBookings(criteria);

                bool isUpdate = false;
                if (searchBookingsResponse.Code == ResultCode.NoContent)
                {
                    // Step 2.1: Booking hasn't been synced yet
                    var bookingForCreation = new BookingForCreation()
                    {
                        Booking_Id = bookingId,
                        Booking_Status = bookingStatusText,
                        Booking_Owner = bookingOwner,
                        Booking_Owner_Name = bookingUserFullName,
                        Booking_Owner_First_Name = bookingUserFirstName,
                        Booking_Owner_Last_Name = bookingUserLastName,
                        Booking_Owner_Email = userEmail,
                        Tracking_Id = trackingId,
                        Booking_Subject = bookingSubject,
                        Booking_Created_Time = bookingCreationTimeText,
                        Booking_Starting_Time = bookingStartTimeText,
                        Booking_Duration_minutes = (int) bookingDuration,
                        Booking_Page = bookingPage,
                        Master_Page = masterPage,
                        Booking_Location = bookingLocation,
                        Event_Type = eventType,
                        External_Calendar = externalCalendarText,
                        Booking_Last_Updated_Time = bookingLastUpdatedTimeText,
                        Conversation = conversation,
                        Rescheduled_Booking_Id = bookingRescheduleId,
                        Cancel_Reschedule_URL = cancelRescheduleURL,
                        Cancel_Reschedule_Information = cancelRescheduleText,
                        Customer_Name = customerName,
                        Customer_First_Name = customerFirstName,
                        Customer_Last_Name = customerLastName,
                        Customer_Email = customerEmail,
                        Customer_Phone = customerPhone,
                        Customer_Mobile = customerMobile,
                        Customer_Company = customerCompany,
                        Customer_Note_1 = customerNote,
                        Customer_Guests = customerGuestsText,
                        Customer_Additional_Information = customerAdditionalText,
                        Client_Time_Zone = customerTimezoneText,
                        Client_Booking_Start_Time = customerBookingStartTime,
                        
                    };

                    if (!string.IsNullOrEmpty(userId))
                    {
                        bookingForCreation.Owner = userId;
                    }

                    if (!string.IsNullOrEmpty(userTimeZoneText))
                    {
                        bookingForCreation.Agent_Time_Zone = userTimeZoneText;
                    }

                    if (!string.IsNullOrEmpty(userBookingStartTime))
                    {
                        bookingForCreation.Agent_Booking_Start_Time = userBookingStartTime;
                    }

                    if (StringHelpers.IsValidURL(bookingJoinUrl))
                    {
                        bookingForCreation.Booking_Join_URL = bookingJoinUrl;
                    }
                    else
                    {
                        bookingForCreation.Booking_Join_Text = bookingJoinUrl;
                    }

                    if (!string.IsNullOrEmpty(setter))
                    {
                        bookingForCreation.Appointment_Setter = setter;
                    }

                    var createBookingRequest = new UpsertRequest<BookingForCreation>();
                    createBookingRequest.data.Add(bookingForCreation);
                    createBookingRequest.trigger.Add(CommonConstants.ZohoWorkflow);
                    Thread.Sleep(500);
                    var createBookingResponse = await _oneCorpCrmService.CreateBooking(createBookingRequest);
                    if (createBookingResponse.Code != ResultCode.OK)
                    {
                        apiResult.Message = OneCorpConstants.SSB_CreateBooking_400;
                        return apiResult;
                    }
                }
                else if (searchBookingsResponse.Code == ResultCode.OK)
                {
                    isUpdate = true;
                    // Step 2.2: Booking has already been synced
                    var soBookingDetails = searchBookingsResponse.Data.data[0];
                    string soBookingId = soBookingDetails.id;
                    var bookingForUpdation = new BookingForUpdation()
                    {
                        // Booking_Status = bookingStatusText,
                        Tracking_Id = trackingId,
                        Booking_Subject = bookingSubject,
                        Booking_Created_Time = bookingCreationTimeText,
                        Booking_Starting_Time = bookingStartTimeText,
                        Booking_Duration_minutes = (int) bookingDuration,
                        Booking_Page = bookingPage,
                        Booking_Owner = bookingOwner,
                        Booking_Owner_Name = bookingUserFullName,
                        Booking_Owner_First_Name = bookingUserFirstName,
                        Booking_Owner_Last_Name = bookingUserLastName,
                        Booking_Owner_Email = userEmail,
                        Master_Page = masterPage,
                        Booking_Location = bookingLocation,
                        Event_Type = eventType,
                        External_Calendar = externalCalendarText,
                        Booking_Last_Updated_Time = bookingLastUpdatedTimeText,
                        Conversation = conversation,
                        Rescheduled_Booking_Id = bookingRescheduleId,
                        Cancel_Reschedule_URL = cancelRescheduleURL,
                        Cancel_Reschedule_Information = cancelRescheduleText,
                        Customer_Name = customerName,
                        Customer_First_Name = customerFirstName,
                        Customer_Last_Name = customerLastName,
                        Customer_Email = customerEmail,
                        Customer_Phone = customerPhone,
                        Customer_Mobile = customerMobile,
                        Customer_Company = customerCompany,
                        Customer_Note = customerNote,
                        Customer_Guests = customerGuestsText,
                        Customer_Additional_Information = customerAdditionalText,
                        Client_Time_Zone = customerTimezoneText,
                        Client_Booking_Start_Time = customerBookingStartTime
                    };

                    if (!string.IsNullOrEmpty(userId))
                    {
                        bookingForUpdation.Owner = userId;
                    }

                    if (!string.IsNullOrEmpty(userTimeZoneText))
                    {
                        bookingForUpdation.Agent_Time_Zone = userTimeZoneText;
                    }

                    if (!string.IsNullOrEmpty(userBookingStartTime))
                    {
                        bookingForUpdation.Agent_Booking_Start_Time = userBookingStartTime;
                    }

                    if (StringHelpers.IsValidURL(bookingJoinUrl))
                    {
                        bookingForUpdation.Booking_Join_URL = bookingJoinUrl;
                    }
                    else
                    {
                        bookingForUpdation.Booking_Join_Text = bookingJoinUrl;
                    }

                    if (!string.IsNullOrEmpty(setter))
                    {
                        bookingForUpdation.Appointment_Setter = setter;
                    }

                    var updateBookingRequest = new UpsertRequest<BookingForUpdation>();
                    updateBookingRequest.data.Add(bookingForUpdation);
                    updateBookingRequest.trigger.Add(CommonConstants.ZohoWorkflow);
                    Thread.Sleep(500);
                    var updateBookingResponse = await _oneCorpCrmService.UpdateBooking(soBookingId, updateBookingRequest);
                    if (updateBookingResponse.Code != ResultCode.OK)
                    {
                        apiResult.Message = OneCorpConstants.SSB_UpdateBooking_400;
                        return apiResult;
                    }

                    // Step 2.3: Handle Booking Status
                    if (bookingStatus.Contains("cancel", StringComparison.InvariantCultureIgnoreCase))
                    {
                        if (cancelReschedule != null)
                        {
                            string actionedBy = cancelReschedule.actioned_by;
                            var updateBlueprintRequest = new UpdateBlueprintRequest();
                            if (actionedBy.Equals("customer",
                                StringComparison.InvariantCultureIgnoreCase))
                            {
                                var blueprints = new List<Blueprint>();
                                var blueprint = new Blueprint()
                                {
                                    transition_id = OneCorpConstants.ZohoCRM_ClientCancelled_TransitionId,
                                    data = new EmptyObject()
                                };
                                blueprints.Add(blueprint);
                                updateBlueprintRequest.blueprint = blueprints;
                            }
                            else if (actionedBy.Equals("user",
                                StringComparison.InvariantCultureIgnoreCase))
                            {
                                var blueprints = new List<Blueprint>();
                                var blueprint = new Blueprint()
                                {
                                    transition_id = OneCorpConstants.ZohoCRM_StrategistCancelled_TransitionId,
                                    data = new EmptyObject()
                                };
                                blueprints.Add(blueprint);
                                updateBlueprintRequest.blueprint = blueprints;
                            }
                            var updateBlueprintResponse = await
                                _oneCorpCrmService.UpdateBlueprint("ScheduleOnce_Bookings", soBookingId, updateBlueprintRequest);
                            if (updateBookingResponse.Code != ResultCode.OK)
                            {
                                apiResult.Message = OneBudgetConstants.SSB_UpdateBooking_400;
                                return apiResult;
                            }

                        }
                    }

                }
                else
                {
                    apiResult.Message = OneCorpConstants.SSB_SearchBooking_400;
                    return apiResult;
                }

                apiResult.Code = ResultCode.OK;
                apiResult.Message = OneCorpConstants.SSBC_200;
                if (isUpdate)
                {
                    apiResult.Message = OneCorpConstants.SSBU_200;
                }
                
                return apiResult;
            }
            catch (Exception ex)
            {
                apiResult.Data = $"{ex.Message + " - " + ex.StackTrace}";
                return apiResult;
            }

        }

        public async Task<ApiResultDto<string>> SyncBookingById(string bookingId)
        {
            var apiResult = new ApiResultDto<string>()
            {
                Code = ResultCode.BadRequest,
                Message = OneCorpConstants.SBI_400
            };

            try
            {
                // STEP 1: Get Booking Details
                var getBookingByIdResponse = await _oneCorpOnceHubService.GetBookingById(bookingId);
                var bookingDetails = getBookingByIdResponse.Data;

                // Step 1.1: Extract booking data
                string trackingId = bookingDetails.tracking_id;
                string bookingSubject = bookingDetails.subject;
                string bookingStatus = bookingDetails.status;
                string bookingStatusText = bookingStatus.Replace('_', ' ').ToUpper();

                /*
                if (bookingStatusText == "COMPLETED")
                {
                    apiResult.Code = ResultCode.OK;
                    apiResult.Message = OneCorpConstants.SSB_StatusCompleted;
                    return apiResult;
                }
                */

                // Step 1.2: Handle Booking Owner
                string bookingOwner = bookingDetails.owner;
                var getOnceHubUserByIdResponse = await _oneCorpOnceHubService.GetUserById(bookingOwner);

                var onceHubUserDetails = getOnceHubUserByIdResponse.Data;
                string userEmail = onceHubUserDetails.email;
                string userQuery = $"select time_zone from users WHERE email = '{userEmail}'";

                var selectQuery = new ZohoCoqlRequest()
                {
                    select_query = userQuery
                };
                string userId = string.Empty;
                string userTimeZoneText = string.Empty;
                string userBookingStartTime = string.Empty;
                var queryUsersResponse = await _oneCorpCrmService.QueryUsers(selectQuery);
                if (queryUsersResponse.Code == ResultCode.OK)
                {
                    var userDetails = queryUsersResponse.Data.data[0];
                    userId = userDetails.id;
                    var getCrmUserByIdResponse = await _oneCorpCrmService.GetUserById(userId);
                    var crmUserDetails = getCrmUserByIdResponse.Data.users[0];
                    userTimeZoneText = crmUserDetails.time_zone;
                    var userTimeZone = DateTimeHelpers.OlsonTimeZoneToTimeZoneInfo(userTimeZoneText);
                    if (userTimeZone == null)
                    {
                        userTimeZone = TimeZoneInfo.FindSystemTimeZoneById("E. Australia Standard Time");
                    }
                    userBookingStartTime = TimeZoneInfo.ConvertTimeFromUtc(bookingDetails.starting_time.Value, userTimeZone).ToString("yyyy-MM-ddTHH:mm:ss");
                }

                var tz = TimeZoneInfo.FindSystemTimeZoneById("E. Australia Standard Time");
                string bookingCreationTimeText = TimeZoneInfo.ConvertTimeFromUtc
                    (bookingDetails.creation_time.Value, tz).ToString("yyyy-MM-ddTHH:mm:ss");
                string bookingStartTimeText = TimeZoneInfo.ConvertTimeFromUtc
                    (bookingDetails.starting_time.Value, tz).ToString("yyyy-MM-ddTHH:mm:ss");
                string bookingLastUpdatedTimeText = TimeZoneInfo.ConvertTimeFromUtc
                    (bookingDetails.last_updated_time.Value, tz).ToString("yyyy-MM-ddTHH:mm:ss");

                string customerTimeZone = bookingDetails.customer_timezone;

                decimal? bookingDuration = bookingDetails.duration_minutes;
                var virtualConferencing = bookingDetails.virtual_conferencing;
                string bookingJoinUrl = string.Empty;
                if (virtualConferencing != null)
                {
                    bookingJoinUrl = virtualConferencing.join_url;
                }
                string bookingLocation = bookingDetails.location_description;

                // Step 1.2: Extract reschedule data
                string bookingRescheduleId = bookingDetails.rescheduled_booking_id;
                string cancelRescheduleURL = bookingDetails.cancel_reschedule_url;
                var cancelReschedule = bookingDetails.cancel_reschedule_information;
                string cancelRescheduleText = string.Empty;
                if (cancelReschedule != null)
                {
                    cancelRescheduleText = $"- Reason: {cancelReschedule.reason}\n- Actioned By: {cancelReschedule.actioned_by}\n" +
                        $"- User Id: {cancelReschedule.user_id}";
                }

                // Step 1.3: Extract Form Submission
                var formSubmissionData = bookingDetails.form_submission;
                string customerName = formSubmissionData.name;
                string customerEmail = formSubmissionData.email;
                string customerPhone = formSubmissionData.phone;
                string customerMobile = formSubmissionData.mobile_phone;
                customerMobile = customerMobile.Replace("-", "");
                if (!customerMobile.StartsWith("+"))
                {
                    customerMobile = "+" + customerMobile;
                }
                string customerNote = formSubmissionData.note;
                string customerCompany = formSubmissionData.company;
                var customerGuests = formSubmissionData.guests;
                string customerGuestsText = string.Empty;
                int guestCount = 1;
                if (customerGuests != null && customerGuests.Length > 0)
                {
                    foreach (var guest in customerGuests)
                    {
                        if (string.IsNullOrEmpty(customerGuestsText))
                        {
                            customerGuestsText = $"{guestCount}. {guest}";
                        }
                        else
                        {
                            customerGuestsText += $"\n{guestCount}. {guest}";
                        }
                        guestCount++;
                    }
                }
                
                var customerAdditionalDetails = formSubmissionData.custom_fields;
                string customerAdditionalText = string.Empty;
                if (customerAdditionalDetails != null && customerAdditionalDetails.Count() > 0)
                {
                    foreach (var additional in customerAdditionalDetails)
                    {
                        string cfName = additional.name;
                        string cfValue = additional.value;

                        if (cfName.Equals("lname", StringComparison.InvariantCultureIgnoreCase))
                        {
                            customerName = $"{customerName} {cfValue}";
                        }

                        if (string.IsNullOrEmpty(customerAdditionalText))
                        {
                            customerAdditionalText = $"{cfName}: {cfValue}";
                        }
                        else
                        {
                            customerAdditionalText += $"\n{cfName}: {cfValue}";
                        }
                    }
                }

                // Step 1.4: Other Information
                string bookingPage = bookingDetails.booking_page;
                string masterPage = bookingDetails.master_page;
                string eventType = bookingDetails.event_type;
                string conversation = bookingDetails.conversation;
                var externalCalendar = bookingDetails.external_calendar;
                string externalCalendarText = string.Empty;
                if (externalCalendar != null)
                {
                    externalCalendarText = $"- Type: {externalCalendar.type}\n- Name: {externalCalendar.name}\n" +
                        $"- Id: {externalCalendar.id}\n- Event Id: {externalCalendar.event_id}";
                }

                // STEP 2: Search ScheduleOne Booking on Zoho CRM
                string criteria = $"(Booking_Id:equals:{HttpUtility.UrlEncode(bookingId)})";
                Thread.Sleep(500);
                var searchBookingsResponse = await _oneCorpCrmService.SearchScheduleOnceBookings(criteria);
                if (searchBookingsResponse.Code == ResultCode.NoContent)
                {
                    // Step 2.1: Booking hasn't been synced yet
                    var bookingForCreation = new BookingForCreation()
                    {
                        Booking_Id = bookingId,
                        Booking_Status = bookingStatusText,
                        Booking_Owner = bookingOwner,
                        Tracking_Id = trackingId,
                        Booking_Subject = bookingSubject,
                        Booking_Created_Time = bookingCreationTimeText,
                        Booking_Starting_Time = bookingStartTimeText,
                        Booking_Duration_minutes = (int) bookingDuration,
                        Booking_Page = bookingPage,
                        Master_Page = masterPage,
                        Booking_Location = bookingLocation,
                        Event_Type = eventType,
                        External_Calendar = externalCalendarText,
                        Booking_Last_Updated_Time = bookingLastUpdatedTimeText,
                        Conversation = conversation,
                        Rescheduled_Booking_Id = bookingRescheduleId,
                        Cancel_Reschedule_URL = cancelRescheduleURL,
                        Cancel_Reschedule_Information = cancelRescheduleText,
                        Customer_Name = customerName,
                        Customer_Email = customerEmail,
                        Customer_Phone = customerPhone,
                        Customer_Mobile = customerMobile,
                        Customer_Company = customerCompany,
                        Customer_Note_1 = customerNote,
                        Customer_Guests = customerGuestsText,
                        Customer_Additional_Information = customerAdditionalText,
                        Client_Time_Zone = customerTimeZone,
                    };

                    if (StringHelpers.IsValidURL(bookingJoinUrl))
                    {
                        bookingForCreation.Booking_Join_URL = bookingJoinUrl;
                    }
                    else
                    {
                        bookingForCreation.Booking_Join_Text = bookingJoinUrl;
                    }

                    var createBookingRequest = new UpsertRequest<BookingForCreation>();
                    createBookingRequest.data.Add(bookingForCreation);
                    createBookingRequest.trigger.Add(CommonConstants.ZohoWorkflow);
                    Thread.Sleep(500);
                    var createBookingResponse = await _oneCorpCrmService.CreateBooking(createBookingRequest);
                    if (createBookingResponse.Code != ResultCode.OK)
                    {
                        apiResult.Message = OneCorpConstants.SSB_CreateBooking_400;
                        return apiResult;
                    }
                }
                else if (searchBookingsResponse.Code == ResultCode.OK)
                {
                    // Step 2.2: Booking has already been synced
                    var soBookingDetails = searchBookingsResponse.Data.data[0];
                    string soBookingId = soBookingDetails.id;
                    var bookingForUpdation = new BookingForUpdation()
                    {
                        // Booking_Status = bookingStatusText,
                        Booking_Owner = bookingOwner,
                        Tracking_Id = trackingId,
                        Booking_Subject = bookingSubject,
                        Booking_Created_Time = bookingCreationTimeText,
                        Booking_Starting_Time = bookingStartTimeText,
                        Booking_Duration_minutes = (int) bookingDuration,
                        Booking_Page = bookingPage,
                        Master_Page = masterPage,
                        Booking_Location = bookingLocation,
                        Event_Type = eventType,
                        External_Calendar = externalCalendarText,
                        Booking_Last_Updated_Time = bookingLastUpdatedTimeText,
                        Conversation = conversation,
                        Rescheduled_Booking_Id = bookingRescheduleId,
                        Cancel_Reschedule_URL = cancelRescheduleURL,
                        Cancel_Reschedule_Information = cancelRescheduleText,
                        Customer_Name = customerName,
                        Customer_Email = customerEmail,
                        Customer_Phone = customerPhone,
                        Customer_Mobile = customerMobile,
                        Customer_Company = customerCompany,
                        Customer_Note = customerNote,
                        Customer_Guests = customerGuestsText,
                        Customer_Additional_Information = customerAdditionalText
                    };
                    if (StringHelpers.IsValidURL(bookingJoinUrl))
                    {
                        bookingForUpdation.Booking_Join_URL = bookingJoinUrl;
                    }
                    else
                    {
                        bookingForUpdation.Booking_Join_Text = bookingJoinUrl;
                    }
                    var updateBookingRequest = new UpsertRequest<BookingForUpdation>();
                    updateBookingRequest.data.Add(bookingForUpdation);
                    updateBookingRequest.trigger.Add(CommonConstants.ZohoWorkflow);
                    Thread.Sleep(500);
                    var updateBookingResponse = await _oneCorpCrmService.UpdateBooking(soBookingId, updateBookingRequest);
                    if (updateBookingResponse.Code != ResultCode.OK)
                    {
                        apiResult.Message = OneCorpConstants.SSB_UpdateBooking_400;
                        return apiResult;
                    }
                }
                else
                {
                    apiResult.Message = OneCorpConstants.SSB_SearchBooking_400;
                    return apiResult;
                }

                apiResult.Code = ResultCode.OK;
                apiResult.Message = OneCorpConstants.SBI_200;
                return apiResult;
            }
            catch (Exception ex)
            {
                apiResult.Data = $"{ex.Message + " - " + ex.StackTrace}";
                return apiResult;
            }
        }

        #endregion

        #region Lead Distribution System

        public async Task<ApiResultDto<string>> SyncCallToDBUponCreationUpdation(string callId)
        {

            var apiResult = new ApiResultDto<string>()
            {
                Code = ResultCode.BadRequest,
                Message = OneCorpConstants.SCMCU_400
            };

            try
            {
                // STEP 1: Get Call by Id
                var getCallByIdResult = await _oneCorpCrmService.GetCallById(callId);
                if (getCallByIdResult.Code != ResultCode.OK)
                {
                    apiResult.Message = OneCorpConstants.SCM_GetCallById_400;
                    return apiResult;
                }

                var callDetails = getCallByIdResult.Data.data[0];

                // STEP 2: Connect to MySQL DB
                var (sshClient, localPort) = DatabaseHelpers.ConnectSsh(OneCorpConstants.SSHHostname,
                    OneCorpConstants.SSHUsername, OneCorpConstants.SSHPassword);
                using (sshClient)
                {
                    MySqlConnectionStringBuilder csb = new MySqlConnectionStringBuilder
                    {
                        Server = "127.0.0.1",
                        Port = localPort,
                        UserID = OneCorpConstants.MySQLUsername,
                        Password = OneCorpConstants.MySQLPassword,
                        Database = OneCorpConstants.MySQLSchema
                    };

                    using var connection = new MySqlConnection(csb.ConnectionString);
                    connection.Open();

                    // Step 2.1: Check if Call exist in DB
                    using var selectCommand = new MySqlCommand($"SELECT COUNT(*) FROM Calls WHERE Id = {callId};", connection);
                    int callCount = Convert.ToInt32(selectCommand.ExecuteScalar());

                    // Step 2.2 Prepare Query to Insert to DB
                    string id = callDetails.id;

                    var tz = TimeZoneInfo.FindSystemTimeZoneById("E. Australia Standard Time");
                    var callStartTime = callDetails.Call_Start_Time;
                    string callStartTimeStr = null;
                    if (callStartTime.HasValue)
                    {
                        callStartTimeStr = TimeZoneInfo.ConvertTime(callStartTime.Value, tz).ToString("yyyy-MM-dd HH:mm:ss");
                    }
                    string callOwnerName = callDetails.Owner.name;
                    string callOwnerId = callDetails.Owner.id;
                    string callSubject = callDetails.Subject;
                    string callType = callDetails.Call_Type;
                    string callPurpose = callDetails.Call_Purpose;

                    var createdTime = callDetails.Created_Time;
                    string createdTimeStr = string.Empty;
                    if (createdTime.HasValue)
                    {
                        createdTimeStr = TimeZoneInfo.ConvertTime(createdTime.Value, tz).ToString("yyyy-MM-dd HH:mm:ss");
                    }

                    var modifiedTime = callDetails.Modified_Time;
                    var modifiedTimeStr = string.Empty;
                    if (modifiedTime.HasValue)
                    {
                        modifiedTimeStr = TimeZoneInfo.ConvertTime(modifiedTime.Value, tz).ToString("yyyy-MM-dd HH:mm:ss");
                    }
                    int callDurationInSeconds = callDetails.Call_Duration_in_seconds.HasValue
                        ? callDetails.Call_Duration_in_seconds.Value : 0;

                    string seModule = callDetails.se_module;
                    string contactId = string.Empty;
                    string contactName = string.Empty;
                    string leadId = string.Empty;
                    string accountId = string.Empty;
                    string potentialId = string.Empty;
                    string relatedTo = string.Empty;

                    var whoId = callDetails.Who_Id;
                    if (whoId != null)
                    {
                        contactId = whoId.id;
                        contactName = contactId;
                    }

                    var whatId = callDetails.What_Id;
                    if (whatId != null)
                    {
                        relatedTo = whatId.id;
                        switch (seModule)
                        {
                            case "Leads":
                                leadId = whatId.id;
                                break;
                            case "Accounts":
                                accountId = whatId.id;
                                break;
                            case "Deals":
                                potentialId = whatId.id;
                                break;
                            default:
                                break;
                        }
                    }
                    int callDurationInMinutes = callDurationInSeconds / 60;
                    string callerId = callDetails.Caller_ID;
                    string callAgenda = callDetails.Call_Agenda;
                    string callDuration = callDetails.Call_Duration;
                    string callResult = callDetails.Call_Result;
                    string callStatus = callDetails.Call_Status;

                    string createdById = string.Empty;
                    var createdBy = callDetails.Created_By;
                    if (createdBy != null)
                    {
                        createdById = createdBy.id;
                    }
                    string description = callDetails.Description;
                    string dialledNumber = callDetails.Dialled_Number;

                    var tags = callDetails.Tag;
                    string tagStr = string.Empty;
                    if (tags.Length > 0)
                    {
                        tagStr = string.Join(',', tags);
                    }

                    string modifiedById = string.Empty;
                    var modifiedBy = callDetails.Modified_By;
                    if (modifiedBy != null)
                    {
                        modifiedById = modifiedBy.id;
                    }

                    if (callCount == 0)
                    {
                        // Step 2.3: Insert Call record to DB
                        string insertQuery = $@"INSERT INTO `Calls` (`Id`, `CallStartTime`, `CallOwnerName`, `CallOwner`, 
                                        `Subject`, `CallType`, `CallPurpose`, `CreatedTime`, `ModifiedTime`, `CallDurationInSeconds`, 
                                        `ContactId`, `AccountId`, `PotentialId`, `LeadId`, `CallDurationInMinutes`, `ContactName`, `RelatedTo`, 
                                        `SeModule`, `CallerId`, `CallAgenda`, `CallDuration`, `CallResult`, `CallStatus`, `CreatedBy`, `Description`, 
                                        `DialledNumber`, `ModifiedBy`) VALUES
	                                    ('{id}', '{callStartTimeStr}', '{callOwnerName}', '{callOwnerId}', '{callSubject}', '{callType}', 
                                        '{callPurpose}', '{createdTimeStr}', '{modifiedTimeStr}', {callDurationInSeconds}, '{contactId}', '{accountId}', '{potentialId}', 
                                        '{leadId}', {callDurationInMinutes}, '{contactName}', '{relatedTo}', '{seModule}', '{callerId}', '{callAgenda}', '{callDuration}', 
                                        '{callResult}', '{callStatus}', '{createdById}', '{description}', '{dialledNumber}', '{modifiedById}')";

                        using var insertCommand = new MySqlCommand(insertQuery, connection);
                        int insertCount = insertCommand.ExecuteNonQuery();
                        apiResult.Message = OneCorpConstants.SCM_200_INSERT;
                    }
                    else
                    {
                        // Step 2.3: Update Call record to DB
                        string updateQuery = $@"UPDATE `Calls` SET `CallStartTime` = '{callStartTimeStr}', 
                                        `CallOwnerName` = '{callOwnerName}', `CallOwner` = '{callOwnerId}', `Subject` = '{callSubject}', 
                                        `CallType` = '{callType}', `CallPurpose` = '{callPurpose}', `CreatedTime` = '{createdTimeStr}', 
                                        `ModifiedTime` = '{modifiedTimeStr}', `CallDurationInSeconds` = {callDurationInSeconds}, 
                                        `ContactId` = '{contactId}', `AccountId` = '{accountId}', `PotentialId` = '{potentialId}', 
                                        `LeadId` = '{leadId}', `CallDurationInMinutes` = {callDurationInMinutes}, `ContactName` = '{contactName}', 
                                        `RelatedTo` = '{relatedTo}', `SeModule` = '{seModule}', `CallerId` = '{callerId}', 
                                        `CallAgenda` = '{callAgenda}', `CallDuration` = '{callDuration}', `CallResult` = '{callResult}', 
                                        `CallStatus` = '{callStatus}', `CreatedBy` = '{createdById}', `Description` = '{description}', 
                                        `DialledNumber` = '{dialledNumber}', `ModifiedBy` = '{modifiedById}' WHERE `Id` = '{id}'";

                        using var updateCommand = new MySqlCommand(updateQuery, connection);
                        int updateCount = updateCommand.ExecuteNonQuery();
                        apiResult.Message = OneCorpConstants.SCM_200_UPDATE;
                    }

                }

                apiResult.Code = ResultCode.OK;

                return apiResult;
            }
            catch (Exception ex)
            {
                apiResult.Data = $"{ex.Message + " - " + ex.StackTrace}";
                return apiResult;
            }

        }

        public async Task<ApiResultDto<string>> SyncTaskToDBUponCreationUpdation(string taskId)
        {

            var apiResult = new ApiResultDto<string>()
            {
                Code = ResultCode.BadRequest,
                Message = OneCorpConstants.STMCU_400
            };

            try
            {
                // STEP 1: Get Task by Id
                var getTaskByIdResult = await _oneCorpCrmService.GetTaskById(taskId);
                if (getTaskByIdResult.Code != ResultCode.OK)
                {
                    apiResult.Message = OneCorpConstants.SCM_GetCallById_400;
                    return apiResult;
                }

                var taskDetails = getTaskByIdResult.Data.data[0];

                string taskLdsId = taskDetails.LDS_Id;

                int ldsId = 0;

                // STEP 2: Connect to MySQL DB
                var (sshClient, localPort) = DatabaseHelpers.ConnectSsh(OneCorpConstants.SSHHostname,
                    OneCorpConstants.SSHUsername, OneCorpConstants.SSHPassword);
                using (sshClient)
                {
                    MySqlConnectionStringBuilder csb = new MySqlConnectionStringBuilder
                    {
                        Server = "127.0.0.1",
                        Port = localPort,
                        UserID = OneCorpConstants.MySQLUsername,
                        Password = OneCorpConstants.MySQLPassword,
                        Database = OneCorpConstants.MySQLSchema
                    };

                    using var connection = new MySqlConnection(csb.ConnectionString);
                    connection.Open();

                    // Step 2.1: Check if Task exist in DB
                    using var selectCommand = new MySqlCommand(@$"SELECT COUNT(*) FROM Tasks 
                            WHERE ZohoCrmId = {taskId};", connection);
                    int taskCount = Convert.ToInt32(selectCommand.ExecuteScalar());

                    // Step 2.2: Prepare Query to Insert to DB
                    string taskSubject = "NULL";
                    if (!string.IsNullOrEmpty(taskDetails.Subject))
                    {
                        taskSubject = $"'{StringHelpers.CleanSingleQuote(taskDetails.Subject)}'";
                    }
                    string taskStatus = "NULL";
                    if (!string.IsNullOrEmpty(taskDetails.Status))
                    {
                        taskStatus = $"'{StringHelpers.CleanSingleQuote(taskDetails.Status)}'";
                    }
                    string taskPriority = "NULL";
                    if (!string.IsNullOrEmpty(taskDetails.Priority))
                    {
                        taskPriority = $"'{StringHelpers.CleanSingleQuote(taskDetails.Priority)}'";
                    }
                    string taskDescription = "NULL";
                    if (!string.IsNullOrEmpty(taskDetails.Description))
                    {
                        taskDescription = $"'{StringHelpers.CleanSingleQuote(taskDetails.Description)}'";
                    }
                    var taskOwner = taskDetails.Owner;
                    string taskOwnerId = $"'{taskOwner.id}'";
                    string taskOwnerName = $"'{StringHelpers.CleanSingleQuote(taskOwner.name)}'";

                    // STEP 2.2.1: Handle Time Field
                    var tz = TimeZoneInfo.FindSystemTimeZoneById("E. Australia Standard Time");

                    var closedTime = taskDetails.Closed_Time;
                    string closedTimeStr = "NULL";
                    if (closedTime.HasValue)
                    {
                        closedTimeStr = closedTime.Value.ToString("yyyy-MM-dd HH:mm:ss");
                        closedTimeStr = $"'{closedTimeStr}'";
                    }

                    var dueDate = taskDetails.Due_Date;
                    string dueDateStr = "NULL";
                    if (!string.IsNullOrEmpty(dueDate))
                    {
                        dueDateStr = $"'{dueDate} 00:00:01'";
                    }

                    var createdTime = taskDetails.Created_Time;
                    string createdTimeStr = "NULL";
                    if (createdTime.HasValue)
                    {
                        createdTimeStr = createdTime.Value.ToString("yyyy-MM-dd HH:mm:ss");
                        createdTimeStr = $"'{createdTimeStr}'";
                    }

                    var modifiedTime = taskDetails.Modified_Time;
                    string modifiedTimeStr = "NULL";
                    if (modifiedTime.HasValue)
                    {
                        modifiedTimeStr = modifiedTime.Value.ToString("yyyy-MM-dd HH:mm:ss");
                        modifiedTimeStr = $"'{modifiedTimeStr}'";
                    }

                    var whoId = taskDetails.Who_Id;
                    string contactId = "NULL";
                    string contactName = "NULL";
                    if (whoId != null)
                    {
                        contactId = $"'{whoId.id}'";
                        contactName = $"'{StringHelpers.CleanSingleQuote(whoId.name)}'";
                    }

                    string whatModule = "NULL";
                    if (!string.IsNullOrEmpty(taskDetails.What_Module))
                    {
                        whatModule = $"'{StringHelpers.CleanSingleQuote(taskDetails.What_Module)}'";
                    }
                    var whatId = taskDetails.What_Id;
                    string whatIdStr = "NULL";
                    string whatName = "NULL";
                    if (whatId != null)
                    {
                        whatIdStr = $"'{whatId.id}'";
                        whatName = $"'{StringHelpers.CleanSingleQuote(whatId.name)}'";
                    }

                    var createdBy = taskDetails.Created_By;
                    string createdById = $"'{createdBy.id}'";
                    string createdByName = $"'{StringHelpers.CleanSingleQuote(createdBy.name)}'";

                    var modifiedBy = taskDetails.Modified_By;
                    string modifiedById = "NULL";
                    string modifiedByName = "NULL";
                    if (modifiedBy != null)
                    {
                        modifiedById = $"'{modifiedBy.id}'";
                        modifiedByName = $"'{StringHelpers.CleanSingleQuote(modifiedBy.name)}'";
                    }

                    if (taskCount == 0)
                    {
                        // Step 2.3.1: Handle Insert Task query
                        string fieldPart = @"(`ZohoCRMId`, `Subject`, `Status`, `ClosedTime`,
                                        `DueDate`, `ContactId`, `ContactName`, `Module`, `WhatId`,
                                        `WhatName`, `Priority`, `Description`, `OwnerId`, `Owner`,
                                        `CreatedById`, `CreatedBy`, `CreatedTime`, `ModifiedById`,
                                        `ModifiedBy`, `ModifiedTime`)";

                        string valuesPart = $@"('{taskId}', {taskSubject}, {taskStatus}, {closedTimeStr},
                                        {dueDateStr}, {contactId}, {contactName}, {whatModule}, {whatIdStr},
                                        {whatName}, {taskPriority}, {taskDescription}, {taskOwnerId}, {taskOwnerName},
                                        {createdById}, {createdByName}, {createdTimeStr}, {modifiedById}, {modifiedByName},
                                        {modifiedTimeStr})";

                        // Step 2.3.2: Insert Task record to DB
                        string insertQuery = $@"INSERT INTO Tasks {fieldPart} VALUES {valuesPart}; SELECT LAST_INSERT_ID();";

                        using var insertCommand = new MySqlCommand(insertQuery, connection);
                        ldsId = int.Parse(insertCommand.ExecuteScalar().ToString());
                        apiResult.Message = OneCorpConstants.STMCU_200_INSERT;
                    }
                    else
                    {
                        // Step 2.3: Update Call record to DB
                        string updatePart = @$"Subject = {taskSubject}, Status = {taskStatus},
                                        ClosedTime = {closedTimeStr}, DueDate = {dueDateStr}, ContactId = {contactId},
                                        ContactName = {contactName}, Module = {whatModule}, WhatId = {whatIdStr},
                                        WhatName = {whatName}, Priority = {taskPriority}, Description = {taskDescription},
                                        Owner = {taskOwnerName}, OwnerId = {taskOwnerId}, CreatedById = {createdById},
                                        CreatedBy = {createdByName}, CreatedTime = {createdTimeStr}, ModifiedById = {modifiedById},
                                        ModifiedBy = {modifiedByName}, ModifiedTime = {modifiedTimeStr}";

                        string updateQuery = $@"UPDATE Tasks SET {updatePart} WHERE ZohoCRMId = {taskId}; SELECT Id FROM Tasks WHERE ZohoCRMId = {taskId};";

                        using var updateCommand = new MySqlCommand(updateQuery, connection);
                        var updateResult = updateCommand.ExecuteScalar();
                        ldsId = int.Parse(updateCommand.ExecuteScalar().ToString());
                        apiResult.Message = OneCorpConstants.STMCU_200_UPDATE;
                    }
                }

                // Step 3: Update Task
                if (string.IsNullOrEmpty(taskLdsId) && ldsId > 0)
                {

                    var upsertRequest = new UpsertRequest<TaskForUpdation>();
                    var taskForUpdation = new TaskForUpdation()
                    {
                        LDS_Id = ldsId.ToString()
                    };
                    upsertRequest.data.Add(taskForUpdation);

                    var updateTaskResponse = await _oneCorpCrmService.UpdateTask(taskId, upsertRequest);
                    if (updateTaskResponse.Code != ResultCode.OK)
                    {
                        apiResult.Message = OneCorpConstants.STMCU_UpdateTask_400;
                        return apiResult;
                    }
                }

                apiResult.Code = ResultCode.OK;
                apiResult.Message = OneCorpConstants.STMCU_200;
                return apiResult;
            }
            catch (Exception ex)
            {
                apiResult.Data = $"{ex.Message + " - " + ex.StackTrace}";
                return apiResult;
            }

        }

        public ApiResultDto<string> SyncTaskToDBUponDeletion(string taskId)
        {
            var apiResult = new ApiResultDto<string>()
            {
                Code = ResultCode.BadRequest,
                Message = OneCorpConstants.STMD_400
            };

            try
            {

                // STEP 2: Connect to MySQL DB
                var (sshClient, localPort) = DatabaseHelpers.ConnectSsh(OneCorpConstants.SSHHostname,
                    OneCorpConstants.SSHUsername, OneCorpConstants.SSHPassword);
                using (sshClient)
                {
                    MySqlConnectionStringBuilder csb = new MySqlConnectionStringBuilder
                    {
                        Server = "127.0.0.1",
                        Port = localPort,
                        UserID = OneCorpConstants.MySQLUsername,
                        Password = OneCorpConstants.MySQLPassword,
                        Database = OneCorpConstants.MySQLSchema
                    };

                    using var connection = new MySqlConnection(csb.ConnectionString);
                    connection.Open();

                    string deleteQuery = $@"DELETE FROM Tasks WHERE ZohoCRMId = {taskId}";

                    using var deleteCommand = new MySqlCommand(deleteQuery, connection);
                    var updateResult = deleteCommand.ExecuteNonQuery();

                }

                apiResult.Code = ResultCode.OK;
                apiResult.Message = OneCorpConstants.STMD_200;
                return apiResult;
            }
            catch (Exception ex)
            {
                apiResult.Data = $"{ex.Message + " - " + ex.StackTrace}";
                return apiResult;
            }
        }

        public async Task<ApiResultDto<string>> SyncLeadToDBUponCreationUpdation(string leadId)
        {

            var apiResult = new ApiResultDto<string>()
            {
                Code = ResultCode.BadRequest,
                Message = OneCorpConstants.SLMCU_400
            };

            try
            {
                // STEP 1: Get Lead by Id
                var getLeadByIdResult = await _oneCorpCrmService.GetLeadById(leadId);
                if (getLeadByIdResult.Code != ResultCode.OK)
                {
                    apiResult.Message = OneCorpConstants.SLMCU_GetLeadById_400;
                    return apiResult;
                }

                var leadDetails = getLeadByIdResult.Data.data[0];

                string leadLdsId = leadDetails.LDS_Id;

                // STEP 2: Connect to MySQL DB
                int ldsId = 0;
                var (sshClient, localPort) = DatabaseHelpers.ConnectSsh(OneCorpConstants.SSHHostname,
                    OneCorpConstants.SSHUsername, OneCorpConstants.SSHPassword);
                using (sshClient)
                {
                    MySqlConnectionStringBuilder csb = new MySqlConnectionStringBuilder
                    {
                        Server = "127.0.0.1",
                        Port = localPort,
                        UserID = OneCorpConstants.MySQLUsername,
                        Password = OneCorpConstants.MySQLPassword,
                        Database = OneCorpConstants.MySQLSchema
                    };

                    using var connection = new MySqlConnection(csb.ConnectionString);
                    connection.Open();

                    // Step 2.1: Check if Lead exist in DB
                    using var selectCommand = new MySqlCommand(@$"SELECT COUNT(*) FROM Leads 
                            WHERE ZohoCrmId = {leadId};", connection);
                    int leadCount = Convert.ToInt32(selectCommand.ExecuteScalar());

                    // Step 2.2: Prepare Query to Insert to DB

                    #region Prepare Insert Query

                    string zohoCrmId = $"'{leadId}'";

                    string firstName = "NULL";
                    if (!string.IsNullOrEmpty(leadDetails.First_Name))
                    {
                        firstName = $"'{StringHelpers.CleanSingleQuote(leadDetails.First_Name)}'";
                    }
                    string lastName = "NULL";
                    if (!string.IsNullOrEmpty(leadDetails.Last_Name))
                    {
                        lastName = $"'{StringHelpers.CleanSingleQuote(leadDetails.Last_Name)}'";
                    }
                    string email = "NULL";
                    if (!string.IsNullOrEmpty(leadDetails.Email))
                    {
                        email = $"'{leadDetails.Email}'";
                    }
                    string phone = "NULL";
                    if (!string.IsNullOrEmpty(leadDetails.Phone))
                    {
                        phone = $"'{leadDetails.Phone}'";
                    }
                    string phoneOther = "NULL";
                    if (!string.IsNullOrEmpty(leadDetails.Phone_Other))
                    {
                        phoneOther = $"'{leadDetails.Phone_Other}'";
                    }
                    string age = "NULL";
                    if (!string.IsNullOrEmpty(leadDetails.Age))
                    {
                        age = $"'{leadDetails.Age}'";
                    }
                    string maritalStatus = "NULL";
                    if (!string.IsNullOrEmpty(leadDetails.Marital_Status1))
                    {
                        maritalStatus = $"'{leadDetails.Marital_Status1}'";
                    }
                    string numberOfDependents = "NULL";
                    if (!string.IsNullOrEmpty(leadDetails.Number_of_Dependents))
                    {
                        numberOfDependents = $"'{leadDetails.Number_of_Dependents}'";
                    }
                    string dependentAges = "NULL";
                    if (!string.IsNullOrEmpty(leadDetails.Dependent_1_Age))
                    {
                        dependentAges = $"'{leadDetails.Dependent_1_Age}'";
                    }
                    string wizard = "NULL";
                    if (!string.IsNullOrEmpty(leadDetails.Wizard))
                    {
                        wizard = $"'{leadDetails.Wizard}'";
                    }
                    string leadStatus = "NULL";
                    if (!string.IsNullOrEmpty(leadDetails.Lead_Status))
                    {
                        leadStatus = $"'{leadDetails.Lead_Status}'";
                    }
                    string leadQuality = "NULL";
                    if (!string.IsNullOrEmpty(leadDetails.Lead_Quality))
                    {
                        leadQuality = $"'{leadDetails.Lead_Quality}'";
                    }
                    string leadSource = "NULL";
                    if (!string.IsNullOrEmpty(leadDetails.Lead_Source))
                    {
                        leadSource = $"'{leadDetails.Lead_Source}'";
                    }
                    string referalContact = "NULL";
                    if (!string.IsNullOrEmpty(leadDetails.Referal_Contact1))
                    {
                        referalContact = $"'{leadDetails.Referal_Contact1}'";
                    }
                    string campaignsSegments = "NULL";
                    if (!string.IsNullOrEmpty(leadDetails.Campaigns_List))
                    {
                        campaignsSegments = $"'{leadDetails.Campaigns_List}'";
                    }
                    string marketingPlatform = "NULL";
                    if (!string.IsNullOrEmpty(leadDetails.Single_Line_143))
                    {
                        marketingPlatform = $"'{leadDetails.Single_Line_143}'";
                    }
                    string quizState = "NULL";
                    if (!string.IsNullOrEmpty(leadDetails.Quiz_State))
                    {
                        quizState = $"'{leadDetails.Quiz_State}'";
                    }
                    string adCampaignSource = "NULL";
                    if (!string.IsNullOrEmpty(leadDetails.Ad_Campaign_Source))
                    {
                        adCampaignSource = $"'{leadDetails.Ad_Campaign_Source}'";
                    }
                    string quizAgeGroup = "NULL";
                    if (!string.IsNullOrEmpty(leadDetails.Quiz_Age_Group))
                    {
                        quizAgeGroup = $"'{leadDetails.Quiz_Age_Group}'";
                    }
                    string quizType = "NULL";
                    if (!string.IsNullOrEmpty(leadDetails.Quiz_Type))
                    {
                        quizType = $"'{leadDetails.Quiz_Type}'";
                    }
                    string quizMaritalStatus = "NULL";
                    if (!string.IsNullOrEmpty(leadDetails.Quiz_Marital_Status))
                    {
                        quizMaritalStatus = $"'{leadDetails.Quiz_Marital_Status}'";
                    }
                    string quizHomeOwner = "NULL";
                    if (!string.IsNullOrEmpty(leadDetails.Quiz_Home_Owner))
                    {
                        quizHomeOwner = $"'{leadDetails.Quiz_Home_Owner}'";
                    }
                    string quizIncome = "NULL";
                    if (!string.IsNullOrEmpty(leadDetails.Quiz_Income))
                    {
                        quizIncome = $"'{leadDetails.Quiz_Income}'";
                    }
                    string quizMortgageTerm = "NULL";
                    if (!string.IsNullOrEmpty(leadDetails.Quiz_Mortgage_Term))
                    {
                        quizMortgageTerm = $"'{leadDetails.Quiz_Mortgage_Term}'";
                    }
                    string quizSuper = "NULL";
                    if (!string.IsNullOrEmpty(leadDetails.Quiz_Super))
                    {
                        quizSuper = $"'{leadDetails.Quiz_Super}'";
                    }
                    string quizSavings = "NULL";
                    if (!string.IsNullOrEmpty(leadDetails.Quiz_Savings))
                    {
                        quizSavings = $"'{leadDetails.Quiz_Savings}'";
                    }
                    string quizHowMuchEquity = "NULL";
                    if (!string.IsNullOrEmpty(leadDetails.Quiz_How_Much_Equity))
                    {
                        quizHowMuchEquity = $"'{leadDetails.Quiz_How_Much_Equity}'";
                    }
                    string quizHomeValue = "NULL";
                    if (!string.IsNullOrEmpty(leadDetails.Quiz_Home_Value))
                    {
                        quizHomeValue = $"'{leadDetails.Quiz_How_Much_Equity}'";
                    }
                    string quizResult = "NULL";
                    if (!string.IsNullOrEmpty(leadDetails.Quiz_Result))
                    {
                        quizResult = $"'{leadDetails.Quiz_Result}'";
                    }
                    string appointmentSetter = "NULL";
                    if (!string.IsNullOrEmpty(leadDetails.Appointment_Setter))
                    {
                        appointmentSetter = $"'{leadDetails.Appointment_Setter}'";
                    }
                    string appointmentType = "NULL";
                    if (!string.IsNullOrEmpty(leadDetails.Appointment_Type))
                    {
                        appointmentType = $"'{leadDetails.Appointment_Type}'";
                    }
                    string qcOutcome = "NULL";
                    if (!string.IsNullOrEmpty(leadDetails.QC_Outcome))
                    {
                        qcOutcome = $"'{leadDetails.QC_Outcome}'";
                    }
                    string qcFileCheckComplete = "NULL";
                    if (leadDetails.QC_File_Check_Complete.HasValue
                        && leadDetails.QC_File_Check_Complete.Value)
                    {
                        qcFileCheckComplete = "1";
                    }
                    else
                    {
                        qcFileCheckComplete = "0";
                    }
                    string mortgageBalance = "NULL";
                    if (leadDetails.PPR_Mortgage.HasValue)
                    {
                        mortgageBalance = $"{leadDetails.PPR_Mortgage}";
                    }
                    string salesPerson = "NULL";
                    if (!string.IsNullOrEmpty(leadDetails.Sales_Person))
                    {
                        salesPerson = $"'{leadDetails.Sales_Person}'";
                    }
                    string borrowingCapacityIP = "NULL";
                    if (!string.IsNullOrEmpty(leadDetails.Borrowing_Capacity_IP))
                    {
                        borrowingCapacityIP = $"'{leadDetails.Borrowing_Capacity_IP}'";
                    }
                    string borrowingCapacitySMSF = "NULL";
                    if (!string.IsNullOrEmpty(leadDetails.Borrowing_Capacity_SMSF))
                    {
                        borrowingCapacitySMSF = $"'{leadDetails.Borrowing_Capacity_SMSF}'";
                    }
                    string strategySessionOutcome = "NULL";
                    if (!string.IsNullOrEmpty(leadDetails.st_Appointment_Outcome))
                    {
                        strategySessionOutcome = $"'{leadDetails.st_Appointment_Outcome}'";
                    }
                    string homeOwner = "NULL";
                    if (!string.IsNullOrEmpty(leadDetails.Do_you_own_your_own_home))
                    {
                        homeOwner = $"'{leadDetails.Do_you_own_your_own_home}'";
                    }
                    string interestRate = "NULL";
                    if (!string.IsNullOrEmpty(leadDetails.PPR_Interest_Rate))
                    {
                        interestRate = $"'{leadDetails.PPR_Interest_Rate}'";
                    }
                    string homeValue = "NULL";
                    if (leadDetails.PPR_Home_Value.HasValue)
                    {
                        homeValue = $"{leadDetails.PPR_Home_Value}";
                    }
                    string reasonForEstimatedValue = "NULL";
                    if (!string.IsNullOrEmpty(leadDetails.Reason_for_estimated_value))
                    {
                        reasonForEstimatedValue = $"'{leadDetails.Reason_for_estimated_value}'";
                    }
                    string additionalMortgageRepayments = "NULL";
                    if (!string.IsNullOrEmpty(leadDetails.Additional_Mortgage_Repayments))
                    {
                        additionalMortgageRepayments = $"'{leadDetails.Additional_Mortgage_Repayments}'";
                    }
                    string street = "NULL";
                    if (!string.IsNullOrEmpty(leadDetails.Street))
                    {
                        street = $"'{leadDetails.Street}'";
                    }
                    string city = "NULL";
                    if (!string.IsNullOrEmpty(leadDetails.City))
                    {
                        city = $"'{leadDetails.City}'";
                    }
                    string state = "NULL";
                    if (!string.IsNullOrEmpty(leadDetails.State))
                    {
                        state = $"'{leadDetails.State}'";
                    }
                    string zipCode = "NULL";
                    if (!string.IsNullOrEmpty(leadDetails.Zip_Code))
                    {
                        zipCode = $"'{leadDetails.Zip_Code}'";
                    }
                    string pprLender = "NULL";
                    if (!string.IsNullOrEmpty(leadDetails.PPR_Lender))
                    {
                        pprLender = $"'{leadDetails.PPR_Lender}'";
                    }
                    string rentOrBoardPaid = "NULL";
                    if (leadDetails.Rent_or_Board_Paid.HasValue)
                    {
                        rentOrBoardPaid = $"{leadDetails.Rent_or_Board_Paid}";
                    }
                    string occupation = "NULL";
                    if (!string.IsNullOrEmpty(leadDetails.Occupation_position))
                    {
                        occupation = $"'{leadDetails.Occupation_position}'";
                    }
                    string grossSalary = "NULL";
                    if (leadDetails.Gross_Salary_Per_Annum.HasValue)
                    {
                        grossSalary = $"{leadDetails.Gross_Salary_Per_Annum}";
                    }
                    string employmentStatus = "NULL";
                    if (!string.IsNullOrEmpty(leadDetails.Employment_Status1))
                    {
                        employmentStatus = $"'{leadDetails.Employment_Status1}'";
                    }
                    string howLongInCurrentEmployement = "NULL";
                    if (!string.IsNullOrEmpty(leadDetails.How_long_in_current_employment))
                    {
                        howLongInCurrentEmployement = $"'{leadDetails.How_long_in_current_employment}'";
                    }
                    string shares = "NULL";
                    if (!string.IsNullOrEmpty(leadDetails.Shares))
                    {
                        shares = $"'{leadDetails.Shares}'";
                    }
                    string savings = "NULL";
                    if (leadDetails.Savings.HasValue)
                    {
                        savings = $"{leadDetails.Savings}";
                    }
                    string haveInvestmentProperty = "NULL";
                    if (!string.IsNullOrEmpty(leadDetails.Do_they_have_an_Investment_Property))
                    {
                        haveInvestmentProperty = $"'{leadDetails.Do_they_have_an_Investment_Property}'";
                    }
                    string additionalInvestments = "NULL";
                    if (!string.IsNullOrEmpty(leadDetails.Additional_Comments_Investments))
                    {
                        additionalInvestments = $"'{leadDetails.Additional_Comments_Investments}'";
                    }
                    string otherAssets = "NULL";
                    if (!string.IsNullOrEmpty(leadDetails.Other_Assets))
                    {
                        otherAssets = $"'{leadDetails.Other_Assets}'";
                    }
                    string creditCards = "NULL";
                    if (!string.IsNullOrEmpty(leadDetails.Credit_Cards))
                    {
                        creditCards = $"'{leadDetails.Credit_Cards}'";
                    }
                    string carLoans = "NULL";
                    if (!string.IsNullOrEmpty(leadDetails.Car_Loans))
                    {
                        carLoans = $"'{leadDetails.Car_Loans}'";
                    }
                    string personalLoans = "NULL";
                    if (!string.IsNullOrEmpty(leadDetails.Personal_Loans1))
                    {
                        personalLoans = $"'{leadDetails.Personal_Loans1}'";
                    }
                    string superFundName = "NULL";
                    if (!string.IsNullOrEmpty(leadDetails.Super_Fund_Name))
                    {
                        superFundName = $"'{leadDetails.Super_Fund_Name}'";
                    }
                    string contributions = "NULL";
                    if (!string.IsNullOrEmpty(leadDetails.SMSF_Contributions))
                    {
                        contributions = $"'{leadDetails.SMSF_Contributions}'";
                    }
                    string superBalance = "NULL";
                    if (leadDetails.Super_Balance.HasValue)
                    {
                        superBalance = $"'{leadDetails.Super_Balance}'";
                    }
                    string emailOptOut = "NULL";
                    if (leadDetails.Email_Opt_Out.HasValue && leadDetails.Email_Opt_Out.Value)
                    {
                        emailOptOut = "1";
                    }
                    else
                    {
                        emailOptOut = "0";
                    }
                    string massUpdate = "NULL";
                    if (!string.IsNullOrEmpty(leadDetails.Mass_Update))
                    {
                        massUpdate = $"'{leadDetails.Mass_Update}'";
                    }
                    string sourceUniqueLeadId = "NULL";
                    if (!string.IsNullOrEmpty(leadDetails.Source_Unique_Lead_ID))
                    {
                        sourceUniqueLeadId = $"'{leadDetails.Source_Unique_Lead_ID}'";
                    }
                    string thePlainsContactMapping = "NULL";
                    if (leadDetails.The_Plains_Contact_Mapping.HasValue
                        && leadDetails.The_Plains_Contact_Mapping.Value)
                    {
                        thePlainsContactMapping = "1";
                    }
                    else
                    {
                        thePlainsContactMapping = "0";
                    }
                    string daysUntilSSBooked = "NULL";
                    if (leadDetails.Days_Until_SS_Booked.HasValue)
                    {
                        daysUntilSSBooked = $"{leadDetails.Days_Until_SS_Booked}";
                    }
                    string daysUntilSSCompleted = "NULL";
                    if (leadDetails.Days_Until_SS_Completed.HasValue)
                    {
                        daysUntilSSCompleted = $"{leadDetails.Days_Until_SS_Completed}";
                    }
                    string mortgageTrigger = "NULL";
                    if (!string.IsNullOrEmpty(leadDetails.Mortgage_Trigger))
                    {
                        mortgageTrigger = $"'{leadDetails.Mortgage_Trigger}'";
                    }
                    string pigTrigger = "NULL";
                    if (!string.IsNullOrEmpty(leadDetails.PIG_Trigger))
                    {
                        mortgageTrigger = $"'{leadDetails.PIG_Trigger}'";
                    }
                    string mortgageLeadForm = "NULL";
                    if (leadDetails.Mortgage_Lead_Form.HasValue
                        && leadDetails.Mortgage_Lead_Form.Value)
                    {
                        mortgageLeadForm = "1";
                    }
                    else
                    {
                        mortgageLeadForm = "0";
                    }
                    string mortgageQuiz = "NULL";
                    if (leadDetails.Mortgage_Quiz.HasValue
                        && leadDetails.Mortgage_Quiz.Value)
                    {
                        mortgageQuiz = "1";
                    }
                    else
                    {
                        mortgageQuiz = "0";
                    }
                    string mortgageCallBooked = "NULL";
                    if (leadDetails.Mortgage_Call_Booked.HasValue
                        && leadDetails.Mortgage_Call_Booked.Value)
                    {
                        mortgageCallBooked = "1";
                    }
                    else
                    {
                        mortgageCallBooked = "0";
                    }
                    string pigLead = "NULL";
                    if (leadDetails.PIG_Lead.HasValue
                        && leadDetails.PIG_Lead.Value)
                    {
                        pigLead = "1";
                    }
                    else
                    {
                        pigLead = "0";
                    }
                    string pigQuiz = "NULL";
                    if (leadDetails.PIG_Quiz.HasValue
                        && leadDetails.PIG_Lead.Value)
                    {
                        pigQuiz = "1";
                    }
                    else
                    {
                        pigQuiz = "0";
                    }
                    string pigCallBooked = "NULL";
                    if (leadDetails.PIG_Call_Booked.HasValue
                        && leadDetails.PIG_Call_Booked.Value)
                    {
                        pigCallBooked = "1";
                    }
                    else
                    {
                        pigCallBooked = "0";
                    }
                    string lostReason = "NULL";
                    if (!string.IsNullOrEmpty(leadDetails.Lost_Reason))
                    {
                        lostReason = $"'{leadDetails.Lost_Reason}'";
                    }
                    string utmSource = "NULL";
                    if (!string.IsNullOrEmpty(leadDetails.utm_source))
                    {
                        utmSource = $"'{leadDetails.utm_source}'";
                    }
                    string utmMedium = "NULL";
                    if (!string.IsNullOrEmpty(leadDetails.utm_medium))
                    {
                        utmMedium = $"'{leadDetails.utm_medium}'";
                    }
                    string utmCampaign = "NULL";
                    if (!string.IsNullOrEmpty(leadDetails.utm_campaign))
                    {
                        utmCampaign = $"'{leadDetails.utm_campaign}'";
                    }
                    string utmContent = "NULL";
                    if (!string.IsNullOrEmpty(leadDetails.utm_content))
                    {
                        utmContent = $"'{leadDetails.utm_content}'";
                    }
                    string utmTerm = "NULL";
                    if (!string.IsNullOrEmpty(leadDetails.utm_term))
                    {
                        utmTerm = $"'{leadDetails.utm_term}'";
                    }
                    string referrer = "NULL";
                    if (!string.IsNullOrEmpty(leadDetails.Referrer))
                    {
                        referrer = $"'{leadDetails.Referrer}'";
                    }
                    string numberOfChats = "NULL";
                    if (leadDetails.Number_Of_Chats.HasValue)
                    {
                        numberOfChats = $"{leadDetails.Number_Of_Chats}";
                    }
                    string visitorScore = "NULL";
                    if (leadDetails.Visitor_Score.HasValue)
                    {
                        visitorScore = $"{leadDetails.Visitor_Score}";
                    }
                    string averageTimeSpent = "NULL";
                    if (leadDetails.Average_Time_Spent_Minutes.HasValue)
                    {
                        averageTimeSpent = $"{leadDetails.Average_Time_Spent_Minutes}";
                    }
                    string firstVisitedUrl = "NULL";
                    if (!string.IsNullOrEmpty(leadDetails.First_Visited_URL))
                    {
                        firstVisitedUrl = $"'{leadDetails.First_Visited_URL}'";
                    }
                    string daysVisited = "NULL";
                    if (leadDetails.Days_Visited.HasValue)
                    {
                        daysVisited = $"{leadDetails.Days_Visited}";
                    }
                    var owner = leadDetails.Owner;
                    string ownerId = $"'{owner.id}'";
                    string ownerName = $"'{owner.name}'";

                    var leadOwner = leadDetails.Owner;
                    string leadOwnerId = $"'{leadOwner.id}'";
                    string leadOwnerName = $"'{leadOwner.name}'";

                    // STEP 2.2.1: Handle Time Field
                    var tz = TimeZoneInfo.FindSystemTimeZoneById("E. Australia Standard Time");

                    var selfBookedDateTime = leadDetails.Self_Booked_Date_Time;
                    string selfBookedDateTimeStr = "NULL";
                    if (selfBookedDateTime.HasValue)
                    {
                        selfBookedDateTimeStr = selfBookedDateTime.Value.ToString("yyyy-MM-dd HH:mm:ss");
                        selfBookedDateTimeStr = $"'{selfBookedDateTimeStr}'";
                    }

                    var quizDateTime = leadDetails.Quiz_Date_and_Time;
                    string quizDateTimeStr = "NULL";
                    if (quizDateTime.HasValue)
                    {
                        quizDateTimeStr = quizDateTime.Value.ToString("yyyy-MM-dd HH:mm:ss");
                        quizDateTimeStr = $"'{quizDateTimeStr}'";
                    }

                    var discoveryCallBookingMadeDateTime = leadDetails.Appointment_Date_Time;
                    string discoveryCallBookingMadeDateTimeStr = "NULL";
                    if (discoveryCallBookingMadeDateTime.HasValue)
                    {
                        discoveryCallBookingMadeDateTimeStr = discoveryCallBookingMadeDateTime.Value.ToString("yyyy-MM-dd HH:mm:ss");
                        discoveryCallBookingMadeDateTimeStr = $"'{discoveryCallBookingMadeDateTimeStr}'";
                    }

                    var discoveryCallDateTime = leadDetails.Discovery_Call_Date_Time;
                    string discoveryCallDateTimeStr = "NULL";
                    if (discoveryCallDateTime.HasValue)
                    {
                        discoveryCallDateTimeStr = discoveryCallDateTime.Value.ToString("yyyy-MM-dd HH:mm:ss");
                        discoveryCallDateTimeStr = $"'{discoveryCallDateTimeStr}'";
                    }

                    var strategySessionBookingMadeDateTime = leadDetails.Strategy_Session_Booking_Made_Date_Time;
                    string strategySessionBookingMadeDateTimeStr = "NULL";
                    if (strategySessionBookingMadeDateTime.HasValue)
                    {
                        strategySessionBookingMadeDateTimeStr = strategySessionBookingMadeDateTime.Value.ToString("yyyy-MM-dd HH:mm:ss");
                        strategySessionBookingMadeDateTimeStr = $"'{strategySessionBookingMadeDateTimeStr}'";
                    }

                    var strategySessionDateTime = leadDetails.st_Appointment_Date_Time;
                    string strategySessionDateTimeStr = "NULL";
                    if (strategySessionDateTime.HasValue)
                    {
                        strategySessionDateTimeStr = strategySessionDateTime.Value.ToString("yyyy-MM-dd HH:mm:ss");
                        strategySessionDateTimeStr = $"'{strategySessionDateTimeStr}'";
                    }

                    var createdTime = leadDetails.Created_Time;
                    string createdTimeStr = "NULL";
                    if (createdTime.HasValue)
                    {
                        createdTimeStr = createdTime.Value.ToString("yyyy-MM-dd HH:mm:ss");
                        createdTimeStr = $"'{createdTimeStr}'";
                    }

                    var modifiedTime = leadDetails.Modified_Time;
                    string modifiedTimeStr = "NULL";
                    if (modifiedTime.HasValue)
                    {
                        modifiedTimeStr = modifiedTime.Value.ToString("yyyy-MM-dd HH:mm:ss");
                        modifiedTimeStr = $"'{modifiedTimeStr}'";
                    }

                    var firstVisitedTime = leadDetails.First_Visited_Time;
                    string firstVisitedTimeStr = "NULL";
                    if (firstVisitedTime.HasValue)
                    {
                        firstVisitedTimeStr = firstVisitedTime.Value.ToString("yyyy-MM-dd HH:mm:ss");
                        firstVisitedTimeStr = $"'{firstVisitedTimeStr}'";
                    }

                    var mostRecentVisit = leadDetails.Last_Visited_Time;
                    string mostRecentVisitStr = "NULL";
                    if (mostRecentVisit.HasValue)
                    {
                        mostRecentVisitStr = mostRecentVisit.Value.ToString("yyyy-MM-dd HH:mm:ss");
                        mostRecentVisitStr = $"'{mostRecentVisitStr}'";
                    }

                    string tag = "NULL";
                    if (leadDetails.Tag != null && leadDetails.Tag.Length > 0)
                    {
                        var leadTags = leadDetails.Tag.Select(t => t.name);
                        tag = $"'{string.Join(",", leadTags)}'";
                    }

                    var createdBy = leadDetails.Created_By;
                    string createdById = $"'{createdBy.id}'";
                    string createdByName = $"'{createdBy.name}'";

                    var modifiedBy = leadDetails.Modified_By;
                    string modifiedById = "NULL";
                    string modifiedByName = "NULL";
                    if (modifiedBy != null)
                    {
                        modifiedById = $"'{modifiedBy.id}'";
                        modifiedByName = $"'{modifiedBy.name}'";
                    }

                    #endregion
                    
                    if (leadCount == 0)
                    {
                        // Step 2.3.1: Handle Insert Task query
                        string fieldPart = @"(`ZohoCRMId`, `FirstName`, `LastName`, `Email`,
                                        `Phone`, `PhoneOther`, `Age`, `MaritalStatus`, `NumberOfDependents`,
                                        `DependentAges`, `Wizard`, `LeadStatus`, `LeadQuality`, `LeadSource`,
                                        `ReferalContact`, `CampaignsSegments`, `SelfBookedDateTime`, `MarketingPlatform`,
                                        `QuizState`, `AdCampaignSource`, `QuizAgeGroup`, `QuizType`, `QuizMaritalStatus`,
                                        `QuizHomeOwner`, `QuizIncome`, `QuizMortgageTerm`, `QuizSuper`, `QuizSavings`,
                                        `QuizHowMuchEquity`, `QuizHomeValue`, `QuizResult`, `QuizDateTime`, `AppointmentSetter`,
                                        `AppointmentType`, `DiscoveryCallBookingMadeDateTime`, `DiscoveryCallDateTime`, `QCOutcome`,
                                        `QCFileCheckComplete`, `StrategySessionBookingMadeDateTime`, `MortgageBalance`, `StrategySessionDateTime`,
                                        `SalesPerson`, `BorrowingCapacityIP`, `BorrowingCapacitySMSF`, `StrategySessionOutcome`,
                                        `HomeOwner`, `InterestRate`, `HomeValue`, `ReasonForEstimatedValue`, `AdditionalMortgageRepayments`,
                                        `Street`, `City`, `State`, `ZipCode`, `Lender`, `RentOrBoardPaid`, `Occupation`,
                                        `GrossSalary`, `EmploymentStatus`, `HowLongInCurrentEmployment`, `Shares`, `Savings`,
                                        `HaveInvestmentProperty`, `AdditionalInvestments`, `OtherAssets`, `CreditCards`,
                                        `CarLoans`, `PersonalLoans`, `SuperFundName`, `Contributions`, `SuperBalance`,
                                        `EmailOptOut`, `MassUpdate`, `SourceUniqueLeadId`, `ThePlainsContactMapping`,
                                        `DaysUntilSSBooked`, `DaysUntilSSCompleted`, `MortgageTrigger`, `PIGTrigger`,
                                        `MortgageLeadForm`, `MortgageQuiz`, `MortgageCallBooked`, `PIGLead`, `PIGQuiz`,
                                        `PIGCallBooked`, `LostReason`, `UtmSource`, `UtmMedium`, `UtmCampaign`, `UtmContent`,
                                        `UtmTerm`, `FirstVisit`, `Referrer`, `MostRecentVisit`, `NumberOfChats`, `VisitorScore`,
                                        `AverageTimeSpent`, `FirstPageVisited`, `DaysVisited`, `Tag`, `OwnerId`, `Owner`, 
                                        `CreatedById`, `CreatedBy`, `CreatedTime`, `ModifiedById`, `ModifiedBy`, `ModifiedTime`)";

                        string valuesPart = $@"('{leadId}', {firstName}, {lastName}, {email},
                                        {phone}, {phoneOther}, {age}, {maritalStatus}, {numberOfDependents},
                                        {dependentAges}, {wizard}, {leadStatus}, {leadQuality}, {leadSource},
                                        {referalContact}, {campaignsSegments}, {selfBookedDateTimeStr}, {marketingPlatform},
                                        {quizState}, {adCampaignSource}, {quizAgeGroup}, {quizType}, {quizMaritalStatus},
                                        {quizHomeOwner}, {quizIncome}, {quizMortgageTerm}, {quizSuper}, {quizSavings},
                                        {quizHowMuchEquity}, {quizHomeValue}, {quizResult}, {quizDateTimeStr}, {appointmentSetter},
                                        {appointmentType}, {discoveryCallBookingMadeDateTimeStr}, {discoveryCallDateTimeStr}, {qcOutcome},
                                        {qcFileCheckComplete}, {strategySessionBookingMadeDateTimeStr}, {mortgageBalance}, {strategySessionDateTimeStr},
                                        {salesPerson}, {borrowingCapacityIP}, {borrowingCapacitySMSF}, {strategySessionOutcome},
                                        {homeOwner}, {interestRate}, {homeValue}, {reasonForEstimatedValue}, {additionalMortgageRepayments},
                                        {street}, {city}, {state}, {zipCode}, {pprLender}, {rentOrBoardPaid}, {occupation},
                                        {grossSalary}, {employmentStatus}, {howLongInCurrentEmployement}, {shares}, {savings},
                                        {haveInvestmentProperty}, {additionalInvestments}, {otherAssets}, {creditCards},
                                        {carLoans}, {personalLoans}, {superFundName}, {contributions}, {superBalance},
                                        {emailOptOut}, {massUpdate}, {sourceUniqueLeadId}, {thePlainsContactMapping},
                                        {daysUntilSSBooked}, {daysUntilSSCompleted}, {mortgageTrigger}, {pigTrigger},
                                        {mortgageLeadForm}, {mortgageQuiz}, {mortgageCallBooked}, {pigLead}, {pigQuiz},
                                        {pigCallBooked}, {lostReason}, {utmSource}, {utmMedium}, {utmCampaign}, {utmContent},
                                        {utmTerm}, {firstVisitedTimeStr}, {referrer}, {mostRecentVisitStr}, {numberOfChats}, {visitorScore},
                                        {averageTimeSpent}, {firstVisitedUrl}, {daysVisited}, {tag}, {leadOwnerId}, {leadOwnerName}, 
                                        {createdById}, {createdByName}, {createdTimeStr}, {modifiedById}, {modifiedByName}, {modifiedTimeStr})";

                        // Step 2.3.2: Insert Task record to DB
                        string insertQuery = $@"INSERT INTO Leads {fieldPart} VALUES {valuesPart}; SELECT LAST_INSERT_ID();";

                        using (var insertCommand = new MySqlCommand(insertQuery, connection))
                        {
                            ldsId = int.Parse(insertCommand.ExecuteScalar().ToString());
                            apiResult.Message = OneCorpConstants.STMCU_200_INSERT;
                        }

                    }
                    else
                    {
                        // Step 2.4: Update Lead record to DB
                        string updatePart = @$"FirstName = {firstName}, LastName = {lastName}, Email = {email},
                                        Phone = {phone}, PhoneOther = {phoneOther}, Age = {age}, MaritalStatus = {maritalStatus},
                                        NumberOfDependents = {numberOfDependents}, DependentAges = {dependentAges}, Wizard = {wizard},
                                        LeadStatus = {leadStatus}, LeadQuality = {leadQuality}, LeadSource = {leadSource},
                                        ReferalContact = {referalContact}, CampaignsSegments = {campaignsSegments}, SelfBookedDateTime = {selfBookedDateTimeStr},
                                        MarketingPlatform = {marketingPlatform}, QuizState = {quizState}, AdCampaignSource = {adCampaignSource},
                                        QuizAgeGroup = {quizAgeGroup}, QuizType = {quizType}, QuizMaritalStatus = {quizMaritalStatus},
                                        QuizHomeOwner = {quizHomeOwner}, QuizIncome = {quizIncome}, QuizMortgageTerm = {quizMortgageTerm},
                                        QuizSuper = {quizSuper}, QuizSavings = {quizSavings}, QuizHowMuchEquity = {quizHowMuchEquity},
                                        QuizHomeValue = {quizHomeValue}, QuizResult = {quizResult}, QuizDateTime = {quizDateTimeStr},
                                        AppointmentSetter = {appointmentSetter}, AppointmentType = {appointmentType},
                                        DiscoveryCallBookingMadeDateTime = {discoveryCallBookingMadeDateTimeStr}, DiscoveryCallDateTime = {discoveryCallDateTimeStr},
                                        QCOutcome = {qcOutcome}, QCFileCheckComplete = {qcFileCheckComplete}, StrategySessionBookingMadeDateTime = {strategySessionBookingMadeDateTimeStr},
                                        MortgageBalance = {mortgageBalance}, StrategySessionDateTime = {strategySessionDateTimeStr},
                                        SalesPerson = {salesPerson}, BorrowingCapacityIP = {borrowingCapacityIP}, BorrowingCapacitySMSF = {borrowingCapacitySMSF},
                                        StrategySessionOutcome = {strategySessionOutcome}, HomeOwner = {homeOwner}, InterestRate = {interestRate},
                                        HomeValue = {homeValue}, ReasonForEstimatedValue = {reasonForEstimatedValue}, AdditionalMortgageRepayments = {additionalMortgageRepayments},
                                        Street = {street}, City = {city}, State = {state}, ZipCode = {zipCode}, Lender = {pprLender},
                                        RentOrBoardPaid = {rentOrBoardPaid}, Occupation = {occupation}, GrossSalary = {grossSalary},
                                        EmploymentStatus = {employmentStatus}, HowLongInCurrentEmployment = {howLongInCurrentEmployement},
                                        Shares = {shares}, Savings = {savings}, HaveInvestmentProperty = {haveInvestmentProperty},
                                        AdditionalInvestments = {additionalInvestments}, OtherAssets = {otherAssets},
                                        CreditCards = {creditCards}, CarLoans = {carLoans}, PersonalLoans = {personalLoans},
                                        SuperFundName = {superFundName}, Contributions = {contributions}, SuperBalance = {superBalance},
                                        EmailOptOut = {emailOptOut}, MassUpdate = {massUpdate}, SourceUniqueLeadId = {sourceUniqueLeadId},
                                        ThePlainsContactMapping = {thePlainsContactMapping}, DaysUntilSSBooked = {daysUntilSSBooked},
                                        DaysUntilSSCompleted = {daysUntilSSCompleted}, MortgageTrigger = {mortgageTrigger},
                                        PIGTrigger = {pigTrigger}, MortgageLeadForm = {mortgageLeadForm}, MortgageQuiz = {mortgageQuiz},
                                        MortgageCallBooked = {mortgageCallBooked}, PIGLead = {pigLead}, PIGQuiz = {pigQuiz},
                                        PIGCallBooked = {pigCallBooked}, LostReason = {lostReason}, UtmSource = {utmSource},
                                        UtmMedium = {utmMedium}, UtmCampaign = {utmCampaign}, UtmContent = {utmContent},
                                        UtmTerm = {utmTerm}, FirstVisit = {firstVisitedTimeStr}, Referrer = {referrer},
                                        MostRecentVisit = {mostRecentVisitStr}, NumberOfChats = {numberOfChats}, VisitorScore = {visitorScore},
                                        AverageTimeSpent = {averageTimeSpent}, FirstPageVisited = {firstVisitedUrl}, DaysVisited = {daysVisited},
                                        Tag = {tag}, Owner = {leadOwnerName}, OwnerId = {leadOwnerId}, CreatedById = {createdById},
                                        CreatedBy = {createdByName}, CreatedTime = {createdTimeStr}, ModifiedById = {modifiedById},
                                        ModifiedBy = {modifiedByName}, ModifiedTime = {modifiedTimeStr}";

                        string updateQuery = $@"UPDATE Leads SET {updatePart} WHERE ZohoCRMId = {leadId}; SELECT Id FROM Leads WHERE ZohoCRMId = {leadId};";

                        using (var updateCommand = new MySqlCommand(updateQuery, connection))
                        {
                            ldsId = int.Parse(updateCommand.ExecuteScalar().ToString());
                            apiResult.Message = OneCorpConstants.SLMCU_200_UPDATE;
                        }

                    }
                
                }

                // Step 3: Update LDS Id to Lead
                if (ldsId > 0 && string.IsNullOrEmpty(leadLdsId))
                {
                    var upsertRequest = new UpsertRequest<LeadForUpdation>();
                    var leadForUpdation = new LeadForUpdation()
                    {
                        LDS_Id = ldsId.ToString()
                    };
                    upsertRequest.data.Add(leadForUpdation);
                    var updateLeadResponse = await _oneCorpCrmService.UpdateLead(leadId, upsertRequest);

                    if (updateLeadResponse.Code != ResultCode.OK)
                    {
                        apiResult.Message = OneCorpConstants.SLMCU_UpdateLead_400;
                        return apiResult;
                    }
                }

                apiResult.Message = OneCorpConstants.SLMCU_200;
                apiResult.Code = ResultCode.OK;
                return apiResult;

            }
            catch (Exception ex)
            {
                apiResult.Data = $"{ex.Message + " - " + ex.StackTrace}";
                return apiResult;
            }

        }

        public ApiResultDto<string> SyncLeadToDBUponDeletion(string leadId)
        {
            var apiResult = new ApiResultDto<string>()
            {
                Code = ResultCode.BadRequest,
                Message = OneCorpConstants.SLMD_400
            };

            try
            {

                // STEP 2: Connect to MySQL DB
                var (sshClient, localPort) = DatabaseHelpers.ConnectSsh(OneCorpConstants.SSHHostname,
                    OneCorpConstants.SSHUsername, OneCorpConstants.SSHPassword);
                using (sshClient)
                {
                    MySqlConnectionStringBuilder csb = new MySqlConnectionStringBuilder
                    {
                        Server = "127.0.0.1",
                        Port = localPort,
                        UserID = OneCorpConstants.MySQLUsername,
                        Password = OneCorpConstants.MySQLPassword,
                        Database = OneCorpConstants.MySQLSchema
                    };

                    using var connection = new MySqlConnection(csb.ConnectionString);
                    connection.Open();

                    string deleteQuery = $@"DELETE FROM Leads WHERE ZohoCRMId = {leadId}; 
                        DELETE FROM LeadStatusHistories WHERE CrmLeadId = {leadId};
                        DELETE FROM Tasks WHERE WhatId = {leadId}";

                    using var deleteCommand = new MySqlCommand(deleteQuery, connection);
                    var updateResult = deleteCommand.ExecuteNonQuery();

                }

                apiResult.Code = ResultCode.OK;
                apiResult.Message = OneCorpConstants.SLMD_200;
                return apiResult;
            }
            catch (Exception ex)
            {
                apiResult.Data = $"{ex.Message + " - " + ex.StackTrace}";
                return apiResult;
            }
        }

        public async Task<ApiResultDto<string>> SyncLeadStatusHistoryToDB(string leadId)
        {
            var apiResult = new ApiResultDto<string>()
            {
                Code = ResultCode.BadRequest,
                Message = OneCorpConstants.SLSH_400
            };
            var tz = TimeZoneInfo.FindSystemTimeZoneById("E. Australia Standard Time");

            try
            {

                // STEP 2: Get Lead Details
                var getLeadById = await _oneCorpCrmService.GetLeadById(leadId);
                if (getLeadById.Code != ResultCode.OK)
                {
                    apiResult.Message = OneCorpConstants.SLSH_GetLeadById_400;
                    return apiResult;
                }
                var leadDetails = getLeadById.Data.data[0];

                // STEP 2: Get Lead Status Histories
                var getLeadStatusHistories = await _oneCorpCrmService.GetRelatedStatusHistories(leadId);
                if (getLeadStatusHistories.Code != ResultCode.OK)
                {
                    apiResult.Message = OneCorpConstants.SLSH_GetLeadStatusHistoryById_400;
                    return apiResult;
                }
                // STEP 3: Handle Saving the records to DB
                var statusHistories = getLeadStatusHistories.Data.data;
                var ascStatusHistories = statusHistories.OrderBy(h => h.Modified_Time).ToList();

                // STEP 3.1: Delete all status histories by Lead Id
                // STEP 2: Connect to MySQL DB
                var (sshClient, localPort) = DatabaseHelpers.ConnectSsh(OneCorpConstants.SSHHostname,
                    OneCorpConstants.SSHUsername, OneCorpConstants.SSHPassword);
                using (sshClient)
                {
                    MySqlConnectionStringBuilder csb = new MySqlConnectionStringBuilder
                    {
                        Server = "127.0.0.1",
                        Port = localPort,
                        UserID = OneCorpConstants.MySQLUsername,
                        Password = OneCorpConstants.MySQLPassword,
                        Database = OneCorpConstants.MySQLSchema
                    };

                    using var connection = new MySqlConnection(csb.ConnectionString);
                    connection.Open();

                    // Step 2.1: Delete all Lead Status Histories by Lead Id

                    using (var deleteCommand = new MySqlCommand(@$"DELETE FROM LeadStatusHistories
                            WHERE CrmLeadId = {leadId};", connection))
                    {
                        int deleteCount = Convert.ToInt32(deleteCommand.ExecuteScalar());
                    }

                    // Step 2.2: Prepare Query to Insert Lead Status History to DB
                    string zohoCrmId = $"'{leadId}'";

                    StatusDetails previousHistory = null;
                    StatusDetails currentHistory = null;

                    // STEP 3.2: Add Lead Status History records in DB
                    int index = 0;
                    int historyLength = statusHistories.Length;

                    string insertStatusQuery = string.Empty;
                    string fieldsPart = string.Empty;
                    string valuesPart = string.Empty;

                    var leadOwner = leadDetails.Owner;
                    string leadOwnerId = $"'{leadOwner.id}'";
                    string leadOwnerName = $"'{leadOwner.name}'";
                    string crmLeadId = $"'{leadId}'";

                    foreach (var history in ascStatusHistories)
                    {
                        if (index == 0)
                        {
                            previousHistory = history;
                        }
                        else
                        {
                            currentHistory = history;
                            var currentModifiedTime = currentHistory.Modified_Time;
                            var previousModifiedTime = previousHistory.Modified_Time;

                            string leadStatus = $"'{history.Lead_Status}'";

                            int livingTime = (int)(currentModifiedTime.Value - previousModifiedTime.Value).TotalSeconds;
                            string livingTimeStr = $"{livingTime}";
                            string friendlyLivingTime = $"'{DateTimeHelpers.ConvertSecondsToDisplayTime(livingTime)}'";

                            var startTime = previousHistory.Modified_Time;
                            string startTimeStr = "NULL";
                            if (startTime.HasValue)
                            {
                                startTimeStr = startTime.Value.ToString("yyyy-MM-dd HH:mm:ss");
                                startTimeStr = $"'{startTimeStr}'";
                            }

                            var endTime = currentHistory.Modified_Time;
                            string endTimeStr = "NULL";
                            if (endTime.HasValue)
                            {
                                endTimeStr = endTime.Value.ToString("yyyy-MM-dd HH:mm:ss");
                                endTimeStr = $"'{endTimeStr}'";
                            }

                            var previousModifiedBy = previousHistory.Modified_By;
                            string modifiedById = $"'{previousModifiedBy.id}'";
                            string modifiedByName = $"'{previousModifiedBy.name}'";

                            var modifiedTime = previousHistory.Modified_Time;
                            string modifiedTimeStr = "NULL";
                            if (modifiedTime.HasValue)
                            {
                                modifiedTimeStr = modifiedTime.Value.ToString("yyyy-MM-dd HH:mm:ss");
                                modifiedTimeStr = $"'{modifiedTimeStr}'";
                            }

                            fieldsPart = @"(`CrmLeadId`, `LeadOwnerId`, `LeadOwnerName`, `LeadStatus`, 
                                    `LivingTime`, `FriendlyLivingTime`, `StartTime`, `EndTime`,  `ModifiedById`, 
                                    `ModifiedBy`, `ModifiedTime`)";

                            valuesPart = @$"({crmLeadId}, {leadOwnerId}, {leadOwnerName}, {leadStatus},
                                     {livingTimeStr}, {friendlyLivingTime}, {startTimeStr}, {endTimeStr}, {modifiedById}, 
                                     {modifiedByName}, {modifiedTimeStr})";

                            insertStatusQuery = @$"INSERT INTO LeadStatusHistories {fieldsPart}
                                    VALUES {valuesPart};";

                            using (var insertCommand = new MySqlCommand(insertStatusQuery, connection))
                            {
                                int insertCount = insertCommand.ExecuteNonQuery();
                            }

                            previousHistory = currentHistory;
                        }

                        index++;
                    }

                    // Handle the last status
                    var lastHistory = ascStatusHistories[historyLength - 1];
                    var lastModifiedTime = lastHistory.Modified_Time;

                    var lastModifiedBy = lastHistory.Modified_By;
                    string lastModifiedById = $"'{lastModifiedBy.id}'";
                    string lastModifiedByName = $"'{lastModifiedBy.name}'";

                    int lastLivingTime = (int)(DateTime.UtcNow - lastModifiedTime.Value).TotalSeconds;
                    string lastLivingTimeStr = $"{lastLivingTime}";
                    string lastFriendlyLivingTime = $"'{DateTimeHelpers.ConvertSecondsToDisplayTime(lastLivingTime)}'";

                    string lastLeadStatus = $"'{lastHistory.Lead_Status}'";

                    var lastStartTime = lastHistory.Modified_Time;
                    string lastStartTimeStr = "NULL";
                    if (lastStartTime.HasValue)
                    {
                        lastStartTimeStr = lastStartTime.Value.ToString("yyyy-MM-dd HH:mm:ss");
                        lastStartTimeStr = $"'{lastStartTimeStr}'";
                    }

                    var lastEndTime = DateTime.UtcNow;
                    string lastEndTimeStr = "NULL";
                    lastEndTimeStr = lastEndTime.ToString("yyyy-MM-dd HH:mm:ss");
                    lastEndTimeStr = $"'{lastEndTimeStr}'";

                    string lastModifiedTimeStr = "NULL";
                    if (lastModifiedTime.HasValue)
                    {
                        lastModifiedTimeStr = lastModifiedTime.Value.ToString("yyyy-MM-dd HH:mm:ss");
                        lastModifiedTimeStr = $"'{lastModifiedTimeStr}'";
                    }

                    string lastFieldsPart = @"(`CrmLeadId`, `LeadOwnerId`, `LeadOwnerName`, `LeadStatus`, 
                                    `LivingTime`, `FriendlyLivingTime`, `StartTime`, `EndTime`,  `ModifiedById`, 
                                    `ModifiedBy`, `ModifiedTime`)";

                    string lastValuesPart = @$"({leadId}, {leadOwnerId}, {leadOwnerName}, {lastLeadStatus},
                                     {lastLivingTimeStr}, {lastFriendlyLivingTime}, {lastStartTimeStr}, {lastEndTimeStr}, {lastModifiedById}, 
                                     {lastModifiedByName}, {lastModifiedTimeStr})";

                    insertStatusQuery = @$"INSERT INTO LeadStatusHistories {lastFieldsPart}
                                    VALUES {lastValuesPart};";

                    using (var insertCommand = new MySqlCommand(insertStatusQuery, connection))
                    {
                        int insertCount = insertCommand.ExecuteNonQuery();
                    }

                }

                apiResult.Code = ResultCode.OK;
                apiResult.Message = OneCorpConstants.SLSH_200;
                return apiResult;

            }
            catch (Exception ex)
            {
                apiResult.Message = $"{ex.Message} - {ex.StackTrace}";
                return apiResult;
            }
        }

        public async Task<ApiResultDto<string>> ImportTasksToDB()
        {

            var apiResult = new ApiResultDto<string>()
            {
                Code = ResultCode.BadRequest,
                Message = OneCorpConstants.IT2D_400
            };
            int index = 0;
            try
            {

                // STEP 1: Read data from csv files
                string allTasks = File.ReadAllText(@"C:\Upwork\OneCorp\Lead Distribution\First 20K Tasks.txt");
                var taskList = allTasks.Split("\r\n");

                foreach (string taskId in taskList)
                {
                    index++;
                    // STEP 2: Sync Task to DB
                    var syncTaskResponse = await SyncTaskToDBUponCreationUpdation(taskId);
                    if (syncTaskResponse.Code != ResultCode.OK)
                    {
                        throw new Exception($"Sync Task FAILED with reason: {syncTaskResponse.Data}");
                    }
                    Thread.Sleep(1000);
                }

                return apiResult;

            }
            catch (Exception ex)
            {
                apiResult.Message = $"{ex.Message} - {ex.StackTrace}";
                return apiResult;
            }

        }

        #endregion

        #region Zoho Workdrive

        public async Task<ApiResultDto<string>> UploadZohoSignDocument2Workdrive(string requestId)
        {

            var apiResult = new ApiResultDto<string>()
            {
                Code = ResultCode.BadRequest,
                Message = OneCorpConstants.USDW_400
            };
            string filePath = "";

            try
            {

                // STEP 1: Get Sign Document Details by Id
                var getDocumentDetailsByIdResponse = await _oneCorpSignService.GetDocumentDetailsById(requestId);
                if (getDocumentDetailsByIdResponse.Code != ResultCode.OK)
                {
                    apiResult.Data = OneCorpConstants.USDW_E01;
                    return apiResult;
                }
                var documentDetails = getDocumentDetailsByIdResponse.Data.requests;
                var documentActions = documentDetails.actions;

                var signTimestamp = documentDetails.sign_submitted_time;
                string status = documentDetails.request_status;
                if (status != "completed")
                {
                    apiResult.Data = OneCorpConstants.USDW_E04;
                    return apiResult;
                }

                string fileName = documentDetails.request_name;
                if (string.IsNullOrEmpty(fileName))
                {
                    apiResult.Data = OneCorpConstants.USDW_E01;
                    return apiResult;
                }

                if (documentActions.Length > 0)
                {
                    var documentAction = documentActions[0];
                    string recipientName = documentAction.recipient_name;
                    fileName = $"{fileName} - {recipientName}";
                }

                var signTime = DateTimeHelpers.UnixTimeStampToDateTime(signTimestamp.Value / 1000);
                var tz = TimeZoneInfo.FindSystemTimeZoneById("E. Australia Standard Time");
                var aestTime = TimeZoneInfo.ConvertTimeFromUtc(signTime, tz).ToString("dd MMM yyyy");
                fileName = $"{fileName} - {aestTime}.pdf";

                // STEP 2: Download PDF document
                var downloadPdfByIdResponse = await _oneCorpSignService.DownloadDocumentById(requestId, fileName);
                if (downloadPdfByIdResponse.Code != ResultCode.OK)
                {
                    apiResult.Data = OneCorpConstants.USDW_E02;
                    return apiResult;
                }

                // STEP 3: Upload Document to Workdrive
                // Step 3.1: Handle Parent Id
                string folderId = string.Empty;
                if (fileName.Contains("Credit Card Authority", StringComparison.InvariantCultureIgnoreCase))
                {
                    folderId = OneCorpConstants.JMVAuthorities_CreditCardAuthorities_FolderId;
                }
                else if (fileName.Contains("Credit Card And DD Authority", StringComparison.InvariantCultureIgnoreCase))
                {
                    folderId = OneCorpConstants.QSLAuthorities_CreditCardAuthorities_FolderId;
                }
                if (string.IsNullOrEmpty(folderId))
                {
                    apiResult.Data = OneCorpConstants.USDW_E03;
                    return apiResult;
                }

                // Step 3.2: Handle file content
                filePath = downloadPdfByIdResponse.Data;
                var bytes = File.ReadAllBytes(filePath);
                var uploadFileRequest = new UploadFileRequest()
                {
                    FileName = fileName,
                    OverrideNameExist = true,
                    ParentId = folderId,
                    Content = bytes
                };

                // Step 3.3: Call API to upload file
                var uploadFileResponse = await _oneCorpWorkdriveService.UploadFile(uploadFileRequest);
                if (uploadFileResponse.Code != ResultCode.OK)
                {
                    apiResult.Data = OneCorpConstants.USDW_E05;
                    return apiResult;
                }

                // STEP 4: Delete document from Zoho Sign
                var deleteDocumentResponse = await _oneCorpSignService.DeleteDocument(requestId);
                if (deleteDocumentResponse.Code != ResultCode.OK)
                {
                    apiResult.Data = OneCorpConstants.USDW_E06;
                    return apiResult;
                }

                apiResult.Code = ResultCode.OK;
                apiResult.Message = OneCorpConstants.USDW_200;
                return apiResult;
            }
            catch (Exception ex)
            {
                apiResult.Data = $"{ex.Message} - {ex.StackTrace}";
                return apiResult;
            }
            finally
            {
                if (!string.IsNullOrEmpty(filePath) && File.Exists(filePath))
                {
                    File.Delete(filePath);
                }
            }
        }

        public async Task<ApiResultDto<string>> CreateSubFolders(string[] subFolders, string parentId)
        {
            var apiResult = new ApiResultDto<string>()
            {
                Code = ResultCode.BadRequest,
                Message = OneCorpConstants.CSF_400
            };

            try
            {

                // STEP 1: Check if input sub folders are valid
                if (subFolders == null || subFolders.Length == 0)
                {
                    apiResult.Data = OneCorpConstants.CSF_E01;
                    return apiResult;
                }

                // STEP 2: List all Folders in Parent Folder
                string filterBy = "filter%5Btype%5D=folder";
                var listFoldersResponse = await _oneCorpWorkdriveService.ListFilesFolders(parentId, filterBy);
                if (listFoldersResponse.Code != ResultCode.OK)
                {
                    apiResult.Data = OneCorpConstants.CSF_E02;
                    return apiResult;
                }
                var allFolders = listFoldersResponse.Data.data;

                var folderNameList = new List<string>();
                var folderIdList = new List<string>();
                foreach (var folder in allFolders)
                {
                    var folderAttributes = folder.attributes;
                    string folderName = folderAttributes.name;
                    string folderId = folder.id;
                    folderNameList.Add(folderName);
                    folderIdList.Add(folderId);
                }

                // STEP 2: Loop through all Sub Folders
                string folderIds = string.Empty;
                int folderIndex = -1;
                foreach (string subFolder in subFolders)
                {
                    folderIndex++;
                    if (folderNameList.Contains(subFolder))
                    {
                        // Folder already exists, no need to create again
                        string folderId = folderIdList[folderIndex];
                        if (string.IsNullOrEmpty(folderIds))
                        {
                            folderIds = folderId;
                        }
                        else
                        {
                            folderIds += $",{folderId}";
                        }
                        continue;
                    }
                    else
                    {
                        var createFolderRequest = new CreateFolderRequest()
                        {
                            FolderName = subFolder,
                            ParentId = parentId
                        };
                        var createFolderResponse = await _oneCorpWorkdriveService.CreateFolder(createFolderRequest);
                        if (createFolderResponse.Code != ResultCode.OK)
                        {
                            apiResult.Data = OneCorpConstants.CSF_E03;
                            return apiResult;
                        }
                        string folderId = createFolderResponse.Data.data.id;
                        if (string.IsNullOrEmpty(folderIds))
                        {
                            folderIds = folderId;
                        }
                        else
                        {
                            folderIds += $",{folderId}";
                        }
                    }

                }

                apiResult.Code = ResultCode.OK;
                apiResult.Message = OneCorpConstants.CSF_200;
                apiResult.Data = folderIds;
                return apiResult;
            }
            catch (Exception ex)
            {
                apiResult.Data = $"{ex.Message} - {ex.StackTrace}";
                return apiResult;
            }
        }

        #endregion

        #region Zoho Projects

        public async Task<ApiResultDto<List<List<string>>>> QueryZPComments(ZohoCoqlRequest coqlRequest)
        {
            var apiResult = new ApiResultDto<List<List<string>>>()
            {
                Code = ResultCode.BadRequest,
                Message = OneCorpConstants.QZC_400
            };

            try
            {
                var getZPCommentsResponse = await _oneCorpCrmService.QueryZPComments(coqlRequest);

                if (getZPCommentsResponse.Code != ResultCode.OK)
                {
                    return apiResult;
                }

                var zpComments = getZPCommentsResponse.Data.data;
                var newZpComments = new List<List<string>>();
                int startingRow = 1;
                apiResult.Data = new List<List<string>>();
                foreach (var comment in zpComments)
                {
                    string commentId = comment.id;
                    var commentRow = new List<string>();
                    commentRow.Add(startingRow.ToString());

                    // Handle Added Person
                    string addedPerson = comment.Added_Person;
                    commentRow.Add(addedPerson);

                    // Handle Content
                    string content = comment.Content;
                    commentRow.Add(content);

                    // Handle Project 
                    string projectName = comment.Project_Name;
                    string projectUrl = comment.Project_URL;
                    string projectCell = $"<a target='_blank' href='{projectUrl}'>{projectName}</a>";
                    commentRow.Add(projectCell);

                    // Handle Task
                    string taskName = comment.Task_Name;
                    string taskUrl = comment.Task_URL;
                    string taskCell = $"<a target='_blank' href='{taskUrl}'>{taskName}</a>";
                    commentRow.Add(taskCell);

                    // Handle Attachment
                    bool hasAttachment = comment.Has_Attachment.Value;
                    string attachmentsCell = "";
                    if (hasAttachment)
                    {
                        attachmentsCell = "<div class='row'>";
                        var getCommentByIdResponse = await _oneCorpCrmService.GetZPCommentById(commentId);
                        var commentDetails = getCommentByIdResponse.Data.data[0];
                        var commentAttachments = commentDetails.Comment_Attachments;

                        if (commentAttachments != null && commentAttachments.Count() > 0)
                        {
                            int numberOfAttachments = commentAttachments.Count();
                            string col = "col-6";
                            int width = 120;
                            string fontSize = "10px";
                            if (numberOfAttachments == 1)
                            {
                                col = "col-12";
                                width = 200;
                                fontSize = "12px";
                            }
                            foreach (var attachment in commentAttachments)
                            {
                                string attachmentName = attachment.File_Name;
                                string previewUrl = attachment.Preview_URL;
                                string permanentUrl = attachment.Permanent_URL;
                                string attachmentCell = $"<div class='{col}' style='text-align: center;'>";
                                attachmentCell += $"<a target='_blank' href='{permanentUrl}'>";
                                attachmentCell += $"<div><img width='{width}' src='{previewUrl}'/></div>";
                                attachmentCell += $"<div><span style='font-size: {fontSize};'>{attachmentName}</span></div>";
                                attachmentCell += "</a></div>";
                                attachmentsCell += attachmentCell;
                            }
                            attachmentsCell += "</div>";
                        }
                    }
                    commentRow.Add(attachmentsCell);

                    // Handle Comment Time
                    var tz = TimeZoneInfo.FindSystemTimeZoneById("E. Australia Standard Time");
                    string createdTimeStr = TimeZoneInfo.ConvertTime
                        (comment.Comment_Created_Time.Value, tz).ToString("dd MMM yyyy hh:mm tt");
                    // var commentCreatedTime = comment.Comment_Created_Time;
                    // string createdTimeStr = commentCreatedTime.Value.ToString("dd MMM yyyy hh:mm tt");
                    commentRow.Add(createdTimeStr);
                    newZpComments.Add(commentRow);
                    startingRow++;
                }
                apiResult.Code = ResultCode.OK;
                apiResult.Message = OneCorpConstants.QZC_200;
                apiResult.Data = newZpComments;
                return apiResult;
            }
            catch (Exception ex)
            {
                apiResult.Message = $"{ex.Message} - {ex.StackTrace}";
                return apiResult;
            }
        }

        public async Task<ApiResultDto<List<List<string>>>> QueryZPTasks(ZohoCoqlRequest coqlRequest)
        {
            var apiResult = new ApiResultDto<List<List<string>>>()
            {
                Code = ResultCode.BadRequest,
                Message = OneCorpConstants.QZT_400
            };

            try
            {
                var getZPTasksResponse = await _oneCorpCrmService.QueryZPTasks(coqlRequest);

                if (getZPTasksResponse.Code != ResultCode.OK)
                {
                    return apiResult;
                }

                var zpTasks = getZPTasksResponse.Data.data;
                var newZpTasks = new List<List<string>>();
                int startingRow = 1;
                apiResult.Data = new List<List<string>>();
                foreach (var task in zpTasks)
                {
                    var commentRow = new List<string>();
                    commentRow.Add(startingRow.ToString());

                    // Handle Project Name
                    string projectName = task.Project_Name;
                    string projectUrl = task.Project_URL;
                    string projectCell = $"<a target='_blank' href='{projectUrl}'>{projectName}</a>";
                    commentRow.Add(projectCell);

                    // Handle Task Name
                    string taskName = task.Task_Name;
                    string taskUrl = task.Task_URL;
                    string taskCell = $"<a target='_blank' href='{taskUrl}'>{taskName}</a>";
                    commentRow.Add(taskCell);

                    // Handle Task Status
                    string taskStatus = task.Task_Status;
                    string taskStatusColor = task.Task_Status_Color_Code;
                    string taskStatusCell = $"<i class='fa fa-circle' style='color: {taskStatusColor}; text-shadow: 0 0 1px #000;' aria-hidden='true'></i> {taskStatus}";
                    commentRow.Add(taskStatusCell);

                    newZpTasks.Add(commentRow);

                    startingRow++;
                }
                apiResult.Code = ResultCode.OK;
                apiResult.Message = OneCorpConstants.QZC_200;
                apiResult.Data = newZpTasks;
                return apiResult;
            }
            catch (Exception ex)
            {
                apiResult.Message = $"{ex.Message} - {ex.StackTrace}";
                return apiResult;
            }
        }


        #endregion

        #region Sakari

        public async Task<ApiResultDto<string>> SendSakariSMSToZohoContact(SendSakariSMSToContactRequest sendSMSRequest)
        {

            var apiResult = new ApiResultDto<string>()
            {
                Code = ResultCode.BadRequest,
                Message = OneCorpConstants.SS2C_400
            };

            try
            {
                string dealId = sendSMSRequest.DealId;
                string contactId = sendSMSRequest.ContactId;
                string contactFirstName = sendSMSRequest.ContactFirstName;
                string contactLastName = sendSMSRequest.ContactLastName;
                string contactFullName = sendSMSRequest.ContactFullName;
                string contactEmail = sendSMSRequest.ContactEmail;
                string contactPhone = sendSMSRequest.ContactPhone;

                string ownerId = sendSMSRequest.UserId;
                string ownerFullName = sendSMSRequest.UserFullName;
                string ownerEmail = sendSMSRequest.UserEmail;
                string smsContent = sendSMSRequest.SmsContent;
                string groupId = sendSMSRequest.GroupId;

                // STEP 1: Send SMS to Contact Phone
                var sakariRequest = new SendSakariSMSRequest();
                
                var sakariMobile = new SakariMobile();
                sakariMobile.number = contactPhone;
                sakariMobile.country = "AU";

                var sakariContact = new SakariContact();
                sakariContact.firstName = contactFirstName;
                sakariContact.lastName = contactLastName;
                sakariContact.email = contactEmail;
                sakariContact.mobile = sakariMobile;

                var sakariPhoneNumberFilter = new SakariPhoneNumberFilter();
                var sakariGroup = new SakariGroup();
                sakariGroup.id = groupId;
                sakariPhoneNumberFilter.group = sakariGroup;
                sakariRequest.phoneNumberFilter = sakariPhoneNumberFilter;

                sakariRequest.contacts.Add(sakariContact);
                sakariRequest.template = smsContent;
                var sendSmsResponse = await _oneCorpSakariService.SendSakariSMS(sakariRequest);

                // STEP 2: Create Sakari SMS Log record
                if (sendSmsResponse.Code != ResultCode.OK)
                {
                    apiResult.Data = OneCorpConstants.SS2C_E01;
                    return apiResult;
                }

                var sendSmsDetails = sendSmsResponse.Data.data;
                var messageDetails = sendSmsDetails.messages[0];
                string messageId = messageDetails.id;
                string messageTemplate = messageDetails.template;
                string messageStatus = messageDetails.status;
                decimal messagePrice = messageDetails.price.Value;

                var conversationDetails = messageDetails.conversation;
                string conversationId = conversationDetails.id;

                var messageCreatedAt = messageDetails.created.at;
                var tz = TimeZoneInfo.FindSystemTimeZoneById("E. Australia Standard Time");

                string messageCreatedAtStr = TimeZoneInfo.ConvertTimeFromUtc
                    (messageCreatedAt.Value, tz).ToString("yyyy-MM-ddTHH:mm:ss");

                var createSmsLogRequest = new UpsertRequest<SMSLogForUpsert>();
                var sakariSmsLog = new SMSLogForUpsert
                {
                    Platform = "Sakari",
                    Related_Deal = dealId,
                    Contact_First_Name = contactFirstName,
                    Contact_Last_Name = contactLastName,
                    Contact_Full_Name = contactFullName,
                    Contact_Mobile = contactPhone,
                    Contact_Email = contactEmail,
                    Related_Contact = contactId,
                    Message_Id = messageId,
                    Conversation_Id = conversationId,
                    Sent_Time = messageCreatedAtStr,
                    Status = messageStatus,
                    Price = decimal.Round(messagePrice, 3),
                    Template = smsContent,
                    Direction = "Outgoing",
                    Owner = ownerId,
                };

                var smsList = new List<SMSLogForUpsert>
                {
                    sakariSmsLog
                };
                createSmsLogRequest.data = smsList;

                /*
                var upsertResponse = await _oneCorpCrmService.CreateSakariSMSLog(createSmsLogRequest);
                if (upsertResponse.Code != ResultCode.OK)
                {
                    apiResult.Data = OneCorpConstants.SS2C_E02;
                    return apiResult;
                }
                */

                apiResult.Code = ResultCode.OK;
                apiResult.Message = OneCorpConstants.SS2C_200;

                return apiResult;

            }
            catch (Exception ex)
            {
                apiResult.Data = $"{ex.Message} - {ex.StackTrace}";
                return apiResult;
            }

        }

        public async Task<ApiResultDto<string>> SendSakariSMSToZohoLead(SendSakariSMSToLeadRequest sendSMSRequest)
        {

            var apiResult = new ApiResultDto<string>()
            {
                Code = ResultCode.BadRequest,
                Message = OneCorpConstants.SS2L_400
            };

            try
            {
                string leadId = sendSMSRequest.LeadId;
                string leadFirstName = sendSMSRequest.LeadFirstName;
                string leadLastName = sendSMSRequest.LeadLastName;
                string leadFullName = sendSMSRequest.LeadFullName;
                string leadEmail = sendSMSRequest.LeadEmail;
                string leadPhone = sendSMSRequest.LeadPhone;

                string ownerId = sendSMSRequest.UserId;
                string ownerFullName = sendSMSRequest.UserFullName;
                string ownerEmail = sendSMSRequest.UserEmail;
                string smsContent = sendSMSRequest.SmsContent;
                string groupId = sendSMSRequest.GroupId;

                // STEP 1: Send SMS to Contact Phone
                var sakariRequest = new SendSakariSMSRequest();
                var sakariContact = new SakariContact();
                var sakariMobile = new SakariMobile();
                sakariMobile.number = leadPhone;
                sakariMobile.country = "AU";
                sakariContact.mobile = sakariMobile;

                var sakariPhoneNumberFilter = new SakariPhoneNumberFilter();
                var sakariGroup = new SakariGroup();
                sakariGroup.id = groupId;
                sakariPhoneNumberFilter.group = sakariGroup;
                sakariRequest.phoneNumberFilter = sakariPhoneNumberFilter;

                sakariRequest.contacts.Add(sakariContact);
                sakariRequest.template = smsContent;
                var sendSmsResponse = await _oneCorpSakariService.SendSakariSMS(sakariRequest);

                // STEP 2: Create Sakari SMS Log record
                if (sendSmsResponse.Code != ResultCode.OK)
                {
                    apiResult.Data = OneCorpConstants.SS2C_E01;
                    return apiResult;
                }

                var sendSmsDetails = sendSmsResponse.Data.data;
                var messageDetails = sendSmsDetails.messages[0];
                string messageId = messageDetails.id;
                string messageTemplate = messageDetails.template;
                string messageStatus = messageDetails.status;
                decimal messagePrice = messageDetails.price.Value;
                var conversationDetails = messageDetails.conversation;
                string conversationId = conversationDetails.id;

                var messageCreatedAt = messageDetails.created.at;
                var tz = TimeZoneInfo.FindSystemTimeZoneById("E. Australia Standard Time");

                string messageCreatedAtStr = TimeZoneInfo.ConvertTimeFromUtc
                    (messageCreatedAt.Value, tz).ToString("yyyy-MM-ddTHH:mm:ss");

                var createSmsLogRequest = new UpsertRequest<SMSLogForUpsert>();
                var sakariSmsLog = new SMSLogForUpsert
                {
                    Platform = "Sakari",
                    Contact_First_Name = leadFirstName,
                    Contact_Last_Name = leadLastName,
                    Contact_Full_Name = leadFullName,
                    Contact_Mobile = leadPhone,
                    Contact_Email = leadEmail,
                    Related_Lead = leadId,
                    Message_Id = messageId,
                    Conversation_Id = conversationId,
                    Sent_Time = messageCreatedAtStr,
                    Status = messageStatus,
                    Price = decimal.Round(messagePrice, 3),
                    Template = smsContent,
                    Direction = "Outgoing",
                    Owner = ownerId,
                };

                var smsList = new List<SMSLogForUpsert>
                {
                    sakariSmsLog
                };
                createSmsLogRequest.data = smsList;
                
                /*
                var upsertResponse = await _oneCorpCrmService.CreateSakariSMSLog(createSmsLogRequest);

                if (upsertResponse.Code != ResultCode.OK)
                {
                    apiResult.Data = OneCorpConstants.SS2C_E02;
                    return apiResult;
                }
                */

                apiResult.Code = ResultCode.OK;
                apiResult.Message = OneCorpConstants.SS2C_200;

                return apiResult;

            }
            catch (Exception ex)
            {
                apiResult.Data = $"{ex.Message} - {ex.StackTrace}";
                return apiResult;
            }

        }

        public async Task<ApiResultDto<string>> SendSakariSMSFromWorkflow(
            SendSakariSMSFromWorkflowRequest sendSMSRequest)
        {

            var apiResult = new ApiResultDto<string>()
            {
                Code = ResultCode.BadRequest,
                Message = OneCorpConstants.SSFW_400
            };
            var emailContent = new EmailContent
            {
                Email = EmailConstants.MyEmail_Username,
                Password = EmailConstants.MyEmail_Password,
                Subject = $"[OneCorp] Send Sakari from Workflow FAILED",
                Body = @$"The outcome of the function is:<br/>$Result$<br/><br/>Thanks & Regards,<br/>OneCorp Automation",
                Clients = "hoangtran7292@gmail.com,marketing@onecorpaustralia.com.au",
                SmtpPort = EmailConstants.SmtpPort,
                SmtpServer = EmailConstants.Gmail_SmtpServer
            };

            try
            {
                string phone = sendSMSRequest.ContactPhone;
                string template = sendSMSRequest.MessageContent;
                string ownerId = sendSMSRequest.OwnerId;

                // STEP 1: Get Phone Group by Owner Id
                // Step 1.1: Get User by Id
                var getUserByIdResponse = await _oneCorpCrmService.GetUserById(ownerId);
                if (getUserByIdResponse.Code != ResultCode.OK)
                {
                    apiResult.Message = OneCorpConstants.SSFW_E01;
                    return apiResult;
                }

                var userDetails = getUserByIdResponse.Data.users[0];
                string userEmail = userDetails.email;

                // Step 1.2: Search Sakari Phone Group by Email
                string selectQuery = $"SELECT Name FROM Sakari_Users WHERE Email = '{userEmail}'";
                var coqlRequest = new ZohoCoqlRequest()
                {
                    select_query = selectQuery
                };
                var queryPhoneGroupsResponse = await _oneCorpCrmService.QuerySakariUsers(coqlRequest);
                if (queryPhoneGroupsResponse.Code != ResultCode.OK)
                {
                    apiResult.Message = OneCorpConstants.SSFW_E02;
                    return apiResult;
                }

                // STEP 1.3: Get Sakari User Details by Id
                var sakariUsers = queryPhoneGroupsResponse.Data.data;
                var sakariUser = sakariUsers[0];

                string sakariUserId = sakariUser.id;
                var getSakariUserByIdResponse = await _oneCorpCrmService.GetSakariUserById(sakariUserId);
                var sakariUserDetails = getSakariUserByIdResponse.Data.data[0];

                var phoneGroups = sakariUserDetails.Sakari_Groups;
                if (phoneGroups == null || phoneGroups.Count() == 0)
                {
                    apiResult.Message = OneCorpConstants.SSFW_E03;
                    return apiResult;
                }

                var phoneGroup = phoneGroups[0];
                string phoneGroupId = phoneGroup.Group_Id;

                // STEP 2: Send Sakari SMS to Client
                var sendSakariSmsRequest = new SendSakariSMSRequest()
                {
                    type = "SMS",
                    template = template,
                };

                // Step 2.1: Handle Sakari Contacts
                var sakariContacts = new List<SakariContact>();
                var sakariContact = new SakariContact();
                var sakariMobile = new SakariMobile()
                {
                    country = "AU",
                    number = phone
                };
                sakariContact.mobile = sakariMobile;
                sakariContacts.Add(sakariContact);
                sendSakariSmsRequest.contacts = sakariContacts;

                // Step 2.2: Handle Sakari Phone Group
                var phoneFilter = new SakariPhoneNumberFilter();
                var sakariGroup = new SakariGroup()
                {
                    id = phoneGroupId
                };
                phoneFilter.group = sakariGroup;
                sendSakariSmsRequest.phoneNumberFilter = phoneFilter;

                var sendSakariSmsResponse = await _oneCorpSakariService.SendSakariSMS(sendSakariSmsRequest);
                if (sendSakariSmsResponse.Code != ResultCode.OK)
                {
                    apiResult.Message = OneCorpConstants.SSFW_E04;
                    return apiResult;
                }

                apiResult.Code = ResultCode.OK;
                apiResult.Message = OneCorpConstants.SSFW_200;

                return apiResult;

            }
            catch (Exception ex)
            {
                apiResult.Data = $"{ex.Message} - {ex.StackTrace}";
                emailContent.Body = emailContent.Body.Replace("$Result$", JsonConvert.SerializeObject(apiResult));
                await EmailHelpers.SendEmail(emailContent);
                return apiResult;
            }
        }

        public async Task<ApiResultDto<string>> SyncSakariPhoneGroup2Zoho()
        {
            var apiResult = new ApiResultDto<string>()
            {
                Code = ResultCode.BadRequest,
                Message = OneCorpConstants.SSPG2Z_400
            };

            try
            {
                // STEP 1: Get all Phone Groups
                var getAllPhoneGroupsResponse = await _oneCorpSakariService.GetSakariPhoneGroups();
                if (getAllPhoneGroupsResponse.Code != ResultCode.OK)
                {
                    apiResult.Message = OneCorpConstants.SSPG2Z_E01;
                    return apiResult;
                }
                var userDicts = new Dictionary<string, List<SakariUserPhoneGroup>>();

                // STEP 2: Create Users - Groups linking
                var phoneGroups = getAllPhoneGroupsResponse.Data.data;

                foreach (var phoneGroup in phoneGroups)
                {
                    string groupId = phoneGroup.id;
                    string groupName = phoneGroup.name;
                    var groupUsers = phoneGroup.users;
                    var groupTags = phoneGroup.tags;

                    string groupTag = "";

                    if (groupTags != null && groupTags.Length > 0)
                    {
                        groupTag = string.Join(",", groupTags);
                    }

                    var phoneNumbers = phoneGroup.phoneNumbers;
                    string phoneNumber = string.Empty;
                    if (phoneNumbers.Length > 0)
                    {
                        var firstPhone = phoneNumbers[0];
                        phoneNumber = firstPhone.number;
                    }

                    foreach (var user in groupUsers)
                    {
                        string userFirstName = user.firstName;
                        string userLastName = user.lastName;
                        string userFullName = string.Empty;

                        if (!string.IsNullOrEmpty(userFirstName))
                        {
                            userFullName = userFirstName;
                        }

                        if (!string.IsNullOrEmpty(userLastName))
                        {
                            if (string.IsNullOrEmpty(userFullName))
                            {
                                userFullName = userLastName;
                            }
                            else
                            {
                                userFullName = userFullName + " " + userLastName;
                                userFullName = userFullName.Trim();
                            }
                        }
                        
                        string userEmail = user.email;

                        if (string.IsNullOrEmpty(userFullName))
                        {

                            // Search Users by Email
                            var coqlRequest = new ZohoCoqlRequest()
                            {
                                select_query = $"SELECT first_name, last_name FROM users WHERE email = '{userEmail}'"
                            };
                            var queryUsersResponse = await _oneCorpCrmService.QueryUsers(coqlRequest);
                            if (queryUsersResponse.Code == ResultCode.OK)
                            {
                                var userDetails = queryUsersResponse.Data.data[0];
                                string firstName = userDetails.first_name;
                                string lastName = userDetails.last_name;
                                if (!string.IsNullOrEmpty(firstName))
                                {
                                    userFullName = firstName;
                                }

                                if (!string.IsNullOrEmpty(lastName))
                                {
                                    if (string.IsNullOrEmpty(userFullName))
                                    {
                                        userFullName = lastName;
                                    }
                                    else
                                    {
                                        userFullName = userFullName + " " + lastName;
                                        userFullName = userFullName.Trim();
                                    }
                                }
                            }
                        }

                        if (string.IsNullOrEmpty(userFullName))
                        {
                            userFullName = userEmail;
                        }

                        var userGroup = new SakariUserPhoneGroup()
                        {
                            UserName = userFullName,
                            GroupId = groupId,
                            GroupName = groupName,
                            GroupTags = groupTag,
                            PhoneNumber = phoneNumber
                        };

                        if (userDicts.ContainsKey(userEmail))
                        {
                            var userGroups = userDicts[userEmail];
                            if (userGroups == null)
                            {
                                userGroups = new List<SakariUserPhoneGroup>();
                            }
                            userGroups.Add(userGroup);
                        }
                        else
                        {
                            var userGroups = new List<SakariUserPhoneGroup>();
                            userGroups.Add(userGroup);
                            userDicts.Add(userEmail, userGroups);
                        }
                    }

                }

                // STEP 3: Check if Sakari user exist in Zoho CRM
                foreach (var userEmail in userDicts.Keys)
                {

                    var userGroups = userDicts[userEmail];
                    string userFullName = userGroups[0].UserName;
                    var coqlRequest = new ZohoCoqlRequest()
                    {
                        select_query = $"select Name from Sakari_Users WHERE Email = '{userEmail}'" 
                    };
                    var querySakariUserResponse = await _oneCorpCrmService.QuerySakariUsers(coqlRequest);

                    if (querySakariUserResponse.Code == ResultCode.OK)
                    {

                        // Step 3.1: Update Sakari User
                        var sakariUser = querySakariUserResponse.Data.data[0];
                        var sakariForUpsert = new SakariUserForUpsert()
                        {
                            Email = userEmail,
                            Name = userFullName
                        };

                        string sakariUserId = sakariUser.id;
                        var getSakariUserByIdResponse = await _oneCorpCrmService.GetSakariUserById(sakariUserId);

                        var sakariUserDetails = getSakariUserByIdResponse.Data.data[0];
                        var sakariUserGroups = sakariUserDetails.Sakari_Groups;

                        bool checkExist = true;
                        if (sakariUserGroups == null)
                        {
                            checkExist = false;
                        }

                        var groupsForUpsert = new List<SakariGroupForUpsert>();
                        foreach (var group in userGroups)
                        {
                            var groupForUpsert = new SakariGroupForUpsert() { 
                                Group_Id = group.GroupId,
                                Group_Name = group.GroupName,
                                Phone_Number = group.PhoneNumber
                            };
                            string groupId = group.GroupId;
                            string groupName = group.GroupName;
                            string phoneNumber = group.PhoneNumber;

                            if (checkExist)
                            {
                                var zohoGroup = sakariUserGroups.FirstOrDefault(g => g.Group_Id == groupId);
                                if (zohoGroup != null)
                                {
                                    groupForUpsert.id = zohoGroup.id;
                                }
                            }
                            groupsForUpsert.Add(groupForUpsert);
                        }
                        sakariForUpsert.Sakari_Groups = groupsForUpsert;

                        var upsertRequest = new UpsertRequest<SakariUserForUpsert>();
                        upsertRequest.data.Add(sakariForUpsert);

                        var updateSakariUserResponse = await _oneCorpCrmService.UpdateSakariUser(sakariUserId, upsertRequest);
                        if (updateSakariUserResponse.Code != ResultCode.OK)
                        {
                            apiResult.Message = OneCorpConstants.SSPG2Z_E03;
                            return apiResult;
                        }

                    }
                    else
                    {
                        // Step 3.2: Create Sakari User
                        var sakariForUpsert = new SakariUserForUpsert()
                        {
                            Email = userEmail,
                            Name = userFullName
                        };

                        var groupsForUpsert = new List<SakariGroupForUpsert>();
                        foreach (var group in userGroups)
                        {
                            var groupForUpsert = new SakariGroupForUpsert()
                            {
                                Group_Id = group.GroupId,
                                Group_Name = group.GroupName,
                                Phone_Number = group.PhoneNumber
                            };
                            string groupId = group.GroupId;
                            string groupName = group.GroupName;
                            string phoneNumber = group.PhoneNumber;
                            groupsForUpsert.Add(groupForUpsert);
                        }
                        sakariForUpsert.Sakari_Groups = groupsForUpsert;

                        var upsertRequest = new UpsertRequest<SakariUserForUpsert>();
                        upsertRequest.data.Add(sakariForUpsert);

                        var createSakariUserResponse = await _oneCorpCrmService.CreateSakariUser(upsertRequest);
                        if (createSakariUserResponse.Code != ResultCode.OK)
                        {
                            apiResult.Message = OneCorpConstants.SSPG2Z_E02;
                            return apiResult;
                        }
                    }

                }
                apiResult.Code = ResultCode.OK;
                apiResult.Message = OneCorpConstants.SSPG2Z_200;
                return apiResult;
            }
            catch (Exception ex)
            {
                apiResult.Message = $"{ex.Message} - {ex.StackTrace}";
                return apiResult;
            }
        }

        public async Task<ApiResultDto<GetSakariUserByIdResponse>> GetSakariUsersDetailsByEmail(string userEmail)
        {
            var apiResult = new ApiResultDto<GetSakariUserByIdResponse>()
            {
                Code = ResultCode.NoContent,
                Message = CommonConstants.MSG_400
            };

            try
            {
                // STEP 1: Query Sakari Ussers
                var coqlRequest = new ZohoCoqlRequest()
                {
                    select_query = $"Select Name from Sakari_Users WHERE Email = '{userEmail}'"
                };
                var querySakariUsersResponse = await _oneCorpCrmService.QuerySakariUsers(coqlRequest);
                if (querySakariUsersResponse.Code != ResultCode.OK)
                {
                    apiResult.Message = OneCorpConstants.QSE_400;
                    return apiResult;
                }
                var sakariUser = querySakariUsersResponse.Data.data[0];
                var sakariUserId = sakariUser.id;

                // STEP 2: Get Sakari User by Id
                var getSakariUserByIdResponse = await _oneCorpCrmService.GetSakariUserById(sakariUserId);
                if (getSakariUserByIdResponse.Code != ResultCode.OK)
                {
                    apiResult.Message = OneCorpConstants.QSE_400;
                    return apiResult;
                }
                
                apiResult.Code = ResultCode.OK;
                apiResult.Data = getSakariUserByIdResponse.Data;
                apiResult.Message = OneCorpConstants.QSE_200;

                return apiResult;
            }
            catch (Exception ex)
            {
                apiResult.Message = $"{ex.Message} - {ex.StackTrace}";
                return apiResult;
            }
        }

        public async Task<ApiResultDto<string>> SyncSakariMessagePayload(MessagePayload messagePayload)
        {
            var apiResult = new ApiResultDto<string>()
            {
                Code = ResultCode.BadRequest,
                Message = CommonConstants.MSG_400
            };

            try
            {

                var upsertLogRequest = new SMSLogForUpsert();
                upsertLogRequest.Platform = "Sakari";
                var payload = messagePayload.payload;
                string eventType = messagePayload.eventType;
                if (eventType == "message-received")
                {
                    upsertLogRequest.Direction = "Incoming";
                }
                else
                {
                    upsertLogRequest.Direction = "Outgoing";
                }

                var messageCreatedAt = payload.created.at;
                var tz = TimeZoneInfo.FindSystemTimeZoneById("E. Australia Standard Time");
                string messageCreatedAtStr = TimeZoneInfo.ConvertTimeFromUtc
                    (messageCreatedAt.Value, tz).ToString("yyyy-MM-ddTHH:mm:ss");
                upsertLogRequest.Sent_Time = messageCreatedAtStr;

                string status = payload.status;
                upsertLogRequest.Status = status;

                string message = payload.message;
                upsertLogRequest.Template = message;

                string messageId = payload.id;
                upsertLogRequest.Message_Id = messageId;

                var conversation = payload.conversation;
                string conversationId = conversation.id;
                upsertLogRequest.Conversation_Id = conversationId;

                var group = payload.group;
                string groupName = group.name;
                upsertLogRequest.Phone_Group = groupName;

                var phoneNumber = conversation.phoneNumber;
                string oneCorpNumber = phoneNumber.number;
                upsertLogRequest.OneCorp_Number = oneCorpNumber;

                var messageContact = payload.contact;
                var contactMobile = messageContact.mobile;
                string contactNumber = contactMobile.number;
                upsertLogRequest.Contact_Mobile = contactNumber;
                var sakariContactEmail = messageContact.email;
                upsertLogRequest.Contact_Email = sakariContactEmail;
                decimal price = 0;
                if (payload.price.HasValue)
                {
                    price = Math.Round(payload.price.Value, 2);
                }
                upsertLogRequest.Price = price;

                // STEP 1: Query Lead by Phone
                string leadId = string.Empty;
                string leadFullName = string.Empty;
                string leadFirstName = string.Empty;
                string leadLastName = string.Empty;
                string leadPhone = string.Empty;
                string leadEmail = string.Empty;

                var searchLeadsByPhoneResponse = await _oneCorpCrmService.SearchLeadsByPhone(contactNumber);
                if (searchLeadsByPhoneResponse.Code == ResultCode.OK)
                {
                    var leadDetails = searchLeadsByPhoneResponse.Data.data[0];
                    leadId = leadDetails.id;
                    leadFullName = leadDetails.Full_Name;
                    leadFirstName = leadDetails.First_Name;
                    leadLastName = leadDetails.Last_Name;
                    leadEmail = leadDetails.Email;
                    leadPhone = leadDetails.Phone;
                    var leadOwner = leadDetails.Owner;
                    string leadOwnerId = leadOwner.id;

                    upsertLogRequest.Related_Lead = leadId;
                    upsertLogRequest.Contact_Full_Name = leadFullName;
                    upsertLogRequest.Contact_First_Name = leadFirstName;
                    upsertLogRequest.Contact_Last_Name = leadLastName;
                    upsertLogRequest.Contact_Email = leadEmail;
                    upsertLogRequest.Contact_Mobile = leadPhone;
                    upsertLogRequest.Owner = leadOwnerId;
                }

                // STEP 2: Query Contact by Phone
                string contactId = string.Empty;
                string contactFullName = string.Empty;
                string contactFirstName = string.Empty;
                string contactLastName = string.Empty;
                string contactPhone = string.Empty;
                string contactEmail = string.Empty;
                var searchContactsByPhoneResponse = await _oneCorpCrmService.SearchContactsByPhone(contactNumber);
                if (searchContactsByPhoneResponse.Code == ResultCode.OK)
                {
                    var contactDetails = searchContactsByPhoneResponse.Data.data[0];
                    contactId = contactDetails.id;
                    contactFullName = contactDetails.Full_Name;
                    contactFirstName = contactDetails.First_Name;
                    contactLastName = contactDetails.Last_Name;
                    contactPhone = contactDetails.Phone;
                    contactEmail = contactDetails.Email;
                    var contactOwner = contactDetails.Owner;
                    string contactOwnerId = contactOwner.id;

                    upsertLogRequest.Related_Contact = contactId;
                    upsertLogRequest.Contact_Full_Name = contactFullName;
                    upsertLogRequest.Contact_First_Name = contactFirstName;
                    upsertLogRequest.Contact_Last_Name = contactLastName;
                    upsertLogRequest.Contact_Mobile = contactPhone;
                    upsertLogRequest.Contact_Email = contactEmail;
                    upsertLogRequest.Owner = contactOwnerId;

                    // STEP 2.1: Query Deals using Contact Id
                    string dealQuery = @$"SELECT Deal_Name FROM Deals WHERE Contact_Name = '{contactId}' 
                        AND (Pipeline = 'IP' OR Pipeline = 'SMSF') ORDER BY Created_Time DESC";
                    var dealCoqlRequest = new ZohoCoqlRequest()
                    {
                        select_query = dealQuery
                    };
                    var queryDealsResponse = await _oneCorpCrmService.QueryDeals(dealCoqlRequest);
                    string dealId = "";
                    if (queryDealsResponse.Code == ResultCode.OK)
                    {
                        var dealDetails = queryDealsResponse.Data.data[0];
                        dealId = dealDetails.id;
                    }
                    if (!string.IsNullOrEmpty(dealId))
                    {
                        upsertLogRequest.Related_Deal = dealId;
                    }
                }

                // STEP 3: Query Sakari Logs by Message Id
                string logId = string.Empty;
                var coqlRequest = new ZohoCoqlRequest()
                {
                    select_query = $"select Name from Sakari_SMS_Logs WHERE Message_Id = '{messageId}'"
                };
                var queryLogsResponse = await _oneCorpCrmService.QuerySakariSMSLogs(coqlRequest);
                if (queryLogsResponse.Code == ResultCode.OK)
                {
                    logId = queryLogsResponse.Data.data[0].id;
                }
                var upsertRequest = new UpsertRequest<SMSLogForUpsert>();
                upsertRequest.data.Add(upsertLogRequest);
                upsertRequest.trigger.Add("workflow");

                if (string.IsNullOrEmpty(logId))
                {

                    var createLogResponse = await _oneCorpCrmService.CreateSakariSMSLog(upsertRequest);
                    if (createLogResponse.Code != ResultCode.OK)
                    {
                        apiResult.Message = OneCorpConstants.SSM2Z_E02;
                        return apiResult;
                    }

                }
                else
                {
                    var updateLogResponse = await _oneCorpCrmService.UpdateSakariSMSLog(logId, upsertRequest);
                    if (updateLogResponse.Code != ResultCode.OK)
                    {
                        apiResult.Message = OneCorpConstants.SSM2Z_E03;
                        return apiResult;

                    }
                }

                apiResult.Code = ResultCode.OK;
                apiResult.Message = OneCorpConstants.SSM2Z_200;

                return apiResult;
            }
            catch (Exception ex)
            {
                apiResult.Message = $"{ex.Message} - {ex.StackTrace}";
                return apiResult;
            }
        }

        public async Task<ApiResultDto<string>> MassSyncSakariMessages()
        {

            var apiResult = new ApiResultDto<string>
            {
                Code = ResultCode.BadRequest,
                Message = OneCorpConstants.MSSM2Z_400
            };
            try
            {

                // STEP 1: Get all User Groups
                var getUserGroupsResponse = await _oneCorpSakariService.GetSakariPhoneGroups();
                if (getUserGroupsResponse.Code != ResultCode.OK)
                {
                    apiResult.Message = OneCorpConstants.MSSM2Z_E01;
                    return apiResult;
                }
                var userGroups = getUserGroupsResponse.Data.data;
                int startOffset = 0;
                var responseCode = ResultCode.OK;

                while (startOffset >= 0 && responseCode == ResultCode.OK)
                {
                    
                    // STEP 3: Get Sakari Messages
                    var getSakariMessagesResponse = await _oneCorpSakariService.GetSakariMessages(startOffset);
                    responseCode = getSakariMessagesResponse.Code;
                    if (responseCode != ResultCode.OK)
                    {

                        apiResult.Message = OneCorpConstants.MSSM2Z_E02;
                        return apiResult;
                    }
                    var sakariMessages = getSakariMessagesResponse.Data.data;
                    sakariMessages = sakariMessages.OrderBy(m => m.created.at.Value)
                        .ToList();
    
                    foreach (var message in sakariMessages)
                    {
                        var upsertLogRequest = new SMSLogForUpsert();

                        upsertLogRequest.Platform = "Sakari";
                        bool outgoing = message.outgoing.Value;
                        if (outgoing)
                        {
                            upsertLogRequest.Direction = "Outgoing";
                        }
                        else
                        {
                            upsertLogRequest.Direction = "Incoming";
                        }

                        var messageCreatedAt = message.created.at;
                        var tz = TimeZoneInfo.FindSystemTimeZoneById("E. Australia Standard Time");
                        string messageCreatedAtStr = TimeZoneInfo.ConvertTimeFromUtc
                            (messageCreatedAt.Value, tz).ToString("yyyy-MM-ddTHH:mm:ss");
                        upsertLogRequest.Sent_Time = messageCreatedAtStr;

                        string status = message.status;
                        upsertLogRequest.Status = status;

                        string template = message.message;
                        upsertLogRequest.Template = template;

                        string messageId = message.id;
                        upsertLogRequest.Message_Id = messageId;

                        var conversation = message.conversation;
                        string conversationId = conversation.id;
                        upsertLogRequest.Conversation_Id = conversationId;

                        var group = message.group;
                        if (group != null)
                        {
                            string groupId = group.id;

                            var userGroup = userGroups.Where(g => g.id == groupId).FirstOrDefault();
                            string groupName = string.Empty;
                            string oneCorpNumber = string.Empty;
                            if (userGroup != null)
                            {
                                groupName = userGroup.name;
                                var numbers = userGroup.phoneNumbers;
                                if (numbers != null && numbers.Length > 0)
                                {
                                    var firstNumber = numbers[0];
                                    oneCorpNumber = firstNumber.number;
                                }
                            }

                            upsertLogRequest.Phone_Group = groupName;

                            var phoneNumber = conversation.phoneNumber;
                            upsertLogRequest.OneCorp_Number = oneCorpNumber;
                        }

                        var messageContact = message.contact;
                        var contactMobile = messageContact.mobile;
                        string contactNumber = contactMobile.number;
                        upsertLogRequest.Contact_Mobile = contactNumber;
                        var sakariContactEmail = messageContact.email;
                        upsertLogRequest.Contact_Email = sakariContactEmail;
                        decimal price = 0;
                        if (message.price.HasValue)
                        {
                            price = Math.Round(message.price.Value, 2);
                        }
                        upsertLogRequest.Price = price;

                        // STEP 1: Query Lead by Phone
                        string leadId = string.Empty;
                        string leadFullName = string.Empty;
                        string leadFirstName = string.Empty;
                        string leadLastName = string.Empty;
                        string leadPhone = string.Empty;
                        string leadEmail = string.Empty;

                        var searchLeadsByPhoneResponse = await _oneCorpCrmService.SearchLeadsByPhone(contactNumber);
                        // Thread.Sleep(1000);
                        if (searchLeadsByPhoneResponse.Code == ResultCode.OK)
                        {
                            var leadDetails = searchLeadsByPhoneResponse.Data.data[0];
                            leadId = leadDetails.id;
                            leadFullName = leadDetails.Full_Name;
                            leadFirstName = leadDetails.First_Name;
                            leadLastName = leadDetails.Last_Name;
                            leadEmail = leadDetails.Email;
                            leadPhone = leadDetails.Phone;
                            var leadOwner = leadDetails.Owner;
                            string leadOwnerId = leadOwner.id;

                            upsertLogRequest.Related_Lead = leadId;
                            upsertLogRequest.Contact_Full_Name = leadFullName;
                            upsertLogRequest.Contact_First_Name = leadFirstName;
                            upsertLogRequest.Contact_Last_Name = leadLastName;
                            upsertLogRequest.Contact_Email = leadEmail;
                            upsertLogRequest.Contact_Mobile = leadPhone;
                            upsertLogRequest.Owner = leadOwnerId;
                        }

                        // STEP 2: Query Contact by Phone
                        string contactId = string.Empty;
                        string contactFullName = string.Empty;
                        string contactFirstName = string.Empty;
                        string contactLastName = string.Empty;
                        string contactPhone = string.Empty;
                        string contactEmail = string.Empty;
                        var searchContactsByPhoneResponse = await _oneCorpCrmService.SearchContactsByPhone(contactNumber);
                        // Thread.Sleep(1000);
                        if (searchContactsByPhoneResponse.Code == ResultCode.OK)
                        {
                            var contactDetails = searchContactsByPhoneResponse.Data.data[0];
                            contactId = contactDetails.id;
                            contactFullName = contactDetails.Full_Name;
                            contactFirstName = contactDetails.First_Name;
                            contactLastName = contactDetails.Last_Name;
                            contactPhone = contactDetails.Phone;
                            contactEmail = contactDetails.Email;
                            var contactOwner = contactDetails.Owner;
                            string contactOwnerId = contactOwner.id;

                            upsertLogRequest.Related_Contact = contactId;
                            upsertLogRequest.Contact_Full_Name = contactFullName;
                            upsertLogRequest.Contact_First_Name = contactFirstName;
                            upsertLogRequest.Contact_Last_Name = contactLastName;
                            upsertLogRequest.Contact_Mobile = contactPhone;
                            upsertLogRequest.Contact_Email = contactEmail;
                            upsertLogRequest.Owner = contactOwnerId;

                            // STEP 2.1: Query Deals using Contact Id
                            string dealQuery = @$"SELECT Deal_Name FROM Deals WHERE Contact_Name = '{contactId}' 
                        AND (Pipeline = 'IP' OR Pipeline = 'SMSF') ORDER BY Created_Time DESC";
                            var dealCoqlRequest = new ZohoCoqlRequest()
                            {
                                select_query = dealQuery
                            };
                            var queryDealsResponse = await _oneCorpCrmService.QueryDeals(dealCoqlRequest);
                            Thread.Sleep(1000);
                            string dealId = "";
                            if (queryDealsResponse.Code == ResultCode.OK)
                            {
                                var dealDetails = queryDealsResponse.Data.data[0];
                                dealId = dealDetails.id;
                            }
                            if (!string.IsNullOrEmpty(dealId))
                            {
                                upsertLogRequest.Related_Deal = dealId;
                            }
                        }

                        // STEP 3: Query Sakari Logs by Message Id
                        string logId = string.Empty;
                        var coqlRequest = new ZohoCoqlRequest()
                        {
                            select_query = $"select Name from Sakari_SMS_Logs WHERE Message_Id = '{messageId}'"
                        };
                        var queryLogsResponse = await _oneCorpCrmService.QuerySakariSMSLogs(coqlRequest);
                        // Thread.Sleep(1000);
                        if (queryLogsResponse.Code == ResultCode.OK)
                        {
                            logId = queryLogsResponse.Data.data[0].id;
                        }
                        var upsertRequest = new UpsertRequest<SMSLogForUpsert>();
                        upsertRequest.data.Add(upsertLogRequest);
                        upsertRequest.trigger.Add("workflow");

                        if (string.IsNullOrEmpty(logId))
                        {
                            var createLogResponse = await _oneCorpCrmService.CreateSakariSMSLog(upsertRequest);
                            Thread.Sleep(1000);
                        }
                        else
                        {
                            var updateLogResponse = await _oneCorpCrmService.UpdateSakariSMSLog(logId, upsertRequest);
                            Thread.Sleep(1000);
                        }

                    }
                    startOffset = startOffset - 100;
                }

                apiResult.Code = ResultCode.OK;
                apiResult.Message = OneCorpConstants.MSSM2Z_200;
                return apiResult;

            }
            catch (Exception ex)
            {
                apiResult.Message = $"{ex.Message} - {ex.StackTrace}";
                return apiResult;
            }


        }



        #endregion

        #region Twilio

        public async Task<ApiResultDto<string>> SyncHistoryTwilioSMSLogs()
        {
            var apiResult = new ApiResultDto<string>()
            {
                Code = ResultCode.BadRequest,
                Message = OneCorpConstants.STME2H_400
            };

            try
            {

                // STEP 1: Read multiple Message resources
                var currentTime = DateTime.UtcNow;

                // STEP 2: Setup Query Parameters to get messages
                string nextParam = $"?PageSize=500&DateSent<=2023-09-06T18:10:00Z";

                while (!string.IsNullOrEmpty(nextParam))
                {
                    var getTwilioMessagesResponse = await _oneCorpTwilioService.ReadMultipleMessageResources(nextParam);

                    if (getTwilioMessagesResponse.Code != ResultCode.OK)
                    {
                        apiResult.Message = OneCorpConstants.STME2H_E01;
                        return apiResult;
                    }

                    var smsLogs = getTwilioMessagesResponse.Data.messages;
                    string nextUrl = getTwilioMessagesResponse.Data.next_page_uri;

                    var urlSplits = nextUrl.Split("?");
                    if (urlSplits.Length > 1)
                    {
                        nextParam = urlSplits[1];
                    }
                    nextParam = $"?{nextParam}";
                        
                    foreach (var smsLog in smsLogs)
                    {
                        // STEP 3: Upsert SMS Log in Zoho CRM
                        // Step 3.1: Search Log by Message Id
                        string messageId = smsLog.sid;

                        var coqlRequest = new ZohoCoqlRequest()
                        {
                            select_query = $"SELECT Name FROM Sakari_SMS_Logs WHERE Message_Id = '{messageId}'"
                        };
                        var queryLogsResponse = await _oneCorpCrmService.QuerySakariSMSLogs(coqlRequest);

                        string smsLogId = "";
                        if (queryLogsResponse.Code == ResultCode.OK)
                        {
                            var logDetails = queryLogsResponse.Data.data[0];
                            smsLogId = logDetails.id;
                        }

                        // Step 3.2: Prepare Upsert Log Request
                        var upsertRequest = new UpsertRequest<SMSLogForUpsert>();
                        var smsLogForUpsert = new SMSLogForUpsert();
                        smsLogForUpsert.Platform = "Twilio";
                        smsLogForUpsert.Owner = OneCorpConstants.ZohoCRM_JoshUserId;
                        string template = smsLog.body;
                        smsLogForUpsert.Template = template;

                        string priceStr = smsLog.price;
                        decimal price = decimal.Parse(priceStr);
                        price = Math.Abs(price);
                        price = decimal.Round(price, 2);
                        smsLogForUpsert.Price = price;

                        string status = smsLog.status;
                        smsLogForUpsert.Status = status;
                        smsLogForUpsert.Message_Id = messageId;

                        string customerPhone = "";
                        string direction = smsLog.direction;

                        string fromNumber = smsLog.from;
                        string toNumber = smsLog.to;

                        if (direction.Contains("outbound", StringComparison.InvariantCultureIgnoreCase))
                        {
                            smsLogForUpsert.Direction = "Outgoing";
                            smsLogForUpsert.OneCorp_Number = fromNumber;
                            smsLogForUpsert.Contact_Mobile = toNumber;
                            customerPhone = toNumber;
                        }
                        else if (direction.Contains("inbound", StringComparison.InvariantCultureIgnoreCase))
                        {
                            smsLogForUpsert.Direction = "Incoming";
                            smsLogForUpsert.OneCorp_Number = toNumber;
                            smsLogForUpsert.Contact_Mobile = fromNumber;
                            customerPhone = fromNumber;
                        }

                        // Step 3.3: Search Leads using Customer Phone
                        var searchLeadsResponse = await _oneCorpCrmService.SearchLeadsByPhone(customerPhone);
                        Thread.Sleep(1000);
                        if (searchLeadsResponse.Code == ResultCode.OK)
                        {
                            var leadDetails = searchLeadsResponse.Data.data[0];
                            string leadId = leadDetails.id;
                            string leadFirstName = leadDetails.First_Name;
                            string leadLastName = leadDetails.Last_Name;
                            string leadFullName = leadDetails.Full_Name;
                            string leadEmail = leadDetails.Email;
                            smsLogForUpsert.Related_Lead = leadId;
                            smsLogForUpsert.Contact_First_Name = leadFirstName;
                            smsLogForUpsert.Contact_Last_Name = leadLastName;
                            smsLogForUpsert.Contact_Full_Name = leadFullName;
                            smsLogForUpsert.Contact_Email = leadEmail;

                        }

                        // Step 3.4: Search Contacts using Customer Phone
                        var searchContactsResponse = await _oneCorpCrmService.SearchContactsByPhone(customerPhone);
                        Thread.Sleep(1000);
                        if (searchContactsResponse.Code == ResultCode.OK)
                        {
                            var contactDetails = searchContactsResponse.Data.data[0];
                            string contactId = contactDetails.id;
                            string contactFirstName = contactDetails.First_Name;
                            string contactLastName = contactDetails.Last_Name;
                            string contactFullName = contactDetails.Full_Name;
                            string contactEmail = contactDetails.Email;
                            smsLogForUpsert.Related_Contact = contactId;
                            smsLogForUpsert.Contact_First_Name = contactFirstName;
                            smsLogForUpsert.Contact_Last_Name = contactLastName;
                            smsLogForUpsert.Contact_Full_Name = contactFullName;
                            smsLogForUpsert.Contact_Email = contactEmail;
                        }

                        // STEP 3.5: Handle Sent Time
                        string dateSentStr = smsLog.date_sent;
                        var dateSent = DateTime.Parse(dateSentStr);

                        string sentTime = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(dateSent, "E. Australia Standard Time")
                            .ToString(CommonConstants.ZohoDateTimeFormat);
                        smsLogForUpsert.Sent_Time = sentTime;

                        upsertRequest.data.Add(smsLogForUpsert);

                        if (string.IsNullOrEmpty(smsLogId))
                        {
                            var createLogResponse = await _oneCorpCrmService.CreateSakariSMSLog(upsertRequest);
                            Thread.Sleep(1000);
                            if (createLogResponse.Code != ResultCode.OK)
                            {
                                apiResult.Message = OneCorpConstants.STME2H_E02;
                                return apiResult;
                            }

                        }
                        else
                        {
                            var updateLogResponse = await _oneCorpCrmService.UpdateSakariSMSLog(smsLogId, upsertRequest);
                            Thread.Sleep(1000);
                            if (updateLogResponse.Code != ResultCode.OK)
                            {
                                apiResult.Message = OneCorpConstants.STME2H_E03;
                                return apiResult;
                            }
                        }
                    }

                }

                apiResult.Code = ResultCode.OK;
                apiResult.Message = OneCorpConstants.STME2H_200;
                return apiResult;

            }
            catch (Exception ex)
            {
                apiResult.Message = $"{ex.Message} - {ex.StackTrace}";
                return apiResult;
            }
        }

        public async Task<ApiResultDto<string>> SyncTwilioSMSLogsEvery2Hours()
        {

            var apiResult = new ApiResultDto<string>()
            {
                Code = ResultCode.BadRequest,
                Message = OneCorpConstants.STME2H_400
            };

            try
            {

                // STEP 1: Read multiple Message resources every 2 hours

                var currentTime = DateTime.UtcNow;
                var endTime = currentTime;
                var startTime = currentTime.AddMinutes(-125);

                string endTimeStr = DateTimeHelpers.ToUniversalIso8601(endTime);
                string startTimeStr = DateTimeHelpers.ToUniversalIso8601(startTime);

                // STEP 2: Setup Query Parameters to get messages
                string queryParams = $"?DateSent>={startTimeStr}&DateSent<={endTimeStr}&PageSize=500";
                var getTwilioMessagesResponse = await _oneCorpTwilioService.ReadMultipleMessageResources(queryParams);

                if (getTwilioMessagesResponse.Code != ResultCode.OK)
                {
                    apiResult.Message = OneCorpConstants.STME2H_E01;
                    return apiResult;
                }

                var smsLogs = getTwilioMessagesResponse.Data.messages;

                foreach (var smsLog in smsLogs)
                {
                    // STEP 3: Upsert SMS Log in Zoho CRM
                    // Step 3.1: Search Log by Message Id
                    string messageId = smsLog.sid;

                    var coqlRequest = new ZohoCoqlRequest()
                    {
                        select_query = $"SELECT Name FROM Sakari_SMS_Logs WHERE Message_Id = '{messageId}'"
                    };
                    var queryLogsResponse = await _oneCorpCrmService.QuerySakariSMSLogs(coqlRequest);

                    string smsLogId = "";
                    if (queryLogsResponse.Code == ResultCode.OK)
                    {
                        var logDetails = queryLogsResponse.Data.data[0];
                        smsLogId = logDetails.id;
                    }

                    // Step 3.2: Prepare Upsert Log Request
                    var upsertRequest = new UpsertRequest<SMSLogForUpsert>();
                    var smsLogForUpsert = new SMSLogForUpsert();
                    smsLogForUpsert.Platform = "Twilio";
                    smsLogForUpsert.Owner = OneCorpConstants.ZohoCRM_JoshUserId;
                    string template = smsLog.body;
                    smsLogForUpsert.Template = template;

                    string priceStr = smsLog.price;
                    if (!string.IsNullOrEmpty(priceStr))
                    {
                        bool canParse = decimal.TryParse(priceStr, out decimal price);
                        if (canParse)
                        {
                            price = Math.Abs(price);
                            price = decimal.Round(price, 2);
                            smsLogForUpsert.Price = price;
                        }
                    }
                    

                    string status = smsLog.status;
                    smsLogForUpsert.Status = status;
                    smsLogForUpsert.Message_Id = messageId;

                    string customerPhone = "";
                    string direction = smsLog.direction;

                    string fromNumber = smsLog.from;
                    string toNumber = smsLog.to;

                    if (direction.Contains("outbound", StringComparison.InvariantCultureIgnoreCase))
                    {
                        smsLogForUpsert.Direction = "Outgoing";
                        smsLogForUpsert.OneCorp_Number = fromNumber;
                        smsLogForUpsert.Contact_Mobile = toNumber;
                        customerPhone = toNumber;
                    }
                    else if (direction.Contains("inbound", StringComparison.InvariantCultureIgnoreCase))
                    {
                        smsLogForUpsert.Direction = "Incoming";
                        smsLogForUpsert.OneCorp_Number = toNumber;
                        smsLogForUpsert.Contact_Mobile = fromNumber;
                        customerPhone = fromNumber;
                    }

                    // Step 3.3: Search Leads using Customer Phone
                    var searchLeadsResponse = await _oneCorpCrmService.SearchLeadsByPhone(customerPhone);
                    Thread.Sleep(1000);
                    if (searchLeadsResponse.Code == ResultCode.OK)
                    {
                        var leadDetails = searchLeadsResponse.Data.data[0];
                        string leadId = leadDetails.id;
                        string leadFirstName = leadDetails.First_Name;
                        string leadLastName = leadDetails.Last_Name;
                        string leadFullName = leadDetails.Full_Name;
                        string leadEmail = leadDetails.Email;
                        var leadOwner = leadDetails.Owner;
                        string leadOwnerId = leadOwner.id;

                        smsLogForUpsert.Related_Lead = leadId;
                        smsLogForUpsert.Contact_First_Name = leadFirstName;
                        smsLogForUpsert.Contact_Last_Name = leadLastName;
                        smsLogForUpsert.Contact_Full_Name = leadFullName;
                        smsLogForUpsert.Contact_Email = leadEmail;
                        smsLogForUpsert.Owner = leadOwnerId;

                    }

                    // Step 3.4: Search Contacts using Customer Phone
                    var searchContactsResponse = await _oneCorpCrmService.SearchContactsByPhone(customerPhone);
                    Thread.Sleep(1000);
                    if (searchContactsResponse.Code == ResultCode.OK)
                    {
                        var contactDetails = searchContactsResponse.Data.data[0];
                        string contactId = contactDetails.id;
                        string contactFirstName = contactDetails.First_Name;
                        string contactLastName = contactDetails.Last_Name;
                        string contactFullName = contactDetails.Full_Name;
                        string contactEmail = contactDetails.Email;
                        var contactOwner = contactDetails.Owner;
                        string contactOwnerId = contactOwner.id;

                        smsLogForUpsert.Related_Contact = contactId;
                        smsLogForUpsert.Contact_First_Name = contactFirstName;
                        smsLogForUpsert.Contact_Last_Name = contactLastName;
                        smsLogForUpsert.Contact_Full_Name = contactFullName;
                        smsLogForUpsert.Contact_Email = contactEmail;
                        smsLogForUpsert.Owner = contactOwnerId;

                        string dealQuery = @$"SELECT Deal_Name FROM Deals WHERE Contact_Name = '{contactId}' 
                                            AND (Pipeline = 'IP' OR Pipeline = 'SMSF') ORDER BY Created;";
                        var dealCoqlRequest = new ZohoCoqlRequest()
                        {
                            select_query = dealQuery
                        };

                        string dealId = string.Empty;
                        var queryDealsResponse = await _oneCorpCrmService.QueryDeals(dealCoqlRequest);
                        if (queryDealsResponse.Code == ResultCode.OK)
                        {
                            var dealDetails = queryDealsResponse.Data.data[0];
                            dealId = dealDetails.id;
                        }
                        if (!string.IsNullOrEmpty(dealId))
                        {
                            smsLogForUpsert.Related_Deal = dealId;
                        }

                    }

                    // STEP 3.5: Handle Sent Time
                    string dateSentStr = smsLog.date_sent;
                    var dateSent = DateTime.Parse(dateSentStr);
                    // var tz = TimeZoneInfo.FindSystemTimeZoneById("E. Australia Standard Time");

                    string sentTime = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(dateSent, "E. Australia Standard Time")
                        .ToString(CommonConstants.ZohoDateTimeFormat);
                    smsLogForUpsert.Sent_Time = sentTime;

                    upsertRequest.data.Add(smsLogForUpsert);

                    if (string.IsNullOrEmpty(smsLogId))
                    {
                        var createLogResponse = await _oneCorpCrmService.CreateSakariSMSLog(upsertRequest);
                        Thread.Sleep(1000);
                        if (createLogResponse.Code != ResultCode.OK)
                        {
                            apiResult.Message = OneCorpConstants.STME2H_E02;
                            return apiResult;
                        }

                    }
                    else
                    {
                        var updateLogResponse = await _oneCorpCrmService.UpdateSakariSMSLog(smsLogId, upsertRequest);
                        Thread.Sleep(1000);
                        if (updateLogResponse.Code != ResultCode.OK)
                        {
                            apiResult.Message = OneCorpConstants.STME2H_E03;
                            return apiResult;
                        }
                    }
                }

                apiResult.Code = ResultCode.OK;
                apiResult.Message = OneCorpConstants.STME2H_200;
                return apiResult;

            }
            catch (Exception ex)
            {
                apiResult.Message = $"{ex.Message} - {ex.StackTrace}";
                return apiResult;
            }

        }

        public async Task<ApiResultDto<string>> SyncLeadToSakari(string leadId)
        {
            var apiResult = new ApiResultDto<string>()
            {
                Code = ResultCode.BadRequest,
                Message = OneCorpConstants.SL2S_400
            };

            try
            {

                // STEP 1: Get Lead by Id
                var getLeadByIdResponse = await _oneCorpCrmService.GetLeadById(leadId);
                if (getLeadByIdResponse.Code != ResultCode.OK)
                {
                    apiResult.Message = OneCorpConstants.SL2S_E01;
                    return apiResult;
                }
                var leadDetails = getLeadByIdResponse.Data.data[0];
                string leadMobile = leadDetails.Phone;
                string leadFirstName = leadDetails.First_Name;
                string leadLastName = leadDetails.Last_Name;
                string leadEmail = leadDetails.Email;
                var leadTag = leadDetails.Tag;
                string leadSakariId = leadDetails.Sakari_Id;

                // STEP 2: Fetch Sakari Contacts by Mobile
                if (string.IsNullOrEmpty(leadMobile))
                {
                    apiResult.Message = OneCorpConstants.SL2S_E02;
                    return apiResult;
                }
                var fetchContactParams = new FetchContactsParameters()
                {
                    mobile = StringHelpers.CleanPhoneNumber(leadMobile)
                };

                var fetchContactsResponse = await _oneCorpSakariService
                    .FetchContacts(fetchContactParams);

                var upsertContactRequest = new UpsertContactRequest()
                {
                    firstName = leadFirstName,
                    lastName = leadLastName,
                    email = leadEmail,
                };
                var sakariMobile = new ContactMobile()
                {
                    country = "AU",
                    number = leadMobile
                };
                upsertContactRequest.mobile = sakariMobile;
                var sakariTags = new List<ContactTag>();
                foreach (var tag in leadTag)
                {
                    string tagName = tag.name;
                    var sakariTag = new ContactTag()
                    {
                        tag = tagName,
                        visible = true
                    };
                    sakariTags.Add(sakariTag);
                }
                upsertContactRequest.tags = sakariTags;

                string contactId = string.Empty;
                if (fetchContactsResponse.Code != ResultCode.OK)
                {
                    apiResult.Message = OneCorpConstants.SL2S_E06;
                    return apiResult;
                }

                var contactList = fetchContactsResponse.Data.data;
                if (contactList != null && contactList.Count() > 0)
                {
                    // STEP 3: Update Sakari Contact
                    var contactDetails = fetchContactsResponse.Data.data[0];
                    contactId = contactDetails.id;
                    var updateSakariContactResponse = await _oneCorpSakariService.UpdateContact(contactId, upsertContactRequest);
                    if (updateSakariContactResponse.Code != ResultCode.OK)
                    {
                        apiResult.Message = OneCorpConstants.SL2S_E04;
                        return apiResult;
                    }
                }
                else
                {
                    // STEP 4: Create Sakari Contact
                    var createSakariContactResponse = await _oneCorpSakariService.CreateContact(upsertContactRequest);
                    if (createSakariContactResponse.Code != ResultCode.OK)
                    {
                        apiResult.Message = OneCorpConstants.SL2S_E03;
                        return apiResult;
                    }
                    var contactDetails = createSakariContactResponse.Data.data;
                    contactId = contactDetails.id;
                }
                if (!string.IsNullOrEmpty(contactId))
                {
                    var leadForUpdation = new LeadForUpdation()
                    {
                        Sakari_Id = contactId
                    };
                    var upsertRequest = new UpsertRequest<LeadForUpdation>();
                    upsertRequest.data.Add(leadForUpdation);
                    var updateLeadResponse = await _oneCorpCrmService.UpdateLead(leadId, upsertRequest);
                    if (updateLeadResponse.Code != ResultCode.OK)
                    {
                        apiResult.Message = OneCorpConstants.SL2S_E05;
                        return apiResult;
                    }
                }

                apiResult.Code = ResultCode.OK;
                apiResult.Message = OneCorpConstants.SL2S_200;
                return apiResult;
            
            }
            catch (Exception ex)
            {
                apiResult.Message = $"{ex.Message} - {ex.StackTrace}";
                return apiResult;
            }
        }

        public async Task<ApiResultDto<string>> SyncContactToSakari(string contactId)
        {

            var apiResult = new ApiResultDto<string>()
            {
                Code = ResultCode.BadRequest,
                Message = OneCorpConstants.SC2S_400
            };

            try
            {
                
                // STEP 1: Get Contact by Id
                var getContactByIdResponse = await _oneCorpCrmService.GetContactById(contactId);
                if (getContactByIdResponse.Code != ResultCode.OK)
                {
                    apiResult.Message = OneCorpConstants.SC2S_E01;
                    return apiResult;
                }
                var contactDetails = getContactByIdResponse.Data.data[0];
                string contactMobile = contactDetails.Phone;
                string contactFirstName = contactDetails.First_Name;
                string contactLastName = contactDetails.Last_Name;
                string contactEmail = contactDetails.Email;
                var contactTag = contactDetails.Tag;

                // STEP 2: Fetch Sakari Contacts by Mobile
                if (string.IsNullOrEmpty(contactMobile))
                {
                    apiResult.Message = OneCorpConstants.SC2S_E02;
                    return apiResult;
                }
                var fetchContactParams = new FetchContactsParameters()
                {
                    mobile = StringHelpers.CleanPhoneNumber(contactMobile)
                };

                var fetchContactsResponse = await _oneCorpSakariService
                    .FetchContacts(fetchContactParams);

                var upsertContactRequest = new UpsertContactRequest()
                {
                    firstName = contactFirstName,
                    lastName = contactLastName,
                    email = contactEmail,
                };
                var sakariMobile = new ContactMobile()
                {
                    country = "AU",
                    number = contactMobile
                };
                upsertContactRequest.mobile = sakariMobile;
                var sakariTags = new List<ContactTag>();
                foreach (var tag in contactTag)
                {
                    string tagName = tag.name;
                    var sakariTag = new ContactTag()
                    {
                        tag = tagName,
                        visible = true
                    };
                    sakariTags.Add(sakariTag);
                }
                upsertContactRequest.tags = sakariTags;

                string sakariContactId = string.Empty;
                if (fetchContactsResponse.Code != ResultCode.OK)
                {
                    apiResult.Message = OneCorpConstants.SC2S_E06;
                    return apiResult;
                }

                var contactList = fetchContactsResponse.Data.data;
                if (contactList != null && contactList.Count() > 0)
                {
                    // STEP 3: Update Sakari Contact
                    var sakariContactDetails = fetchContactsResponse.Data.data[0];
                    sakariContactId = sakariContactDetails.id;
                    var updateSakariContactResponse = await _oneCorpSakariService.UpdateContact(sakariContactId, upsertContactRequest);
                    if (updateSakariContactResponse.Code != ResultCode.OK)
                    {
                        apiResult.Message = OneCorpConstants.SC2S_E04;
                        return apiResult;
                    }
                }
                else
                {
                    // STEP 4: Create Sakari Contact
                    var createSakariContactResponse = await _oneCorpSakariService.CreateContact(upsertContactRequest);
                    if (createSakariContactResponse.Code != ResultCode.OK)
                    {
                        apiResult.Message = OneCorpConstants.SC2S_E03;
                        return apiResult;
                    }
                    var sakariContactDetails = createSakariContactResponse.Data.data;
                    sakariContactId = sakariContactDetails.id;
                }
                if (!string.IsNullOrEmpty(sakariContactId))
                {
                    var contactForUpdation = new ContactForUpdation()
                    {
                        Sakari_Id = sakariContactId
                    };
                    var upsertRequest = new UpsertRequest<ContactForUpdation>();
                    upsertRequest.data.Add(contactForUpdation);
                    var updateLeadResponse = await _oneCorpCrmService.UpdateContact(contactId, upsertRequest);
                    if (updateLeadResponse.Code != ResultCode.OK)
                    {
                        apiResult.Message = OneCorpConstants.SL2S_E05;
                        return apiResult;
                    }
                }

                apiResult.Code = ResultCode.OK;
                apiResult.Message = OneCorpConstants.SC2S_200;
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
