using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HoangZoho1.Constants
{
    public class LegendaryFundingConstants
    {

        public const string LegendaryFundingGroup = "Legendary Funding Group";

        #region Zoho Auth

        public const string ZohoAuthEndpoint = "https://accounts.zoho.com/oauth/v2/token";

        public static string ZohoClientId => EnvironmentConstants.Get("LEGENDARY_FUNDING_ZOHO_CLIENT_ID");

        public static string ZohoClientSecret => EnvironmentConstants.Get("LEGENDARY_FUNDING_ZOHO_CLIENT_SECRET");

        #endregion

        #region Zoho Workdrive + Zoho Writer

        public static string ZohoWorkDrive_RefreshToken => EnvironmentConstants.Get("LEGENDARY_FUNDING_ZOHO_WORK_DRIVE_REFRESH_TOKEN");

        #endregion

    }
}
