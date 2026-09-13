using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HoangZoho1.Constants
{

    public class MarcoInteriorsConstants
    {

        public const string MarcoInteriors = "Marco Interiors";

        #region Zoho Auth

        public const string ZohoAuthEndpoint = "https://accounts.zoho.eu/oauth/v2/token";

        public static string ZohoClientId => EnvironmentConstants.Get("MARCO_INTERIORS_ZOHO_CLIENT_ID");

        public static string ZohoClientSecret => EnvironmentConstants.Get("MARCO_INTERIORS_ZOHO_CLIENT_SECRET");

        #endregion

        #region Zoho CRM

        public const string ZohoCRM_EndpointV2 = "https://www.zohoapis.eu/crm/v2";

        public const string ZohoCRM_EndpointV3 = "https://www.zohoapis.eu/crm/v3";

        public static string ZohoCRM_RefreshToken => EnvironmentConstants.Get("MARCO_INTERIORS_ZOHO_CRM_REFRESH_TOKEN");

        #endregion

        #region Zoho People

        public static string ZohoPeople_RefreshToken => EnvironmentConstants.Get("MARCO_INTERIORS_ZOHO_PEOPLE_REFRESH_TOKEN");

        #endregion

    }

}
