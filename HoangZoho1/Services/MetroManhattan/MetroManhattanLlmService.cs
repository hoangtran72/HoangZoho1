using HoangZoho1.Constants;
using HoangZoho1.Helpers;
using HoangZoho1.Models.Common;
using Newtonsoft.Json;
using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace HoangZoho1.Services.MetroManhattan
{

    public class MetroManhattanLlmService : IMetroManhattanLlmService
    {

        public async Task<ApiResultDto<string>> ChatCompletions(string userContent)
        {

            var apiResult = new ApiResultDto<string>()
            {
                Code = ResultCode.BadRequest,
                Message = CommonConstants.MSG_400,
            };

            try
            {

                var client = new HttpClient();

                string endpoint = "https://api.openai.com/v1/chat/completions";

                string openAiKey = StringHelpers.Base64Decode(MetroManhattanConstants.OpenAiKey_Encoded);

                client.DefaultRequestHeaders.Authorization = new 
                    AuthenticationHeaderValue("Bearer", openAiKey);

                var payload = new
                {
                    model = "gpt-4o", // or "gpt-3.5-turbo"
                    messages = new[]
                    {
                    new { role = "system", content = MetroManhattanConstants.SystemPrompt },
                    new { role = "user", content = userContent }
                },
                    temperature = 0.7
                };

                string json = JsonConvert.SerializeObject(payload);

                var response = await client.PostAsync(endpoint, new StringContent(json, Encoding.UTF8, "application/json"));
                string result = await response.Content.ReadAsStringAsync();

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    apiResult.Code = ResultCode.OK;
                    apiResult.Message = CommonConstants.MSG_200;
                    apiResult.Data = result;
                    return apiResult;
                }    
                else
                {
                    apiResult.Message = result;
                    return apiResult;
                }    

            }
            catch (Exception ex)
            {

                apiResult.Message = $"{ex.Message} - {ex.StackTrace}";
                return apiResult;
            }

        }

        public async Task<ApiResultDto<string>> CallClaudeAsync(string userMessage)
        {

            var apiResult = new ApiResultDto<string>()
            {
                Code = ResultCode.BadRequest,
                Message = CommonConstants.MSG_400,
            };

            try
            {

                // Get Data from Constants 
                string apiKeyEncoded = MetroManhattanConstants.Anthropic_ApiKey;
                string apiKey = StringHelpers.Base64Decode(apiKeyEncoded);

                string version = MetroManhattanConstants.Anthropic_Version;
                string model = MetroManhattanConstants.Anthropic_Api_Version;

                var url = "https://api.anthropic.com/v1/messages";

                var client = new HttpClient();

                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Add("x-api-key", apiKey);
                client.DefaultRequestHeaders.Add("anthropic-version", version);

                var requestBody = new
                {
                    model,
                    max_tokens = 4000,
                    messages = new[]
                    {
                    new
                    {
                        role = "user",
                        content = userMessage
                    }
                }
                };

                var requestBodyStr = JsonConvert.SerializeObject(requestBody);
                var content = new StringContent(requestBodyStr, Encoding.UTF8, "application/json");

                var response = await client.PostAsync(url, content);
                var responseString = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    apiResult.Message = $"API Error: {response.StatusCode} - {responseString}";

                    return apiResult;
                }

                // If parsing fails, return raw response
                apiResult.Code = ResultCode.OK;
                apiResult.Message = CommonConstants.MSG_200;
                apiResult.Data = responseString;
                return apiResult;
            }
            catch (HttpRequestException ex)
            {
                apiResult.Message = $"HTTP Request Error: {ex.Message}";
                return apiResult;
            }
            catch (TaskCanceledException ex)
            {
                apiResult.Message = $"Request Timeout: {ex.Message}";
                return apiResult;
            }
            catch (Exception ex)
            {
                apiResult.Message = $"Unexpected Error: {ex.Message}";
                return apiResult  ;
            }

        }

    }

}
