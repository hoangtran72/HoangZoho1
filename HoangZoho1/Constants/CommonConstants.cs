using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HoangZoho1.Constants
{
    public class CommonConstants
    {

        #region Date Time

        public const int SecondsInMinute = 60;

        public const int SecondsInHour = 3600; // 60 * 60

        public const int SecondsInDay = 86400; // 24 * 3600

        public const int SecondsInMonth = 2592000; // Approximate using 30 days per month

        public const string NoImageURL = "https://drive.google.com/file/d/1Iz_u-hsFXdbS8Cch-1vSSqXxQMOeKSYj/view?usp=sharing";

        #endregion

        #region Status Code

        public const string MSG_200 = "Success";

        public const string MSG_204 = "No Content";

        public const string MSG_400 = "Bad Request";

        public const string MSG_401 = "Unauthorized";

        public const string MSG_404 = "Not Found";

        #endregion

        #region Zoho

        public const string ZohoCRM = "ZohoCRM";

        public const string ZohoMail = "ZohoMail";

        public const string ZohoBooks = "ZohoBooks";

        public const string ZohoInventory = "ZohoInventory";

        public const string ZohoSign = "ZohoSign";

        public const string ZohoPeople = "ZohoPeople";

        public const string ZohoWorkDrive = "ZohoWorkDrive";

        public const string ZohoProjects = "ZohoProjects";

        public const string ZohoDesk = "ZohoDesk";

        public const string ZohoFSM = "ZohoFSM";

        public const string ZohoRecruit = "ZohoRecruit";

        public const string ZohoAnalytics = "ZohoAnalytics";

        public const string ZohoApproval = "approval";

        public const string ZohoWorkflow = "workflow";

        public const string ZohoBlueprint = "blueprint";

        public const string ZohoDateFormat = "yyyy-MM-dd";

        public const string ZohoDateTimeFormat = "yyyy-MM-ddTHH:mm:ss";

        public const int MultilineLength = 32000;

        #endregion

        #region Xero

        public const string Xero_AuthEndpoint = "https://identity.xero.com/connect/token";

        #endregion

        #region Mail Platform

        public const string HoangTestGmail = "hoagsun@gmail.com";

        public static string HoangTestPassword => EnvironmentConstants.Get("COMMON_HOANG_TEST_PASSWORD");

        public const string Gmail_SmtpServer = "smtp.gmail.com";

        public const int SmtpPort = 587;

        #endregion

        #region Custom Functions

        // CTS: Convert TimeStamp

        public const string CTS_200 = "Convert TimeStamp SUCCESSFULLY";

        public const string CTS_400 = "Convert TimeStamp FAILED";

        // JWD: Jaro-Winkler Distance

        public const string JWD_200 = "Calculate Jaro-Winkler Distance SUCCESSFULLY";

        public const string JWD_400 = "Calculate Jaro-Winkler Distance FAILED";

        // GZT: Get Zoho Token

        public const string GZT_200 = "Get Zoho Token SUCCESSFULLY";

        public const string GZT_400 = "Get Zoho Token FAILED";

        // SW: Scrape Website

        public const string SW_200 = "Scrape Website SUCCESSFULLY";

        public const string SW_400 = "Scrape Website FAILED";

        // CH2T: Convert HTML to Text

        public const string CH2T_200 = "Convert HTML to Text SUCCESSFULLY";

        public const string CH2T_400 = "Convert HTML to Text FAILED";

        // GCT: Get Current Time

        public const string GCT_200 = "Get Current Time SUCCESSFULLY";

        public const string GCT_400 = "Get Current Time FAILED";

        // GMD: Get Middle Date

        public const string GMD_200 = "Get Middle Date SUCCESSFULLY";

        public const string GMD_400 = "Get Middle Date FAILED";

        // GUO: Get UTC Offset

        public const string GUO_200 = "Get UTC Offset SUCCESSFULLY";

        public const string GUO_400 = "Get UTC Offset FAILED";

        // CS2T: Convert Seconds to Text

        public const string CS2T_200 = "Convert Seconds to Text SUCCESSFULLY";

        public const string CS2T_400 = "Convert Seconds to Text FAILED";

        // CVU: Check Valid URL

        public const string CVU_200 = "Check Valid URL SUCCESSFULLY";

        public const string CVU_400 = "Check Valid URL FAILED";

        // EJS: Extract JSON Data

        public const string EJS_200 = "Extract JSON Data SUCCESSFULLY";

        public const string EJS_400 = "Extract JSON Data FAILED";

        // CPT: Clean PHP Text

        public const string CPT_200 = "Clean PHP Text SUCCESSFULLY";

        public const string CPT_400 = "Clean PHP Text FAILED";

        // FCPC: Format Canadian Postal Code

        public const string FCPC_200 = "Format Canadian Postal Code SUCCESSFULLY";

        public const string FCPC_400 = "Format Canadian Postal Code FAILED";

        // GFLM: Get First and Last Day of Month

        public const string GFLM_200 = "Get First and Last Day of Month SUCCESSFULLY";

        public const string GFLM_400 = "Get First and Last Day of Month FAILED";

        // CL2P: Convert Label to PDF

        public const string CL2P_200 = "Convert Label to PDF SUCCESSFULLY";

        public const string CL2P_400 = "Convert Label to PDF FAILED";

        // DFFU: Download File from URL

        public const string DFFU_200 = "Download File from URL SUCCESSFULLY";

        public const string DFFU_400 = "Download File from URL FAILED";

        // EML: Extract Meeting Link

        public const string EML_200 = "Extract Meeting Link SUCCESSFULLY";

        public const string EML_400 = "Extract Meeting Link FAILED";

        // AZT: Adjust Zoho Time

        public const string AZT_200 = "Adjust Zoho Time SUCCESSFULLY";

        public const string AZT_400 = "Adjust Zoho Time FAILED";

        // CZ2U: Convert Zoho Time to Text

        public const string CZ2T_200 = "Convert Zoho Time to Text SUCCESSFULLY";

        public const string CZ2T_400 = "Convert Zoho Time to Text FAILED";

        // CZ2U: Convert Zoho Time to Unix Epoch

        public const string CZ2U_200 = "Convert Zoho Time to Unix Epoch SUCCESSFULLY";

        public const string CZ2U_400 = "Convert Zoho Time to Unix Epoch FAILED";

        // GPT: Get Page Title

        public const string GPT_200 = "Get Page Title SUCCESSFULLY";

        public const string GPT_400 = "Get Page Title FAILED";

        // CU: Clean URL

        public const string CU_200 = "Clean URL SUCCESSFULLY";

        public const string CU_400 = "Clean URL FAILED";

        #endregion

    }
}
