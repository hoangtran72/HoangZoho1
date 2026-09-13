using HoangZoho1.Constants;
using HoangZoho1.Helpers;
using HoangZoho1.Models.Common;
using HoangZoho1.Models.OneBudget.ZohoCRM;
using HoangZoho1.Models.OneCorp.ScheduleOnce;
using HoangZoho1.Services.OneCorp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace HoangZoho1.Services.OneBudget
{

    public class OneBudgetCustomService : IOneBudgetCustomService
    {

        private readonly IOneBudgetCrmService _oneBudgetCrmService;
        private readonly IOneCorpCrmService _oneCorpCrmService;
        private readonly IOneCorpOnceHubService _oneCorpOnceHubService;

        public OneBudgetCustomService(IOneBudgetCrmService oneBudgetCrmService,
            IOneCorpOnceHubService oneCorpOnceHubService, IOneCorpCrmService oneCorpCrmService)
        {
            _oneBudgetCrmService = oneBudgetCrmService;
            _oneCorpCrmService = oneCorpCrmService;
            _oneCorpOnceHubService = oneCorpOnceHubService;
        }

        #region ScheduleOnce Bookings

        public async Task<ApiResultDto<string>> ScheduleOnce_SyncBooking(BookingPayload bookingPayload)
        {
            var apiResult = new ApiResultDto<string>()
            {
                Code = ResultCode.BadRequest,
                Message = OneBudgetConstants.SSB_400
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
                string bookingCreationTimeText = TimeZoneInfo.ConvertTimeFromUtc
                    (bookingDetails.creation_time.Value, tz).ToString("yyyy-MM-ddTHH:mm:ss");
                string bookingStartTimeText = TimeZoneInfo.ConvertTimeFromUtc
                    (bookingDetails.starting_time.Value, tz).ToString("yyyy-MM-ddTHH:mm:ss");
                string bookingLastUpdatedTimeText = TimeZoneInfo.ConvertTimeFromUtc
                    (bookingDetails.last_updated_time.Value, tz).ToString("yyyy-MM-ddTHH:mm:ss");

                var customerTimeZone = DateTimeHelpers.OlsonTimeZoneToTimeZoneInfo(customerTimezoneText);
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

                string bookingUserEmail = onceHubUserDetails.email;
                string userQuery = $"select time_zone, status from users WHERE email = '{bookingUserEmail}'";

                var selectQuery = new ZohoCoqlRequest()
                {
                    select_query = userQuery
                };
                string userId = string.Empty;
                string userStatus = string.Empty;
                string userTimeZoneText = string.Empty;
                string userBookingStartTime = string.Empty;
                var queryUsersResponse = await _oneBudgetCrmService.QueryUsers(selectQuery);
                if (queryUsersResponse.Code == ResultCode.OK)
                {
                    var userDetails = queryUsersResponse.Data.data[0];
                    userId = userDetails.id;
                    if (!userStatus.Equals("active", StringComparison.InvariantCultureIgnoreCase))
                    {
                        userId = OneBudgetConstants.ZohoCRM_JoshUserId;
                    }    
                    var getCrmUserByIdResponse = await _oneBudgetCrmService.GetUserById(userId);
                    var crmUserDetails = getCrmUserByIdResponse.Data.users[0];
                    userTimeZoneText = crmUserDetails.time_zone;
                    var userTimeZone = DateTimeHelpers.OlsonTimeZoneToTimeZoneInfo(userTimeZoneText);
                    if (userTimeZone == null)
                    {
                        userTimeZone = TimeZoneInfo.FindSystemTimeZoneById("E. Australia Standard Time");
                    }
                    userBookingStartTime = TimeZoneInfo.ConvertTimeFromUtc(bookingDetails.starting_time.Value, userTimeZone).ToString("yyyy-MM-ddTHH:mm:ss");
                }
                else
                {
                    userId = OneBudgetConstants.ZohoCRM_JoshUserId;
                    userTimeZoneText = "Australia/Melbourne";
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

                        if (string.IsNullOrEmpty(customerAdditionalText))
                        {
                            customerAdditionalText = $"{additional.name}: {additional.value}";
                        }
                        else
                        {
                            customerAdditionalText += $"\n{additional.name}: {additional.value}";
                        }
                    }
                }

                // Step 1.6: Other Information
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
                var searchBookingsResponse = await _oneBudgetCrmService.SearchScheduleOnceBookings(criteria);
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
                        Booking_Owner_Email = bookingUserEmail,
                        Tracking_Id = trackingId,
                        Booking_Subject = bookingSubject,
                        Booking_Created_Time = bookingCreationTimeText,
                        Booking_Starting_Time = bookingStartTimeText,
                        Booking_Duration_minutes = (int)bookingDuration,
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

                    // if (!string.IsNullOrEmpty(userId))
                    // {
                    //    bookingForCreation.Owner = userId;
                    // }

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

                    var createBookingRequest = new UpsertRequest<BookingForCreation>();
                    createBookingRequest.data.Add(bookingForCreation);
                    createBookingRequest.trigger.Add(CommonConstants.ZohoWorkflow);
                    Thread.Sleep(500);
                    var createBookingResponse = await _oneBudgetCrmService.CreateBooking(createBookingRequest);
                    if (createBookingResponse.Code != ResultCode.OK)
                    {
                        apiResult.Message = OneBudgetConstants.SSB_CreateBooking_400;
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
                        Tracking_Id = trackingId,
                        Booking_Subject = bookingSubject,
                        Booking_Owner = bookingOwner,
                        Booking_Owner_Name = bookingUserFullName,
                        Booking_Owner_First_Name = bookingUserFirstName,
                        Booking_Owner_Last_Name = bookingUserLastName,
                        Booking_Owner_Email = bookingUserEmail,
                        Booking_Created_Time = bookingCreationTimeText,
                        Booking_Starting_Time = bookingStartTimeText,
                        Booking_Duration_minutes = (int)bookingDuration,
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
                        Client_Booking_Start_Time = customerBookingStartTime
                    };

                    // if (!string.IsNullOrEmpty(userId))
                    // {
                    //    bookingForUpdation.Owner = userId;
                    // }

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

                    var updateBookingRequest = new UpsertRequest<BookingForUpdation>();
                    updateBookingRequest.data.Add(bookingForUpdation);
                    updateBookingRequest.trigger.Add(CommonConstants.ZohoWorkflow);
                    Thread.Sleep(500);
                    var updateBookingResponse = await _oneBudgetCrmService.UpdateBooking(soBookingId, updateBookingRequest);
                    if (updateBookingResponse.Code != ResultCode.OK)
                    {
                        apiResult.Message = OneBudgetConstants.SSB_UpdateBooking_400;
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
                                    transition_id = OneBudgetConstants.ZohoCRM_ClientCancelled_TransitionId,
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
                                    transition_id = OneBudgetConstants.ZohoCRM_StrategistCancelled_TransitionId,
                                    data = new EmptyObject()
                                };
                                blueprints.Add(blueprint);
                                updateBlueprintRequest.blueprint = blueprints;
                            }
                            var updateBlueprintResponse = await
                                _oneBudgetCrmService.UpdateBlueprint("ScheduleOnce_Bookings", soBookingId, updateBlueprintRequest);
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
                    apiResult.Message = OneBudgetConstants.SSB_SearchBooking_400;
                    return apiResult;
                }

                apiResult.Code = ResultCode.OK;
                apiResult.Message = OneBudgetConstants.SSB_200;
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
                Message = OneBudgetConstants.SBI_400
            };

            try
            {
                // STEP 1: Get Booking Details
                var getBookingByIdResponse = await _oneCorpOnceHubService.GetBookingById(bookingId);
                var bookingDetails = getBookingByIdResponse.Data;

                // Step 1.1: Extract booking data
                string trackingId = bookingDetails.tracking_id;
                string bookingSubject = bookingDetails.subject;

                if (bookingSubject.Contains("OneBudget", StringComparison.InvariantCultureIgnoreCase))
                {
                    apiResult.Code = ResultCode.OK;
                    apiResult.Message = OneBudgetConstants.SSB_NotOneBudget;
                }    

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
                /*
                var queryUsersResponse = await _oneBudgetCrmService.QueryUsers(selectQuery);
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
                */

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
                string customerFirstName = customerName;
                string customerLastName = string.Empty;
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
                            customerLastName = cfValue;
                        }

                        if (string.IsNullOrEmpty(customerAdditionalText))
                        {
                            customerAdditionalText = $"{additional.name}: {additional.value}";
                        }
                        else
                        {
                            customerAdditionalText += $"\n{additional.name}: {additional.value}";
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
                var searchBookingsResponse = await _oneBudgetCrmService.SearchScheduleOnceBookings(criteria);
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
                        Booking_Duration_minutes = (int)bookingDuration,
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
                    var createBookingResponse = await _oneBudgetCrmService.CreateBooking(createBookingRequest);
                    if (createBookingResponse.Code != ResultCode.OK)
                    {
                        apiResult.Message = OneBudgetConstants.SSB_CreateBooking_400;
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
                        Booking_Duration_minutes = (int)bookingDuration,
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
                    var updateBookingResponse = await _oneBudgetCrmService.UpdateBooking(soBookingId, updateBookingRequest);
                    if (updateBookingResponse.Code != ResultCode.OK)
                    {
                        apiResult.Message = OneBudgetConstants.SSB_UpdateBooking_400;
                        return apiResult;
                    }
                }
                else
                {
                    apiResult.Message = OneBudgetConstants.SSB_SearchBooking_400;
                    return apiResult;
                }

                apiResult.Code = ResultCode.OK;
                apiResult.Message = OneBudgetConstants.SBI_200;
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
