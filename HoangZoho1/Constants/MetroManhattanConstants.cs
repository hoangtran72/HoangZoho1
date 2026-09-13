namespace HoangZoho1.Constants
{

    public class MetroManhattanConstants
    {

        public const string MetroManhattan = "Metro-Manhattan";

        #region Zoho Auth

        public const string ZohoAuthEndpoint = "https://accounts.zohocloud.ca/oauth/v2/token";

        public static string ZohoClientId => EnvironmentConstants.Get("METRO_MANHATTAN_ZOHO_CLIENT_ID");

        public static string ZohoClientSecret => EnvironmentConstants.Get("METRO_MANHATTAN_ZOHO_CLIENT_SECRET");

        #endregion

        #region Zoho CRM

        public static string ZohoCRM_RefreshToken => EnvironmentConstants.Get("METRO_MANHATTAN_ZOHO_CRM_REFRESH_TOKEN");

        #endregion

        #region OpenAI

        public const string OpenAiEndpoint_V1 = "https://api.openai.com/v1";

        public static string OpenAiKey_Encoded => EnvironmentConstants.Get("METRO_MANHATTAN_OPEN_AI_KEY_ENCODED");

        public const string OpenAI_Model_GPT41 = "gpt-4o";

        public const string SystemPrompt = @"You are a business intelligence assistant.";

        #endregion

        #region Anthropic

        public static string Anthropic_ApiKey => EnvironmentConstants.Get("METRO_MANHATTAN_ANTHROPIC_API_KEY");

        public const string Anthropic_Endpoint = "https://api.anthropic.com/v1/messages";

        public const string Anthropic_Api_Version = "claude-sonnet-4-5";

        public const string Anthropic_Version = "2023-06-01";

        #endregion

        #region Email Assistant

        public const string OfficeRental_Email_Prompt = @"You are a professional and polite email assistant for a property company specializing in office rentals. Your task is to write a personalized email to a potential client.";

        #endregion

    }

}
