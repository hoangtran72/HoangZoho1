using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HoangZoho1.Constants
{
    public class DiamondMindsConstants
    {

        public const string DiamondMinds = "DiamondMinds";

        #region Xero

        // Demo Company
        public static string Demo_ClientId => EnvironmentConstants.Get("DIAMOND_MINDS_DEMO_CLIENT_ID");

        public static string Demo_ClientSecret => EnvironmentConstants.Get("DIAMOND_MINDS_DEMO_CLIENT_SECRET");

        public static string Demo_RefreshToken => EnvironmentConstants.Get("DIAMOND_MINDS_DEMO_REFRESH_TOKEN");

        public const string Demo_TenantId = "e2608c53-1832-4e58-b981-b68fe5f04c5f";

        public static string Fisher_RefreshToken => EnvironmentConstants.Get("DIAMOND_MINDS_FISHER_REFRESH_TOKEN");

        public const string Fisher_TenantId = "d9c7412c-d96f-4b24-b65e-feee696c2d1b";

        #endregion
    
    }
}
