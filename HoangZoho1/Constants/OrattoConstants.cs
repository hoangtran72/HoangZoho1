

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HoangZoho1.Constants
{

    public class OrattoConstants
    {

        public const string Oratto = "Oratto";

        public const string MarkEmail = "mark@oratto.co.uk";

        public const string ContactEmail = "contact@oratto.co.uk";

        public static string SendEmailEndpoint => EnvironmentConstants.Get("ORATTO_SEND_EMAIL_ENDPOINT");

        #region Zoho Auth

        public const string ZohoAuthEndpoint = "https://accounts.zoho.eu/oauth/v2/token";

        public static string ZohoClientId => EnvironmentConstants.Get("ORATTO_ZOHO_CLIENT_ID");

        public static string ZohoClientSecret => EnvironmentConstants.Get("ORATTO_ZOHO_CLIENT_SECRET");

        public static string ZohoMail_ClientId => EnvironmentConstants.Get("ORATTO_ZOHO_MAIL_CLIENT_ID");

        public static string ZohoMail_ClientSecret => EnvironmentConstants.Get("ORATTO_ZOHO_MAIL_CLIENT_SECRET");

        #endregion

        #region Zoho CRM

        public const string ZohoCRM_EndpointV2 = "https://www.zohoapis.eu/crm/v2";

        public const string ZohoCRM_EndpointV3 = "https://www.zohoapis.eu/crm/v3";

        public const string ZohoCRM_EndpointV5 = "https://www.zohoapis.eu/crm/v5";

        public static string ZohoCRM_RefreshToken => EnvironmentConstants.Get("ORATTO_ZOHO_CRM_REFRESH_TOKEN");

        #endregion

        #region Zoho Mail

        public const string ZohoMailEndpoint = "https://mail.zoho.eu/api";

        public static string ZohoMail_RefreshToken => EnvironmentConstants.Get("ORATTO_ZOHO_MAIL_REFRESH_TOKEN");

        #endregion

        #region OpenAI

        public static string OpenAiToken => EnvironmentConstants.Get("ORATTO_OPEN_AI_TOKEN");

        public const string OpenAiV1Endpoint = "https://api.openai.com/v1";

        #endregion

        #region Gemini

        public const string GeminiEndpoint = "https://generativelanguage.googleapis.com";

        public static string GeminiApiKey => EnvironmentConstants.Get("ORATTO_GEMINI_API_KEY");

        #endregion

        #region Custom Functions

        // SERS: Send Email when reassign Solicitor

        public const string AssignEmailTemplate = "Hi Mr. Mark,<br><br>Please kindly review the matter below:<br><ul><li>Matter <a target='_blank' href='https://crm.zoho.eu/crm/org20086343963/tab/CustomModule2/$MatterId$'>$MatterName$</a> was updated to the status <b>Re-assigned to new solicitor</b><br></li><li>Previous Solicitor: <a target='_blank' href='https://crm.zoho.eu/crm/org20086343963/tab/Contacts/$SolicitorId$'>$SolicitorName$</a></li><li>New Solicitor: <a target='_blank' href='https://crm.zoho.eu/crm/org20086343963/tab/Contacts/$NewSolicitorId$'>$NewSolicitorName$</a></li></ul>Thanks & Regards,<br>Oratto Automation";

        public const string CannotAssignEmailTemplate = "Hi Mr. Mark,<br><br>Please kindly review the matter below:<br><ul><li>Matter <a target='_blank' href='https://crm.zoho.eu/crm/org20086343963/tab/CustomModule2/$MatterId$'>$MatterName$</a> was updated to the status<b> Re-assigned to new solicitor</b><br></li><li>Previous Solicitor: $PreviousSolicitor$</li><li>New Solicitor: <a target='_blank' href='mailto:$SolicitorEmail$'>$SolicitorEmail$</a> does not exist in Zoho CRM.</li></ul>Thanks & Regards,<br>Oratto Automation";

        public const string SERS_200 = "Send email when reassign Solicitor SUCCESSFULLY";

        public const string SERS_400 = "Send email when reassign Solicitor FAILED";

        public const string SERS_E01 = "Get Matter by Id FAILED";

        public const string SERS_E02 = "Update Matter FAILED";

        public const string SERS_E03 = "Send Email FAILED";

        // SEFC: Send Email from Contact

        public const string SEFC_200 = "Send email when Contact SUCCESSFULLY";

        public const string SEFC_400 = "Send email from Contact FAILED";

        // SELU: Send Email for Last Updated

        public const string LastUpdatedTemplate = "Hi Mr. Mark,<br><br>FYI, the Solicitor <a target='_blank' href='https://crm.zoho.eu/crm/org20086343963/tab/Contacts/$SolicitorId$'>$SolicitorName$</a> has just updated the matter: <a target='_blank' href='https://crm.zoho.eu/crm/org20086343963/tab/CustomModule2/$MatterId$'>$MatterName$</a><ul><li>Leads Referred: <a target='_blank' href='https://crm.zoho.eu/crm/org20086343963/tab/Leads/$LeadId$'>$LeadName$</a></li><li>Updated: $History$</li></ul><br>Please review the matter.<br><br>Thanks & Regards,<br>Oratto Automation";

        public const string SELU_200 = "Send email for Last Updated SUCCESSFULLY";

        public const string SELU_400 = "Send email for Last Updated SUCCESSFULLY";

        public const string SELU_E01 = "Get Matter Timeline FAILED";

        public const string SELU_E02 = "Get Matter by Id FAILED";

        public const string SELU_E03 = "Referred Solicitor must NOT be EMPTY";

        // QS: Query Solicitors

        public const string QS_200 = "Query Solicitors SUCCESSFULLY";

        public const string QS_400 = "Query Solicitors FAILED";

        // AS2L: Assign Solicitors to Lead

        public const string AS2L_200 = "Assign Solicitors to Lead SUCCESSFULLY";

        public const string AS2L_400 = "Assign Solicitors to Lead FAILED";

        // UMA2O: Upload Mail Attachments to OpenAI

        public const string UMA2O_200 = "Upload Mail Attachments to OpenAI SUCCESSFULLY";

        public const string UMA2O_400 = "Upload Mail Attachments to OpenAI FAILED";

        // UMA2G: Upload Mail Attachments to Gemini

        public const string UMA2G_200 = "Upload Mail Attachments to Gemini SUCCESSFULLY";

        public const string UMA2G_400 = "Upload Mail Attachments to Gemini FAILED";

        // GACBG: Generate Attachmen Content by Gemini

        public const string GACBG_200 = "Generate Attachment Content by Gemini SUCCESSFULLY";

        public const string GACBG_400 = "Generate Attachment Content by Gemini FAILED";

        public const string GACBG_E01 = "Upload Attachment to Gemini FAILED";

        public const string GACBG_E02 = "Generate Attachment Content from file FAILED";
        
        public const string GACBG_E03 = "Attachment Id is EMPTY";

        public const string GACBG_E04 = "Update Queued Attachment FAILED";

        public const string GACBG_E05 = "Attachment is already GENERATED";

        public const string GACBG_ExtractFileContent = "Please retrieve the detailed content from the file.";

        // CC4QE: Chat Completions for Queued Email

        public const string CC4QE_200 = "Handle Chat Completions for Queued Email SUCCESSFULLY";

        public const string CC4QE_400 = "Handle Chat Completions for Queued Email FAILED";

        public const string CC4QE_E01 = "Call API to OpenAI FAILED";

        public const string CC4QE_E02 = "Update Queued Email FAILED";

        #endregion

    }

}
