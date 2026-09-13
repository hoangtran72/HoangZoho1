using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HoangZoho1.Constants
{

    public class GoogleAPIConstants
    {

        public static string TokenEndpoint => EnvironmentConstants.Get("GOOGLE_API_TOKEN_ENDPOINT");

        public const string GmailEndpoint = "https://www.googleapis.com/gmail/v1";

        public static string GrantType_RefreshToken => EnvironmentConstants.Get("GOOGLE_API_GRANT_TYPE_REFRESH_TOKEN");

        #region Google API List

        public const string SearchEmailMessages_200 = "Search Emails Successfully";

        public const string SearchEmailMessages_400 = "Search Emails FAILED!";

        #endregion
    }

}
