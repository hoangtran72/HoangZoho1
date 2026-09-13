namespace HoangZoho1.Constants
{

    public class DoAbilityConstants
    {

        #region Google Ads

        public static string GoogleAds_DeveloperToken => EnvironmentConstants.Get("DO_ABILITY_GOOGLE_ADS_DEVELOPER_TOKEN");

        public static string GoogleAds_ClientId => EnvironmentConstants.Get("DO_ABILITY_GOOGLE_ADS_CLIENT_ID");

        public static string GoogleAds_ClientSecret => EnvironmentConstants.Get("DO_ABILITY_GOOGLE_ADS_CLIENT_SECRET");

        public static string GoogleAds_RefreshToken => EnvironmentConstants.Get("DO_ABILITY_GOOGLE_ADS_REFRESH_TOKEN");

        public const string GoogleAds_Login_CustomerId = "7834779632";

        #endregion

        #region Custom Functions

        // AU2CMUL: Add User to Customer Match User List

        public const string AU2CMUL_200 = "Add User to Customer Match User List SUCCESSFULLY";

        public const string AU2CMUL_400 = "Add User to Customer Match User List FAILED";

        // RUFCMUL: Remove User from Customer Match User List

        public const string RUFCMUL_200 = "Remove User from Customer Match User List SUCCESSFULLY";

        public const string RUFCMUL_400 = "Remove User from Customer Match User List FAILED";

        #endregion

    }

}
