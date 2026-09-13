using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HoangZoho1.Constants
{
    public class GoSunnySolarConstants
    {
        public const string GoSunnySolar = "GoSunnySolar";

        #region Zoho Auth

        public const string ZohoAuthEndpoint = "https://accounts.zoho.com.au/oauth/v2/token";

        public static string ZohoClientId => EnvironmentConstants.Get("GO_SUNNY_SOLAR_ZOHO_CLIENT_ID");

        public static string ZohoClientSecret => EnvironmentConstants.Get("GO_SUNNY_SOLAR_ZOHO_CLIENT_SECRET");

        public static string ZohoCRM_RefreshToken => EnvironmentConstants.Get("GO_SUNNY_SOLAR_ZOHO_CRM_REFRESH_TOKEN");

        #endregion

        #region Podium

        public const string PodiumAuthEndpoint = "https://api.podium.com/oauth/token";

        public const string PodiumApiEndpointV4 = "https://api.podium.com/v4";

        public static string PodiumClientId => EnvironmentConstants.Get("GO_SUNNY_SOLAR_PODIUM_CLIENT_ID");

        public static string PodiumClientSecret => EnvironmentConstants.Get("GO_SUNNY_SOLAR_PODIUM_CLIENT_SECRET");

        public static string PodiumRefreshToken => EnvironmentConstants.Get("GO_SUNNY_SOLAR_PODIUM_REFRESH_TOKEN");

        #endregion

        #region Zoho CRM

        public const string ZohoCRM_Endpoint = "https://www.zohoapis.com.au/crm/v2";

        #endregion


        #region Go High Level

        public const string GhlEndpoint = "https://rest.gohighlevel.com/v1";

        public static string GhlApiKey => EnvironmentConstants.Get("GO_SUNNY_SOLAR_GHL_API_KEY");

        public const string GhlPrefixContactUrl = "https://anytime.gosunny.com.au/v2/location/2kTkPip6aHjtLK1vG4ZK/contacts/detail";

        #endregion
    }
}
