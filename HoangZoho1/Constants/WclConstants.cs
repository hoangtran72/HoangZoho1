using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HoangZoho1.Constants
{

    public class WclConstants
    {

        public const string WCLSolutions = "WCL Solutions";

        #region Zoho Auth

        public const string ZohoAuthEndpoint = "https://accounts.zoho.eu/oauth/v2/token";

        public static string ZohoClientId => EnvironmentConstants.Get("WCL_ZOHO_CLIENT_ID");

        public static string ZohoClientSecret => EnvironmentConstants.Get("WCL_ZOHO_CLIENT_SECRET");

        #endregion

        #region Zoho Inventory

        public const string ZohoInventory_EndpointV1 = "https://www.zohoapis.eu/inventory/v1";

        public static string ZohoInventory_RefreshToken => EnvironmentConstants.Get("WCL_ZOHO_INVENTORY_REFRESH_TOKEN");

        public const string ZohoInventory_OrganizationId = "20086267311";

        #endregion

        #region WooCommerce

        public const string EU_Woo_Endpoint_V3 = "https://flightams.com/wp-json/wc/v3";

        public static string EU_Client_Id => EnvironmentConstants.Get("WCL_EU_CLIENT_ID");

        public static string EU_Client_Secret => EnvironmentConstants.Get("WCL_EU_CLIENT_SECRET");

        public const string UK_Woo_Endpoint_V3 = "https://flightams.com/uk/wp-json/wc/v3";

        public static string UK_Client_Id => EnvironmentConstants.Get("WCL_UK_CLIENT_ID");

        public static string UK_Client_Secret => EnvironmentConstants.Get("WCL_UK_CLIENT_SECRET");

        public const string RW_Woo_Endpoint_V3 = "https://flightams.com/rw/wp-json/wc/v3";

        public static string RW_Client_Id => EnvironmentConstants.Get("WCL_RW_CLIENT_ID");

        public static string RW_Client_Secret => EnvironmentConstants.Get("WCL_RW_CLIENT_SECRET");

        #endregion

        #region Standalone Function

        public static string GetMasterItemsAndBatchesUrl => EnvironmentConstants.Get("WCL_GET_MASTER_ITEMS_AND_BATCHES_URL");

        public static string WooSyncOrderToInventoryUrl => EnvironmentConstants.Get("WCL_WOO_SYNC_ORDER_TO_INVENTORY_URL");

        #endregion

    }

}
