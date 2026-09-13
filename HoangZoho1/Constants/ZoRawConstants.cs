namespace HoangZoho1.Constants
{

    public class ZoRawConstants
    {

        public const string ZoRawChocolates = "ZoRaw Chocolates";

        #region Zoho Auth

        public const string ZohoAuthEndpoint = "https://accounts.zohocloud.ca/oauth/v2/token";

        public static string ZohoClientId => EnvironmentConstants.Get("ZO_RAW_ZOHO_CLIENT_ID");

        public static string ZohoClientSecret => EnvironmentConstants.Get("ZO_RAW_ZOHO_CLIENT_SECRET");

        #endregion

        #region Zoho CRM

        public const string ZohoCRM_EndpointV8 = "https://www.zohoapis.ca/crm/v8";

        public static string ZohoCRM_RefreshToken => EnvironmentConstants.Get("ZO_RAW_ZOHO_CRM_REFRESH_TOKEN");

        public const string ZohoCRM_SyncOrderFulfillmentFromSalesOrder_Endpoint = 
            "https://www.zohoapis.ca/crm/v7/functions/inventorysyncorderfulfillmentfromsalesorderstandal/actions/execute?auth_type=apikey&zapikey=1003.91a99e5affb740d0777bfa3e26d0abe3.8441a7e4909fe072b79f813751cdbcfb";

        #endregion

        #region Zoho Inventory

        public const string ZohoInventory_EndpointV1 = "https://www.zohoapis.ca/inventory/v1";

        public static string ZohoInventory_RefreshToken => EnvironmentConstants.Get("ZO_RAW_ZOHO_INVENTORY_REFRESH_TOKEN");

        public const string ZohoInventory_OrganizationId = "110001318978";

        public const string FreightcomShipmentId_CustomFieldId = "63258000000165100";

        public const string StallionShipmentId_CustomFieldId = "63258000003364913";

        public const string ShippingType_CustomFieldId = "63258000003364917";

        public const string PrintShippingLabelUrl = "https://inventory.zohocloud.ca/api/v1/cm_delivery_order/$OrderFulfillmentId$/documents/$DocumentId$?organization_id=110001318978&inline=true";

        #endregion

        #region Stallion Express

        public const string Stallion_Endpoint_V4 = "https://ship.stallionexpress.ca/api/v4";

        public static string Stallion_Token => EnvironmentConstants.Get("ZO_RAW_STALLION_TOKEN");

        #endregion

        #region Freightcom

        public const string Freightcom_Endpoint = "https://external-api.freightcom.com";

        public static string Freightcom_Token => EnvironmentConstants.Get("ZO_RAW_FREIGHTCOM_TOKEN");

        public const string Freightcom_PaymentMethodId = "9HKHZKNJG2ZpjoZ6rme0Qixd00yNaDCk";

        #endregion

        #region Custom Functions

        // SEGR: Stallion Express Get Rates

        public const string SEGR_200 = "[Stallion Express] Get Rates SUCCESSFULLY";

        public const string SEGR_400 = "[Stallion Express] Get Rates FAILED";

        // SECS: Stallion Express Create Shipment

        public const string SECS_200 = "[Stallion Express] Create Shipment SUCCESSFULLY";

        public const string SECS_400 = "[Stallion Express] Create Shipment FAILED";

        // STS: Stallion Track Shipment

        public const string STS_200 = "[Stallion Express] Create Shipment SUCCESSFULLY";

        public const string STS_400 = "[Stallion Express] Create Shipment FAILED";

        // GSBI: Get Sales Order By Id

        public const string GSBI_200 = "[Zoho Inventory] Get Sales Order by Id SUCCESSFULLY";

        public const string GSBI_400 = "[Zoho Inventory] Get Sales Order by Id FAILED";

        // LSBN: List Sales Orders By Number

        public const string LSBN_200 = "[Zoho Inventory] Get Sales Order by Id SUCCESSFULLY";

        public const string LSBN_400 = "[Zoho Inventory] Get Sales Order by Id FAILED";

        // GPBI: Get Package By Id

        public const string GPBI_200 = "[Zoho Inventory] Get Package by Id SUCCESSFULLY";

        public const string GPBI_400 = "[Zoho Inventory] Get Sales Order by Id FAILED";

        // LAL: List All Locations

        public const string LAL_200 = "[Zoho Inventory] List All Locations SUCCESSFULLY";

        public const string LAL_400 = "[Zoho Inventory] List All Locations FAILED";

        // GLBI: Get Location By Id

        public const string GLBI_200 = "[Zoho Inventory] Get Location by Id SUCCESSFULLY";

        public const string GLBI_400 = "[Zoho Inventory] Get Location by Id FAILED";

        // GIBI: Get Item By Id

        public const string GIBI_200 = "[Zoho Inventory] Get Item by Id SUCCESSFULLY";

        public const string GIBI_400 = "[Zoho Inventory] Get Item by Id FAILED";

        // GRFW: Get Rates From Widget

        public const string GRFW_200 = "Get Rates From Widget SUCCESSFULLY";

        public const string GRFW_400 = "Get Rates From Widget FAILED";

        public const string GRFW_E01 = "Get Rates From Stallion FAILED";

        public const string GRFW_E02 = "Package Id is EMPTY, cannot get rate";

        // CSFW: Create Shipment From Widget

        public const string CSFW_200 = "Create Shipment From Widget SUCCESSFULLY";

        public const string CSFW_400 = "Create Shipment From Widget FAILED";

        public const string CSFW_E01 = "Create Shipment From Widget FAILED";

        // SIB: Search Item Batches

        public const string SIB_200 = "Search Item Batches SUCCESSFULLY";

        public const string SIB_400 = "Search Item Batches FAILED";

        // CPK: Create Package

        public const string CPK_200 = "Create Package SUCCESSFULLY";

        public const string CPK_400 = "Create Package FAILED";

        // CSO: Create Shipment Order

        public const string CSO_200 = "Create Shipment Order SUCCESSFULLY";

        public const string CSO_400 = "Create Shipment Order FAILED";

        // UF2S: Upload File To Shipment

        public const string UF2S_200 = "Upload File To Shipment SUCCESSFULLY";

        public const string UF2S_400 = "Upload File To Shipment FAILED";

        // UF2S: Upload File To Delivery Order

        public const string UF2D_200 = "Upload File To Shipment SUCCESSFULLY";

        public const string UF2D_400 = "Upload File To Shipment FAILED";

        // FGS: Freightcom Get Services

        public const string FGS_200 = "[Freightcom] Get Services SUCCESSFULLY";

        public const string FGS_400 = "[Freightcom] Get Services FAILED";

        // FCFC: Freightcom Calculate Freight Class

        public const string FCFC_200 = "[Freightcom] Calculate Freight Class SUCCESSFULLY";

        public const string FCFC_400 = "[Freightcom] Calculate Freight Class FAILED";

        // FRRE: Freightcom Request Rate Estimate

        public const string FRRE_200 = "[Freightcom] Request Rate Estimate SUCCESSFULLY";

        public const string FFRE_400 = "[Freightcom] Request Rate Estimate FAILED";

        // FRAR: Freightcom Retrieve A Rate

        public const string FRAR_200 = "[Freightcom] Retrieve a Rate SUCCESSFULLY";

        public const string FRAR_400 = "[Freightcom] Retrieve a Rate FAILED";

        // FRCS: Freightcom Create Shipment

        public const string FRCS_200 = "[Freightcom] Create Shipment SUCCESSFULLY";

        public const string FRCS_400 = "[Freightcom] Create Shipment FAILED";

        // FRSD: Freightcom Retrieve Shipment Details

        public const string FRSD_200 = "[Freightcom] Retrieve Shipment Details SUCCESSFULLY";

        public const string FRSD_400 = "[Freightcom] Retrieve Shipment Details FAILED";

        // SBR: Search Batches Request

        public const string SBR_200 = "Search Batches SUCCESSFULLY";

        public const string SBR_400 = "Search Batches FAILED";

        // CPFW: Create Package From Widget

        public const string CPFW_200 = "Create Package from Widget SUCCESSFULLY";

        public const string CPFW_400 = "Create Package from Widget FAILED";

        // QBW: Query Box Weights

        public const string QBW_200 = "Query Box Weights SUCCESSFULLY";

        public const string QBW_400 = "Query Box Weights FAILED";

        // SOF: Search Order Fulfillment

        public const string SOF_200 = "Search Order Fulfillment SUCCESSFULLY";

        public const string SOF_400 = "Search Order Fulfillment FAILED";

        // UOF: Update Order Fulfillment

        public const string UOF_200 = "Update Order Fulfillment SUCCESSFULLY";

        public const string UOF_400 = "Update Order Fulfillment FAILED";

        // FGPM: Freightcom Get Payment Methods

        public const string FGPM_200 = "[Freightcom] Get Payment Methods SUCCESSFULLY";

        public const string FGPM_400 = "[Freightcom] Get Payment Methods FAILED";

        // HEFP: Handle Excel File for Purity

        public const string HEFP_200 = "Handle Excel File for Purity SUCCESSFULLY";

        public const string HEFP_400 = "Handle Excel File for Purity FAILED";

        #endregion

    }

}
