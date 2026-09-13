using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HoangZoho1.Constants
{
    public class REOConstants
    {

        public const string RestaurantEquipmentOnline = "Restaurant Equipment Online";

        public const string REO_SalesEmail = "sales@restaurantequipment.com.au";

        #region Blueprint

        public const string Blueprint_ConnectWithLead_TransitionId = "4221896000020534220";

        #endregion

        #region Shopify

        public static string ShopifyToken => EnvironmentConstants.Get("REO_SHOPIFY_TOKEN");

        public const string ShopifyEndpoint = "https://laravel.restaurantequipment.com.au/api";

        #endregion

        #region TNZ

        public const string TNZ_EndpointV203 = "https://api.tnz.co.nz/api/v2.03";

        public static string REO_Token => EnvironmentConstants.Get("REO_REO_TOKEN");

        public static string Maria_Token => EnvironmentConstants.Get("REO_MARIA_TOKEN");

        public static string Julie_Token => EnvironmentConstants.Get("REO_JULIE_TOKEN");

        #endregion

        #region Zoho Auth 

        public const string ZohoAuthEndpoint = "https://accounts.zoho.com/oauth/v2/token";

        public static string ZohoClientId => EnvironmentConstants.Get("REO_ZOHO_CLIENT_ID");

        public static string ZohoClientSecret => EnvironmentConstants.Get("REO_ZOHO_CLIENT_SECRET");

        public static string ZohoCRM_RefreshToken => EnvironmentConstants.Get("REO_ZOHO_CRM_REFRESH_TOKEN");

        public static string ZohoDesk_RefreshToken => EnvironmentConstants.Get("REO_ZOHO_DESK_REFRESH_TOKEN");

        public static string ZohoAnalytics_RefreshToken => EnvironmentConstants.Get("REO_ZOHO_ANALYTICS_REFRESH_TOKEN");

        #endregion

        #region Zoho CRM

        public const string ZohoCRM_EndpointV2 = "https://www.zohoapis.com/crm/v2";

        public const string ZohoCRM_EndpointV6 = "https://www.zohoapis.com/crm/v6";

        public const string MattCrmId = "4221896000000238013";

        public const string ReoCrmId = "4221896000013094001";

        public const string MariaCrmId = "4221896000014665001";

        public const string LeadConnectedTransitionId = "4221896000309391568";

        public const string ProductPrefixURL = "https://crm.zoho.com/crm/org697671743/tab/Products";

        #endregion

        #region Google Cloud

        public static string Google_ClientId => EnvironmentConstants.Get("REO_GOOGLE_CLIENT_ID");

        public static string Google_ClientSecret => EnvironmentConstants.Get("REO_GOOGLE_CLIENT_SECRET");

        public static string Google_RefreshToken => EnvironmentConstants.Get("REO_GOOGLE_REFRESH_TOKEN");

        public const string Google_RefreshGrantType = "refresh_token";

        #endregion

        #region JustCall

        public static string JustCall_ApiKey => EnvironmentConstants.Get("REO_JUST_CALL_API_KEY");

        public static string JustCall_ApiSecret => EnvironmentConstants.Get("REO_JUST_CALL_API_SECRET");

        public const string JustCall_Signature = "54f9288e52a209710b4cb2507283502881a51aa3";

        public const string JustCall_GetUserById_Endpoint = "https://api.justcall.io/v1/users/get";

        public const string JustCall_GetCallById_Endpoint = "https://api.justcall.io/v1/calls/get";

        public const string JustCall_GetListOfSMS_Endpoint = "https://api.justcall.io/v1/texts/list";

        public const string JustCall_GetListOfCalls_Endpoint = "https://api.justcall.io/v1/calls/list";

        // [SCZ] Sync Call to Zoho CRM

        public const string SCZ_200 = "Sync Call from JustCall to Zoho CRM SUCCESSFULLY!";

        public const string SCZ_400 = "Sync Call from JustCall to Zoho CRM FAILED!";

        // [MSCZ] Mass Sync Calls to Zoho CRM

        public const string MSCZ_200 = "Mass Sync Calls from JustCall to Zoho CRM SUCCESSFULLY!";

        public const string MSCZ_400 = "Mass Sync Calls from JustCall to Zoho CRM FAILED!";

        // [SSZ] Sync SMS to Zoho CRM

        public const string SSZ_200 = "Sync SMS from JustCall to Zoho CRM SUCCESSFULLY!";

        public const string SSZ_400 = "Sync SMS from JustCall to Zoho CRM FAILED!";

        // [SSZ] Sync SMS to Zoho CRM

        public const string MSSZ_200 = "Mass sync SMS from JustCall to Zoho CRM SUCCESSFULLY!";

        public const string MSSZ_400 = "Mass sync SMS from JustCall to Zoho CRM FAILED!";

        #endregion

        #region CUSTOM FUNCTIONS

        // GSEP: Handle Sales Email

        public const string CUSTOM_GSEP_200 = "Get Sales Emails and Process SUCCESSFULLY!";

        public const string CUSTOM_GSEP_400 = "Get Sales Emails and Process FAILED!";

        public const string CUSTOM_GSEP_SE1 = "[SE1] Search Emails FAILED!";

        public const string CUSTOM_GSEP_SE2 = "[SE2] There are NO EMAILS IN THE RESPONSE!";

        // SSZ: Sync JustCall SMS to Zoho CRM

        public const string CUSTOM_SSZ_200 = "Sync JustCall SMS to Zoho CRM SUCCESSFULLY!";

        public const string CUSTOM_SSZ_400 = "Sync JustCall SMS to Zoho CRM FALED!";

        public const string CUSTOM_SSZ_E01 = "Just Call Log for the SMS already exist!";

        // MSZ: Mass Sync JustCall SMS to Zoho CRM

        public const string CUSTOM_MSZ_200 = "Mass Sync JustCall SMS to Zoho CRM SUCCESSFULLY!";

        public const string CUSTOM_MSZ_400 = "Mass Sync JustCall SMS to Zoho CRM FALED!";

        // SCZ: Sync JustCall Call to Zoho CRM

        public const string CUSTOM_SCZ_200 = "Sync JustCall Call to Zoho CRM SUCCESSFULLY!";

        public const string CUSTOM_SCZ_400 = "Sync JustCall SMS to Zoho CRM FALED!";

        public const string CUSTOM_SCZ_E01 = "Just Call Log for the Call already exist!";

        // MCZ: Mass Sync JustCall Call to Zoho CRM

        public const string CUSTOM_MCZ_200 = "Mass Sync JustCall Calls to Zoho CRM SUCCESSFULLY!";

        public const string CUSTOM_MCZ_400 = "Mass Sync JustCall Calls to Zoho CRM FALED!";

        // HTW: Handle TNZ Webhook

        public const string CUSTOM_HTW_200 = "Handle TNZ Webhook SUCCESSFULLY!";

        public const string CUSTOM_HTW_400 = "Handle TNZ Webhook FALED!";

        // GSQ: Get Shopify Quote Details

        public const string CUSTOM_GSQ_200 = "Get Shopify Quote Details SUCCESSFULLY!";

        public const string CUSTOM_GSQ_400 = "Get Shopify Quote Details FALED!";

        // STS: Send TNZ SMS

        public const string STS_200 = "Send TNZ SMS SUCCESSFULLY";

        public const string STS_400 = "Send TNZ SMS FAILED";

        public const string CTZ_400 = "Create TNZ Log FAILED";

        // GTAN: Get Timeline and Notes

        public const string GTAN_200 = "Get Timeline and Note SUCCESSFULLY";

        public const string GTAN_400 = "Get Timeline and Note FAILED";

        public const string GTAN_E01 = "Cannot get Record Notes";

        // GPDT: Get Product Data Table

        public const string GPDT_200 = "Get Product Data Table SUCCESSFULLY";

        public const string GPDT_400 = "Get Product Data Table FAILED";

        // EAQ: Easy Add Quote

        public const string EAQ_200 = "Easy Add Quote SUCCESSFULLY";

        public const string EAQ_400 = "Easy Add Quote FAILED";

        // GQDT: Get Quote Data Table

        public const string GQDT_200 = "Get Quote Data Table SUCCESSFULLY";

        public const string GQDT_400 = "Get Quote Data Table FAILED";

        // GSDT: Get Suggested Data Table

        public const string GSDT_200 = "Get Suggested Data Table SUCCESSFULLY";

        public const string GSDT_400 = "Get Suggested Data Table FAILED";


        // EUQ: Easy Update Quote

        public const string EUQ_200 = "Easy Update Quote SUCCESSFULLY";

        public const string EUQ_400 = "Easy Update Quote FAILED";

        public const string EUQ_S01 = "Easy Update Quote - No Product Changed";

        // BCWL: Blueprint Connect With Lead

        public const string BCWL_200 = "Connect With Lead SUCCESSFULLY";

        public const string BCWL_400 = "Connect With Lead FAILED";

        #endregion

    }
}
