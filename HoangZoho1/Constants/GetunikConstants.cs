using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HoangZoho1.Constants
{
    
    public class GetunikConstants
    {

        public const string GetUnik = "getUnik";

        #region Zoho Auth

        public const string ZohoAuthEndpoint = "https://accounts.zoho.com/oauth/v2/token";

        public static string ZohoClientId => EnvironmentConstants.Get("GETUNIK_ZOHO_CLIENT_ID");

        public static string ZohoClientSecret => EnvironmentConstants.Get("GETUNIK_ZOHO_CLIENT_SECRET");

        #endregion

        #region Zoho CRM

        public const string ZohoCRM_EndpointV2 = "https://www.zohoapis.com/crm/v2";

        public const string ZohoCRM_EndpointV3 = "https://www.zohoapis.com/crm/v3";

        public const string ZohoCRM_EndpointV5 = "https://www.zohoapis.com/crm/v5";

        public const string ZohoCRM_EndpointV6 = "https://www.zohoapis.com/crm/v6";

        public static string ZohoCRM_RefreshToken => EnvironmentConstants.Get("GETUNIK_ZOHO_CRM_REFRESH_TOKEN");

        #endregion

        #region Zoho Books

        public const string ZohoBooks_EndpointV3 = "https://www.zohoapis.com/books/v3";

        public const string ZohoBooks_LiveOrganizationId = "728859286";

        public static string ZohoBooks_RefreshToken => EnvironmentConstants.Get("GETUNIK_ZOHO_BOOKS_REFRESH_TOKEN");

        #endregion

        #region Zoho Projects

        public static string ZohoProjects_RefreshToken => EnvironmentConstants.Get("GETUNIK_ZOHO_PROJECTS_REFRESH_TOKEN");

        public const string ZohoProjects_PortalId = "657511602";

        public const string ZohoProjects_APIEndpoint_V3 = "https://projectsapi.zoho.com/api/v3";

        #endregion

        #region Custom Functions

        // HCIAS: Handle Create Invoice and Submit 

        public const string HCIAS_200 = "Handle Create Invoice and Submit SUCCESSFULLY";

        public const string HCIAS_400 = "Handle Create Invoice and Submit FAILED";

        public const string HCIAS_E01 = "[HCIAS_E01] Cannot get Temporary Record Details";
        
        public const string HCIAS_E02 = "[HCIAS_E02] Cannot create Invoice using the payload";

        public const string HCIAS_E03 = "[HCIAS_E03] Cannot submit Invoice using the payload";

        // GPDFU: Get Project Details from URL

        public const string GPDFU_200 = "Get Project Details from URL SUCCESSFULLY";

        public const string GPDFU_400 = "Get Project Details from URL FAILED";

        public const string GPDFU_E01 = "[GPDFU_E01] Cannot get Project Id from URL";

        public const string GPDFU_E02 = "[GPDFU_E02] Cannot get Project Details from Project Id";

        // CBT: Create Backlog Tasks

        public const string CBT_200 = "Create Backlog Tasks SUCCESSFULLY";

        public const string CBT_400 = "Create Backlog Tasks FAILED";

        public const string CBT_E01 = "Get Project Tasklists FAILED";

        public const string CBT_E02 = "Cannot find Tasklist Backlog in Project!";

        public const string CBT_E03 = "Get Project Details by Id FAILED";

        public const string CBT_E04 = "Create Main Task $TaskName$ FAILED";

        public const string CBT_E05 = "Create Sub Task $SubTaskName$ for Main Task $TaskName$ FAILED";

        #endregion

    }

}
