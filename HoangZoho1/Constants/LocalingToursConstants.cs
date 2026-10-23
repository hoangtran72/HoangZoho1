using System;

namespace HoangZoho1.Constants
{

    public class LocalingToursConstants
    {

        #region Xero

        public static string Xero_WebhookKey => EnvironmentConstants.Get("LOCALINGTOURS_XERO_WEBHOOK_KEY");

        public const string Xero_TenantId = "86777fd0-2260-4128-909c-ebca82743aea";

        public static string Xero_Sync2Zoho_StandaloneFunction_Url => EnvironmentConstants.Get("LOCALINGTOURS_XERO_SYNC2ZOHO_STANDALONE_FUNCTION_URL");

        #endregion

        #region Zoho

        public static string Zoho_ClientId =>
            EnvironmentConstants.Get("LOCALINGTOURS_ZOHO_CLIENT_ID");

        public static string Zoho_ClientSecret =>
            EnvironmentConstants.Get("LOCALINGTOURS_ZOHO_CLIENT_SECRET");

        public static string Zoho_AuthEndpoint = "https://accounts.zoho.com/oauth/v2/token";

        #region Zoho CRM

        public static string ZohoCrm_RefreshToken =>
            EnvironmentConstants.Get("LOCALINGTOURS_ZOHO_CRM_REFRESH_TOKEN");

        #endregion

        #region Zoho WorkDrive

        public static string ZohoWorkDrive_RefreshToken =>
            EnvironmentConstants.Get("LOCALINGTOURS_ZOHO_WORKDRIVE_REFRESH_TOKEN");

        #endregion

        #endregion

    }

}
