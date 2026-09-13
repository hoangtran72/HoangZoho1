using Microsoft.AspNetCore.Http;

namespace HoangZoho1.Constants
{

    public class DaviesImagingGroupConstants
    {

        #region Zoho CRM Standalone Function

        public static string FrameFlowSendSpecPlusEmailStandalone_Endpoint => EnvironmentConstants.Get("DAVIES_IMAGING_GROUP_FRAME_FLOW_SEND_SPEC_PLUS_EMAIL_STANDALONE_ENDPOINT");

        #endregion

        #region Custom Functions

        public const string SendSpecPlusEmail_200 = "Send Spec Plus Email SUCCESSFULLY";

        public const string SendSpecPlusEmail_400 = "Send Spec Plus Email FAILED";

        #endregion

    }

}
