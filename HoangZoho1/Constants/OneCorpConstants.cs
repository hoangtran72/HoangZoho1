using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HoangZoho1.Constants
{
    public static class OneCorpConstants
    {

        public const string OneCorp = "OneCorp";

        #region OnceHub

        public const string OnceHubEndpointV2 = "https://api.oncehub.com/v2";

        public static string OnceHubApiKey => EnvironmentConstants.Get("ONE_CORP_ONCE_HUB_API_KEY");

        #endregion

        #region Sakari

        public const string Sakari_AuthEndpoint = "https://api.sakari.io/oauth2/token";

        public const string Sakari_EndpointV1 = "https://api.sakari.io/v1";

        public const string Sakari_AccountId = "646851f94130e023c8f4dc80";

        public static string Sakari_ClientId => EnvironmentConstants.Get("ONE_CORP_SAKARI_CLIENT_ID");

        public static string Sakari_ClientSecret => EnvironmentConstants.Get("ONE_CORP_SAKARI_CLIENT_SECRET");

        #endregion

        #region Zoho Auth

        public const string ZohoAuthEndpoint = "https://accounts.zoho.com.au/oauth/v2/token";

        public static string ZohoClientId => EnvironmentConstants.Get("ONE_CORP_ZOHO_CLIENT_ID");

        public static string ZohoClientSecret => EnvironmentConstants.Get("ONE_CORP_ZOHO_CLIENT_SECRET");

        #endregion

        #region Zoho CRM

        public const string ZohoCRM_EndpointV2 = "https://www.zohoapis.com.au/crm/v2";

        public const string ZohoCRM_EndpointV3 = "https://www.zohoapis.com.au/crm/v3";

        public const string ZohoCRM_EndpointV5 = "https://www.zohoapis.com.au/crm/v5";

        public static string ZohoCRM_RefreshToken => EnvironmentConstants.Get("ONE_CORP_ZOHO_CRM_REFRESH_TOKEN");

        public const string ZohoCRM_JoshUserId = "20744000006461574";

        #region Blueprint

        public const string ZohoCRM_ClientCancelled_TransitionId = "20744000076004002";

        public const string ZohoCRM_StrategistCancelled_TransitionId = "20744000076004040";

        #endregion

        #endregion

        #region Zoho Projects

        public const string ZohoProjects_Endpoint = "https://projectsapi.zoho.com.au/restapi/portal/onecorp596";

        public const string ZohoProjects_EndpointV3 = "https://projectsapi.zoho.com.au/api/v3/portal/7001680653";

        public static string ZohoProjects_RefreshToken => EnvironmentConstants.Get("ONE_CORP_ZOHO_PROJECTS_REFRESH_TOKEN");

        #endregion

        #region Zoho Sign

        public const string ZohoSign_EndpointV1 = "https://sign.zoho.com.au/api/v1";

        public static string ZohoSign_RefreshToken => EnvironmentConstants.Get("ONE_CORP_ZOHO_SIGN_REFRESH_TOKEN");

        #endregion

        #region Zoho Workdrive

        public static string ZohoWorkdrive_RefreshToken => EnvironmentConstants.Get("ONE_CORP_ZOHO_WORKDRIVE_REFRESH_TOKEN");

        public const string ZohoWorkdrive_EndpointV1 = "https://www.zohoapis.com.au/workdrive/api/v1";

        public const string ZohoWorkdrive_TeamId = "68rflf42061e9c8eb471197090805f97b8005";

        public const string JMVAuthorities_CreditCardAuthorities_FolderId = "tqb0bd6e89ae7caf843458896e7f000ad6bf8";

        public const string QSLAuthorities_CreditCardAuthorities_FolderId = "tqb0b3fb044deba1845c2890d0e2019489918";

        public const string CreateFolder_RequestBody = "{data:{attributes:{name:\"$FolderName$\",parent_id:\"$ParentId$\"},type:\"files\"}}";

        public const string MoveFolder_RequestBody = "{data:{attributes:{parent_id: \"$DestinationId$\"},type:\"files\"}}";

        #endregion

        #region Twilio

        public const string Twilio_Endpoint = "https://api.twilio.com";

        public const string Twilio_ApiVersion = "2010-04-01";

        public const string Twilio_PhoneNumber = "+61483918322";

        public static string Twilio_AccountSID => EnvironmentConstants.Get("ONE_CORP_TWILIO_ACCOUNT_SID");

        public static string Twilio_AuthToken => EnvironmentConstants.Get("ONE_CORP_TWILIO_AUTH_TOKEN");

        // RMMRR: Read Multiple Message Resources Response

        public const string RMMRR_200 = "Read Multiple Message Resources SUCCESSFULLY";

        public const string RMMRR_400 = "Read Multiple Message Resources FAILED";

        #endregion

        #region MySQL Database

        public static string SSHHostname => EnvironmentConstants.Get("ONE_CORP_SSHHOSTNAME");

        public static string SSHUsername => EnvironmentConstants.Get("ONE_CORP_SSHUSERNAME");

        public static string SSHPassword => EnvironmentConstants.Get("ONE_CORP_SSHPASSWORD");

        public static string MySQLHostname => EnvironmentConstants.Get("ONE_CORP_MY_SQLHOSTNAME");

        public static string MySQLServerPost => EnvironmentConstants.Get("ONE_CORP_MY_SQLSERVER_POST");

        public static string MySQLUsername => EnvironmentConstants.Get("ONE_CORP_MY_SQLUSERNAME");

        public static string MySQLPassword => EnvironmentConstants.Get("ONE_CORP_MY_SQLPASSWORD");

        public const string MySQLSchema = "wvjppsvkbf";

        #endregion

        #region Custom Functions

        // SSB: Sync ScheduleOnce Booking from Webhook

        public const string SSBC_200 = "Sync ScheduleOnce Booking (CREATE) with Zoho CRM SUCCESSFULLY!";

        public const string SSBU_200 = "Sync ScheduleOnce Booking (UPDATE) with Zoho CRM SUCCESSFULLY!";

        public const string SSB_400 = "Sync ScheduleOnce Booking with Zoho CRM FAILED!";

        public const string SSB_SearchBooking_400 = "Search Booking in Zoho CRM FAILED!";

        public const string SSB_CreateBooking_400 = "Create Booking in Zoho CRM FAILED!";

        public const string SSB_UpdateBooking_400 = "Update Booking in Zoho CRM FAILED!";

        public const string SSB_UpdateBookingTransition_400 = "Update Booking Transition in Zoho CRM FAILED!";

        public const string SSB_StatusCompleted = "No need to update if booking status is Completed!";

        // SBI: Sync ScheduleOnce Booking by Id

        public const string SBI_200 = "Sync ScheduleOnce Booking by Id SUCCESSFULLY";

        public const string SBI_400 = "Sync ScheduleOnce Booking by Id FAILED";

        public const string SBI_SearchBooking_400 = "Search Booking in Zoho CRM FAILED!";

        public const string SBI_CreateBooking_400 = "Create Booking in Zoho CRM FAILED!";

        public const string SBI_UpdateBooking_400 = "Update Booking in Zoho CRM FAILED!";

        public const string SBI_StatusCompleted = "No need to update if booking status is Completed!";

        // SCM: Sync Calls with MySQL

        public const string SCM_200 = "Sync Call with MySQL DB SUCCESSFULLY!";

        public const string SCMCU_400 = "Sync Call with MySQL DB FAILED!";

        public const string SCM_200_INSERT = "Sync Call (INSERT) with MySQL DB SUCCESSFULLY!";

        public const string SCM_200_UPDATE = "Sync Call (UPDATE) with MySQL DB SUCCESSFULLY!";

        public const string SCM_GetCallById_400 = "Get Call by Id FAILED!";

        public const string SCM_CallAlreadyExists_400 = "Call Record already exists in DB!";

        // STMCU: Sync Task with MySQL upon Creation/Updation

        public const string STMCU_200 = "Sync Task with MySQL DB upon Creation/Updation SUCCESSFULLY!";

        public const string STMCU_400 = "Sync Task with MySQL DB FAILED!";

        public const string STMCU_200_INSERT = "Sync Task (INSERT) with MySQL DB SUCCESSFULLY!";

        public const string STMCU_200_UPDATE = "Sync Task (UPDATE) with MySQL DB SUCCESSFULLY!";

        public const string STMCU_GetTaskById_400 = "Get Task by Id FAILED!";

        public const string STMCU_TaskAlreadyExists_400 = "Task Record already exists in DB!";

        public const string STMCU_UpdateTask_400 = "Update Task FAILED!";

        // STMD: Sync Task with MySQL upon Deletion

        public const string STMD_200 = "Sync Task with MySQL DB upon Deletion SUCCESSFULLY!";

        public const string STMD_400 = "Sync Task with MySQL DB upon Deletion FAILED!";

        // SLMCU: Sync Lead with MySQL upon Creation/Updation

        public const string SLMCU_200 = "Sync Lead with MySQL DB upon Creation/Updation SUCCESSFULLY!";

        public const string SLMCU_400 = "Sync Lead with MySQL DB upon Creation/Updation FAILED!";

        public const string SLMCU_200_INSERT = "Sync Lead (INSERT) with MySQL DB SUCCESSFULLY!";

        public const string SLMCU_200_UPDATE = "Sync Lead (UPDATE) with MySQL DB SUCCESSFULLY!";

        public const string SLMCU_GetLeadById_400 = "Get Lead by Id FAILED!";

        public const string SLMCU_UpdateLead_400 = "Update Lead FAILED!";

        public const string SLMCU_LeadAlreadyExists_400 = "Lead Record already exists in DB!";

        // SLMD: Sync Lead with MySQL upon Deletion

        public const string SLMD_200 = "Sync Lead with MySQL DB upon Deletion SUCCESSFULLY!";

        public const string SLMD_400 = "Sync Lead with MySQL DB upon Deletion FAILED!";

        // SLSH: Sync Lead Status History with MySQL

        public const string SLSH_200 = "Sync Lead Status History with MySQL DB SUCCESSFULLY!";

        public const string SLSH_200_INSERT = "Sync Lead Status History (INSERT) with MySQL DB SUCCESSFULLY!";

        public const string SLSH_200_UPDATE = "Sync Lead Status History (UPDATE) with MySQL DB SUCCESSFULLY!";

        public const string SLSH_400 = "Sync Lead Status History with MySQL DB FAILED!";

        public const string SLSH_GetLeadStatusHistoryById_400 = "Get Lead Status History by Id FAILED!";

        public const string SLSH_GetLeadById_400 = "Get Lead by Id FAILED!";

        public const string SLSH_LeadStatusHistoryAlreadyExists_400 = "Lead Status History Record already exists in DB!";

        // IT2D: Import Tasks to DB

        public const string IT2D_200 = "Import Tasks to DB SUCCESSFULLY!";

        public const string IT2D_400 = "Import Tasks to DB FAILED!";

        // USDW: Upload Zoho Sign Document to Workdrive

        public const string USDW_200 = "Upload Zoho Sign Document to Zoho Workdrive SUCCESSFULLY";

        public const string USDW_400 = "Upload Zoho Sign Document to Zoho Workdrive FAILED";

        public const string USDW_E01 = "[USDW_E01] Get Document Details by Id FAILED";

        public const string USDW_E02 = "[USDW_E02] Download Document by Id FAILED";

        public const string USDW_E03 = "[USDW_E03] The document is NOT JMV Law or Queen Street Legal";

        public const string USDW_E04 = "[USDW_E04] The document status MUST BE COMPLETED";

        public const string USDW_E05 = "[USDW_E05] Upload File FAILED";

        public const string USDW_E06 = "[USDW_E06] Delete Zoho Sign Document FAILED";

        // CSF: Create Sub Folder

        public const string CSF_200 = "Create Sub Folder SUCCESSFULLY";

        public const string CSF_400 = "Create Sub Folder FAILED";

        public const string CSF_E01 = "[CSF_E01] There is no input folder name";

        public const string CSF_E02 = "[CSF_E02] List Files / Folders FAILED";

        public const string CSF_E03 = "[CSF_E03] Create Sub Folder FAILED";

        // SAF: Search across Folder

        public const string SAF_200 = "Search across folder SUCCESSFULLY";

        public const string SAF_400 = "Search across folder FAILED";

        // MF: Move Folder

        public const string MF_200 = "Move folder SUCCESSFULLY";

        public const string MF_400 = "Move across folder FAILED";

        // SSFW: Send Sakari SMS from Workflow

        public const string SSFW_200 = "Send Sakari SMS from workflow SUCCESSFULLY";

        public const string SSFW_400 = "Send Sakari SMS from workflow FAILED";

        public const string SSFW_E01 = "[SSFW_E01] Get User Details by Id FAILED";

        public const string SSFW_E02 = "[SSFW_E02] Query Sakari Users by Email FAILED";

        public const string SSFW_E03 = "[SSFW_E03] User doesn't have any Phone Groups";

        public const string SSFW_E04 = "[SSFW_E04] Send Sakari SMS FAILED";

        // SS2C: Send Sakari SMS to Contact

        public const string SS2C_200 = "Send Sakari SMS to Contact SUCCESSFULLY";

        public const string SS2C_400 = "Send Sakari SMS to Contact FAILED";

        public const string SS2C_E01 = "[SS2C_E01] Send Sakari SMS to Contact FAILED";

        public const string SS2C_E02 = "[SS2C_E02] Create Note for Contact FAILED";

        public const string SS2C_E03 = "[SS2C_E03] There are no Sakari Users set up.";

        public const string SS2C_E04 = "[SS2C_E04] Cannot get token using the account information.";

        // SS2L: Send Sakari SMS to Lead

        public const string SS2L_200 = "Send Sakari SMS to Lead SUCCESSFULLY";

        public const string SS2L_400 = "Send Sakari SMS to Lead FAILED";

        public const string SS2L_E01 = "[SS2L_E01] Send Sakari SMS to Lead FAILED";

        public const string SS2L_E02 = "[SS2L_E02] Create Note for Lead FAILED";

        public const string SS2L_E03 = "[SS2L_E03] There are no Sakari Users set up.";

        public const string SS2L_E04 = "[SS2L_E04] Cannot get token using the account information.";

        // GSU: Get Sakari Users

        public const string GSU_200 = "Get Sakari Users SUCCESSFULLY";

        public const string GSU_400 = "Get Sakari Users FAILED";

        // QZC: Query Zoho Projects Comments

        public const string QZC_200 = "Query Zoho Projects Comments SUCCESSFULLY";

        public const string QZC_400 = "Query Zoho Projects Comments FAILED";

        // QZT: Query Zoho Projects Tasks

        public const string QZT_200 = "Query Zoho Projects Tasks SUCCESSFULLY";

        public const string QZT_400 = "Query Zoho Projects Tasks FAILED";

        // APT: Associate Project Tag

        public const string APT_200 = "Associate Project Tag SUCCESSFULLY";

        public const string APT_400 = "Associate Project Tag FAILED";

        // SSPG2Z: Sync Sakari Phone Group to Zoho CRM

        public const string SSPG2Z_200 = "Sync Sakari Phone Groups to Zoho CRM SUCCESSFULLY";

        public const string SSPG2Z_400 = "Sync Sakari Phone Groups to Zoho CRM FAILED";

        public const string SSPG2Z_E01 = "Get Sakari Phone Groups FAILED";

        public const string SSPG2Z_E02 = "Create Sakari User FAILED";

        public const string SSPG2Z_E03 = "Update Sakari User FAILED";

        // QSE: Query Sakari User by Email

        public const string QSE_200 = "Query Sakari Users by Email SUCCESSFULLY";

        public const string QSE_400 = "Query Sakari Users by Email FAILED";

        public const string QSE_E01 = "Query Sakari Users FAILED";

        public const string QSE_E02 = "Get Sakari User by Id FAILED";

        // SSM2Z: Sync Sakari Mesages to Zoho CRM

        public const string SSM2Z_200 = "Sync Sakari Mesages to Zoho CRM SUCCESSFULLY";

        public const string SSM2Z_400 = "Sync Sakari Mesages to Zoho CRM FAILED";

        public const string SSM2Z_E01 = "Read multiple Message resources FAILED";

        public const string SSM2Z_E02 = "Create Sakari Message Log FAILED";

        public const string SSM2Z_E03 = "Update Sakari Message Log FAILED";

        // MSSM2Z: Mass Sync Sakari Messages to Zoho CRM

        public const string MSSM2Z_200 = "Mass Sync Sakari Messages to Zoho CRM SUCCESSFULLY";

        public const string MSSM2Z_400 = "Mass Sync Sakari Messages to Zoho CRM FAILED";

        public const string MSSM2Z_E01 = "Get Sakari User Groups FAILED";

        public const string MSSM2Z_E02 = "Get Sakari Messages FAILED";

        // STHM: Sync Twilio History Messages

        public const string STHM_200 = "Sync Twilio History Messages SUCCESSFULLY";

        public const string STHM_400 = "Sync Twilio History Messages FAILED";

        public const string STHM_E01 = "Read multiple Message resources FAILED";

        public const string STHM_E02 = "Create SMS Log FAILED";

        public const string STHM_E03 = "Update SMS Log FAILED";

        // STME2H: Sync Twilio Messages Every 2 Hours

        public const string STME2H_200 = "Sync Twilio Messages every 2h SUCCESSFULLY";

        public const string STME2H_400 = "Sync Twilio Messags every 2h FAILED";

        public const string STME2H_E01 = "Read multiple Message resources FAILED";

        public const string STME2H_E02 = "Create SMS Log FAILED";

        public const string STME2H_E03 = "Update SMS Log FAILED";

        // SL2S: Sync Lead to Sakari

        public const string SL2S_200 = "Sync Lead to Sakari SUCCESSFULLY";

        public const string SL2S_400 = "Sync Lead to Sakari FAILED";

        public const string SL2S_E01 = "Get Lead by Id FAILED";

        public const string SL2S_E02 = "Lead Phone MUST NOT be EMPTY";

        public const string SL2S_E03 = "Create Sakari Contact FAILED";

        public const string SL2S_E04 = "Update Sakari Contact FAILED";

        public const string SL2S_E05 = "Update Lead FAILED";

        public const string SL2S_E06 = "Fetch Contacts FAILED";

        // SC2S: Sync Contact to Sakari

        public const string SC2S_200 = "Sync Contact to Sakari SUCCESSFULLY";

        public const string SC2S_400 = "Sync Contact to Sakari FAILED";

        public const string SC2S_E01 = "Get Contact by Id FAILED";

        public const string SC2S_E02 = "Contact Phone MUST NOT be EMPTY";

        public const string SC2S_E03 = "Create Sakari Contact FAILED";

        public const string SC2S_E04 = "Update Sakari Contact FAILED";

        public const string SC2S_E05 = "Update Lead FAILED";

        public const string SC2S_E06 = "Fetch Contacts FAILED";

        // GPG: Get Phone Groups

        public const string GPG_200 = "Get Phone Groups SUCCESSFULLY";

        public const string GPG_400 = "Get Phone Groups FAILED";

        #endregion

    }
}
