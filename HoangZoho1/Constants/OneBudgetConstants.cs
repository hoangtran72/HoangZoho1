using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HoangZoho1.Constants
{

    public class OneBudgetConstants
    {

        public const string OneBudget = "OneBudget";

        #region Zoho Auth

        public const string ZohoAuthEndpoint = "https://accounts.zoho.com.au/oauth/v2/token";

        public static string ZohoClientId => EnvironmentConstants.Get("ONE_BUDGET_ZOHO_CLIENT_ID");

        public static string ZohoClientSecret => EnvironmentConstants.Get("ONE_BUDGET_ZOHO_CLIENT_SECRET");

        #endregion

        #region Zoho CRM

        public const string ZohoCRM_EndpointV2 = "https://www.zohoapis.com.au/crm/v2";

        public const string ZohoCRM_EndpointV3 = "https://www.zohoapis.com.au/crm/v3";

        public const string ZohoCRM_EndpointV5 = "https://www.zohoapis.com.au/crm/v5";

        public static string ZohoCRM_RefreshToken => EnvironmentConstants.Get("ONE_BUDGET_ZOHO_CRM_REFRESH_TOKEN");

        public const string ZohoCRM_JoshUserId = "68045000000264001";

        #endregion

        #region ScheduleOnce Bookings

        public static List<string> OneCorp_Bookings = new List<string>() { 
            "OneBudget - Discovery Call", "OneBudget - Discovery Session", 
            "OneBudget - Demo Call", "OneBudget - Demo Session" };

        #endregion

        #region Blueprint

        public const string ZohoCRM_ClientCancelled_TransitionId = "68045000001312515";

        public const string ZohoCRM_StrategistCancelled_TransitionId = "68045000001312517";

        #endregion

        #region Custom Functions

        // SSB: Sync ScheduleOnce Booking from Webhook

        public const string SSB_200 = "Sync ScheduleOnce Booking with Zoho CRM SUCCESSFULLY!";

        public const string SSB_400 = "Sync ScheduleOnce Booking with Zoho CRM FAILED!";

        public const string SSB_SearchBooking_400 = "Search Booking in Zoho CRM FAILED!";

        public const string SSB_CreateBooking_400 = "Create Booking in Zoho CRM FAILED!";

        public const string SSB_UpdateBooking_400 = "Update Booking in Zoho CRM FAILED!";

        public const string SSB_UpdateBookingTransition_400 = "Update Booking Transition in Zoho CRM FAILED!";

        public const string SSB_StatusCompleted = "No need to update if booking status is Completed!";

        public const string SSB_NotOneBudget = "No need to update if booking status is Completed!";

        // SBI: Sync ScheduleOnce Booking by Id

        public const string SBI_200 = "Sync ScheduleOnce Booking by Id SUCCESSFULLY";

        public const string SBI_400 = "Sync ScheduleOnce Booking by Id FAILED";

        public const string SBI_SearchBooking_400 = "Search Booking in Zoho CRM FAILED!";

        public const string SBI_CreateBooking_400 = "Create Booking in Zoho CRM FAILED!";

        public const string SBI_UpdateBooking_400 = "Update Booking in Zoho CRM FAILED!";

        public const string SBI_StatusCompleted = "No need to update if booking status is Completed!";

        #endregion

    }

}
