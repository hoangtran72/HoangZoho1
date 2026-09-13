namespace HoangZoho1.Constants
{
    public class GGInsuranceConstants
    {

        public const string GGInsurance = "GG-Insurance";

        #region Zoho Auth

        public const string ZohoAuthEndpoint = "https://accounts.zoho.com/oauth/v2/token";

        public static string ZohoClientId => EnvironmentConstants.Get("GGINSURANCE_ZOHO_CLIENT_ID");

        public static string ZohoClientSecret => EnvironmentConstants.Get("GGINSURANCE_ZOHO_CLIENT_SECRET");

        #endregion

        #region Zoho CRM

        public const string ZohoCRM_EndpointV8 = "https://www.zohoapis.com/crm/v8";

        public static string ZohoCRM_RefreshToken => EnvironmentConstants.Get("GGINSURANCE_ZOHO_CRM_REFRESH_TOKEN");

        public const string ZohoCRM_Search_URL =
            "https://crm.zoho.com/crm/org702898229/search?searchword=$PhoneUrlEncoded$&isRelevance=false";

        public const string ZohoCRM_AccountPrefix_URL = "https://crm.zoho.com/crm/org702898229/tab/Accounts";

        public const string ZohoCRM_ContactPrefix_URL = "https://crm.zoho.com/crm/org702898229/tab/Contacts";

        #endregion

        #region Custom Functions

        // GZCRU4P: Get Zoho CRM Redirect URL for Phone 

        public const string GZCRU4P_200 = "Get Zoho CRM Redirect URL for Phone SUCCESSFULLY";

        public const string GZCRU4P_400 = "Get Zoho CRM Redirect URL for Phone FAILED";

        #endregion

    }
}
