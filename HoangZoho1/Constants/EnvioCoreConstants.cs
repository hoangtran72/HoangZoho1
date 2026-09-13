using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HoangZoho1.Constants
{

    public class EnvioCoreConstants
    {

        public const string EnvioCore = "EnvioCore";

        #region Zoho Auth

        public const string ZohoAuthEndpoint = "https://accounts.zoho.com/oauth/v2/token";

        public static string ZohoClientId => EnvironmentConstants.Get("ENVIOCORE_ZOHO_CLIENT_ID");

        public static string ZohoClientSecret => EnvironmentConstants.Get("ENVIOCORE_ZOHO_CLIENT_SECRET");

        #endregion

        #region Zoho CRM

        public const string ZohoCRM_EndpointV2 = "https://www.zohoapis.com/crm/v2";

        public const string ZohoCRM_EndpointV3 = "https://www.zohoapis.com/crm/v3";

        public static string ZohoCRM_RefreshToken => EnvironmentConstants.Get("ENVIOCORE_ZOHO_CRM_REFRESH_TOKEN");

        #endregion

        #region Zoho FSM

        public static string ZohoFSM_RefreshToken => EnvironmentConstants.Get("ENVIOCORE_ZOHO_FSM_REFRESH_TOKEN");

        #endregion

    }

}
