using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HoangZoho1.Constants
{
    public class SignaramaConstants
    {

        public const string Signarama = "Signarama";

        #region Zoho Auth

        public const string ZohoAuthEndpoint = "https://accounts.zoho.com/oauth/v2/token";

        public static string ZohoClientId => EnvironmentConstants.Get("SIGNARAMA_ZOHO_CLIENT_ID");

        public static string ZohoClientSecret => EnvironmentConstants.Get("SIGNARAMA_ZOHO_CLIENT_SECRET");

        #endregion

        #region Zoho WorkDrive

        public static string ZohoWorkDrive_RefreshToken => EnvironmentConstants.Get("SIGNARAMA_ZOHO_WORK_DRIVE_REFRESH_TOKEN");

        #endregion

        #region Zoho Books

        public const string ZohoBooks_OrganizationId = "785175882";

        public static string ZohoBooks_RefreshToken => EnvironmentConstants.Get("SIGNARAMA_ZOHO_BOOKS_REFRESH_TOKEN");

        #endregion

        #region Gemini

        public const string GeminiEndpoint = "https://generativelanguage.googleapis.com";

        public static string GeminiApiKey => EnvironmentConstants.Get("SIGNARAMA_GEMINI_API_KEY");

        #endregion

        #region Custom Functions

        // HUE Handle Uploaded Estimate

        public const string HUE_ExtractEstimateContent = "Please retrieve the Estimate_Number, Created_Date, Subtotal and Taxes from the attached Estimate and return in JSON format without ```json and ```. Please note that the value for Subtotal and Tax should be in number format, the format of Date is MM/dd/yyyy, should be convert to yyyy-MM-dd.";

        public const string HUE_200 = "Handle Uploaded Estimate SUCCESSFULLY";

        public const string HUE_400 = "Handle Uploaded Estimate FAILED";

        public const string HUE_E01 = "Download File FAILED";

        public const string HUE_E02 = "Upload File to Gemini FAILED";

        // UAB Upload Attachment in Books

        public const string UAB_200 = "Upload Attachment SUCCESSFULLY";

        public const string UAB_400 = "Upload Attachment FAILED";

        public const string UAB_E01 = "Download WorkDrive File FAILED";

        public const string UAB_E02 = "Upload File to Record in Books FAILED";

        #endregion

    }
}
