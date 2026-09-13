using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HoangZoho1.Constants
{
    public class CascadiaWebServicesConstants
    {

        public const string CascadiaWebServices = "Cascadia Web Services";

        #region Zoho Auth

        public const string ZohoAuthEndpoint = "https://accounts.zoho.com/oauth/v2/token";

        public static string ZohoClientId => EnvironmentConstants.Get("CASCADIA_WEB_SERVICES_ZOHO_CLIENT_ID");

        public static string ZohoClientSecret => EnvironmentConstants.Get("CASCADIA_WEB_SERVICES_ZOHO_CLIENT_SECRET");

        #endregion

        #region Zoho Recruit

        public static string ZohoRecruit_RefreshToken => EnvironmentConstants.Get("CASCADIA_WEB_SERVICES_ZOHO_RECRUIT_REFRESH_TOKEN");

        #endregion

    }
}
