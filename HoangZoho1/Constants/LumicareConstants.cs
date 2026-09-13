using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HoangZoho1.Constants
{
    public class LumicareConstants
    {

        public const string Lumicare = "Lumicare";

        #region Talent LMS

        public const string TalentLMSEndpoint = "https://lumicare.talentlms.com/api/v1";

        public static string TalentLMSAPIKey => EnvironmentConstants.Get("LUMICARE_TALENT_LMSAPIKEY");

        public static string TalentLMSPassword => EnvironmentConstants.Get("LUMICARE_TALENT_LMSPASSWORD");

        public const int OnboardingCourse = 127;

        #endregion

        #region Zoho Auth

        public const string ZohoAuthEndpoint = "https://accounts.zoho.com.au/oauth/v2/token";

        public static string ZohoClientId => EnvironmentConstants.Get("LUMICARE_ZOHO_CLIENT_ID");

        public static string ZohoClientSecret => EnvironmentConstants.Get("LUMICARE_ZOHO_CLIENT_SECRET");

        public static string ZohoCRM_RefreshToken => EnvironmentConstants.Get("LUMICARE_ZOHO_CRM_REFRESH_TOKEN");

        public static string ZohoWorkdrive_RefreshToken => EnvironmentConstants.Get("LUMICARE_ZOHO_WORKDRIVE_REFRESH_TOKEN");

        #endregion

        #region Zoho CRM

        public const string ZohoCRM_EndpointV2 = "https://www.zohoapis.com.au/crm/v2";

        public const string ZohoCRM_EndpointV3 = "https://www.zohoapis.com.au/crm/v3";

        #endregion

        #region Zoho Workdrive

        public const string CreateCustomLinkRequestBody = "{data:{attributes:{resource_id: \"$FileId$\",shared_type: \"publish\",role_id: \"34\"},type: \"permissions\"}}";

        #endregion

        #region Custom Functions

        // HTF: Handle Training Form

        public const string CUSTOM_HTF_200 = "Handle Training Form SUCCESSFULLY!";

        public const string CUSTOM_HTF_400 = "Handle Training Form FAILED!";

        public const string CUSTOM_HTF_C01 = "Create Contact FAILED";

        public const string CUSTOM_HTF_C02 = "Update Contact FAILED";

        public const string CUSTOM_HTF_L01 = "Create TalentLMS User FAILED";

        public const string CUSTOM_HTF_L02 = "Add User to Course FAILED";

        // ACE: Approve Client to Enroll

        public const string CUSTOM_ACE_200 = "Approve Client to Enroll SUCCESSFULLY!";

        public const string CUSTOM_ACE_400 = "Approve Client to Enroll FAILED!";

        public const string CUSTOM_ACE_C01 = "Get Contact by Id FAILED!";

        public const string CUSTOM_ACE_C02 = "Contact Email is EMPTY!";

        public const string CUSTOM_ACE_C03 = "Update Contact FAILED!";

        // CDC: Check Distributor Credential

        public const string DistributorFormUrl = "https://zfrmz.com.au/FmnGS5NTCoHNxJqN9HSY";

        public const string CUSTOM_CDC_200 = "Check Distributor Credential SUCCESSFULLY!";

        public const string CUSTOM_CDC_400 = "Check Distributor Credential FAILED!";

        public const string CUSTOM_CDC_D01 = "There is no Distributor with this username and password!";

        // GLN: Get Latest News

        public const string CUSTOM_GLN_200 = "Get Latest News SUCCESSFULLY!";

        public const string CUSTOM_GLN_400 = "Get Latest News FAILED!";

        // GWT: Get Workdrive / Writer Token

        public const string CUSTOM_GWT_200 = "Get Workdrive / Writer Token SUCCESSFULLY!";

        public const string CUSTOM_GWT_400 = "Get Workdrive / Writer Token FAILED!";

        #endregion

    }
}
