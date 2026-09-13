using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HoangZoho1.Constants
{

    public class SpacificConstants
    {

        public const string Spacific = "Spacific";

        #region Zoho Auth

        public const string ZohoAuthEndpoint = "https://accounts.zoho.com/oauth/v2/token";

        public static string ZohoClientId => EnvironmentConstants.Get("SPACIFIC_ZOHO_CLIENT_ID");

        public static string ZohoClientSecret => EnvironmentConstants.Get("SPACIFIC_ZOHO_CLIENT_SECRET");

        #endregion

        #region Zoho Projects

        public static string ZohoProjects_RefreshToken => EnvironmentConstants.Get("SPACIFIC_ZOHO_PROJECTS_REFRESH_TOKEN");

        #endregion

    }

}
