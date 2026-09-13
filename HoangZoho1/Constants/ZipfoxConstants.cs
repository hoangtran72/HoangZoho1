using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HoangZoho1.Constants
{

    public class ZipfoxConstants
    {

        public const string Zipfox = "Zipfox";

        #region Zoho Auth

        public const string ZohoAuthEndpoint = "https://accounts.zoho.com/oauth/v2/token";

        public static string ZohoClientId => EnvironmentConstants.Get("ZIPFOX_ZOHO_CLIENT_ID");

        public static string ZohoClientSecret => EnvironmentConstants.Get("ZIPFOX_ZOHO_CLIENT_SECRET");

        #endregion

        #region Zoho CRM

        public const string ZohoCRM_EndpointV2 = "https://www.zohoapis.com/crm/v2";

        public const string ZohoCRM_EndpointV3 = "https://www.zohoapis.com/crm/v3";

        public static string ZohoCRM_RefreshToken => EnvironmentConstants.Get("ZIPFOX_ZOHO_CRM_REFRESH_TOKEN");

        #endregion

        #region Zoho Desk

        public const string ZohoDesk_EndpointV1 = "https://desk.zoho.com/api/v1";

        public static string ZohoDesk_RefreshToken => EnvironmentConstants.Get("ZIPFOX_ZOHO_DESK_REFRESH_TOKEN");

        #endregion

        #region WhatsApp

        public const string WhatsAppEndpoint = "https://graph.facebook.com/v17.0";

        public static string PermanentToken => EnvironmentConstants.Get("ZIPFOX_PERMANENT_TOKEN");

        public const string WhatsAppBusinessId = "112456001943844";

        public const string PhoneNumberId = "103707019494850";

        public const string TestPhoneNumberId = "102054229657851";

        public const string PhoneNumber = "+1 760 477 7574";

        #endregion

        #region Zoho CRM Functions

        // QC: Query Contacts

        public const string QC_200 = "[QC_200] Query Contacts SUCCESSFULLY";

        public const string QC_400 = "[QC_400] Query Contacts FAILED";

        // CCC: Create CRM Contact

        public const string CCC_200 = "[CC_200] Create CRM Contact SUCCESSFULLY";

        public const string CCC_400 = "[CC_400] Create CRM Contact FAILED";

        // UCC: Update CRM Contact

        public const string UCC_200 = "[UC_200] Update CRM Contact SUCCESSFULLY";

        public const string UCC_400 = "[UC_400] Update CRM Contact FAILED";

        // GARC: Get Account Related Contacts

        public const string GARC_200 = "[GARC_200] Get Account's Related Contacts SUCCESSFULLY";

        public const string GARC_400 = "[GARC_400] Get Account's Related Contacts FAILED";

        // SCP: Search Contacts by Phone

        public const string SCP_200 = "[SCP_200] Search Contacts by Phone SUCCESSFULLY";

        public const string SCP_204 = "[SCP_204] There is no Contact with the provided phone number";

        public const string SCP_400 = "[SCP_400] Search Contacts by Phone FAILED";

        // CWL: Create WhatsApp Log

        public const string CWL_200 = "[CWL_200] Create WhatsApp Log SUCCESSFULLY";

        public const string CWL_400 = "[CWL_400] Create WhatsApp Log FAILED";

        // UWL: Update WhatsApp Log

        public const string UWL_200 = "[CWL_200] Update WhatsApp Log SUCCESSFULLY";

        public const string UWL_400 = "[UWL_400] Update WhatsApp Log FAILED";

        // QWL: Query WhatsApp Logs

        public const string QWL_200 = "[QWL_200] Query WhatsApp Log SUCCESSFULLY";

        public const string QWL_400 = "[QWL_400] Query WhatsApp Log FAILED";

        // QRFQs: Query RFQs

        public const string QRFQs_200 = "[QWL_200] Query WhatsApp Log SUCCESSFULLY";

        public const string QRFQs_400 = "[QWL_400] Query WhatsApp Log FAILED";

        #endregion

        #region Zoho Desk Functions

        // CDT: Create Desk Ticket

        public const string CDT_200 = "[CDT_200] Create Desk Ticket SUCCESSFULLY";

        public const string CDT_400 = "[CDT_400] Create Desk Ticket FAILED";

        #endregion

        #region WhatsApp Functions

        // GWT:  Get all WhatsApp Templates

        public const string GWT_200 = "[GWT_200] Get all WhatsApp Templates SUCCESSFULLY";

        public const string GWT_400 = "[GWT_400] Get all WhatsApp Templates FAILED";

        // SWMT: Send WhatsApp Message by Template

        public const string SWMT_200 = "[SWMT_200] Send WhatsApp Message by Template SUCCESSFULLY";

        public const string SWMT_400 = "[SWMT_200] Send WhatsApp Message by Template FAILED";

        // VWP: Verify WhatsApp Payload

        public const string VWP_200 = "[VWP_200] Verify WhatsApp Payload SUCCESSFULLY";

        public const string VWP_400 = "[VWP_400] Verify WhatsApp Payload FAILED";

        #endregion

        #region Custom Functions

        // HWSP: Handle WhatsApp Status Payload

        public const string HWSP_200 = "[HWP_200] Handle WhatsApp Status Payload SUCCESSFULLY";

        public const string HWSP_400 = "[HWP_400] Handle WhatsApp Status Payload FAILED";

        public const string HWSP_E01 = "[HWSP_E01] Create WhatsApp Log FAILED";

        public const string HWSP_E02 = "[HWSP_E02] Update WhatsApp Log FAILED";

        // HWMP: Handle WhatsApp Message Payload

        public const string HWMP_200 = "[HWMP_200] Handle WhatsApp Message Payload SUCCESSFULLY";

        public const string HWMP_400 = "[HWMP_400] Handle WhatsApp Message Payload FAILED";

        public const string HWMP_E01 = "[HWMP_E01] Create WhatsApp Log FAILED";

        public const string HWMP_E02 = "[HWMP_E02] Update Contact FAILED";

        // CCNFS: Create Contact for Not Found Search

        public const string CCNFS_200 = "[CCNFS_200] Create Contact for Not Found Search SUCCESSFULLY";

        public const string CCNFS_400 = "[CCNFS_400] Create Contact for Not Found Search FAILED";

        // CTNFS: Create Ticket for Not Found Search

        public const string CTNFS_200 = "[CTNFS_200] Create Ticket for Not Found Search SUCCESSFULLY";

        public const string CTNFS_400 = "[CTNFS_400] Create Ticket for Not Found Search FAILED";

        #endregion

    }

}
