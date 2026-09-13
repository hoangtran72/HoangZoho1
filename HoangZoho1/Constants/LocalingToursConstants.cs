namespace HoangZoho1.Constants
{

    public class LocalingToursConstants
    {

        #region Xero

public static string Xero_WebhookKey => EnvironmentConstants.Get("LOCALINGTOURS_XERO_WEBHOOK_KEY");

        public const string Xero_TenantId = "86777fd0-2260-4128-909c-ebca82743aea";

public static string Xero_Sync2Zoho_StandaloneFunction_Url => EnvironmentConstants.Get("LOCALINGTOURS_XERO_SYNC2ZOHO_STANDALONE_FUNCTION_URL");

        #endregion

    }

}
